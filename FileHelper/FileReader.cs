using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public class FileReader
    {
        private string path;

        public FileReader(string path)
        {
            this.path = path;
        }

        public string Read()
        {
            string content;
            using (FileStream stream = System.IO.File.OpenRead(path))
            {
                content = new StreamReader(stream).ReadToEnd();
            }

            return content;
        }
    }
}
