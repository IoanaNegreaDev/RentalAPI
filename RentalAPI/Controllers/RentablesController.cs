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

            var category = await _categoryService.FindAsync(categoryName);
            if (category == null)
                return null;

            var availableRentals = await _rentableService.ListAvailableAsync(category.Id, startDate, endDate);

            return _mapper.Map<IEnumerable<Rentable>, IEnumerable<RentableDTO>>(availableRentals);
        } 

     /*   // POST: Rental
        [HttpPost("CreateContractForClient")]
        public async Task<IActionResult> CreateContractForClient(ClientDTO clientDTO)
        {
            var client = _mapper.Map<ClientDTO, Client>(clientDTO);
            var dbClient = await  _context.Clients.Where(item =>
                                 item.Name == client.Name &&
                                 item.Mobile == client.Mobile).FirstOrDefaultAsync();

            if (dbClient == null)
            {
                var addClientResult = await _context.Clients.AddAsync(client);
                if (addClientResult.State != EntityState.Added)
                    return BadRequest("Failed to add client to database.");

                await _context.SaveChangesAsync();
                dbClient = addClientResult.Entity;
            }

            Contract contract = new Contract()
            {
                ClientId = dbClient.Id,
                TotalBasePriceInDefaultCurrency = 0,
                TotalDamagePriceInDefaultCurrency = 0
            };

            var addContractResult = await _context.Contracts.AddAsync(contract);

            if (addContractResult.State != EntityState.Added)
                return BadRequest("Failed to add contract to database.");

            await _context.SaveChangesAsync();

            return Ok(addContractResult.Entity);
        }
        // POST: Rental
        [HttpPost("Rent")]
        public async Task<IActionResult> Rent(int rentableItemId, int contractId, 
                                              DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                return null;
            if (startDate < DateTime.Today)
                return null;

            var contract = await _context.Contracts.FindAsync(contractId);

            var newRental = new Rental
            {
                RentedItemId = rentableItemId,
                ContractId = contractId,
                StartDate = startDate,
                EndDate = endDate,
                StatusId = 1
            };

            var rentedItem = await _context.Rentables.FindAsync(rentableItemId);
            var daysCount = (float)(newRental.EndDate - newRental.StartDate).TotalDays;
            newRental.BasePrice = daysCount * rentedItem.PricePerDay;
            newRental.DamagePrice = 0;

            var result = await _context.Rentals.AddAsync(newRental);

            if (result.State != EntityState.Added)
                return BadRequest("Failed to add rental to database.");

            contract.TotalBasePriceInDefaultCurrency += newRental.BasePrice;
            
            _context.Contracts.Update(contract);
             await _context.SaveChangesAsync();

            return Ok(result.Entity);
        }

        // POST: Rental
        [HttpPost("ReportDamage")]
        public async Task<IActionResult> ReportDamage(int rentalId, 
                                                      string damageDescription,
                                                      float damageCost)
                                           
        {
            var rentedItemId = await _context.Rentals.Where(item =>
                                    item.Id == rentalId).Select(item => item.RentedItemId).FirstOrDefaultAsync();
            
            if (rentedItemId < 1)
                return BadRequest("Failed to find rental id in database.");

            var damage = new Damage
            {
                RentableItemId = rentedItemId,
                DamageDescription = damageDescription,
                DamageCost = damageCost
            };

            var addDamageToDbresult = await _context.Damages.AddAsync(damage);
            if (addDamageToDbresult.State != EntityState.Added)
                return BadRequest("Failed to add damage to database.");

            await _context.SaveChangesAsync();

            var damagePerRental = new RentalDamage
            {
                DamageId = addDamageToDbresult.Entity.Id,
                RentalId = rentalId
            };

            var addRentalDamageResult = await _context.RentalDamages.AddAsync(damagePerRental);

            if (addRentalDamageResult.State != EntityState.Added)
                return BadRequest("Failed to add rental damage to database.");

            await _context.SaveChangesAsync();

            return Ok(addRentalDamageResult.Entity);
        }

        // POST: Rental
        [HttpPost("AddPayment")]
        public async Task<IActionResult> AddPayment(int contractId,
                                                    int paymentCurrencyId,
                                                    float amountInPaymentCurrency)

        {
            var contract = await _context.Contracts.FindAsync(contractId);
            var currency = await _context.Currencies.FindAsync(paymentCurrencyId);

            if (contract == null)
                return BadRequest("Failed to find contract id in database.");

            if (currency == null)
                return BadRequest("Failed to find currency id in database.");
                 
            var totalPriceInDefaultCurrency = contract.TotalBasePriceInDefaultCurrency +
                contract.TotalDamagePriceInDefaultCurrency;
            var totalPriceInPaymentCurrency = totalPriceInDefaultCurrency;
            if (!currency.Default)
            {
                var defaultCurrency = await _context.Currencies.Where(item =>
                       item.Default == true).FirstOrDefaultAsync();

                if (defaultCurrency == null)
                    return BadRequest("Failed to find default currency in database.");

                totalPriceInPaymentCurrency = await new CurrencyRateExchanger().Convert(defaultCurrency.Name,
                                                                                     currency.Name,
                                                                                     totalPriceInDefaultCurrency);                                                         
            }

            var payment = new Payment
            {
                ContractId = contract.Id,
                PaymentCurrencyId = currency.Id,
                PaidAmountInPaymentCurrency = amountInPaymentCurrency,
                TotalPriceInPaymentCurrency = totalPriceInPaymentCurrency
            };

            var addPaymentToDbResult = await _context.Payments.AddAsync(payment);
            if (addPaymentToDbResult.State != EntityState.Added)
                return BadRequest("Failed to add payment to database.");

            await _context.SaveChangesAsync();

            return Ok(addPaymentToDbResult.Entity);
        }


        // POST: Rental
        [HttpPost("AddDamage")]
        public async Task<IActionResult> AddDamage(int rentalId,
                                                   string damageDescription,
                                                   float damageCost)

        {
            var rental = await _context.Rentals.FindAsync(rentalId);

            var newDamage = new Damage
            {
                RentableItemId = rental.RentedItemId,
                DamageDescription = damageDescription,
                DamageCost = damageCost
            };

            var newRentalDamage = new RentalDamage
            {
                RentalId = rentalId,
                Damage = newDamage,
                Rental = rental
            };

            var addRentalDamageResult = await _context.RentalDamages.AddAsync(newRentalDamage);
      
            rental.DamagePrice += damageCost;
            rental.Contract.TotalDamagePriceInDefaultCurrency += damageCost;
            _context.Rentals.Update(rental);

            await _context.SaveChangesAsync();

            return Ok(addRentalDamageResult.Entity);
        }


        // GET: Rental
        [HttpGet("GetContractDetails")]
        public async Task<ActionResult<ContractDTO>> GetContractDetails(int contractId, int paymentCurrencyId)     
        {
            var contract = await _context.Contracts.FindAsync(contractId);

            var currency = await _context.Currencies.FindAsync(paymentCurrencyId); 
            
            if (contract == null)
                return BadRequest("Failed to find contract id in database.");

            if (currency == null)
                return BadRequest("Failed to find currency id in database.");

            contract.TotalDamagePriceInDefaultCurrency = 0;
            foreach (var rental in contract.Rentals)
            {
                contract.TotalDamagePriceInDefaultCurrency += rental.BasePrice;

                rental.DamagePrice = 0;
                foreach (var rentalDamage in rental.RentalDamages)
                    rental.DamagePrice += rentalDamage.Damage.DamageCost;

                contract.TotalDamagePriceInDefaultCurrency += rental.DamagePrice;
            }

            var contractDTO = _mapper.Map<Contract, ContractDTO>(contract);

            contractDTO.ClientName = contract.Client.Name;
            contractDTO.ClientMobile = contract.Client.Mobile;
            contractDTO.PaymentCurrencyId = paymentCurrencyId;
            contractDTO.TotalBasePriceInDefaultCurrency = contract.TotalBasePriceInDefaultCurrency;
            contractDTO.TotalDamagePriceInDefaultCurrency = contract.TotalDamagePriceInDefaultCurrency;

            if (!currency.Default)
            {
                var defaultCurrency = await _context.Currencies.Where(item =>
                       item.Default == true).FirstOrDefaultAsync();

                if (defaultCurrency == null)
                    return BadRequest("Failed to find default currency in database.");               

                contractDTO.TotalBasePriceInPaymentCurrency = await new CurrencyRateExchanger().Convert(currency.Name,
                                                                defaultCurrency.Name,
                                                                contract.TotalBasePriceInDefaultCurrency);

                contractDTO.TotalDamagePriceInPaymentCurrency = await new CurrencyRateExchanger().Convert(currency.Name,
                                                               defaultCurrency.Name,
                                                               contract.TotalDamagePriceInDefaultCurrency);

            }
            return Ok(contractDTO);
        }*/
    }
}
