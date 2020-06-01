using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class FileManagerProperty
    {
        IDataAccessObject dataAccessObject;

        // Свойство, через которое будет внедрена зависимость.
        public IDataAccessObject DataAccessObject
        {
            set { dataAccessObject = value; }
            get
            {
                if (dataAccessObject == null)
                {
                    throw new MemberAccessException("DataAccessObject has not been initialized.");
                }
                return dataAccessObject;
            }
        }

        public bool FindLogFile(string fileName)
        {
            List<string> files = DataAccessObject.GetFiles();

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
