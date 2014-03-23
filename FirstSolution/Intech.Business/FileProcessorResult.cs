using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class FileProcessorResult
    {
        public int TotalFileCount { get; internal set; }
        public int TotalHiddenFileCount { get; internal set; }
        public int TotalDirectoryCount { get; internal set; }
        public int TotalHiddenDirectoryCount { get; internal set; }
        public int TotalUnaccessibleFileCount { get; internal set; }
        public DateTime ProcessingDate { get; internal set; }

        private readonly string _rootPath;
        
        public string RootPath 
        {
            get
            {
                return _rootPath;
            }
        }

        internal FileProcessorResult(string rootPath)
        {
            _rootPath = rootPath;
        }

        public bool RootPathExists
        {
            get
            {
                return this.TotalDirectoryCount != 0;
            }
        }

        /// <summary>Renders the results in the console</summary>
        public void RenderResultsInConsole()
        {
            Console.WriteLine("The file processor found these information in root directory " + this.RootPath + " : ");
            Console.WriteLine("Total Files = " + this.TotalFileCount);
            Console.WriteLine("Total Hidden Files  = " + this.TotalHiddenFileCount);
            Console.WriteLine("Total Directories = " + this.TotalDirectoryCount);
            Console.WriteLine("Total Hidden Directories = " + this.TotalHiddenDirectoryCount);
            Console.WriteLine("Total Unaccessible Files = " + this.TotalUnaccessibleFileCount);
            Console.WriteLine("Processing date : " + this.ProcessingDate);
        }
    }
}
