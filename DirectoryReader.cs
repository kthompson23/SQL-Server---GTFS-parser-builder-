using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace SQL_Server_GTFS_builder_parser
{
    // class DirectoryReader is given a file directory. It will take the given directory and create an arrayList of the files it contains,
    // which should be text files of transport data
    class DirectoryReader
    {
        public DirectoryReader(string directoryName)
        {
            this.directoryName = directoryName;
        }

        public void OpenDirectory()
        {
            if (!Directory.Exists(directoryName))
            {
                Console.WriteLine("Invalid directory specified. Exiting.");
                Environment.Exit(1);
            }

            transportFiles = Directory.GetFiles(directoryName);   
        }

        private string directoryName;
        public string [] transportFiles;
    }
}
