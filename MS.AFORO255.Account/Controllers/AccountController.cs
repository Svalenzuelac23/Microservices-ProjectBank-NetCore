using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MS.AFORO255.Account.DTO;
using MS.AFORO255.Account.Service;
using MS.AFORO255.Cross.Metrics.Registry;

namespace MS.AFORO255.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMetricsRegistry _metricsRegistry;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger,  IMetricsRegistry metricsRegistry, IAccountService accountService)
        {
            _logger = logger;
            _metricsRegistry = metricsRegistry;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Get()
        {

            _metricsRegistry.IncrementFindQuery(); //ACA ESTAMOS REGISTRANDO METRICAS DE NUESTRO METODO GET
            _logger.LogInformation("Get Account");
            return Ok(_accountService.GetAll());
        }

        [HttpPost("Deposit")]
        public IActionResult Deposit([FromBody] AccountRequest request)
        {
            var result = _accountService.GetAll().Where(x => x.IdAccount == request.IdAccount).FirstOrDefault();
            Model.Account account = new Model.Account()
            {
                IdAccount = request.IdAccount,
                IdCustomer = result.IdCustomer,
                TotalAmount = result.TotalAmount + request.Amount,
                Customer = result.Customer
            };

            _accountService.Deposit(account);
            return Ok();
        }

        [HttpPost("Withdrawal")]
        public IActionResult Withdrawal([FromBody] AccountRequest request)
        {
            var result = _accountService.GetAll().Where(x => x.IdAccount == request.IdAccount).FirstOrDefault();
            if(result.TotalAmount < request.Amount)
            {
                return BadRequest(new { message = "The indicated amount cannot be withdrawal" });
            }

            Model.Account account = new Model.Account()
            {
                IdAccount = request.IdAccount,
                IdCustomer = result.IdCustomer,
                TotalAmount = result.TotalAmount - request.Amount,
                Customer = result.Customer
            };

            _accountService.Withdrawal(account);
            return Ok();
        }
    }
}