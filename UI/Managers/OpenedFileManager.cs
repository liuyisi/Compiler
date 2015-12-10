using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Files;

namespace UI.Managers
{
    class OpenedFileManager
    {
        public delegate void OnOpenedFileUpdated(List<Source> files);
        public event OnOpenedFileUpdated onOpenedFileUpdated;

        private readonly static OpenedFileManager manager = new OpenedFileManager();

        private List<Source> lstOpenedFiles;

        private OpenedFileManager()
        {
            lstOpenedFiles = new List<Source>();
        }

        public static OpenedFileManager GetManager()
        {
            return manager;
        }

        public void Open(Source file)
        {
            lstOpenedFiles.Add(file);
            _UpdateOpenedFiles();
        }

        public void Close(Source file)
        {
            lstOpenedFiles.Remove(file);
            _UpdateOpenedFiles();
        }

        public void Save()
        { 
        
        }

        private void _UpdateOpenedFiles()
        {
            onOpenedFileUpdated(lstOpenedFiles);
        }
    }
}
