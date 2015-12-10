using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    public class FileWriter
    {
        private string path;
        private string content;

        public FileWriter(string path, string content)
        {
            this.path = path;
            this.content = content;
        }

        public void Write()
        {
            using (FileStream stream = File.OpenWrite(path))
            { 
                byte[] arrContent = Encoding.UTF8.GetBytes(content);
                stream.Seek(0, SeekOrigin.Begin);
                stream.SetLength(0);
                stream.Write(arrContent, 0, arrContent.Length);
            }
        }
    }
}
