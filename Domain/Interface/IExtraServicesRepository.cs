using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Interface
{
    public interface IExtraServicesRepository
    {
        ICollection<ExtraService> GetExtraServices();
        ExtraService GetExtraService(int id);
        bool CreateExtraService(ExtraService extraService);
        bool DeleteExtraService(ExtraService extraService);
        bool UpdateExtraService(ExtraService extraService);

        bool servicesExist(int id);
        bool Save();
    }
}
