using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalAPI.DTO;
using RentalAPI.Models;
using RentalAPI.Persistance;
using RentalAPI.Services;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [ApiController]
    [Route("Rentables")]
    public class RentablesController : Controller
    {
        private readonly IRentableService _rentableService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public RentablesController(IRentableService rentableService, ICategoryService categoryService, IMapper mapper)
        {
            _rentableService = rentableService;
            _categoryService = categoryService;
   
            _mapper = mapper;
        }

        // GET: Rental
        [HttpGet("Available")]
        public async Task<IEnumerable<RentableDTO>> Available(string categoryName,
                                                        DateTime startDate,
                                                        DateTime endDate)
        {
            if (categoryName == null ||
                categoryName == string.Empty)
                return null;

            if (startDate > endDate)
                return null;
            if (startDate < DateTime.Today)
                return null;

            var category = await _categoryService.FindByNameAsync(categoryName);
            if (category == null)
                return null;

            var availableRentals = await _rentableService.ListAvailableAsync(category.Id, startDate, endDate);

            return _mapper.Map<IEnumerable<Rentable>, IEnumerable<RentableDTO>>(availableRentals);
        } 
    }
}
