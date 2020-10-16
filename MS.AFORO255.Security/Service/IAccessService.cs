using MS.AFORO255.Security.Model;
using System.Collections.Generic;

namespace MS.AFORO255.Security.Service
{
    public interface IAccessService
    {
        IEnumerable<Access> GetAll();
        bool Validate(string userName, string password);
    }
}
