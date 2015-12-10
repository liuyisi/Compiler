using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI.Files;
using UI.Managers;

namespace UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void miNewProject_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "SNL工程(*.snlproj)|*.snlproj";
            if ((bool)dialog.ShowDialog())
            {
                Project project = new Project()
                {
                    Path = dialog.FileName.Remove(dialog.FileName.Length - dialog.SafeFileName.Length),
                    Name = dialog.SafeFileName.Remove(dialog.SafeFileName.Length - (".snlproj").Length)
                };

                ProjectManager.GetManager().NewProject(project);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectManager.GetManager().onProjectUpdated += MainWindow_onProjectUpdated;
            OpenedFileManager.GetManager().onOpenedFileUpdated += MainWindow_onOpenedFileUpdated;
        }

        void MainWindow_onOpenedFileUpdated(List<Source> files)
        {
            tcOpenedFiles.Items.Clear();

            foreach (var u in files)
            {
                TabItem item = new TabItem() { Header = u.Name };
                ContextMenu cmOpenedFile = new ContextMenu();
                item.ContextMenu = cmOpenedFile;
                MenuItem miCloseOpenFile = new MenuItem() { Header = "Close" };
                miCloseOpenFile.CommandParameter = u;
                miCloseOpenFile.Click += miCloseOpenFile_Click;
                cmOpenedFile.Items.Add(miCloseOpenFile);

                tcOpenedFiles.Items.Add(item);
            }
        }

        void miCloseOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenedFileManager.GetManager().Close((Source)((MenuItem)sender).CommandParameter);
        }

        void MainWindow_onProjectUpdated(List<TreeViewItem> items)
        {
            tvProject.Items.Clear();
            foreach (var u in items)
            {
                tvProject.Items.Add(u);
            }
        }

        private void miOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "SNL Project(*.snlproj)|*.snlproj|SNL Source(*.snl)|*.snl" };
            if ((bool)dialog.ShowDialog())
            {
                switch (dialog.FilterIndex)
                { 
                    case 1:
                        _OpenProject(dialog.FileName);
                        break;
                    case 2:
                        break;
                }
            }
        }

        private void _OpenProject(string path)
        {
            ProjectManager.GetManager().OpenProject(path);
        }
    }
}
