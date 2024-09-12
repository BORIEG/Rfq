using RfqDemo.Models;

namespace RfqDemo.MarketData
{
	public interface IPricingService
	{
		public bool TryGetMarketData(string ticker, long quantity, out Quote quote);
	}
}
