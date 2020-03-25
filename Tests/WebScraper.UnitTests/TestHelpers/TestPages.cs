﻿using System;

namespace WebScraper.UnitTests.TestHelpers
{
    public static class TestPages
    {
        public static ModellbahnshopLippePages ModellbahnshopLippe => new ModellbahnshopLippePages();
    }

    public sealed class ModellbahnshopLippePages
    {
        public Uri ProductPage => new Uri(@"http://localhost/ModellbahnshopLippe/ACME_60472.html");
    }
}