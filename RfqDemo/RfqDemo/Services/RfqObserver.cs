using RfqDemo.MarketData;
using RfqDemo.Models;

namespace RfqDemo.Services
{
	public class RfqObserver(ILogger<RfqObserver> logger, RfqCache rfqCache, IPricingService pricingService)
	{
		public IEnumerable<string> GetRfqList() => rfqCache.GetAll();
		public bool ProcessRfq(Rfq? rfq)
		{
			if(rfq?.Ticker == null)
			{
				return false;
			}

			if (pricingService.TryGetMarketData(rfq.Ticker, rfq.Quantity, out Quote quote))
			{
				logger.LogInformation("Quote received for {ticker}: {bid}/{ask}", rfq.Ticker, quote.Bid, quote.Ask);
				rfq.Quote = quote;
				rfqCache.Save(rfq);
				return true;
			}

			logger.LogWarning("Failed to get a quote for {ticker}", rfq.Ticker);
			return false;
		}
		
		public bool TryRejectRfq(string key)
		{
			if (rfqCache.UpdateRfqStatus(key, RfqStatus.Rejected, out _))
			{
				logger.LogInformation("RFQ rejected: {key}", key);
				return true;
			}

			logger.LogWarning("Can't find pending RFQ {key}", key);
			return false;
		}

		public bool TryAcceptRfq(string key, string buySell)
		{
			if (rfqCache.UpdateRfqStatus(key, RfqStatus.Accepted, out Rfq? rfqAccepted))
			{
				logger.LogInformation("RFQ accepted: {key}", key);
				Trade? trade = Trade.CreateFromRfq(rfqAccepted!, buySell);
				return TryExecuteTrade(trade);
			}

			logger.LogWarning("Can't find pending RFQ {key}", key);
			return false;
		}

		private bool TryExecuteTrade(Trade? trade)
		{
			if (IsEligible(trade))
			{
				logger.LogInformation("Trade booked");
				return true;
			}

			return false;
		}

		private static bool IsEligible(Trade? trade)
		{
			if (trade != null) //Add checks here
			{
				return true;
			}

			return false;
		}
	}
}
