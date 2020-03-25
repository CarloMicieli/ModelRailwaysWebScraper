using System;

namespace WebScraper.Model
{
    public sealed class Feature : IEquatable<Feature>
    {
        public string Icon { set; get; }
        public string Label { set; get; }

        public override int GetHashCode() => HashCode.Combine(Icon, Label);

        public override bool Equals(object obj)
        {
            if (obj is Feature that)
            {
                return AreEquals(this, that);
            }

            return false;
        }

        public bool Equals(Feature other) => AreEquals(this, other);

        private static bool AreEquals(Feature left, Feature right) => 
            left.Icon == right.Icon && left.Label == right.Label;
    }
}
