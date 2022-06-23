using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalAPI.Controllers.ResourceParameters;
using RentalAPI.DbAccessors.SortingServices;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/rentables")]
    public class RentablesController : Controller
    {
        private readonly IRentableService _service;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;

        public RentablesController(IRentableService rentableService, IPropertyMappingService propertyMappingService, IMapper mapper)
        {
            _service = rentableService;
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet( Name = "GetRentables")]
        [EnableQuery]

        public async Task<ActionResult<IEnumerable<RentableDTO>>> Get([FromQuery] RentablesResourceParameters rentablesResourceParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<Rentable, RentableDTO>(rentablesResourceParameters.OrderBy))
                return BadRequest();

            var result = await _service.ListAsync(rentablesResourceParameters);

            if (!result.Success)
                return BadRequest(result.Message);

            if (result._entity == null)
                return NoContent();

            if (((ICollection<Rentable>)result._entity).Count == 0)
                return NoContent();

            var resultDTO = _mapper.Map<IEnumerable<Rentable>, IEnumerable<RentableDTO>>(result._entity);

            AddPaginationInRequestHeader(rentablesResourceParameters, result._entity);

            return Ok(resultDTO);
        }

  

        [AllowAnonymous]
        [HttpGet("{rentableId}")]
        [EnableQuery]
        public async Task<ActionResult<RentableDTO>> Get([FromRoute] int rentableId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (rentableId <= 0)
                return BadRequest("rentableId must be bigger than 0.");

            var result = await _service.FindByIdAsync(rentableId);

            if (result == null)
                return NotFound();

            var resultDTO = _mapper.Map<Rentable, RentableDTO>(result);

            return Ok(resultDTO);
        }
        [AllowAnonymous]
        [HttpGet("available")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<RentableDTO>>> GetAllAvailable(int categoryId,
                                                                                    [FromQuery] DateTime startDate,
                                                                                    [FromQuery] DateTime endDate)
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

        private object BuildPaginationMetadata(RentablesResourceParameters rentablesResourceParameters, PagedList<Rentable> pagedList)
        {
            var previousPageLink = pagedList.HasPrevious ?
              CreateRentalsResourceUri(rentablesResourceParameters, ResourceUriType.PreviousPage)
              : null;

            var nextPageLink = pagedList.HasNext ?
                CreateRentalsResourceUri(rentablesResourceParameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = pagedList.TotalCount,
                pageSize = pagedList.PageSize,
                currentPage = pagedList.CurrentPage,
                totalPages = pagedList.TotalPages,
                previousPageLink,
                nextPageLink
            };
            return paginationMetadata;
        }

        private void AddPaginationInRequestHeader(RentablesResourceParameters rentablesResourceParameters, PagedList<Rentable> pagedList)
        {
            var paginationMetadata = BuildPaginationMetadata(rentablesResourceParameters, pagedList);

            Response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationMetadata));
        }

        private string CreateRentalsResourceUri(RentablesResourceParameters rentablesResourceParameters, ResourceUriType resourceUriType)
        {
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetRentables",
                        new
                        {
                            orderBy = rentablesResourceParameters.OrderBy,
                            pageNumber = rentablesResourceParameters.PageNumber - 1,
                            pageSize = rentablesResourceParameters.PageSize,
                            category = rentablesResourceParameters.category,
                            searchQuery = rentablesResourceParameters.searchQuery
                        }); 
                case ResourceUriType.NextPage:
                    return Url.Link("GetRentables",
                        new
                        {
                            orderBy = rentablesResourceParameters.OrderBy,
                            pageNumber = rentablesResourceParameters.PageNumber + 1,
                            pageSize = rentablesResourceParameters.PageSize,
                            category = rentablesResourceParameters.category,
                            searchQuery = rentablesResourceParameters.searchQuery
                        });
                default:
                    return Url.Link("GetRentables",
                        new
                        {
                            orderBy = rentablesResourceParameters.OrderBy,
                            pageNumber = rentablesResourceParameters.PageNumber,
                            pageSize = rentablesResourceParameters.PageSize,
                            category = rentablesResourceParameters.category,
                            searchQuery = rentablesResourceParameters.searchQuery
                        });
            };

        }
    }
}
