using System;
using System.Threading.Tasks;
using WebScraper.Model;
using WebScraper.Resources;
using WebScraper.Resources.Collections;

namespace WebScraper.Scraping
{
    public interface IWrapper
    {
        // Returns the list of links for the manufactures
        Task<IManufacturersCollection> GetManufacturers();

        // Returns all categories for the provided manufacturer
        Task<ICategoriesCollection> GetCategories(Manufacturer manufacturer);

        // Returns all products for the provided category
        Task<IProductsCollection> GetProducts(Category category);

        // Extract the information and produce a product info DTO
        Task<ProductInfo> ExtractProductInfo(Product product);

        Uri BaseUri { get; }
        HtmlParser Parser { get; }
    }
}
