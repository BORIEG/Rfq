using Microsoft.Extensions.Logging;
using RfqDemo.MarketData;
using RfqDemo.Models;
using RfqDemo.Services;
using RfqDemo.Constants;

namespace RfqDemoTests
{
	public class Tests
	{
		[Test]
		public void RfqsAreAcceptedOrRejectedTest()
		{
			//Arrange
			var rfqToAccept = new Rfq()
			{
				Ticker = "TickerA",
				Quantity = 1000
			};
			string keyRfqToAccept = rfqToAccept.Key;

			var rfqToRefuse = new Rfq()
			{
				Ticker = "TickerB",
				Quantity = 1000
			};
			string keyRfqToRefuse = rfqToRefuse.Key;

			using LoggerFactory loggerFactory = new();
			RfqObserver observer = new(loggerFactory.CreateLogger<RfqObserver>(), new RfqCache(), new PricingService());

			//Act
			observer.ProcessRfq(rfqToAccept);
			observer.ProcessRfq(rfqToRefuse);
			observer.TryAcceptRfq(keyRfqToAccept, TradeConstants.BUY);
			observer.TryRejectRfq(keyRfqToRefuse);

			//Assert
			IEnumerable<string> allRfqs = observer.GetRfqList();
			Assert.That(allRfqs.Count(), Is.EqualTo(2));

			IEnumerable<string> acceptedRfqs = allRfqs.Where(x => x.Contains(RfqStatus.Accepted.ToString()));
			Assert.That(acceptedRfqs.Count(), Is.EqualTo(1));
			Assert.That(acceptedRfqs.First(), Does.Contain(keyRfqToAccept));

			IEnumerable<string> rejectedRfqs = allRfqs.Where(x => x.Contains(RfqStatus.Rejected.ToString()));
			Assert.That(rejectedRfqs.Count(), Is.EqualTo(1));
			Assert.That(rejectedRfqs.First(), Does.Contain(keyRfqToRefuse));
		}
	}
}