using System.Collections.Generic;

namespace DataAccess
{
    public interface IDataAccessObject
    {
        List<string> GetFiles();
    }
}
