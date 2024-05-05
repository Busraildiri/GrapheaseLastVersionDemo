using Graphease.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Text.Json;
using Graphease.Extensions;



namespace Graphease.Controllers
{
    public class AnalysisController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProfitByCategory()
        {
            try
            {
                Console.WriteLine("SalesByCategory başladı.");
                List<Product> products = HttpContext.Session.Get<List<Product>>("Products");

                if (products == null || !products.Any())
                {
                    TempData["Error"] = "Önce bir Excel dosyası yüklemelisiniz.";
                    Console.WriteLine("Ana sayfaya yönlendiriliyor.");
                    return RedirectToAction("Index");
                }
 
                var ProfitByCategory = CalculateProfitByCategory(products);

                var viewModel = new ProfitAnalysisViewModel
                {
                    ProfitData = CalculateProfitByCategory(products),
                    ProfitPercentageData = CalculateProfitPercentageByCategory(products) 
                };

                Console.WriteLine("SalesByCategory bitti.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["Error"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Index");
            }
        }

        private List<KeyValuePair<string, decimal>> CalculateProfitPercentageByCategory(List<Product> products)
        {
            var totalProfit = products.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti);
            return products.GroupBy(p => p.Kategori)
                            .Select(g => new KeyValuePair<string, decimal>(g.Key, (g.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti) / totalProfit) * 100m))
                           .ToList();
        }


        private List<KeyValuePair<string, decimal>> CalculateProfitByCategory(List<Product> products)
        {
            var profitByProduct = products
                .GroupBy(p => new { p.UrunAdi, p.Kategori })
                .Select(group => new
                {
                    Key = group.Key.Kategori,
                    ProductName = group.Key.UrunAdi,
                    Profit = group.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti)
                }).ToList();

            return profitByProduct
                .GroupBy(p => p.Key)
                .Select(group => new KeyValuePair<string, decimal>(
                    group.Key,
                    group.Sum(p => p.Profit)))
                .ToList();
        }


        public IActionResult ProfitByCity()
        {
            try
            {
                Console.WriteLine("ProfitByCity başladı.");

                List<Product> products = HttpContext.Session.Get<List<Product>>("Products");

                if (products == null || !products.Any())
                {
                    TempData["Error"] = "Önce bir Excel dosyası yüklemelisiniz.";
                    Console.WriteLine("Ana sayfaya yönlendiriliyor.");
                    return RedirectToAction("Index");
                }

                var ProfitByCity = CalculateProfitByCity(products);

                var viewModel = new ProfitAnalysisViewModel
                {
                    ProfitData = CalculateProfitByCity(products),
                    ProfitPercentageData = CalculateProfitPercentageByCity(products) 
                };

                Console.WriteLine("ProfitByCity bitti.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["Error"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Index");
            }
        }

        private List<KeyValuePair<string, decimal>> CalculateProfitPercentageByCity(List<Product> products)
        {
            var totalProfit = products.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti);
            return products.GroupBy(p => p.Sehir)
                           .Select(g => new KeyValuePair<string, decimal>(g.Key, (g.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti) / totalProfit) * 100m))
                           .ToList();
        }

        private List<KeyValuePair<string, decimal>> CalculateProfitByCity(List<Product> products)
        {
            var profitByProduct = products
                .GroupBy(p => new { p.UrunAdi, p.Sehir })
                .Select(group => new
                {
                    Key = group.Key.Sehir,
                    ProductName = group.Key.UrunAdi,
                    Profit = group.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti)
                }).ToList();

            return profitByProduct
                .GroupBy(p => p.Key)
                .Select(group => new KeyValuePair<string, decimal>(
                    group.Key,
                    group.Sum(p => p.Profit)))
                .ToList();
        }



        public IActionResult ProfitByGender()
        {
            try
            {

                List<Product> products = HttpContext.Session.Get<List<Product>>("Products");

                if (products == null || !products.Any())
                {
                    TempData["Error"] = "Önce bir Excel dosyası yüklemelisiniz.";
                    Console.WriteLine("Ana sayfaya yönlendiriliyor.");
                    return RedirectToAction("Index");
                }

                var ProfitByGender = CalculateProfitByGender(products);

       
                var viewModel = new ProfitAnalysisViewModel
                {
                    ProfitData = CalculateProfitByGender(products),
                    ProfitPercentageData = CalculateProfitPercentageByGender(products) 
                };

                Console.WriteLine("ProfitByGender bitti.");

        
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["Error"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Index");
            }
        }

        private List<KeyValuePair<string, decimal>> CalculateProfitPercentageByGender(List<Product> products)
        {
            var totalProfit = products.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti);
            return products.GroupBy(p => p.Cinsiyet) // Kategori yerine Cinsiyet kullandık.
                            .Select(g => new KeyValuePair<string, decimal>(g.Key, (g.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti) / totalProfit) * 100m))
                           .ToList();
        }

        private List<KeyValuePair<string, decimal>> CalculateProfitByGender(List<Product> products)
        {
            var profitByProduct = products
                .GroupBy(p => new { p.UrunAdi, p.Cinsiyet }) 
                .Select(group => new
                {
                    Key = group.Key.Cinsiyet, 
                    ProductName = group.Key.UrunAdi,
                    Profit = group.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti)
                }).ToList();

            return profitByProduct
                .GroupBy(p => p.Key)
                .Select(group => new KeyValuePair<string, decimal>(
                    group.Key,
                    group.Sum(p => p.Profit)))
                .ToList();
        }


        public IActionResult ProfitMarginAnalysis()
        {
            try
            {
                Console.WriteLine("ProfitMarginAnalysis başladı.");

                List<Product> products = HttpContext.Session.Get<List<Product>>("Products");

                if (products == null || !products.Any())
                {
                    TempData["Error"] = "Önce bir Excel dosyası yüklemelisiniz.";
                    Console.WriteLine("Ana sayfaya yönlendiriliyor.");
                    return RedirectToAction("Index");
                }

                var ProfitMarginByProduct = CalculateProfitMarginByProduct(products);

                var viewModel = new ProfitAnalysisViewModel
                {
                    ProfitData = ProfitMarginByProduct,
                    ProfitPercentageData = CalculateProfitMarginPercentageByProduct(products)
                };

                Console.WriteLine("ProfitMarginAnalysis bitti.");

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["Error"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Index");
            }
        }
        private List<KeyValuePair<string, decimal>> CalculateProfitMarginByProduct(List<Product> products)
{
    return products.GroupBy(p => p.UrunAdi)
                   .Select(group => new KeyValuePair<string, decimal>(
                       group.Key,
                       group.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti)))
                   .OrderByDescending(pair => pair.Value)
                   .ToList();
}

