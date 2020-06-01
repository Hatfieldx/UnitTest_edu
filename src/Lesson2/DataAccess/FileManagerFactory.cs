using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class FileManagerFactory
    {
        protected virtual IDataAccessObject CreateDataAccessObject()
        {
            return new FileDataObject();
        }

        public bool FindLogFile(string fileName)
        {
            var dataAccessObject = CreateDataAccessObject();

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
