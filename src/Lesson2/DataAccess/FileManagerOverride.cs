using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class FileManagerOverride
    {
        protected virtual List<string> GetLogFiles()
        {
            return new FileDataObject().GetFiles();
        }

        public bool FindLogFile(string fileName)
        {
            List<string> files = GetLogFiles();

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
