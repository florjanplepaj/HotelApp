using HotelApp1.Entities.Models;

namespace HotelApp1.Domain.Interface
{
    public interface IDataRepository
    {
        ICollection<BrowsingData> GetBrowsingData();
        BrowsingData GetData(int id);
        ICollection<BrowsingData> GetDataFromClientId(int clientId);
        bool CreateData(BrowsingData data);
        bool DeleteData(BrowsingData browsingData);
        void datacreation(BrowsingData data);
        bool DeleteDatas(List<BrowsingData> data);   
        
        bool DataExist(int id);
        bool Save();
    }
}
