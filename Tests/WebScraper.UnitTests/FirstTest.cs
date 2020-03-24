using Xunit;
using FluentAssertions;
using WebScraper.UnitTests.Scraping.Testing;

namespace WebScraper.UnitTests
{
    public class FirstTest
    {
        [Fact]
        public async System.Threading.Tasks.Task Test1()
        {
            var source = new FakeWebCrawler();
            var page = await source.FetchResource(@"prova.html");

            page.Should().NotBeNull();
        }
    }
}
