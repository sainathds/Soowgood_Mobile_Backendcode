﻿using IdentityMicroservice.Model;
using IdentityMicroservice.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Base;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;
using ParticipantStatus = Twilio.Rest.Video.V1.Room.ParticipantResource.StatusEnum;

namespace IdentityMicroservice.Services
{
    public class VideoService : IVideoService
    {
        readonly TwilioSettings _twilioSettings;

        public VideoService(Microsoft.Extensions.Options.IOptions<TwilioSettings> twilioOptions)
        {
            _twilioSettings =
                twilioOptions?.Value
             ?? throw new ArgumentNullException(nameof(twilioOptions));

            TwilioClient.Init(_twilioSettings.ApiKey, _twilioSettings.ApiSecret);
        }

        public string GetTwilioJwt(string identity)
            => new Token(_twilioSettings.AccountSid,
                         _twilioSettings.ApiKey,
                         _twilioSettings.ApiSecret,
                         identity ?? Guid.NewGuid().ToString(),
                         grants: new HashSet<IGrant> { new VideoGrant() }).ToJwt();
        public async Task<IEnumerable<RoomDetails>> GetAllRoomsAsync()
        {
            var rooms = await RoomResource.ReadAsync();
            var tasks = rooms.Select(
                room => GetRoomDetailsAsync(
                    room,
                    ParticipantResource.ReadAsync(
                        room.Sid,
                        ParticipantStatus.Connected)));

            return await Task.WhenAll(tasks);

            
        }

        public async Task<RoomDetails> GetRoomDetailsAsync(
                RoomResource room,
                Task<ResourceSet<ParticipantResource>> participantTask)
        {
            var participants = await participantTask;
            return new RoomDetails
            {
                Name = room.UniqueName,
                MaxParticipants = room.MaxParticipants ?? 0,
                ParticipantCount = participants.ToList().Count
            };
        }
    }
}

