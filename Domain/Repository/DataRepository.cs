using HotelApp1.Domain.Interface;
using HotelApp1.Entities.Data;
using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Repository
{
	public class DataRepository : IDataRepository
    {
        private readonly HotelAppContext _context;

        public DataRepository(HotelAppContext context)
        {
            _context = context;
        }

		public bool CreateData(BrowsingData data)
		{
			_context.Add(data);
			return Save();
		}

		public void datacreation(BrowsingData data)
		{
            _context.Add(data);
            _context.SaveChanges();
		}

		public bool DataExist(int id)
        {
            if (_context.BrowsingData.Any(t => t.BrowsingId == id)) return true;
            else return false;
        }

		public bool DeleteData(BrowsingData browsingData)
		{
            _context.Remove(browsingData);
            return Save();
		}

		public bool DeleteDatas(List<BrowsingData> data)
		{
			_context.RemoveRange(data);
            return Save();  
		}

		public ICollection<BrowsingData> GetBrowsingData()
        {
            return _context.BrowsingData.ToList();
        }

        public BrowsingData GetData(int id)
        {
            return _context.BrowsingData.Where(p => p.BrowsingId == id).FirstOrDefault();
        }

		public ICollection<BrowsingData> GetDataFromClientId(int clientId)
		{
			return _context.BrowsingData.Where(w=>w.ClientId == clientId).ToList();
		}

		public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
