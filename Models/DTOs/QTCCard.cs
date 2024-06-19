namespace QuickTie.Portal.Models
{
    public class QTCCard
    {
        public string Series { get; set; }
        public string DisplayName { get; set; }
        public string DefaultImageUrl { get; set; }
        public string Feet { get; set; }
        public string Inches { get; set; }
        public List<string> FeetList { get; set; } = Enumerable.Range(6, 62).Select(i => i.ToString()).ToList();
        public List<string> InchesList { get; set; } = Enumerable.Range(0, 12).Select(i => i.ToString()).ToList();

        public QTCCard(string series, string displayName, string defaultImageUrl)
        {
            Series = series;
            DisplayName = displayName;
            DefaultImageUrl = defaultImageUrl;
            Feet = "6";
            Inches = "0";
        }

        public static List<QTCCard> GetDefaultCables()
        {
            return new List<QTCCard>
            {
                new QTCCard("blue", "QuickTie Blue Cable", "https://quicktie.s3.amazonaws.com/products/images/generic/QTB.svg"),
                new QTCCard("green", "QuickTie Green Cable", "https://quicktie.s3.amazonaws.com/products/images/generic/QTB.svg"),
                new QTCCard("orange", "QuickTie Orange Cable", "https://quicktie.s3.amazonaws.com/products/images/generic/QTB.svg"),
                new QTCCard("red", "QuickTie Red Cable", "https://quicktie.s3.amazonaws.com/products/images/generic/QTB.svg")
            };
        }
    }
}
