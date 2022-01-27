using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class VehicleRentalRepository: RentalRepository, IVehicleRentalRepository
	{
		public VehicleRentalRepository(RentalDbContext context) : base(context)
		{ }


	}
}
