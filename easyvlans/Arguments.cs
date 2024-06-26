﻿using CommandLine;

namespace easyvlans
{
    internal class Arguments
    {

        [Option('v', "verbose")]
        public bool VeryVerbose { get; set; }

        [Option('h', "hidden")]
        public bool Hidden { get; set; }

        [Option('n', "nothidden")]
        public bool NotHidden { get; set; }

        [Value(0)]
        public string ConfigFile { get; set; }

    }
}
