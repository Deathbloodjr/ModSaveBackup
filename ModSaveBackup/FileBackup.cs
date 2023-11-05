using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModSaveBackup
{
    internal class FileBackup
    {
        public static void BackupFolder(string folderPath, string time)
        {
            //The plan was to take the contents of the file, zip it up, and place it within that folder
            // Or maybe place it in a backup folder within that folder?

            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            if (dirInfo.Exists)
            {
                var files = dirInfo.GetFiles("*.*");
                var subDirs = dirInfo.GetDirectories("*");
                string backupDirectory = Path.Combine(folderPath, "Backup", time);
                if (!Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }
               
                for (int i = 0; i < files.Length; i++)
                {
                    if (!files[i].FullName.Contains(Path.Combine(folderPath, "Backup")))
                    {
                        var relativePath = files[i].FullName.Remove(0, dirInfo.FullName.Length + 1);
                        File.Copy(files[i].FullName, Path.Combine(backupDirectory, relativePath));
                    }
                }

                for (int i = 0; i < subDirs.Length; i++)
                {
                    if (subDirs[i].Name != "Backup")
                    {
                        var relativePath = subDirs[i].FullName.Remove(0, dirInfo.FullName.Length + 1);
                        CopyDirectory(subDirs[i].FullName, Path.Combine(backupDirectory, relativePath), true);
                    }
                }
            }

        }

        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }


    }
}
