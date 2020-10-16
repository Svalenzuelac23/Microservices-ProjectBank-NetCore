using MongoDB.Driver;
using MS.AFORO255.History.DTO;
using MS.AFORO255.History.Model;
using MS.AFORO255.History.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MS.AFORO255.History.Service
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepository;

        public HistoryService(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<bool> Add(HistoryTransaction historyTransaction)
        {
            await _historyRepository.HistoryCredit.InsertOneAsync(historyTransaction);
            return true;
        }

        public async Task<IEnumerable<HistoryResponse>> GetAll()
        {
            var data = await _historyRepository.HistoryCredit.Find(_ => true).ToListAsync();
            var response = new List<HistoryResponse>();
            foreach (var item in data)
            {
                response.Add(new HistoryResponse()
                {
                    AccountId = item.AccountId,
                    Amount = item.Amount,
                    CreationDate = item.CreationDate,
                    IdTransaction = item.IdTransaction,
                    Type = item.Type
                });
            }

            return response;
        }
    }
}
