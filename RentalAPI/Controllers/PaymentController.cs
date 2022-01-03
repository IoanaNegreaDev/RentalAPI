using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RentalAPI.DTO;
using RentalAPI.DTOs;
using RentalAPI.Models;
using RentalAPI.Persistance;
using RentalAPI.Services.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Controllers
{
    [ApiController]
    [Route("Payments")]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IContractService _contractService;
    
        private readonly IMapper _mapper;
        public PaymentsController(IPaymentService paymentService,
                                    IContractService contractService,                                
                                    IMapper mapper)
        {
            _paymentService = paymentService;
            _contractService = contractService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> Payments()
        {
            var result = await _paymentService.ListAsync();
            var resource = _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDTO>>(result);

            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(PaymentCreationDTO paymentDTO)
        {
            var dbContract = await _contractService.FindByIdAsync(paymentDTO.ContractId);
            if (dbContract == null)
                return BadRequest("Failed to find payment's contract in database.");

            var payment = _mapper.Map<PaymentCreationDTO, Payment>(paymentDTO);                
            var addPaymentResult = await _paymentService.AddAsync(payment);
            if (!addPaymentResult.Success)
                return BadRequest(addPaymentResult.Message);
         
            var addPaymentResultDTO = _mapper.Map<Payment, PaymentDTO>(addPaymentResult._entity);

            return Ok(addPaymentResultDTO);
        }
    }
}
