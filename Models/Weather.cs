namespace DynamicSun.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; } 
        public TimeSpan Time { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double DewPoint { get; set; }
        public double Pressure { get; set; }
        public string WindDirection { get; set; }
        public double WindSpeed { get; set; }
        public double CloudCover { get; set; }
        public double LBCloudCover { get; set; }
        public double HorizontalVisibility { get; set; }
        public string WeatherPhenomena { get; set; }
    }
}
