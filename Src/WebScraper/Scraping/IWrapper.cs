using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using WebScraper.Resources;
using WebScraper.Scraping.Model;
using WebScraper.Scraping.Results;

namespace WebScraper.Scraping
{
    public interface IWrapper
    {
        // Returns the list of links for the manufactures
        Task<ImmutableList<Manufacturer>> GetManufacturers();

        // Returns all categories for the provided manufacturer
        Task<CategoriesResult> GetCategories(Manufacturer manufacturer);

        // Returns all products for the provided category
        Task<ProductsResult> GetProducts(Category category);

        // Extract the information and produce a product info DTO
        Task<ProductInfo> ExtractProductInfo(Product product);

        Uri BaseUri { get; }
        HtmlParser Parser { get; }
    }
}
