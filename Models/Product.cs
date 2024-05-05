namespace Graphease.Models
{
    public class Product
    {
        public string UrunAdi { get; set; }
        public string Kategori { get; set; }
        public string Cinsiyet { get; set; }
        public decimal BirimFiyati { get; set; }
        public decimal SatisFiyati { get; set; }
        public string Sehir { get; set; }
        public int SatisAdeti { get; set; }
        public decimal Kar => (SatisFiyati - BirimFiyati) * SatisAdeti;
    }
    public class RenderCardViewModel
    {
        public string Action { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string ColorClass { get; set; }
    }

}
