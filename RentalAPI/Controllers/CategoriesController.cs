using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _categoryService.ListAsync();
            if (result == null)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(result);

            return Ok(resultDTO);
        }
    }
}
