using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class ClientRepository: GenericRepository<Client>, IClientRepository
    {
		public ClientRepository(RentalDbContext context) : base(context)
		{ }

		public async Task<Client> FindByNameAsync(string name)
			=> await _table.Where(item =>
								item.Name.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();    
	}
}
