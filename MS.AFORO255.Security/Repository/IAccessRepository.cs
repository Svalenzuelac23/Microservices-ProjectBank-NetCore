using MS.AFORO255.Security.Model;
using System.Collections.Generic;

namespace MS.AFORO255.Security.Repository
{
    public interface IAccessRepository
    {
        IEnumerable<Access> GetAll();
    }
}
