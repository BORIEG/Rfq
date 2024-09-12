using RfqDemo.Constants;

namespace RfqDemo.Models
{
	public class Trade
	{
		public string? Ticker { get; set; }
		public long Quantity { get; set; }
		public double Price { get; set; }
		public string? BuySell { get; set; }
		private Trade() { }
		public static Trade? CreateFromRfq(Rfq rfq, string buySell)
		{
			if(rfq?.Quote == null || rfq.Status != RfqStatus.Accepted)
			{
				return null;
			}

			return new Trade()
			{
				Ticker = rfq.Ticker,
				Quantity = rfq.Quantity,
				Price = buySell == TradeConstants.BUY ? rfq.Quote.Ask : rfq.Quote.Bid,
				BuySell = buySell
			};
		}
	}
}
