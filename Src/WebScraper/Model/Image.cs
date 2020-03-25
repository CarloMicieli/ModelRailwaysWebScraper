using System;

namespace WebScraper.Model
{
    public sealed class Image : IEquatable<Image>
    {
        public Uri ImageSrc { get; set; }
        public string AltText { get; set; }

        public override string ToString() => $"Image({ImageSrc}, {AltText})";

        public override int GetHashCode() => HashCode.Combine(ImageSrc, AltText);

        public override bool Equals(object obj)
        {
            if (obj is Image other)
            {
                return AreEquals(this, other);
            }

            return false;
        }

        public bool Equals(Image other) => AreEquals(this, other);

        private static bool AreEquals(Image left, Image right) =>
            left.ImageSrc == right.ImageSrc &&
            left.AltText == right.AltText;
    }
}
