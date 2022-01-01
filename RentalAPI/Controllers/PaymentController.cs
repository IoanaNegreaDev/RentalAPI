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
        private readonly IVehicleContractService _contractService;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyRateExchanger _currencyRateExchanger;
        //  private readonly IClientService _clientService;

        private readonly IMapper _mapper;
        public PaymentsController(IPaymentService paymentService,
                                    IVehicleContractService contractService,
                                    ICurrencyService currencyService,
                                    ICurrencyRateExchanger currencyRateExchanger,

                                    IMapper mapper)
        {
            _paymentService = paymentService;
            _contractService = contractService;
            _currencyService = currencyService;
            _currencyRateExchanger = currencyRateExchanger;
            _mapper = mapper;
        }

        // GET: Payment
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
                return BadRequest("Failed to find contract in database.");

            if (paymentDTO.PaymentCurrency == string.Empty)
                return BadRequest("Invalid payment currency");

            var dbCurrency = await _currencyService.FindByNameAsync(paymentDTO.PaymentCurrency);
            if (dbCurrency == null)
                return BadRequest("Invalid payment currency name");

            var newPayment = new Payment
            {
                ContractId = paymentDTO.ContractId,
                PaymentCurrencyId = dbCurrency.Id,
                PaidAmountInPaymentCurrency = paymentDTO.PaidAmountInPaymentCurrency
            };

            var totalAmountInDefaultCurrency = dbContract.TotalBasePriceInDefaultCurrency +
                                           dbContract.TotalDamagePriceInDefaultCurrency +
                                           dbContract.TotalFullTankPriceInDefaultCurrency;

            if (dbCurrency.Default)
            {
                newPayment.TotalPriceInPaymentCurrency = totalAmountInDefaultCurrency;
            }
            else
            {
                var defaultCurrency = await _currencyService.GetDefaultAsync();
                var totalAmountInPaymentCurrency = await _currencyRateExchanger.Convert(defaultCurrency.Name,
                                                                                  dbCurrency.Name, 
                                                                                  totalAmountInDefaultCurrency);
                if (totalAmountInPaymentCurrency == 0.0)
                    return BadRequest("Failed to connect with currency converter.");

                newPayment.TotalPriceInPaymentCurrency = totalAmountInPaymentCurrency;
            }   
       
            var addPaymentResult = await _paymentService.AddAsync(newPayment);

       //     var resource = _mapper.Map<Payment, PaymentDTO>(addPaymentResult._entity);

            return Ok(addPaymentResult);
        }
    }
}
