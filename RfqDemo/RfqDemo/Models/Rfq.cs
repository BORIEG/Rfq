namespace RfqDemo.Models
{
	public class Rfq : ICloneable
	{
		public string Key { get; private set; }
		public DateTime DateReceived { get; private set; }
		public string Ticker { get; set; }
		public long Quantity { get; set; }
		public Quote? Quote { get; set; }
		public RfqStatus Status { get; set; }
		public Rfq()
		{
			Key = Guid.NewGuid().ToString();
			DateReceived = DateTime.UtcNow;
			Status = RfqStatus.Pending;
		}
		public override string ToString() => $"{Key} - {DateReceived:MM/dd HH:mm} - {Ticker} * {Quantity} - {Status}";

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
