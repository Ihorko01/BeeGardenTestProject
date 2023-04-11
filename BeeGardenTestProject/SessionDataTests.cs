using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGardenTestProject
{
    public class SessionDataTests: TestBase
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
        public void LoginCreateAndLogout()
        {
            login.LoginUser(username, password);

            // Login to the system
            Login("username", "password");

            // Perform some actions such as creating resources and adding audio
            CreateResource("Resource 1");
            AddAudio("Audio 1");

            // Logout from the system
            Logout();
        }

        public void Login(string username, string password)
        {
            login.LoginUser(username, password);

            // Navigate to the login page
            driver.Navigate().GoToUrl("https://localhost:3000/login");

            // Enter username and password
            driver.FindElement(By.Id("username")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);

            // Click on the login button
            driver.FindElement(By.Id("login-button")).Click();

            // Verify that the user is logged in
            Assert.IsTrue(driver.Url.Contains("https://localhost:3000/dashboard"));
            Assert.IsTrue(driver.Title.Contains("Dashboard"));
        }

        public void CreateResource(string resourceName)
        {
            login.LoginUser(username, password);

            // Navigate to the add resource page
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Enter the resource name
            driver.FindElement(By.Id("resource-name")).SendKeys(resourceName);

            // Click on the create button
            driver.FindElement(By.Id("create-resource-button")).Click();

            // Verify that the resource is created
            Assert.IsTrue(driver.Url.Contains("https://localhost:3000/resource"));
            Assert.IsTrue(driver.Title.Contains(resourceName));
        }

        public void AddAudio(string audioName)
        {
            login.LoginUser(username, password);

            // Navigate to the add audio page
            driver.Navigate().GoToUrl("https://localhost:3000/add-audio");

            // Enter the audio name
            driver.FindElement(By.Id("audio-name")).SendKeys(audioName);

            // Click on the upload button
            driver.FindElement(By.Id("upload-audio-button")).Click();

            // Verify that the audio is uploaded
            Assert.IsTrue(driver.Url.Contains("https://localhost:3000/audio"));
            Assert.IsTrue(driver.Title.Contains(audioName));
        }

        public void Logout()
        {
            login.LoginUser(username, password);

            // Navigate to the settings page
            driver.Navigate().GoToUrl("https://localhost:3000/settings");

            // Click on the logout button
            driver.FindElement(By.Id("logout-button")).Click();

            // Verify that the user is logged out
            Assert.IsTrue(driver.Url.Contains("https://localhost:3000/login"));
            Assert.IsTrue(driver.Title.Contains("Login"));
        }

    }
}
