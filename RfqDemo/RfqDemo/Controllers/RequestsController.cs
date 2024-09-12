using Microsoft.AspNetCore.Mvc;
using RfqDemo.Models;
using RfqDemo.Services;
using RfqDemo.Constants;

namespace RfqDemo.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class RequestsController(ILogger<RequestsController> Logger, RfqObserver RfqObserver) : Controller
	{
		[HttpPost]
		public IActionResult SendRfq(string ticker, long quantity)
		{
			if (quantity <= 0)
			{
				return BadRequest("Quantity must be more than 0.");
			}

			Logger.LogInformation("RFQ received: {ticker} * {quantity}", ticker, quantity);
			Rfq rfq = new()
			{
				Ticker = ticker,
				Quantity = quantity
			};

			if (RfqObserver.ProcessRfq(rfq))
			{
				return Ok(rfq);
			}

			return NotFound($"Failed to get a quote for this instrument: {ticker}");
		}

		[HttpGet]
		public IActionResult ListRfq()
		{
			IEnumerable<string> allRfqs = RfqObserver.GetRfqList();
			Logger.LogInformation("Displaying {count} Rfqs", allRfqs.Count());
			return Ok(string.Join(Environment.NewLine, allRfqs));
		}

		[HttpPost]
		public IActionResult AcceptRfqBuy(string key)
		{
			Logger.LogInformation("RFQ accepted with {Buy} order: {key}", TradeConstants.BUY, key);
			return Ok(RfqObserver.TryAcceptRfq(key, TradeConstants.BUY));
		}

		[HttpPost]
		public IActionResult AcceptRfqSell(string key)
		{
			Logger.LogInformation("RFQ accepted with {Sell} order: {key}", TradeConstants.SELL, key);
			return Ok(RfqObserver.TryAcceptRfq(key, TradeConstants.SELL));
		}

		[HttpPost]
		public IActionResult RejectRfq(string key)
		{
			Logger.LogInformation("RFQ rejected: {key}", key);
			return Ok(RfqObserver.TryRejectRfq(key));
		}
	}
}
