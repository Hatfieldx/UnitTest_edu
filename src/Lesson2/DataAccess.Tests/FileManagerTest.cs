using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DataAccess.Tests
{
    class FileManagerFactoryUnderTest : FileManagerFactory
    {
        protected override IDataAccessObject CreateDataAccessObject()
        {
            return new StubFileDataObject();
        }
    }

    class FileManagerOverrideUnderTest : FileManagerOverride
    {
        protected override List<string> GetLogFiles()
        {
            return new List<string> { "file1.txt", "file2.log", "file3.exe", "main.log" };
        }
    }
    public class FileManagerTest
    {
        private readonly IDataAccessObject _dataAccessObject;
        public FileManagerTest()
        {
            _dataAccessObject = new StubFileDataObject();
        }

        [Fact]
        public void FileManagerConstructor_FindFileLogByName_2()
        {            
            var fileManager = new FileManagerConstructor(_dataAccessObject); // Dependency Injection
            string fileName = "main.log";

            bool exists = fileManager.FindLogFile(fileName);

            Assert.True(exists, string.Format("File {0} was not found.", fileName));
        }
        [Fact]
        public void FileManagerProperty_FindFileLogByName_3()
        {
            IDataAccessObject accessObject = new StubFileDataObject();
            var fileManager = new FileManagerProperty();
            fileManager.DataAccessObject = _dataAccessObject;// Dependency Injection
            string fileName = "main.log";

            bool exists = fileManager.FindLogFile(fileName);

            Assert.True(exists, string.Format("File {0} was not found.", fileName));
        }
        [Fact]
        public void FileManagerParamMethod_FindFileLogByName_4()
        {
            
            var fileManager = new FileManagerParamMethod();

            string fileName = "main.log";

            bool exists = fileManager.FindLogFile(fileName, _dataAccessObject); // Dependency Injection

            Assert.True(exists, string.Format("File {0} was not found.", fileName));
        }
        [Fact]
        public void FileManagerFactory_FindFileLogByName_5()
        {
            
            var fileManager = new FileManagerFactoryUnderTest();
            string fileName = "main.log";

            bool exists = fileManager.FindLogFile(fileName);

            Assert.True(exists, string.Format("File {0} was not found.", fileName));
        }
        [Fact]
        public void FileManagerOverride_FindFileLogByName_5()
        {

            var fileManager = new FileManagerOverrideUnderTest();
            string fileName = "main.log";

            bool exists = fileManager.FindLogFile(fileName);

            Assert.True(exists, string.Format("File {0} was not found.", fileName));
        }


    }
}
