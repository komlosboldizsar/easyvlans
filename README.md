# Wiki

You can find a wiki about the configuration and usage at Bálint's fork: https://github.com/hallabalint/easyvlans/wiki

# For developers

The build artifacts (EXE, DLL, etc.) are copied to the `\_OUTPUT\_(ConfigName)_` (e.g. `\_OUTPUT\_Debug`) folder and run from there. The [launch settings](easyvlans/Properties/launchSettings.json) are configured to load the test_config.xml file as configuration when starting the EXE from Visual Studio, so make a copy of the [sample configuration](easyvlans/sample_config.xml) to the root directory (same level as this README) and name it test_config.xml!

# For users

Check the [sample configuration](easyvlans/sample_config.xml)! Not all the options are listed at the moment and there is no explanation for them :) Wiki coming soon.
