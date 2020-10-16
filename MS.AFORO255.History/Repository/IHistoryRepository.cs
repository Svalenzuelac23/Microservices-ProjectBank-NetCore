using MongoDB.Driver;
using MS.AFORO255.History.Model;

namespace MS.AFORO255.History.Repository
{
    public interface IHistoryRepository
    {
        IMongoCollection<HistoryTransaction> HistoryCredit { get; }
    }
}