private List<KeyValuePair<string, decimal>> CalculateProfitMarginPercentageByProduct(List<Product> products)
{
    var totalProfit = products.Sum(p => (p.SatisFiyati - p.BirimFiyati) * p.SatisAdeti);
    var profitByProduct = CalculateProfitMarginByProduct(products);

    return profitByProduct.Select(p => new KeyValuePair<string, decimal>(
                       p.Key,
                       (p.Value / totalProfit) * 100m))
                   .ToList();
}



        public IActionResult TopSellingProducts()
        {
            try
            {
                Console.WriteLine("TopSellingProducts başladı.");

                List<Product> products = HttpContext.Session.Get<List<Product>>("Products");

                if (products == null || !products.Any())
                {
                    TempData["Error"] = "Önce bir Excel dosyası yüklemelisiniz.";
                    Console.WriteLine("Ana sayfaya yönlendiriliyor.");
                    return RedirectToAction("Index");
                }

                var viewModel = new ProfitAnalysisViewModel
                {
                    SalesData = CalculateSalesByProductName(products),
                    ProfitPercentageData = CalculateSalesPercentageByProductName(products)
                };


                Console.WriteLine("TopSellingProducts bitti.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                TempData["Error"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Index");
            }
        }

        private List<KeyValuePair<string, decimal>> CalculateSalesPercentageByProductName(List<Product> products)
        {
            var totalSales = products.Sum(p => p.SatisAdeti);
            return products.GroupBy(p => p.UrunAdi)
                           .Select(g => new KeyValuePair<string, decimal>(g.Key, (g.Sum(p => p.SatisAdeti) / (decimal)totalSales) * 100m))
                           .ToList();
        }

        private List<KeyValuePair<string, int>> CalculateSalesByProductName(List<Product> products)
        {
            return products.GroupBy(p => p.UrunAdi)
                           .Select(g => new KeyValuePair<string, int>(g.Key, g.Sum(p => p.SatisAdeti)))
                           .ToList();
        }



    }

}
