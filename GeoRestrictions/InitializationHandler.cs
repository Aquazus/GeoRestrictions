using Smod2;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Text.RegularExpressions;

namespace GeoRestrictions
{
    class InitializationHandler : IEventHandlerConnect
    {
        private GeoRestrictions plugin;

        public InitializationHandler(GeoRestrictions plugin)
        {
            this.plugin = plugin;
        }

        public void OnConnect(ConnectEvent ce)
        {
            if (!plugin.IsInitialized() && ce.Connection.IpAddress.Equals("localClient"))
            {
                plugin.Initialize();
            }
        }
    }
}
