using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [ApiController]
    [Route("api/currencies")]
    public class CurrenciesController : Controller
    {
        private readonly ICurrencyService _service;
        private readonly IMapper _mapper;
        public CurrenciesController(ICurrencyService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<CurrencyDTO>>> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _service.ListAsync();
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Currency>, IEnumerable<CurrencyDTO>>(result);

            return Ok(resultDTO);
        }

        [HttpGet("{currencyId}")]
        [EnableQuery]
        public async Task<ActionResult<CurrencyDTO>> Get(int currencyId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (currencyId <= 0)
                return BadRequest("currencyId must be bigger than 0.");

            var result = await _service.FindByIdAsync(currencyId);
            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<Currency, CurrencyDTO>(result);

            return Ok(resultDTO);
        }
    }
}
