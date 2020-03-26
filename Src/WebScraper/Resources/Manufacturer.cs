using System;

namespace WebScraper.Resources
{
    public sealed class Manufacturer : WebResource, IEquatable<Manufacturer>
    {
        public string Name { set; get; }

        public override int GetHashCode() => HashCode.Combine(Name, ResourceUri);

        public override bool Equals(object obj)
        {
            if (obj is Manufacturer that)
            {
                return AreEquals(this, that);
            }
            return false;
        }

        public bool Equals(Manufacturer other) => AreEquals(this, other);

        private static bool AreEquals(Manufacturer left, Manufacturer right) =>
            left.ResourceUri == right.ResourceUri && left.Name == right.Name;
    }
}
