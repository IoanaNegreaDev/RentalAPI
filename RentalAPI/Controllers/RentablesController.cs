using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [ApiController]
    [Route("api/rentables")]
    public class RentablesController : Controller
    {
        private readonly IRentableService _service;
        private readonly IMapper _mapper;
        public RentablesController(IRentableService rentableService, IMapper mapper)
        {
            _service = rentableService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<RentableDTO>>> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.ListAsync();
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Rentable>, IEnumerable<RentableDTO>>(result);

            return Ok(resultDTO);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<RentableDTO>> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (id <= 0)
                return BadRequest("id must be bigger than 0.");

            var result = await _service.FindByIdAsync(id);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<Rentable, RentableDTO>(result);

            return Ok(resultDTO);
        }

        [HttpGet("Available")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<RentableDTO>>> GetAllAvailable(int categoryId,
                                                 DateTime startDate,
                                                 DateTime endDate)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (categoryId<=0)
                return BadRequest("CategoryId must be bigger than 0.");

            if (startDate > endDate)
                return BadRequest("EndDate must be bigger than StartDate");

            if (startDate < DateTime.Today)
                return BadRequest("StartDate must be bigger than today.");

            var result = await _service.ListAvailableAsync(categoryId, startDate, endDate);
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Rentable>, IEnumerable<RentableDTO>>(result);

            return Ok(resultDTO);
        }
    }
}
