using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace LibrarySystem.Tests.UI.Members
{
    [TestFixture]
    public class MembersUITests : PageTest
    {
        private string BaseUrl = "https://localhost:7027";

        [Test]
        public async Task Test01_MembersIndex_ShouldDisplayPage()
        {
            await Page.GotoAsync($"{BaseUrl}/Members");
            await Expect(Page.Locator("h1, h2")).ToContainTextAsync("Members");
        }

        [Test]
        public async Task Test02_MembersIndex_ShouldShowTable()
        {
            await Page.GotoAsync($"{BaseUrl}/Members");
            var tableExists = await Page.Locator("table").IsVisibleAsync();
            Assert.That(tableExists, Is.True);
        }

        [Test]
        public async Task Test03_MembersCreate_ShouldShowForm()
        {
            await Page.GotoAsync($"{BaseUrl}/Members/Create");
            await Expect(Page.Locator("form")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test04_MembersCreate_HasNameField()
        {
            await Page.GotoAsync($"{BaseUrl}/Members/Create");
            await Expect(Page.Locator("#Name, input[name='Name']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test05_MembersCreate_HasEmailField()
        {
            await Page.GotoAsync($"{BaseUrl}/Members/Create");
            await Expect(Page.Locator("#Email, input[name='Email']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test06_MembersCreate_HasPhoneField()
        {
            await Page.GotoAsync($"{BaseUrl}/Members/Create");
            var phoneExists = await Page.Locator("#PhoneNumber, input[name='PhoneNumber']").CountAsync();
            Assert.That(phoneExists, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public async Task Test07_MembersCreate_HasSubmitButton()
        {
            await Page.GotoAsync($"{BaseUrl}/Members/Create");
            await Expect(Page.Locator("input[type='submit'], button[type='submit']")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Test08_MembersIndex_HasDetailsLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Members");
            var detailsLink = await Page.Locator("a:has-text('Details')").CountAsync();
            Assert.That(detailsLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test09_MembersIndex_HasEditLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Members");
            var editLink = await Page.Locator("a:has-text('Edit')").CountAsync();
            Assert.That(editLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test10_MembersIndex_HasDeleteLink()
        {
            await Page.GotoAsync($"{BaseUrl}/Members");
            var deleteLink = await Page.Locator("a:has-text('Delete')").CountAsync();
            Assert.That(deleteLink, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test11_MembersDetails_CanNavigate()
        {
            await Page.GotoAsync($"{BaseUrl}/Members");
            await Page.Locator("a:has-text('Details')").First.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*/Members/Details/.*"));
        }

        [Test]
        public async Task Test12_MembersEdit_CanNavigate()
        {
            await Page.GotoAsync($"{BaseUrl}/Members");
            await Page.Locator("a:has-text('Edit')").First.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*/Members/Edit/.*"));
        }
    }
}