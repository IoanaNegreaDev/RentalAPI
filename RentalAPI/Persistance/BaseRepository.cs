using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class BaseRepository
    {
        protected readonly RentalDbContext _context;

        public BaseRepository(RentalDbContext context)
        {
            _context = context;
        }
    }
}
