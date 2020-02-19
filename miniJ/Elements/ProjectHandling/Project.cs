using miniJ.Grammar;
using System.Collections.Generic;
using System.IO;

namespace miniJ.Elements
{
    class Project
    {
        public List<Folder> Folders { get; set; }
        public string Name { get; set; }
        public ProjectTarget ProjectTarget { get; set; }

        public Project()
        {
        }
    }

    class File
    {
        public string Name { get; set; }
    }

    class Folder
    {
        public string Path { get; set; }
        public List<File> Files { get; set; }

        public Folder(string path)
        {
            Files = new List<File>();
            Path = path;

            if (!Path.EndsWith(Delimiters.Backslash.Value))
            {
                Path += Delimiters.Backslash.Value;
            }
        }

        public static Folder Open(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            Folder folder = new Folder(directoryInfo.FullName);

            foreach (FileInfo file in directoryInfo.GetFiles("*.jpl"))
            {
                folder.Files.Add(new File() { Name = file.Name });
            }
            return folder;
        }
    }
}