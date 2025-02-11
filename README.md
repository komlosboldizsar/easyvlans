# Wiki  

You can find the [wiki](https://github.com/komlosboldizsar/easyvlans/wiki) for configuration and usage.  

# For Developers  

The build artifacts (EXE, DLL, etc.) are copied to the `\_OUTPUT\_(ConfigName)_` folder (e.g., `\_OUTPUT\_Debug`) and run from there. The [launch settings](easyvlans/Properties/launchSettings.json) are configured to load the `test_config.xml` file as the configuration when starting the EXE from Visual Studio.  

To set this up, make a copy of the [sample configuration](easyvlans/sample_config.xml), place it in the root directory (at the same level as this README), and rename it to `test_config.xml`.  

# For Users  

Check out the [sample configuration](easyvlans/sample_config.xml)! Not all options are listed in the configuration, but you can find more details in the wiki. :)
