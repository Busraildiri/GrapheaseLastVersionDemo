namespace Graphease.Models
{
    public class ProfitAnalysisViewModel
    {
        public List<KeyValuePair<string, decimal>>? ProfitData { get; set; }
        public List<KeyValuePair<string, decimal>>? ProfitPercentageData { get; set; }

        public List<KeyValuePair<string, int>>? SalesData { get; set; }

    }

}
