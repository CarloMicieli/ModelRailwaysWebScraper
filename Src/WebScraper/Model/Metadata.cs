using System;

namespace WebScraper.Model
{
    public sealed class Metadata : IEquatable<Metadata>
    {
        public string Name { set; get; }
        public string Content { set; get; }

        public override string ToString() => $"Metadata({Name}, {Content})";

        public override bool Equals(object obj)
        {
            if (obj is Metadata other)
            {
                return AreEquals(this, other);
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Name, Content);

        public bool Equals(Metadata other) => AreEquals(this, other);

        private static bool AreEquals(Metadata left, Metadata right) =>
            left.Name == right.Name &&
            left.Content == right.Content;
    }
}
