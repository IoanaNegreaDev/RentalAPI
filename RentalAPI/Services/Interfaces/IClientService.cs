using RentalAPI.Models;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IClientService: IBaseService<Client>
    {
          public Task<Client> FindByNameAsync(string name);
    }
}
