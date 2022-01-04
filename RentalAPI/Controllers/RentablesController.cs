using AutoMapper;
using Microsoft.AspNet.OData;
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
    [Route("api/rentables")]
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

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<RentableDTO>>> Get()
        {
            var result = await _rentableService.ListAsync();
            var resource = _mapper.Map<IEnumerable<Rentable>, IEnumerable<RentableDTO>>(result);

            return Ok(resource);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<RentableDTO>> Get(int id)
        {
            var result = await _rentableService.FindByIdAsync(id);
            var resultDTO = _mapper.Map<Rentable, RentableDTO>(result);

            return Ok(resultDTO);
        }

        [HttpGet("Available")]
        [EnableQuery]
        public async Task<IEnumerable<RentableDTO>> GetAllAvailable(int categoryId,
                                                 DateTime startDate,
                                                 DateTime endDate)
        {
            if (categoryId<=0)
                return null;

            if (startDate > endDate)
                return null;
            if (startDate < DateTime.Today)
                return null;

            var availableRentals = await _rentableService.ListAvailableAsync(categoryId, startDate, endDate);

            return _mapper.Map<IEnumerable<Rentable>, IEnumerable<RentableDTO>>(availableRentals);
        }
    }
}
