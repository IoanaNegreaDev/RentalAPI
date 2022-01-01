using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class DamageRepository : GenericRepository<Damage>, IDamageRepository
    {
        public DamageRepository(RentalDbContext context) : base(context)
        {
        }
    }
}
