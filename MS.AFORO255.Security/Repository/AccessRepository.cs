using MS.AFORO255.Security.Model;
using MS.AFORO255.Security.Repository.Data;
using System.Collections.Generic;
using System.Linq;

namespace MS.AFORO255.Security.Repository
{
    public class AccessRepository : IAccessRepository
    {
        private readonly IContextDatabase _contextDatabase;

        public AccessRepository(IContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        public IEnumerable<Access> GetAll()
        {
            return _contextDatabase.Access.ToList();
        }
    }
}
