using FileHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using UI.Managers;

namespace UI.Files
{
    class Source:FileItem
    {
        private Project _Project;

        public Source(Project project)
        {
            this.FileType = FileTypes.File;
            this._Project = project;
        }

        public string AbsolutePath
        {
            get { return _Project + Path; }
        }

        public override XElement ToXML()
        {
            XElement elem = new XElement("File");
            elem.SetAttributeValue("Name", Name);
            elem.SetAttributeValue("Path", Path);
            elem.SetAttributeValue("UUID", UUID);
            return elem;
        }

        public override TreeViewItem ToTreeViewItem()
        {
            TreeViewItem item = new TreeViewItem() { Header = "源文件 - " + Name + ".snl" };
            item.MouseDoubleClick += item_MouseDoubleClick;
            ContextMenu cmFile = new ContextMenu();
            item.ContextMenu = cmFile;

            MenuItem itemCompile = new MenuItem() { Header = "编译" };
            itemCompile.Click += itemCompile_Click;
            cmFile.Items.Add(itemCompile);

            cmFile.Items.Add(new Separator());

            MenuItem itemShowTokenList = new MenuItem() { Header="显示Token链"};
            itemShowTokenList.Click += itemShowTokenList_Click;
            cmFile.Items.Add(itemShowTokenList);

            MenuItem itemShowTree = new MenuItem() { Header = "显示语法分析树" };
            itemShowTree.Click += itemShowTree_Click;
            cmFile.Items.Add(itemShowTree);

            return item;
        }

        void item_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenedFileManager.GetManager().Open(this);
        }

        void itemCompile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void itemShowTree_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void itemShowTokenList_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /*
        public static Source Create(Project project)
        {
           SaveFileDialog dialog = new SaveFileDialog();
           dialog.Filter = "SNL Source File(*.snl)|*.snl";
            if ((bool)dialog.ShowDialog())
            {


                return new Source(project)
                {
                    Path = dialog.FileName,
                    Name = dialog.SafeFileName.Remove(dialog.SafeFileName.Length - (".snl").Length)
                };
            }

            return null;
        }
         */
    }
}
