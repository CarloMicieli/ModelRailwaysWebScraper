using System;

namespace WebScraper.UnitTests.TestHelpers
{
    public static class TestPages
    {
        public static ModellbahnshopLippePages ModellbahnshopLippe => new ModellbahnshopLippePages();
    }

    public sealed class ModellbahnshopLippePages
    {
        public Uri ProductPage => new Uri(@"http://localhost/ModellbahnshopLippe/ACME_60472.html");

        public Uri ManufacturersPage => new Uri(@"http://localhost/ModellbahnshopLippe/Manufacturers.html");

        public Uri ManufacturerPage => new Uri(@"http://localhost/ModellbahnshopLippe/Manufacturer_ACME.html");

        public Uri ProductsList => new Uri(@"http://localhost/ModellbahnshopLippe/ACME_Electric_Locomotives.html");
    }
}
