using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace LibrarySystem.Tests.UI.Books
{
    [TestFixture]
    public class BooksUITests : PageTest
    {
        private string BaseUrl = "https://localhost:7027";

        [Test]
        public async Task Test01_BooksIndex_ShouldDisplayPage()
        {
            await Page.GotoAsync($"{BaseUrl}/Books");
            await Expect(Page.Locator("h1, h2")).ToContainTextAsync("Books");
        }

        [Test]
        public async Task Test02_BooksIndex_ShouldShowTable()
        {
            await Page.GotoAsync($"{BaseUrl}/Books");
            var tableExists = await Page.Locator("table").IsVisibleAsync();
            Assert.That(tableExists, Is.True);
        }

        [Test]
        public async Task Test03_BooksCreate_ShouldShowForm()
        {
            await Page.GotoAsync($"{BaseUrl}/Books/Create");
            await Expect(Page.Locator("form")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test04_BooksCreate_HasTitleField()
        {
            await Page.GotoAsync($"{BaseUrl}/Books/Create");
            await Expect(Page.Locator("#Title, input[name='Title']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test05_BooksCreate_HasISBNField()
        {
            await Page.GotoAsync($"{BaseUrl}/Books/Create");
            await Expect(Page.Locator("#ISBN, input[name='ISBN']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test06_BooksCreate_HasAuthorDropdown()
        {
            await Page.GotoAsync($"{BaseUrl}/Books/Create");
            await Expect(Page.Locator("#AuthorId, select[name='AuthorId']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test07_BooksCreate_HasSubmitButton()
        {
            await Page.GotoAsync($"{BaseUrl}/Books/Create");
            await Expect(Page.Locator("input[type='submit'], button[type='submit']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test08_BooksIndex_HasDetailsLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Books");
            var detailsLink = await Page.Locator("a:has-text('Details')").CountAsync();
            Assert.That(detailsLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test09_BooksIndex_HasEditLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Books");
            var editLink = await Page.Locator("a:has-text('Edit')").CountAsync();
            Assert.That(editLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test10_BooksIndex_HasDeleteLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Books");
            var deleteLink = await Page.Locator("a:has-text('Delete')").CountAsync();
            Assert.That(deleteLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test11_BooksDetails_CanNavigate()
        {
            await Page.GotoAsync($"{BaseUrl}/Books");
            await Page.Locator("a:has-text('Details')").First.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*/Books/Details/.*"));
        }

        [Test]
        public async Task Test12_BooksEdit_CanNavigate()
        {
            await Page.GotoAsync($"{BaseUrl}/Books");
            await Page.Locator("a:has-text('Edit')").First.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*/Books/Edit/.*"));
        }

        [Test]
        public async Task Test13_BooksDelete_ShowsConfirmation()
        {
            await Page.GotoAsync($"{BaseUrl}/Books");
            await Page.Locator("a:has-text('Delete')").First.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*/Books/Delete/.*"));
        }

        [Test]
        public async Task Test14_BooksCreate_HasPublishedYearField()
        {
            await Page.GotoAsync($"{BaseUrl}/Books/Create");
            await Expect(Page.Locator("#PublishedYear, input[name='PublishedYear']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test15_BooksCreate_HasGenreField()
        {
            await Page.GotoAsync($"{BaseUrl}/Books/Create");
            await Expect(Page.Locator("#Genre, input[name='Genre']")).ToBeVisibleAsync();
        }
    }
}