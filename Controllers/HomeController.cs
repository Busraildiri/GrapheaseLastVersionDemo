using Graphease.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Graphease.Extensions;

namespace Graphease.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        private decimal CleanAndParsePrice(string price)
        {
            string cleanedPrice = price.Replace(" TL", "").Trim();
            return decimal.Parse(cleanedPrice);
        }


        [HttpPost]
        public IActionResult Upload(IFormFile excelFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (excelFile == null || excelFile.Length == 0)
            {
                return View("Index");
            }

            var products = new List<Product>();

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        products.Add(new Product
                        {
                            UrunAdi = worksheet.Cells[row, 1].Text,
                            Kategori = worksheet.Cells[row, 2].Text,
                            Cinsiyet = worksheet.Cells[row, 3].Text,
                            BirimFiyati = CleanAndParsePrice(worksheet.Cells[row, 4].Text),
                            SatisFiyati = CleanAndParsePrice(worksheet.Cells[row, 5].Text),
                            Sehir = worksheet.Cells[row, 6].Text,
                            SatisAdeti = int.Parse(worksheet.Cells[row, 7].Text)
                        });
                    }
                }
            }

            HttpContext.Session.Set("Products", products);
            if (HttpContext.Session.Get<List<Product>>("Products") != null)
            {
                Console.WriteLine("Ürünler başarıyla oturuma kaydedildi.");
            }
            else
            {
                Console.WriteLine("Ürünler oturuma kaydedilemedi!");
            }

            return RedirectToAction("Index", "Analysis");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}