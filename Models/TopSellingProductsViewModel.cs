namespace Graphease.Models
{
    public class TopSellingProductsViewModel
    {
        public List<KeyValuePair<string, decimal>>? TopSellingProductsData { get; set; }
        public List<KeyValuePair<string, decimal>> ProfitPercentageData { get; set; } = new List<KeyValuePair<string, decimal>>();
        public List<KeyValuePair<string, int>>? SalesData { get; set; }

    }
}

