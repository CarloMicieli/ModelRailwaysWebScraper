using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Flurl;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WebScraper.Model;
using WebScraper.Resources.Collections;

namespace WebScraper.Scraping.WebSites
{
    public sealed class ModellbahnshopLippe : AbstractWrapper
    {
        public ModellbahnshopLippe(IWebCrawler webCrawler) 
            : base(webCrawler, 
                  new Uri(@"https://www.modellbahnshop-lippe.com"),
                  new Uri(@"https://www.modellbahnshop-lippe.com/Manufacturer/Products/gb/hersteller.html"))
        {
        }

        protected override IManufacturersCollection ExtractManufacturers(IHtmlDocument html)
        {
            #region [ Extract Manufacturers ]
            throw new System.NotImplementedException();
            #endregion
        }

        protected override ICategoriesCollection ExtractCategories(IHtmlDocument html)
        {
            #region [ Extract Categories ]
            throw new System.NotImplementedException();
            #endregion
        }

        protected override IProductsCollection ExtractProducts(IHtmlDocument html)
        {
            #region [ Extract Products ]
            throw new System.NotImplementedException();
            #endregion
        }

        protected override ProductInfo ExtractProductInfo(IHtmlDocument html)
        {
            #region [ Extract Product Info ]

            var title = html.Title;
            var metadata = html.QuerySelectorAll("meta")
                .Where(meta => meta.Attributes.Any(it => it.Name == "name") && meta.Attributes.Any(it => it.Name == "content"))
                .Select(it => new Metadata
                {
                    Name = it.GetAttribute("name"),
                    Content = it.GetAttribute("content")
                })
                .ToImmutableList();

            var imageElement = html.QuerySelector("span#zoomWrap");
            var images = imageElement.Children
                .Where(it => it.LocalName == "img")
                .SelectMany(ExtractImages)
                .ToImmutableList();

            return new ProductInfo
            {
                Title = title,
                Metadata = metadata,
                Images = images
            };
            #endregion
        }

        private IEnumerable<Image> ExtractImages(IElement img)
        {
            return new List<Image>
            {
                new Image
                {
                    AltText = img.GetAttribute("alt"),
                    ImageSrc = CombineUrl(img.GetAttribute("src"))
                },
                new Image
                {
                    AltText = img.GetAttribute("alt"),
                    ImageSrc = CombineUrl(img.GetAttribute("data-zoom-image"))
                }
            };
        }
    }
}
