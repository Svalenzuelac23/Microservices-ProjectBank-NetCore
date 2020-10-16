using System.Threading.Tasks;

namespace MS.AFORO255.Cross.Consul.Fabio
{
    public interface IFabioHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
    }
}