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
    [Route("api/categories/{categoryId}/rentables")]
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
        public async Task<ActionResult<IEnumerable<RentableDTO>>> Get(int categoryId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (categoryId <= 0)
                return BadRequest("categoryId must be bigger than 0.");

            var result = await _service.ListAsync(categoryId);

            if (!result.Success)
                return BadRequest(result.Message);

            if (result._entity == null)
                return NoContent();

            if (((ICollection<Rentable>)result._entity).Count==0)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Rentable>, IEnumerable<RentableDTO>>(result._entity);

            return Ok(resultDTO);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<RentableDTO>> Get(int categoryId, int rentableId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (categoryId <= 0)
                return BadRequest("categoryId must be bigger than 0.");

            if (rentableId <= 0)
                return BadRequest("rentableId must be bigger than 0.");

            var result = await _service.FindByIdAsync(categoryId, rentableId);

            if (!result.Success)
                return BadRequest(result.Message);

            if (result._entity == null)
                return NotFound();

            if (((ICollection<Rentable>)result._entity).Count == 0)
                return NotFound();

            var resultDTO = _mapper.Map<Rentable, RentableDTO>(result._entity);

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
                return BadRequest("categoryId must be bigger than 0.");

            if (startDate > endDate)
                return BadRequest("EndDate must be bigger than StartDate");

            if (startDate < DateTime.Today)
                return BadRequest("StartDate must be bigger than today.");

            var result = await _service.ListAvailableAsync(categoryId, startDate, endDate);

            if (!result.Success)
                return BadRequest(result.Message);

            if (result._entity == null)
                return NoContent();

            if (((ICollection<Rentable>)result._entity).Count == 0)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Rentable>, IEnumerable<RentableDTO>>(result._entity);

            return Ok(resultDTO);
        }
    }
}
