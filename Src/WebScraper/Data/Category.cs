namespace WebScraper.Data
{
    public sealed class Category
    {
        public string CategoryName { set; get; }
        public string PowerMethod { set; get; }

        public static Category Of(string cat, string sys)
        {
            return new Category
            {
                CategoryName = cat,
                PowerMethod = sys
            };
        }
    }
}
