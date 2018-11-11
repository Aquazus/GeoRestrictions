using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Smod2;
using Smod2.Attributes;
using Smod2.EventHandlers;
using Smod2.Events;

namespace GeoRestrictions
{
    [PluginDetails(
    author = "Aquazus",
    name = "GeoRestrictions",
    description = "A ServerMod plugin to restrict your server to one or more countries",
    id = "aquazus.georestrictions",
    version = "1.0.1",
    SmodMajor = 3,
    SmodMinor = 1,
    SmodRevision = 13
    )]
    class GeoRestrictions : Plugin
    {
        private bool initialized = false;
        private LookupService lookupService;
        private Mode mode = Mode.Disabled;

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
            this.Info("Loading GeoIP database into memory...");
            if (File.Exists("geoip.dat"))
                this.lookupService = new LookupService("geoip.dat", LookupService.GEOIP_MEMORY_CACHE);
            else
                this.Error("Error: The geoip database file cannot be found. Make sure it is in the same folder as SCPSL.exe. You can also check the online documentation/configuration guide.");
        }

        public override void Register()
        {
            this.Info("Creating events handlers...");
            this.AddEventHandler(typeof(IEventHandlerPlayerJoin), new PlayerJoinHandler(this), Priority.Highest);
            this.AddEventHandler(typeof(IEventHandlerConnect), new InitializationHandler(this), Priority.Highest);
            this.Info("Registering config settings...");
            this.AddConfig(new Smod2.Config.ConfigSetting("georestrictions_mode", "configureme", Smod2.Config.SettingType.STRING, true, "The plugin mode explained in the configuration guide. Can be either whitelist of blacklist"));
            this.AddConfig(new Smod2.Config.ConfigSetting("georestrictions_bypass", new string[] { }, Smod2.Config.SettingType.LIST, true, "A list of SteamIDs that are able to bypass the filter."));
            this.AddConfig(new Smod2.Config.ConfigSetting("georestrictions_country_codes", new string[] { }, Smod2.Config.SettingType.LIST, true, "A list of Country Code ISO 3166-1 alpha-2 that will be applied by the filter."));
            this.AddConfig(new Smod2.Config.ConfigSetting("georestrictions_kick_message", "This is the default GeoRestrictions kick message.", Smod2.Config.SettingType.STRING, true, "Your custom kick message for the players that get restricted. You can put new lines using \\n"));
        }

        public Country GetPlayerCountry(string ip)
        {
            return lookupService.getCountry(ip);
        }

        public bool IsAuthorizedCountry(Country country)
        {
            if (this.mode == Mode.Whitelist)
                return country.getCode().Equals("--") || this.GetConfigList("georestrictions_country_codes").Contains(country.getCode());
            else if (this.mode == Mode.Blacklist)
                return country.getCode().Equals("--") || !this.GetConfigList("georestrictions_country_codes").Contains(country.getCode());
            else
                return true;
        }

        public bool IsWhitelisted(string steamid)
        {
            return this.GetConfigList("georestrictions_bypass").Contains(steamid);
        }

        public bool IsInitialized()
        {
            return this.initialized;
        }

        public Mode getMode()
        {
            return this.mode;
        }

        public void Initialize()
        {
            if (this.initialized)
                return;
            this.initialized = true;
            String mode = this.GetConfigString("georestrictions_mode").ToLower();
            mode = char.ToUpper(mode[0]) + mode.Substring(1);
            if (Enum.IsDefined(typeof(Mode), mode))
                this.mode = (Mode)Enum.Parse(typeof(Mode), mode);
            else
            {
                if (mode.Equals("configureme"))
                {
                    this.Error("Error: It seems like the plugin is not configured. Please check the online documentation/configuration guide.");
                    return;
                }
                else
                    this.Error("Error: \"" + mode + "\" is not a valid mode, Please check the online documentation/configuration guide.");
            }
            if (this.GetConfigList("georestrictions_country_codes").Length == 0)
                this.Error("Error: Country Codes list is empty, the plugin won't work. Please check the online documentation/configuration guide.");
            if (this.GetConfigString("georestrictions_kick_message").Equals("This is the default GeoRestrictions kick message.\nPlease contact the administrators of this server and notify them about this message."))
                this.Warn("Warning: The kick message is set on the default value and can be customized. Please check the online documentation/configuration guide.");
        }
    }
}
