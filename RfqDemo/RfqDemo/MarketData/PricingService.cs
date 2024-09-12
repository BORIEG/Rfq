using RfqDemo.Models;

namespace RfqDemo.MarketData
{
	public class PricingService : IPricingService
	{
		public bool TryGetMarketData(string ticker, long quantity, out Quote quote)
		{
			if (string.IsNullOrWhiteSpace(ticker) || quantity <= 0)
			{
				quote = null;
				return false;
			}

			var randomGenerator = new Random();
			int mid = randomGenerator.Next(80, 120);
			int spread = randomGenerator.Next(1, 3);

			quote = new Quote()
			{
				Ask = mid - spread,
				Bid = mid + spread
			};
			return true;
		}
	}
}
