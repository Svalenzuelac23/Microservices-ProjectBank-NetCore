using System.Threading.Tasks;
using Consul;

namespace MS.AFORO255.Cross.Consul.Consul
{
    public interface IConsulServicesRegistry
    {
        Task<AgentService> GetAsync(string name);
    }
}