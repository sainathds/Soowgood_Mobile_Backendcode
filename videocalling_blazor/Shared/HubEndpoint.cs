using System;
using System.Collections.Generic;
using System.Text;

namespace videocalling_blazor.Shared
{
    public class HubEndpoints
    {
        public const string NotificationHub = "/notifications";
        public const string RoomsUpdated = nameof(RoomsUpdated);
    }
}
