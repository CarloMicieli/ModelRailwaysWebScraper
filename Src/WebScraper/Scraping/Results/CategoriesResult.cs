using NodaTime;
using System;
using System.Collections.Immutable;
using WebScraper.Resources;

namespace WebScraper.Scraping.Results
{
    public sealed class CategoriesResult : IEquatable<CategoriesResult>
    {
        public CategoriesResult(
            Manufacturer resource,
            Instant timestamp,
            ImmutableList<Category> categories)
        {
            Resource = resource;
            Timestamp = timestamp;
            Categories = categories;
        }

        public Manufacturer Resource { get; }
        public Instant Timestamp { get; }
        public ImmutableList<Category> Categories { get; }

        public override int GetHashCode() => HashCode.Combine(Resource, Categories);

        public override bool Equals(object obj)
        {
            if (obj is CategoriesResult that)
            {
                return AreEquals(this, that);
            }
            return false;
        }

        public bool Equals(CategoriesResult other) => AreEquals(this, other);

        private static bool AreEquals(CategoriesResult left, CategoriesResult right) =>
            left.Resource == right.Resource &&
            left.Categories == right.Categories;
    }
}
