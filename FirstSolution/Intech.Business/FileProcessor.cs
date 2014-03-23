using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class FileProcessor
    {
        /// <summary> Processes a given directory </summary>
        /// <param name = "path">The directory's path</param>
        /// <param name = "hasHiddenParent">True if current directory has a hidden parent</param>
        public FileProcessorResult Process (string path)
        {
            FileProcessorResult processorResult = new FileProcessorResult(path);

            if (new DirectoryInfo(path).Exists)
            {
                ProcessDirectory(path, processorResult, false);
                processorResult.ProcessingDate = DateTime.UtcNow;
            } 

            return processorResult;
        }

        /// <summary> Processes a given directory </summary>
        /// <param name = "path">The directory's path</param>
        /// <param name = "hasHiddenParent">True if current directory has a hidden parent</param>
        private void ProcessDirectory(string path, FileProcessorResult result, bool hasHiddenParent)
        {
            // get files and directories in current directories
            var directoryPaths = Directory.GetDirectories(path);
            string[] filePaths = Directory.GetFiles(path);

            // current directory is part of the total count of directories !
            result.TotalDirectoryCount++;
            
            // check if current directory is hidden
            if (FileHelpers.IsFileOrDirectoryHidden(new DirectoryInfo(path).Attributes))
            {
                result.TotalHiddenDirectoryCount++;
                hasHiddenParent = true;
            }

            // process files
            foreach (string filePath in filePaths)
                ProcessFile(filePath, result, hasHiddenParent);

            // process subdirectories
            foreach (string directory in directoryPaths)
                ProcessDirectory(directory, result, hasHiddenParent);
        }

        /// <summary> Processes a file </summary>
        /// <param name = "filePath">The file's path</param>
        /// <param name = "hasHiddenParent">True if a directory in the file's path is hidden</param>
        private void ProcessFile(string filePath, FileProcessorResult result, bool hasHiddenParent)
        {
            bool isFileHidden = FileHelpers.IsFileOrDirectoryHidden(File.GetAttributes(filePath));

            result.TotalFileCount++;
            if (isFileHidden)
                result.TotalHiddenFileCount++;

            if (isFileHidden || hasHiddenParent)
                result.TotalUnaccessibleFileCount++;
        }
    }
}