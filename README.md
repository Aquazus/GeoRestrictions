# GeoRestrictions
A ServerMod plugin to restrict your server to one or more countries.  
This plugin uses a GeoIP database instead of a Web API so it doesn't have any rate-limit and it is more reliable.

## Installation guide
### Downloading the plugin and the GeoIP database
- Have a look into the releases tab and download the latest dll and dat file.
- Place the dll file into your sm_plugins folder
- Place the dat file in your root folder (next to SCPSL.exe)
### Configuration guide
- First, you need to set the "georestrictions_mode" on either whitelist or blacklist, here's an explanation:  
**Whitelist:** All countries are blocked by default and you choose a list of authorized countries.  
**Blacklist:** All countries are allowed by default and you choose a list of denied countries.  
- And then, you have to input your country codes into the "georestrictions_country_codes" config setting. The format for country codes is the [Country Code ISO 3166-1 alpha-2](https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Current_codes) one. Example: US,UK,AU
- You can customise the kick message with the "georestrictions_kick_message" setting.
- You can put a list of SteamIDs that can bypass the check into the "georestrictions_bypass" setting (useful for whitelisting specific players on the filter). Example: id1,id2,id3

#### Configuration options
Config Option | Value Type | Default Value | Description
--- | :---: | :---: | ---
georestrictions_mode | String | configureme | The plugin mode explained in the configuration guide. Can be either **whitelist** of **blacklist**
georestrictions_country_codes | List | **Empty** | A list of [Country Code ISO 3166-1 alpha-2](https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2#Current_codes) that will be applied by the filter.
georestrictions_kick_message | String | This is the default GeoRestrictions kick message. | Your custom kick message for the players that get restricted. You can put new lines using **\n**
georestrictions_bypass | List | **Empty** | A list of SteamIDs that are able to bypass the filter.

###### This product includes GeoLite2 data created by MaxMind, available from <a href="http://www.maxmind.com">http://www.maxmind.com</a>.
