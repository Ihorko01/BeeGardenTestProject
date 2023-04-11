using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGardenTestProject
{
    public class SettingsTests: TestBase
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
        public void LogoutAndAccessAppFeatures()
        {
            login.LoginUser(username, password);

            // Navigate to the settings page
            driver.Navigate().GoToUrl("https://localhost:3000/settings");

            // Click on the 'Log Out' button
            driver.FindElement(By.Id("logout-button")).Click();

            // Wait for the user to be logged out and redirected to the login page
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("https://localhost:3000/login"));

            // Attempt to access any app feature (e.g., add resource, view audio details)
            driver.Navigate().GoToUrl("https://localhost:3000/resource");

            // Assert that the user is redirected to the login page and cannot access the app feature
            Assert.IsTrue(driver.Url.Contains("https://localhost:3000/login"));
        }

        [Test]
        public void LogoutAndAccessAppFeaturesWithoutLoggingIn()
        {
            login.LoginUser(username, password);

            // Navigate to the settings page
            driver.Navigate().GoToUrl("https://localhost:3000/resource");

            // Click on the 'Log Out' button
            driver.FindElement(By.Id("logout-button")).Click();

            // Wait for the user to be logged out and redirected to the login page
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("https://localhost:3000/login"));

            // Attempt to access any app feature without logging in again
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Assert that the user is redirected to the login page and cannot access the app feature
            Assert.IsTrue(driver.Url.Contains("https://localhost:3000/login"));
            Assert.IsTrue(driver.PageSource.Contains("Please log in to access this feature"));

            // Attempt to access another app feature without logging in again
            driver.Navigate().GoToUrl("https://localhost:3000/view-audio-details");

            // Assert that the user is redirected to the login page and cannot access the app feature
            Assert.IsTrue(driver.Url.Contains("https://localhost:3000/login"));
        }

        [Test]
        public void LoginCreateResourcesAndDeleteAccount()
        {
            login.LoginUser(username, password);

            // Navigate to the settings page
            IWebElement settingsButton = driver.FindElement(By.CssSelector("button.settings-button"));
            settingsButton.Click();

            // Delete the account
            IWebElement deleteAccountButton = driver.FindElement(By.CssSelector(".settings-page .delete-account-button"));
            deleteAccountButton.Click();

            IAlert confirmationDialog = driver.SwitchTo().Alert();
            confirmationDialog.Accept();

            // Verify that the account has been deleted and the login page is displayed
            IWebElement loginPageTitle = driver.FindElement(By.CssSelector("h1.login-page-title"));
            Assert.AreEqual("Login to the System", loginPageTitle.Text);
        }

        [Test]
        public void LoginCreateResourcesAndLogout()
        {
            login.LoginUser(username, password);

            // Navigate to the settings page
            IWebElement settingsButton = driver.FindElement(By.CssSelector("button.settings-button"));
            settingsButton.Click();

            // Logout from the system
            IWebElement logoutButton = driver.FindElement(By.CssSelector("button.logout-button"));
            logoutButton.Click();

            // Verify that the user is logged out and the login page is displayed
            IWebElement loginPageTitle = driver.FindElement(By.CssSelector("h1.login-page-title"));
            Assert.AreEqual("Login to the System", loginPageTitle.Text);
        }
    }
}
