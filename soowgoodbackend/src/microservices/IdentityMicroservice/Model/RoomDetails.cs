using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace IdentityMicroservice.Model
{
    public class RoomDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int ParticipantCount { get; set; }
        public int MaxParticipants { get; set; }
    }
}
