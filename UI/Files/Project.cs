using FileHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using UI.Managers;
using UI.Views;

namespace UI.Files
{
    class Project:FileItem
    {
        public List<FileItem> Items;

        private bool _Dirty;
        public bool Dirty
        {
            get { return _Dirty; }
            set
            {
                _Dirty = value;
                ProjectManager.GetManager().ProjectUpdated();
            }
        }

        public Project()
        {
            this.FileType = FileTypes.Project;
            this.Items = new List<FileItem>();
        }

        public override XElement ToXML()
        {
            XElement elem = new XElement("Project");
            elem.SetAttributeValue("Name", Name);
            elem.SetAttributeValue("UUID", UUID);
            foreach (var u in Items)
            {
                elem.Add(u.ToXML());
            }
            return elem;
        }

        public override TreeViewItem ToTreeViewItem()
        {
            TreeViewItem item = new TreeViewItem() { Header = "工程 - " + Name + (_Dirty ? " *" : "") };
            ContextMenu cmProject = new ContextMenu();
            item.ContextMenu = cmProject;
            MenuItem itemAdd = new MenuItem() { Header = "添加" };
            cmProject.Items.Add(itemAdd);
            MenuItem itemAddFolder = new MenuItem() { Header = "文件夹"};
            itemAddFolder.Click += itemAddFolder_Click;
            itemAdd.Items.Add(itemAddFolder);
            MenuItem itemAddFile = new MenuItem() { Header = "文件" };
            itemAddFile.Click += itemAddFile_Click;
            itemAdd.Items.Add(itemAddFile);
            foreach (var u in Items)
                item.Items.Add(u.ToTreeViewItem());
            return item;
        }

        void itemAddFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InputBox dialog = new InputBox();
            if ((bool)dialog.ShowDialog())
            {
                Source file = new Source(this) { Name = dialog.Content, Path = "/" + dialog.Content + ".snl" };
                System.IO.File.Create(Path + file.Path).Close();
                Items.Add(file);

                Save();
                ProjectManager.GetManager().ProjectUpdated();
            }
        }

        void itemAddFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InputBox dialog = new InputBox();
            if ((bool)dialog.ShowDialog())
            {
                Folder folder = new Folder(this) { Name = dialog.Content, Path = "/" + dialog.Content };
                System.IO.Directory.CreateDirectory(Path + folder.Path);
                Items.Add(folder);

                Save();
                ProjectManager.GetManager().ProjectUpdated();
            }
        }

        public void Save()
        {
            FileWriter writer = new FileWriter(this.Path + Name + ".snlproj", this.ToXML().ToString());
            writer.Write();
        }
    }
}
