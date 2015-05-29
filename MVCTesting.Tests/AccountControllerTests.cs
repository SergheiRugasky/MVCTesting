using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ASP;
using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MVCTesting.Controllers;
using MVCTesting.Models;
using NUnit.Framework;
using OpenQA.Selenium;
using RazorGenerator.Testing;
using Rhino.Mocks;
using TestStack.FluentMVCTesting;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.WebServers;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace MVCTesting.Tests
{
    [TestFixture]
    class AccountControllerTests
    {

        public AccountController CreateSUT()
        {
            return new AccountController();
        }

        [Test]
        public void Should_display_login()
        {
            var accountController = CreateSUT();


            accountController.WithCallTo(x => x.Login(string.Empty)).ShouldRenderDefaultView();
            //var actionResult = accountController.Login(string.Empty) as ViewResult;

            //actionResult.ViewName.Should().Be("Login");
        }

        [Test]
        public void Should_post_to_login()
        {
            var accountController = CreateSUT();
            //ApplicationUserManager userManager, IAuthenticationManager authenticationManager
            var userStore = MockRepository.GenerateMock<IUserStore<ApplicationUser>>();
            var authenticationManager = MockRepository.GenerateMock<IAuthenticationManager>();
            var userManager = MockRepository.GenerateStub<ApplicationUserManager>(userStore);
            var applicationSignInManager = MockRepository.GenerateStub<ApplicationSignInManager>(userManager, authenticationManager);
            LoginViewModel model = new LoginViewModel
            {
                Email = "xxx@a.com",
                Password = "Test",
            };
            applicationSignInManager.Stub(
                x => x.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false))
                .Return(Task.FromResult(SignInStatus.Success));
            string returnUrl = "xxx";
            accountController.SignInManager = applicationSignInManager;
            accountController.Url = MockRepository.GenerateMock<UrlHelper>();

            accountController.WithCallTo(x => x.Login(model, returnUrl)).ShouldRedirectTo<HomeController>(x=>x.Index());
            //var actionResult = accountController.Login(string.Empty) as ViewResult;

            //actionResult.ViewName.Should().Be("Login");
        }

        [Test]
        public void ShouldGenerateView()
        {
            var sut = new _Views_Account_Login_cshtml();
            var model = new LoginViewModel();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            //httpContext.Stub(x=>x.)
            var html = sut.RenderAsHtml(httpContext,model);

        }


        [Test]
        public void UseSeleno()
        {
            var selenoHost = BrowserHost.Instance;
            selenoHost.Application.Browser.Navigate().GoToUrl(BrowserHost.RootUrl + @"MVCTesting/Account/Login");

            IWebElement email = selenoHost.Application.Browser.FindElement(By.Name("Email"));
            email.SendKeys("adrianboboc2003@yahoo.com");

            IWebElement password = selenoHost.Application.Browser.FindElement(By.Name("Password"));
            password.SendKeys("Adrianb_13");

            Thread.Sleep(TimeSpan.FromSeconds(2));

            IWebElement submit = selenoHost.Application.Browser.FindElement(By.ClassName("btn"));
            submit.Click();

            Thread.Sleep(TimeSpan.FromSeconds(5));

            selenoHost.Application.Browser.Url.Should().Be(BrowserHost.RootUrl);


        }

        public static class BrowserHost
        {
            public static readonly SelenoHost Instance = new SelenoHost();
            public static readonly string RootUrl;

            static BrowserHost()
            {
                Instance.Run(
                    configure =>
                        configure.ProjectToTest(
                            new WebApplication(
                                ProjectLocation.FromPath(
                                    @"G:\Projects\GitProjects\EntityFramework\MVCTesting\MVCTesting"), 1234)));
                RootUrl = Instance.Application.Browser.Url;
            }
        }
    }
}
