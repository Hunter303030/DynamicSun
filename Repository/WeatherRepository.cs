using DynamicSun.Data;
using DynamicSun.Models;
using DynamicSun.Repository.Interfaces;

namespace DynamicSun.Repository
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly DataContext _context;

        public WeatherRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Weather> List()
        {
            IEnumerable<Weather> weathers = _context.Weather.ToList();
            return weathers;
        }

        public void Add(Weather weather)
        {
            _context.Weather.Add(weather);
            _context.SaveChanges();
        }
    }
}
