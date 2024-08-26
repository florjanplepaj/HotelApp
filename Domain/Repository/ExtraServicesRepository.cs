using HotelApp1.Domain.Interface;
using HotelApp1.Entities.Data;
using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Repository
{
    public class ExtraServicesRepository : IExtraServicesRepository
    {
        private readonly HotelAppContext _context;

        public ExtraServicesRepository(HotelAppContext context)
        {
            _context = context;
        }
        public bool servicesExist(int id)
        {
            if (_context.ExtraServices.Any(t => t.ServicesId == id)) return true;
            else return false;
        }

        public ExtraService GetExtraService(int id)
        {
            return _context.ExtraServices.Where(s => s.ServicesId == id).FirstOrDefault();
        }

        public ICollection<ExtraService> GetExtraServices()
        {
            return _context.ExtraServices.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

		public bool CreateExtraService(ExtraService extraService)
		{
			_context.Add(extraService); 
            return Save();
		}

		public bool UpdateExtraService(ExtraService extraService)
		{
			_context.Update(extraService);
            return Save();
		}

		public bool DeleteExtraService(ExtraService extraService)
		{
			_context.Remove(extraService);
            return Save();
		}
	}
}
