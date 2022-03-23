using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace SapManPowerFTPClient
{
    class Options
    {
        [Option('h', "host", Required = true, HelpText = "ftphost")]
        public string InputHost { get; set; }      

        [Option('u', "username", Required = true, HelpText = "ftp username")]
        public string InputUser { get; set; }

        [Option("password", Required = true, HelpText = "ftp password")]
        public string InputPassword { get; set; }

        [Option("path", Required = true, HelpText = "file path")]
        public string InputPath { get; set; }

      

    }
}
