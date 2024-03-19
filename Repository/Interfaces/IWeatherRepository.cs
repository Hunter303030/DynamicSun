using DynamicSun.Models;

namespace DynamicSun.Repository.Interfaces
{
    public interface IWeatherRepository
    {
        public IEnumerable<Weather> List();
        public void Add(Weather weather);
    }
}
