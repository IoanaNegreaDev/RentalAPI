using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class ClientRepository:BaseRepository, IClientRepository
    {
		public ClientRepository(RentalDbContext context) : base(context)
		{ }

		public async Task<IEnumerable<Client>> ListAsync()
		{
			return await _context.Clients.ToListAsync();
		}
		public async Task<Client> FindByIdAsync(int id)
		{
			return await _context.Clients.FindAsync(id);
		}

		public async Task<Client> FindByNameAsync(string name)
        {
			return await _context.Clients.Where(item =>
									item.Name.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();
        }
		public async Task AddAsync(Client client)
		{
			await _context.Clients.AddAsync(client);
		} 

		public void Update(Client client)
		{
			_context.Clients.Update(client);
		}

	}
}
