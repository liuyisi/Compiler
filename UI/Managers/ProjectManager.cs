using FileHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using UI.Files;

namespace UI.Managers
{
    class ProjectManager
    {
        public delegate void OnProjectUpdated(List<TreeViewItem> items);
        public event OnProjectUpdated onProjectUpdated;

        private static readonly ProjectManager manager = new ProjectManager();

        private List<Project> Projects;

        private ProjectManager()
        {
            this.Projects = new List<Project>();
        }

        public static ProjectManager GetManager()
        {
            return manager;
        }

        public void Open()
        { 
            
        }

        public void NewProject(Project project)
        {
            project.Save();

            Projects.Add(project);
            ProjectUpdated();
        }

        public void ProjectUpdated()
        {
            List<TreeViewItem> items = new List<TreeViewItem>();
            foreach (var u in Projects)
            {
                items.Add(u.ToTreeViewItem());
            }
            onProjectUpdated(items);
        }

        public void OpenProject(string path)
        {
            FileReader reader = new FileReader(path);
            XDocument doc = XDocument.Parse(reader.Read());

            var elem = doc.Element("Project");

            Project project = new Project()
            {
                Name = elem.Attribute("Name").Value,
                Path = path,
                UUID = Guid.Parse(elem.Attribute("UUID").Value)
            };

            foreach (var u in elem.Elements())
            {
                project.Items.Add(ParseProject(u, project));
            }

            Projects.Add(project);

            ProjectUpdated();
        }

        private FileItem ParseProject(XElement elem, Project project)
        {
            if (elem.Name == "Folder")
            {
                Folder folder = new Folder(project)
                {
                    Name = elem.Attribute("Name").Value,
                    Path = elem.Attribute("Path").Value,
                    UUID = Guid.Parse(elem.Attribute("UUID").Value)
                };

                foreach (var u in elem.Elements())
                {
                    folder.Items.Add(ParseProject(u, project));
                }

                return folder;
            }
            else if (elem.Name == "File")
            {
                Source source = new Source(project)
                {
                    Name = elem.Attribute("Name").Value,
                    Path = elem.Attribute("Path").Value,
                    UUID = Guid.Parse(elem.Attribute("UUID").Value)
                };
                return source;
            }
            else
            {
                throw new Exception("Illegal Node");
            }
        }
    }
}
