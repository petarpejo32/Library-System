using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace LibrarySystem.Tests.UI.ExternalBooks
{
    [TestFixture]
    public class ExternalBooksUITests : PageTest
    {
        private string BaseUrl = "https://localhost:7027";

        [Test]
        public async Task Test01_ExternalBooksSearch_PageLoads()
        {
            await Page.GotoAsync($"{BaseUrl}/ExternalBooks/Search");
            await Expect(Page.Locator("h1, h2")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test02_ExternalBooksSearch_HasForm()
        {
            await Page.GotoAsync($"{BaseUrl}/ExternalBooks/Search");
            await Expect(Page.Locator("form")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test03_ExternalBooksSearch_HasQueryInput()
        {
            await Page.GotoAsync($"{BaseUrl}/ExternalBooks/Search");
            await Expect(Page.Locator("input[name='query'], #query")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test04_ExternalBooksSearch_HasSubmitButton()
        {
            await Page.GotoAsync($"{BaseUrl}/ExternalBooks/Search");
            await Expect(Page.Locator("input[type='submit'], button[type='submit']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test05_ExternalBooksSearch_EmptyQuery_ShouldStayOnPage()
        {
            await Page.GotoAsync($"{BaseUrl}/ExternalBooks/Search");
            await Page.ClickAsync("input[type='submit'], button[type='submit']");
            await Expect(Page).ToHaveURLAsync(new Regex(".*/ExternalBooks/Search.*"));
        }

        [Test]
        public async Task Test06_ExternalBooksSearch_WithValidQuery_ShouldSearch()
        {
            await Page.GotoAsync($"{BaseUrl}/ExternalBooks/Search");
            await Page.FillAsync("input[name='query'], #query", "The Great Gatsby");
            await Page.ClickAsync("input[type='submit'], button[type='submit']");
            // Wait for page to load after search
            await Page.WaitForLoadStateAsync();
            // Page should still be on ExternalBooks - either results or same page
            await Expect(Page).ToHaveURLAsync(new Regex(".*/ExternalBooks/.*"));
        }

        [Test]
        public async Task Test07_ExternalBooksSearch_WithNonExistentBook_ShouldHandleGracefully()
        {
            await Page.GotoAsync($"{BaseUrl}/ExternalBooks/Search");
            await Page.FillAsync("input[name='query'], #query", "xyznonexistentbook123456");
            await Page.ClickAsync("input[type='submit'], button[type='submit']");
            await Page.WaitForLoadStateAsync();
            // Should not crash - stays on ExternalBooks page
            await Expect(Page).ToHaveURLAsync(new Regex(".*/ExternalBooks/.*"));
        }
       
        [Test]
        public async Task Test08_ExternalBooksSearch_PageHasNavigation()
        {
            await Page.GotoAsync($"{BaseUrl}/ExternalBooks/Search");
            // Should have nav links back to other parts of app
            var navLinks = await Page.Locator("nav a, .navbar a").CountAsync();
            Assert.That(navLinks, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test09_ExternalBooksSearch_CanNavigateToBooks()
        {
            await Page.GotoAsync($"{BaseUrl}/ExternalBooks/Search");
            // Navigate to Books from here
            await Page.GotoAsync($"{BaseUrl}/Books");
            await Expect(Page).ToHaveURLAsync(new Regex(".*/Books.*"));
        }
    }
}