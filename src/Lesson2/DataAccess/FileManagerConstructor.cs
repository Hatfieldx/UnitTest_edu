using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class FileManagerConstructor
    {
        private IDataAccessObject dataAccessObject;

        public FileManagerConstructor()
        {
            dataAccessObject = new FileDataObject();
        }

        public FileManagerConstructor(IDataAccessObject dataAccessObject)
        {
            this.dataAccessObject = dataAccessObject;
        }

        public bool FindLogFile(string fileName)
        {
            List<string> files = dataAccessObject.GetFiles();

            foreach (var file in files)
            {
                if (file.Contains(fileName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
