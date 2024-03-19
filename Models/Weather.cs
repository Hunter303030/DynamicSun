namespace DynamicSun.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; } 
        public TimeSpan Time { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double DewPoint { get; set; }
        public int Pressure { get; set; }
        public string WindDirection { get; set; }
        public double WindSpeed { get; set; }
        public int CloudCover { get; set; }
        public int LBCloudCover { get; set; }
        public int HorizontalVisibility { get; set; }
        public string WeatherPhenomena { get; set; }
    }
}
