using System.Threading.Tasks;

namespace MS.AFORO255.Cross.Consul.Consul
{
    public interface IConsulHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
    }
}

