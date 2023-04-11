using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGardenTestProject
{
    public class FooterNavigationTests: TestBase
    {
        Login login;
        string username = "testuser@gmail.com";
        string password = "Password!1";
        [SetUp]
        public void Setup()
        {
            login = new Login();
        }

        [Test]
        public void NavigateToResourcesPage()
        {
            login.LoginUser(username, password);

            // Navigate to any page of the app
            driver.Navigate().GoToUrl("https://localhost:3000/page");

            // Click on the 'Resources' tab from the footer
            driver.FindElement(By.Id("resources-tab")).Click();

            // Verify that the user is navigated to the 'Resources' page
            Assert.IsTrue(driver.Url.Contains("https://localhost:3000/resources"));
            Assert.IsTrue(driver.Title.Contains("Resources"));
        }

        [Test]
        public void NavigateToAddResourcePage()
        {
            login.LoginUser(username, password);

            // Navigate to any page of the app
            driver.Navigate().GoToUrl("https://localhost:3000/resource");

            // Click on the 'Add Resource' tab from the footer
            driver.FindElement(By.Id("add-resource-tab")).Click();

            // Verify that the user is navigated to the 'Add Resource' page
            Assert.IsTrue(driver.Url.Contains("https://localhost:3000/add-resource"));
            Assert.IsTrue(driver.Title.Contains("Add Resource"));
        }

        [Test]
        public void NavigateToSettingPage()
        {
            login.LoginUser(username, password);

            // Navigate to any page of the app
            driver.Navigate().GoToUrl("https://localhost:3000/page");

            // Click on the 'Settings' tab from the footer
            driver.FindElement(By.Id("settings-tab")).Click();

            // Verify that the user is navigated to the 'Resources' page
            Assert.IsTrue(driver.Url.Contains("https://localhost:3000/settings"));
            Assert.IsTrue(driver.Title.Contains("Settings"));
        }
    }
}
