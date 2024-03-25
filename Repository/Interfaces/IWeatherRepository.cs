using DynamicSun.Models;

namespace DynamicSun.Repository.Interfaces
{
    public interface IWeatherRepository
    {
        public IEnumerable<Weather> List();
        public void Add(Weather weather);
        public IEnumerable<Weather> Search(int selectInfo_Month, int selectInfo_Years);
        public void Save();
    }
}
