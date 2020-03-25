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

            ImmutableHashSet<string> categories = null;
            string brandName = null;
            string description = null;
            var header = html.QuerySelector("div.page-header");
            if (header != null)
            {
                var el = header.Children
                    .FirstOrDefault(it => it.LocalName == "h1");
                
                description = el.Children.FirstOrDefault(it => it.LocalName == "small")
                    .TextContent;
                brandName = el.TextContent.Replace(description, "").Trim();

                var catEl = header.Children
                    .FirstOrDefault(it => it.LocalName == "span" && it.ClassName == "textdeco");

                categories = catEl.Children
                    .Select(it => it.TextContent.Trim())
                    .ToImmutableHashSet();
            }

            var features = ImmutableList.Create<Feature>();
            var featuresBox = html.QuerySelector("div.padModInh");
            if (featuresBox != null)
            {
                features = featuresBox.Children
                    .Select(div => div.FirstElementChild)
                    .Select(img => new Feature
                    {
                        Label = img.GetAttribute("alt"),
                        Icon = img.GetAttribute("src")
                    })
                    .ToImmutableList();
            }

            var specificationsBox = html.QuerySelector("h4.panel-title")
                    .ParentElement
                    .ParentElement;

            var children = specificationsBox.Children
                    .Where(it => it.ClassName == "panel-body")
                    .SelectMany(it => it.Children)
                    .ToList();

            var longDescription = children.FirstOrDefault()?.TextContent;

            var specifications = children.LastOrDefault().QuerySelectorAll("tr")
                .Select(row =>
                {
                    var cols = row.Children.Select(c => c.TextContent).ToList();
                    return new
                    {
                        Label = cols.FirstOrDefault(),
                        Value = cols.LastOrDefault()
                    };
                })
                .Where(it => string.IsNullOrWhiteSpace(it.Label) == false)
                .ToImmutableDictionary(it => it.Label, it => it.Value);

            return new ProductInfo
            {
                Title = title,
                Metadata = metadata,
                Images = images,
                Brand = brandName,
                Description = description,
                LongDescription = longDescription,
                Features = features,
                Specifications = specifications,
                Categories = categories
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
