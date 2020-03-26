using System;

namespace WebScraper.Resources
{
    public abstract class WebResource : IEquatable<WebResource>
    {
        public Uri ResourceUri { set; get; }

        public bool Equals(WebResource other)
        {
            if (other is WebResource that)
            {
                return AreEquals(this, that);
            }

            return false;
        }

        private static bool AreEquals(WebResource left, WebResource right) =>
            left.ResourceUri == right.ResourceUri;
    }
}
