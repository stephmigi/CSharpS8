using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class FileHelpers
    {
        /// <summary>Checks if a directory or file is hidden</summary>
        /// <param value = "fileAttributes">the file or directory's file attributes</param>
        public static bool IsFileOrDirectoryHidden(FileAttributes fileAttributes)
        {
            return fileAttributes.HasFlag(FileAttributes.Hidden);
        }

        /// <summary>Create an empty file</summary>
        /// <param value = "fileAttributes">the filename to create</param>
        public static void CreateEmptyFile(string filename)
        {
            File.Create(filename).Dispose();
        }

        /// <summary>Clears the content of a directory</summary>
        /// <param value = "directory">Directory to clear</param>
        public static void ClearDirectory(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles())
                file.Delete();

            foreach (DirectoryInfo dir in directory.GetDirectories())
                dir.Delete(true);
        }
    }
}
