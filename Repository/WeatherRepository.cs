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
            IEnumerable<Weather> weathers = _context.Weather;
            return weathers;
        }

        public void Add(Weather weather)
        {
            if(weather != null)
            {
                _context.Weather.Add(weather);
            }
        }

        public IEnumerable<Weather> Search(int selectInfo_Mounth, int selectInfo_Years)
        {
            IEnumerable<Weather> weathers = _context.Weather.Where(x=>x.Date.Year == selectInfo_Years && x.Date.Month == selectInfo_Mounth).ToList();
            return weathers;
        }

        public IEnumerable<Weather> Search_Month(int selectInfo_Mounth)
        {
            IEnumerable<Weather> weathers = _context.Weather.Where(x => x.Date.Month == selectInfo_Mounth).ToList();
            return weathers;
        }

        public IEnumerable<Weather> Search_Years(int selectInfo_Years)
        {
            IEnumerable<Weather> weathers = _context.Weather.Where(x => x.Date.Year == selectInfo_Years).ToList();
            return weathers;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
