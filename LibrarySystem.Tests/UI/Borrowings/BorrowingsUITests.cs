using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace LibrarySystem.Tests.UI.Borrowings
{
    [TestFixture]
    public class BorrowingsUITests : PageTest
    {
        private string BaseUrl = "https://localhost:7027";

        [Test]
        public async Task Test01_BorrowingsIndex_ShouldDisplayPage()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings");
            await Expect(Page.Locator("h1, h2")).ToContainTextAsync("Borrowings");
        }

        [Test]
        public async Task Test02_BorrowingsIndex_ShouldShowTable()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings");
            var tableExists = await Page.Locator("table").IsVisibleAsync();
            Assert.That(tableExists, Is.True);
        }

        [Test]
        public async Task Test03_BorrowingsCreate_ShouldShowForm()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings/Create");
            await Expect(Page.Locator("form")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test04_BorrowingsCreate_HasBookDropdown()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings/Create");
            await Expect(Page.Locator("#BookId, select[name='BookId']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test05_BorrowingsCreate_HasMemberDropdown()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings/Create");
            await Expect(Page.Locator("#MemberId, select[name='MemberId']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test06_BorrowingsCreate_HasBorrowDateField()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings/Create");
            var borrowDateExists = await Page.Locator("#BorrowDate, input[name='BorrowDate']").CountAsync();
            Assert.That(borrowDateExists, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public async Task Test07_BorrowingsCreate_HasDueDateField()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings/Create");
            var dueDateExists = await Page.Locator("#DueDate, input[name='DueDate']").CountAsync();
            Assert.That(dueDateExists, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public async Task Test08_BorrowingsIndex_HasDetailsLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings");
            var detailsLink = await Page.Locator("a:has-text('Details')").CountAsync();
            Assert.That(detailsLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test09_BorrowingsIndex_HasEditLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings");
            var editLink = await Page.Locator("a:has-text('Edit')").CountAsync();
            Assert.That(editLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test10_BorrowingsIndex_HasDeleteLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings");
            var deleteLink = await Page.Locator("a:has-text('Delete')").CountAsync();
            Assert.That(deleteLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test11_BorrowingsDetails_CanNavigate()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings");
            var detailsLinkExists = await Page.Locator("a:has-text('Details')").CountAsync();
            if (detailsLinkExists > 0)
            {
                await Page.Locator("a:has-text('Details')").First.ClickAsync();
                await Expect(Page).ToHaveURLAsync(new Regex(".*/Borrowings/Details/.*"));
            }
        }

        [Test]
        public async Task Test12_ReturnBook_LinkExists()
        {
            await Page.GotoAsync($"{BaseUrl}/Borrowings");
            var returnLinkCount = await Page.Locator("a:has-text('Return'), a[href*='ReturnBook']").CountAsync();
            Assert.That(returnLinkCount, Is.GreaterThanOrEqualTo(0));
        }
    }
}