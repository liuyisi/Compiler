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
    class Folder:FileItem
    {
        private Project _Project;

        public List<FileItem> Items;

        public string AbsolutePath { get { return _Project.Path + Path; } }

        public Folder(Project _Project)
        {
            this.FileType = FileTypes.Folder;
            this._Project = _Project;
            this.Items = new List<FileItem>();
        }

        public override XElement ToXML()
        {
            XElement elem = new XElement("Folder");
            elem.SetAttributeValue("Name", Name);
            elem.SetAttributeValue("UUID", UUID);
            elem.SetAttributeValue("Path", Path);
            foreach (var u in Items)
            {
                elem.Add(u.ToXML());
            }
            return elem;
        }

        public override TreeViewItem ToTreeViewItem()
        {
            TreeViewItem item = new TreeViewItem() { Header = "目录 - " + Name };
            ContextMenu cmProject = new ContextMenu();
            item.ContextMenu = cmProject;
            MenuItem itemAdd = new MenuItem() { Header = "添加" };
            cmProject.Items.Add(itemAdd);
            MenuItem itemAddFolder = new MenuItem() { Header = "文件夹" };
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
                Source file = new Source(this._Project) { Name = dialog.Content, Path = Path + "/" + dialog.Content + ".snl" };
                System.IO.File.Create(_Project.Path + file.Path).Close();
                Items.Add(file);

                _Project.Save();
                ProjectManager.GetManager().ProjectUpdated();
            }
        }

        void itemAddFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InputBox dialog = new InputBox();
            if ((bool)dialog.ShowDialog())
            {
                Folder folder = new Folder(_Project) { Name = dialog.Content, Path = Path + "/" + dialog.Content }; 
                System.IO.Directory.CreateDirectory(_Project.Path + folder.Path);
                Items.Add(folder);

                _Project.Save();
                ProjectManager.GetManager().ProjectUpdated();
            }
        }
    }
}
