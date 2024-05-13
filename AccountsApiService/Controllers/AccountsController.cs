using AccountsApiService.Infrastructure;
using AccountsApiService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AccountsApiService.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IRepository<Account, long> _repository;

        public AccountsController(IRepository<Account, long> repository)
        {
            _repository = repository;
        }

        // GET: api/Accounts
        [HttpGet(template: "{CustId}")]
        public async Task<IActionResult> GetAllAccountsByCustomerId(int CustId)
        {
            var model = _repository.GetAllAccountsByCustomerID(CustId);
            if(model is null || !model.Any())
            {
                return NotFound("No Customer was found with the provided credentials.");
            }
            return Ok(model);
        }

        // GET: api/Accontss/5
        [HttpGet(template: "AccId:{AccId}")]
        public async Task<ActionResult<Account>> GetAccountByAccountId(long AccId)
        {
            var model = _repository.GetAccountByAccountID(AccId);
            if (model is null || model.accountID ==0)
            {
                return NotFound("No Account was found with the provided credentials.");
            }
            return model;
        }

        [HttpPost(template: "Create")]
        public ActionResult<Account> CreateAccount(Account model)
        {
            try{
                _repository.CreateAccount(model);
                return Ok(new { message = "Account Added" });
            }
            catch (SqlException sqlex)
            {
                return BadRequest(sqlex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(template: "Delete")]
        public async Task<ActionResult<Account>> DeleteAccount(long AccId)
        {
            try{
                _repository.DeleteAccountByAccountId(AccId);
                return Ok();
            }
            catch (SqlException sqlex)
            {
                return BadRequest(sqlex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}