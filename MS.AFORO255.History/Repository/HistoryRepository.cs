using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MS.AFORO255.History.Model;

namespace MS.AFORO255.History.Repository
{
    public class HistoryRepository : IHistoryRepository
    {
        //COMO ES UNA BASE DE DATOS NO RELACIONAL Y NO ESTAMOS USANDO EF CORE NO TENEMOS DBCONTEXT

        private readonly IMongoDatabase _database = null;
        public HistoryRepository(IConfiguration configuration)
        {
            //var client = new MongoClient(configuration["mongo:cn"]);
            var client = new MongoClient(configuration["cnmongo"]);
            if (client != null)
                _database = client.GetDatabase(configuration["mongo:database"]);
        }

        public IMongoCollection<HistoryTransaction> HistoryCredit
        {
            get
            {
                return _database.GetCollection<HistoryTransaction>("HistoryTransaction");
            }
        }
    }
}
