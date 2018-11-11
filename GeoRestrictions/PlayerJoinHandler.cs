using Smod2;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Text.RegularExpressions;

namespace GeoRestrictions
{
    class PlayerJoinHandler : IEventHandlerPlayerJoin
    {
        private Regex ipRegex;
        private GeoRestrictions plugin;

        public PlayerJoinHandler(GeoRestrictions plugin)
        {
            this.ipRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            this.plugin = plugin;
            plugin.Info("Player join handler instantiated successfuly!");
        }

         public void OnPlayerJoin(PlayerJoinEvent pje)
        {
            if (!plugin.IsInitialized() || plugin.getMode() == Mode.Disabled || plugin.GetConfigList("georestrictions_country_codes").Length == 0) return;
            Player player = pje.Player;
            Match match = ipRegex.Match(player.IpAddress);
            if (!match.Success) return;
            string ip = match.Value;
            Country country = plugin.GetPlayerCountry(ip);
            if (!plugin.IsAuthorizedCountry(country))
            {
                if (plugin.IsWhitelisted(player.SteamId))
                {
                    plugin.Info("Allowing " + player.Name + " [" + player.SteamId + "] (" + ip + ") from " + country.getName() + " (" + country.getCode() + ")");
                    return;
                } else if (player.GetAuthToken().Contains("Bypass geo restrictions: YES"))
                {
                    plugin.Info("Allowing " + player.Name + " due to auth token flag.");
                    return;
                }
                plugin.Info("Disconnecting " + player.Name + " [" + player.SteamId + "] (" + ip + ") from " + country.getName() + " (" + country.getCode() + ")");
                player.Disconnect(plugin.GetConfigString("georestrictions_kick_message"));
            }
        }
    }
}
