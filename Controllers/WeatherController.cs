using System;
using System.Collections.Generic;
using System.IO;
using DynamicSun.Models;
using DynamicSun.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

    public IActionResult List()
    {
        IEnumerable<Weather> weatherList = _weatherRepository.List();
        return View("~/Views/Main.cshtml", weatherList);
    }

    [HttpPost]
    public IActionResult AddArchive(IEnumerable<IFormFile> fileArchive)
    {
        string dir = _env.ContentRootPath + "\\Archive";
        foreach (IFormFile item in fileArchive)
        {
            IFormFile file = item;
            using (FileStream target = new FileStream(Path.Combine(dir, item.FileName), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(target);
            }
            IWorkbook workbook;
            using (FileStream fileStream = new FileStream(dir + "/" + file.FileName, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook((Stream)fileStream);
            }
            int numberOfSheets = workbook.NumberOfSheets;
            DataFormatter formatter = new DataFormatter();
            for (int i = 0; i < numberOfSheets; i++)
            {
                ISheet sheet = workbook.GetSheetAt(i);
                int numberOfRows = sheet.LastRowNum;
                int numberOfCell = 12;
                for (int r = 4; r < numberOfRows; r++)
                {
                    for (int c = 0; c < numberOfCell; c++)
                    {
                        IRow row = sheet.GetRow(r);
                        var cellValue = formatter.FormatCellValue(row.GetCell(c));

                        Console.Write($"{cellValue} | ");
                    }
                    Console.WriteLine();
                }
            }
        }
        return List();
    }
}
