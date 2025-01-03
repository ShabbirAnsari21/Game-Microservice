using ApplicationSystem.Repository.ATMSystemRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ATMValuesController : ControllerBase
    {
        private readonly IATMRepository _repository;

        public ATMValuesController(IATMRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("add-cas")]
        public IActionResult AddCash([FromBody] Dictionary<int, int> denominations)
        {
            try
            {
                _repository.AddCash(denominations);
                return Ok("Cash added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("draw-cas")]
        public IActionResult WithdrawCash([FromBody] int amount)
        {
            try
            {
                var result = _repository.WithdrawCash(amount);
                return Ok(new { Message = "Cash dispensed successfully.", Denominations = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("summary-of-nodes")]
        public IActionResult SummaryOfNodes()
        {
            var summary = _repository.GetNodesSummary();
            return Ok(summary);
        }

        [HttpGet("list-of-transactions")]
        public IActionResult ListOfTransactions()
        {
            var transactions = _repository.GetTransactions();
            return Ok(transactions);
        }
    }
}
