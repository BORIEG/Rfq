using RfqDemo.Models;

namespace RfqDemo.Services
{
	public class RfqCache
	{
		private readonly Dictionary<string, Rfq> _rfqCache = [];

		public void Save(Rfq rfq)
		{
			lock (_rfqCache)
			{
				_rfqCache[rfq.Key] = rfq;
			}
		}

		public bool UpdateRfqStatus(string key, RfqStatus status, out Rfq? rfqUpdated)
		{
			lock (_rfqCache)
			{
				if (_rfqCache.TryGetValue(key, out Rfq? rfq) && rfq.Status == RfqStatus.Pending)
				{
					rfq.Status = status;
					rfqUpdated = (Rfq)rfq.Clone();
					return true;
				}
			}

			rfqUpdated = null;
			return false;
		}

		public IEnumerable<string> GetAll()
		{
			lock (_rfqCache)
			{
				return _rfqCache.Values.Select(x => x.ToString());
			}
		}
	}
}
