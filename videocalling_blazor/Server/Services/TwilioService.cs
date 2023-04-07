using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Twilio;
using Twilio.Base;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;
using Microsoft.Extensions.Options;
using ParticipantStatus = Twilio.Rest.Video.V1.Room.ParticipantResource.StatusEnum;
using videocalling_blazor.Shared;
using videocalling_blazor.Server.Options;


namespace videocalling_blazor.Server.Services
{
    public class TwilioService
    {
        readonly TwilioSettings _twilioSettings;

        public TwilioService(Microsoft.Extensions.Options.IOptions<TwilioSettings> twilioOptions)
        {
            _twilioSettings =
                twilioOptions?.Value
             ?? throw new ArgumentNullException(nameof(twilioOptions));

            TwilioClient.Init(_twilioSettings.ApiKey, _twilioSettings.ApiSecret);
        }

        public TwilioJwt GetTwilioJwt(string? identity) =>
            new TwilioJwt
            {
                Token = new Token(
                    _twilioSettings.AccountSid,
                    _twilioSettings.ApiKey,
                    _twilioSettings.ApiSecret,
                    identity ?? Guid.NewGuid().ToString(),
                    grants: new HashSet<IGrant> { new VideoGrant() })
                .ToJwt()
            };

        public async ValueTask<IEnumerable<RoomDetails>> GetAllRoomsAsync()
        {
            try
            {
                var rooms = await RoomResource.ReadAsync();
                var tasks = rooms.Select(
                    room => GetRoomDetailsAsync(
                        room,
                        ParticipantResource.ReadAsync(
                            room.Sid,
                            ParticipantStatus.Connected)));

                return await Task.WhenAll(tasks);

                static async Task<RoomDetails> GetRoomDetailsAsync(
                    RoomResource room,
                    Task<ResourceSet<ParticipantResource>> participantTask)
                {
                    var participants = await participantTask;
                    return new RoomDetails
                    {
                        Name = room.UniqueName,
                        MaxParticipants = room.MaxParticipants ?? 0,
                        ParticipantCount = participants.Count()
                    };
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
