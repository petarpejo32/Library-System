using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace LibrarySystem.Tests.UI.Authors
{
    [TestFixture]
    public class AuthorsUITests : PageTest
    {
        private string BaseUrl = "https://localhost:7027";

        [Test]
        public async Task Test01_AuthorsIndex_ShouldDisplayPage()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors");
            await Expect(Page.Locator("h1, h2")).ToContainTextAsync("Authors");
        }

        [Test]
        public async Task Test02_AuthorsIndex_ShouldShowTable()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors");
            var tableExists = await Page.Locator("table").IsVisibleAsync();
            Assert.That(tableExists, Is.True);
        }

        [Test]
        public async Task Test03_AuthorsCreate_ShouldShowForm()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors/Create");
            await Expect(Page.Locator("form")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test04_AuthorsCreate_HasNameField()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors/Create");
            await Expect(Page.Locator("#Name, input[name='Name']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test05_AuthorsCreate_HasBiographyField()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors/Create");
            var biographyExists = await Page.Locator("#Biography, textarea[name='Biography']").CountAsync();
            Assert.That(biographyExists, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public async Task Test06_AuthorsCreate_HasSubmitButton()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors/Create");
            await Expect(Page.Locator("input[type='submit'], button[type='submit']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test07_AuthorsIndex_HasDetailsLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors");
            var detailsLink = await Page.Locator("a:has-text('Details')").CountAsync();
            Assert.That(detailsLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test08_AuthorsIndex_HasEditLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors");
            var editLink = await Page.Locator("a:has-text('Edit')").CountAsync();
            Assert.That(editLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test09_AuthorsIndex_HasDeleteLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors");
            var deleteLink = await Page.Locator("a:has-text('Delete')").CountAsync();
            Assert.That(deleteLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test10_AuthorsDetails_CanNavigate()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors");
            await Page.Locator("a:has-text('Details')").First.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*/Authors/Details/.*"));
        }

        [Test]
        public async Task Test11_AuthorsEdit_CanNavigate()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors");
            await Page.Locator("a:has-text('Edit')").First.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*/Authors/Edit/.*"));
        }

        [Test]
        public async Task Test12_AuthorsDelete_ShowsConfirmation()
        {
            await Page.GotoAsync($"{BaseUrl}/Authors");
            await Page.Locator("a:has-text('Delete')").First.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*/Authors/Delete/.*"));
        }
    }
}