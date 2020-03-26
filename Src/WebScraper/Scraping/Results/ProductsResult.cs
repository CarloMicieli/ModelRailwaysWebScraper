using NodaTime;
using System;
using System.Collections.Immutable;
using WebScraper.Resources;
using WebScraper.Scraping.Model;

namespace WebScraper.Scraping.Results
{
    public sealed class ProductsResult : IEquatable<ProductsResult>
    {
        public Category Resource { get; }
        public Instant Timestamp { get; }
        public Pagination Pagination { get; }
        public ImmutableList<Product> Products { get; }

        public ProductsResult(Category resource, Instant timestamp, Pagination pagination, ImmutableList<Product> products)
        {
            Resource = resource;
            Timestamp = timestamp;
            Pagination = pagination;
            Products = products;
        }

        public override int GetHashCode() => HashCode.Combine(Resource, Products, Pagination);

        public override bool Equals(object obj)
        {
            if (obj is ProductsResult that)
            {
                return AreEquals(this, that);
            }
            return false;
        }

        public bool Equals(ProductsResult other) => AreEquals(this, other);

        private static bool AreEquals(ProductsResult left, ProductsResult right) =>
            left.Resource == right.Resource &&
            left.Products == right.Products &&
            left.Pagination == right.Pagination;
    }
}
