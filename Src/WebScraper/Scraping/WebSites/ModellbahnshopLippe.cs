using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WebScraper.Resources;
using WebScraper.Scraping.Model;

namespace WebScraper.Scraping.WebSites
{
    public sealed class ModellbahnshopLippe : WrapperWithStartPage
    {
        public ModellbahnshopLippe(IWebCrawler webCrawler)
            : this(webCrawler,
                   new Uri(@"https://www.modellbahnshop-lippe.com/Manufacturer/Products/gb/hersteller.html"))
        {
        }

        public ModellbahnshopLippe(IWebCrawler webCrawler, Uri startPage)
            : base(SystemClock.Instance,
                    webCrawler,
                    new Uri(@"https://www.modellbahnshop-lippe.com"),
                    startPage)
        {
        }

        protected override ImmutableList<Manufacturer> ExtractManufacturers(IHtmlDocument html)
        {
            #region [ Extract Manufacturers ]

            var listElement = html.QuerySelector("div#HERST_LISTE");

            ImmutableList<Manufacturer> res = listElement.Children
                .Select(it => it.QuerySelector("table.wHerstUeb"))
                .Select(table => table.QuerySelector("td").FirstElementChild)
                .Select(it => new Manufacturer
                {
                    ResourceUri = CombineUrl(it.GetAttribute("href")),
                    Name = it.TextContent
                })
                .ToImmutableList();

            return res;

            #endregion
        }

        protected override ImmutableList<Category> ExtractCategories(IHtmlDocument html)
        {
            #region [ Extract Categories ]

            var element = html.QuerySelectorAll("div.container")
                .Skip(3)
                .FirstOrDefault();

            var scaleDivs = element.Children
                .Where(it => it.LocalName == "div" && it.ClassName == "row");

            IEnumerable<Category> categories = new List<Category>();
            foreach (var scaleDiv in scaleDivs)
            {
                var headerDiv = scaleDiv.QuerySelector("div.panel-primary");
                string scaleSlug = headerDiv.Children
                    .Where(it => it.LocalName == "a" && it.GetAttribute("rel") == "nofollow")
                    .First()
                    .GetAttribute("name");
                string scaleName = headerDiv.QuerySelector("div.panel-heading").TextContent;

                var left = scaleDiv.QuerySelector("div.paddAdrRg");
                if (left != null)
                {
                    var categories1 = left.QuerySelectorAll("a.aHlist")
                      .Select(it => new Category
                      {
                          CategoryName = it.TextContent,
                          PowerMethod = null,
                          ResourceUri = CombineUrl(it.GetAttribute("href")),
                          Scale = scaleName
                      });
                    categories = categories.Concat(categories1);
                }

                var right = scaleDiv.QuerySelector("div.paddAdrLf");
                if (right != null)
                {
                    var categories2 = right.QuerySelectorAll("a.aHlist")
                      .Select(it => new Category
                      {
                          CategoryName = it.TextContent,
                          PowerMethod = null,
                          ResourceUri = CombineUrl(it.GetAttribute("href")),
                          Scale = scaleName
                      });
                    categories = categories.Concat(categories2);
                }
            }

            return categories.ToImmutableList();
            #endregion
        }

        protected override (ImmutableList<Product>, ImmutableList<Page>) ExtractProducts(IHtmlDocument html)
        {
            #region [ Extract Products ]

            ImmutableList<Page> pages;
            var paginationEl = html.QuerySelector("ul.pagination");
            if (paginationEl != null)
            {
                pages = paginationEl.Children
                    .Where(it => it.LocalName == "li")
                    .Select(li => (IsActive: li.ClassName == "active", Link: li.FirstElementChild))
                    .Select(it => PageFromLinkElement(it.Link, it.IsActive))
                    .Where(it => it.IsValid)
                    .Select(it => it.Page)
                    .ToImmutableList();
            }
            else
            {
                pages = ImmutableList.Create<Page>();
            }

            var productsDiv = html.QuerySelector("div#dProduktContent");
            var productsPanel = productsDiv.QuerySelector("div.panel-body");

            var products = productsPanel.QuerySelectorAll("div.padProdTab");
            var productsList = products.Select(p =>
                {
                    var productLink = p.QuerySelector("a.grpModList").GetAttribute("href");
                    var productDesc = p.QuerySelector("div.tdProdDesc").TextContent;
                    var brand = p.QuerySelector("img.img_herst").GetAttribute("alt");
                    var info = p.QuerySelector("div.dSpur").TextContent;

                    return new Product
                    {
                        Brand = brand,
                        Description = productDesc,
                        ResourceUri = CombineUrl(productLink),
                        Info = info
                    };
                })
                .ToImmutableList();

            return (productsList, pages);

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

        private (bool IsValid, Page Page) PageFromLinkElement(IElement it, bool isActive)
        {
            string page = it.TextContent;
            string link = it.GetAttribute("href");
            bool isValid = Page.TryCreate(page, link, isActive, out var result);
            return (isValid, result);
        }
    }
}
