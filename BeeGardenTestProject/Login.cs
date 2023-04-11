using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGardenTestProject
{
    public class Login: TestBase
    {
        public void LoginUser(string username, string password)
        {
            // Navigate to the login page
            driver.Navigate().GoToUrl("http://localhost:3000/login");

            // Enter username and password
            driver.FindElement(By.Id("username")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);

            // Click on the login button
            driver.FindElement(By.Id("login-button")).Click();
        }
    }
}
