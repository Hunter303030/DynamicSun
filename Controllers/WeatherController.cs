using DynamicSun.Models;
using DynamicSun.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;

namespace DynamicSun.Controllers
{
    public class WeatherController : Controller
    {

        private readonly IWeatherRepository _weatherRepository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public WeatherController(IWeatherRepository weatherRepository,
                                 Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _weatherRepository = weatherRepository;
            _env = env;
        }

        public IActionResult List()
        {
            var weatherList = _weatherRepository.List();

            return View("~/Views/Main.cshtml", weatherList);
        }

        [HttpPost]
        public IActionResult AddArchive(IEnumerable<IFormFile> fileArchive)
        {
            var dir = _env.ContentRootPath + "\\Archive";

            foreach(var item in fileArchive)
            {
                IWorkbook workbook;
                IFormFile file = item;
                using (FileStream fileStream = new FileStream(Path.Combine(dir,item.FileName), FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);                    
                }

                using (FileStream fileStream = new FileStream($"{dir}/{file.FileName}", FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(fileStream);
                }
                int numberOfSheets = workbook.NumberOfSheets;
                for (int i = 0; i < numberOfSheets; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    int numberOfRows = sheet.LastRowNum;
                    int numberOfCell = 12;

                    for (int r = 4; r < numberOfRows; r++)
                    {
                        for (int c = 0; c < numberOfCell; c++)
                        {
                            DataFormatter formatter = new DataFormatter();
                            IRow row = sheet.GetRow(r);
                            var cellValue =formatter.FormatCellValue(row.GetCell(c));
                            
                            Console.WriteLine(cellValue);
                        }
                    }
                }
            }
            

            return List();
        }
    }
}
