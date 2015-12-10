using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace UI.Files
{
    public enum FileTypes { Project, Folder, File };

    abstract class FileItem
    {
        private Guid _UUID;
        public Guid UUID
        {
            get { return this._UUID; }
            set { this._UUID = value; }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { this._Name = value; }
        }

        private FileTypes _FileType;
        public FileTypes FileType
        {
            get { return _FileType; }
            set { this._FileType = value; }
        }
        
        private string _Path;
        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        public FileItem()
        {
            this._UUID = Guid.NewGuid();
        }

        public abstract XElement ToXML();

        public abstract TreeViewItem ToTreeViewItem();
    }
}
