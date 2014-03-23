using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Intech.Business;
using Intech.Business.Tests;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class FileProcessorTests
    {
        [SetUp]
        public void Init()
        {
            string fullName = TestHelpers.TestSupportFolder.FullName;

            //directory names
            string emptyFolder = Path.Combine(fullName, "EmptyFolder");
            string folderWithAFile = Path.Combine(fullName, "FolderWithAFile");
            string folderWithAHiddenFile = Path.Combine(fullName, "FolderWithAHiddenFile");
            string hiddenFolderWithAHiddenFile = Path.Combine(fullName, "HiddenFolderWithAHiddenFile");
            string hiddenFolderWithAFile = Path.Combine(fullName, "HiddenFolderWithAFile");
            
            // create 5 directories
            DirectoryInfo emptyFolderInfo = Directory.CreateDirectory(emptyFolder);
            DirectoryInfo folderWithAFileInfo = Directory.CreateDirectory(folderWithAFile);
            DirectoryInfo folderWithAHiddenFileInfo = Directory.CreateDirectory(folderWithAHiddenFile);
            DirectoryInfo hiddenFolderWithAHiddenFileInfo = Directory.CreateDirectory(hiddenFolderWithAHiddenFile);
            DirectoryInfo hiddenFolderWithAFileInfo = Directory.CreateDirectory(hiddenFolderWithAFile);
            
            // hide 2 directories
            hiddenFolderWithAHiddenFileInfo.Attributes = FileAttributes.Hidden;
            hiddenFolderWithAFileInfo.Attributes = FileAttributes.Hidden;

            // create files in each directory
            FileHelpers.CreateEmptyFile(Path.Combine(folderWithAFileInfo.FullName, "file1"));
            FileHelpers.CreateEmptyFile(Path.Combine(folderWithAHiddenFileInfo.FullName, "file2"));
            FileHelpers.CreateEmptyFile(Path.Combine(hiddenFolderWithAHiddenFileInfo.FullName, "file3"));
            FileHelpers.CreateEmptyFile(Path.Combine(hiddenFolderWithAFileInfo.FullName, "file4"));
            
            // hide 2 files
            File.SetAttributes(Path.Combine(fullName, "FolderWithAHiddenFile", "file2"), FileAttributes.Hidden);
            File.SetAttributes(Path.Combine(fullName, "HiddenFolderWithAHiddenFile", "file3"), FileAttributes.Hidden);
        }

        [Test]
        public void FileProcessor_CheckReturnType()
        {
            FileProcessor p = new FileProcessor();

            FileProcessorResult  result = p.Process(TestHelpers.TestSupportFolder.FullName);

            Assert.That(result.GetType() == typeof(FileProcessorResult));
        }

        [Test]
        public void FileProcessor_CheckUnexistingDirectory()
        {
            FileProcessor p = new FileProcessor();

            FileProcessorResult result = p.Process("_)àçé(^]~unexistingDirectory");

            Assert.That(result.RootPathExists == false);
            Assert.That(result.TotalFileCount, Is.EqualTo(0));
        }

        [Test]
        public void FileProcessor_CheckEmptyDirectory()
        {
            FileProcessor p = new FileProcessor();

            FileProcessorResult result = p.Process(Path.Combine(TestHelpers.TestSupportFolder.FullName, "EmptyFolder"));

            Assert.That(result.TotalDirectoryCount, Is.EqualTo(1));
        }

        [Test]
        public void FileProcessor_CheckTotalDirectoryCount()
        {
            FileProcessor p = new FileProcessor();

            FileProcessorResult result = p.Process(TestHelpers.TestSupportFolder.FullName);

            Assert.That(result.TotalDirectoryCount, Is.EqualTo(6));
        }

        [Test]
        public void FileProcessor_CheckTotalFileCount()
        {
            FileProcessor p = new FileProcessor();

            FileProcessorResult result = p.Process(TestHelpers.TestSupportFolder.FullName);

            Assert.That(result.TotalFileCount, Is.EqualTo(4));
        }

        [Test]
        public void FileProcessor_CheckTotalHiddenDirectoryCount()
        {
            FileProcessor p = new FileProcessor();

            FileProcessorResult result = p.Process(TestHelpers.TestSupportFolder.FullName);

            Assert.That(result.TotalHiddenDirectoryCount, Is.EqualTo(2));
        }

        [Test]
        public void FileProcessor_CheckTotalHiddenFileCount()
        {
            FileProcessor p = new FileProcessor();

            FileProcessorResult result = p.Process(TestHelpers.TestSupportFolder.FullName);

            Assert.That(result.TotalHiddenFileCount, Is.EqualTo(2));
        }

        [Test]
        public void FileProcessor_CheckTotalUnaccessibleFileCount()
        {
            FileProcessor p = new FileProcessor();

            FileProcessorResult result = p.Process(TestHelpers.TestSupportFolder.FullName);

            Assert.That(result.TotalUnaccessibleFileCount, Is.EqualTo(3));
        }

        [TearDown]
        public void CleanUp()
        {
            FileHelpers.ClearDirectory(new DirectoryInfo(TestHelpers.TestSupportFolder.FullName));
        }
    }
}
