using DynamicSun.Data;
using DynamicSun.Models;
using DynamicSun.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DynamicSun.Controllers;

public class WeatherController : Controller
{
    private readonly IWeatherRepository _weatherRepository;
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

    public WeatherController(IWeatherRepository weatherRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
    {
        _weatherRepository = weatherRepository;
        _env = env;
    }

    public async Task<IActionResult> List(int pageNumber = 1)
    {
        var weatherList = _weatherRepository.List();
        return View("~/Views/Main.cshtml", await PaginationList<Weather>.CreateAsyns(weatherList, pageNumber, 10));
    }

    public IActionResult AddArchive(IEnumerable<IFormFile> fileArchive)
    {
        string dir = _env.ContentRootPath + "\\Archive";
        foreach (IFormFile item in fileArchive)
        {
            IFormFile file = item;
            IWorkbook workbook;
            using (FileStream target = new FileStream(Path.Combine(dir, item.FileName), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(target);
            }
            using (FileStream fileStream = new FileStream(dir + "/" + file.FileName, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = new XSSFWorkbook(fileStream);
            }

            int numberOfSheets = workbook.NumberOfSheets;


            CheackErrorXlsx(numberOfSheets, dir, file, workbook);
            InsertToDb(numberOfSheets, workbook);
        }
        return RedirectToAction("List", "Weather");
    }

    private static void CheackErrorXlsx(int numberOfSheets, string dir, IFormFile file, IWorkbook workbook)
    {
        DataFormatter formatter = new DataFormatter();

        for (int i = 0; i < numberOfSheets; i++)
        {
            ISheet sheet = workbook.GetSheetAt(i);
            int numberOfRows = sheet.LastRowNum;
            for (int r = 4; r < numberOfRows; r++)
            {
                IRow row = sheet.GetRow(r);

                for (int q = 0; q < 12; q++)
                {
                    var current = formatter.FormatCellValue(row.GetCell(q));
                    if (current == " " || current == null)
                    {
                        if (q == 6 || q == 11)
                        {
                            row.GetCell(q).SetCellValue("-");
                            continue;
                        }

                        if (q == 7 || q == 8 || q == 9 || q == 10)
                        {
                            row.GetCell(q).SetCellValue("0");
                            continue;
                        }
                    }
                }
            }
        }

        using (FileStream fileStream = new FileStream(dir + "/" + file.FileName, FileMode.Create))
        {
            workbook.Write(fileStream, false);
        }
    }

    private void InsertToDb(int numberOfSheets, IWorkbook workbook)
    {
        DataFormatter formatter = new DataFormatter();
        for (int i = 0; i < numberOfSheets; i++)
        {
            ISheet sheet = workbook.GetSheetAt(i);
            int numberOfRows = sheet.LastRowNum;
            for (int r = 4; r < numberOfRows; r++)
            {
                try
                {
                    IRow row = sheet.GetRow(r);

                    Weather newWeather = new()
                    {
                        Date = DateOnly.Parse(formatter.FormatCellValue(row.GetCell(0))),
                        Time = TimeSpan.Parse(formatter.FormatCellValue(row.GetCell(1))),
                        Temperature = double.Parse(formatter.FormatCellValue(row.GetCell(2))),
                        Humidity = double.Parse(formatter.FormatCellValue(row.GetCell(3))),
                        DewPoint = double.Parse(formatter.FormatCellValue(row.GetCell(4))),
                        Pressure = double.Parse(formatter.FormatCellValue(row.GetCell(5))),
                        WindDirection = formatter.FormatCellValue(row.GetCell(6)),
                        WindSpeed = double.Parse(formatter.FormatCellValue(row.GetCell(7))),
                        CloudCover = double.Parse(formatter.FormatCellValue(row.GetCell(8))),
                        LBCloudCover = double.Parse(formatter.FormatCellValue(row.GetCell(9))),
                        HorizontalVisibility = double.Parse(formatter.FormatCellValue(row.GetCell(10))),
                        WeatherPhenomena = formatter.FormatCellValue(row.GetCell(11)),
                    };
                    _weatherRepository.Add(newWeather);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"================================================================================\n{ex}\n================================================================================");
                }
            }
        }
        _weatherRepository.Save();
    }


    public IActionResult SearchInfo(int selectInfo_Month, int selectInfo_Years)
    {
        var searchInfo = _weatherRepository.Search(selectInfo_Month, selectInfo_Years);
        var cur = searchInfo.First();
        int number = cur.Id / 10 + 1;        
        return RedirectToAction("List", new { pageNumber = number});
    }
}
