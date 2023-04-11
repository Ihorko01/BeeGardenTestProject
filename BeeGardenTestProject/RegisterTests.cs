using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGardenTestProject
{
    public class RegisterTests : TestBase
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
        public void ValidGoogleAccountTests()
        {
            login.LoginUser(username, password);

            // Click the 'Log in with Google' button
            IWebElement loginButton = driver.FindElement(By.XPath("//button[contains(text(),'Log in with Google')]"));
            loginButton.Click();

            // Switch to the Google sign-in window
            string currentWindow = driver.CurrentWindowHandle;
            foreach (string windowHandle in driver.WindowHandles)
            {
                if (windowHandle != currentWindow)
                {
                    driver.SwitchTo().Window(windowHandle);
                    break;
                }
            }

            // Enter valid Google account credentials
            IWebElement emailInput = driver.FindElement(By.CssSelector("input[type='email']"));
            emailInput.SendKeys("ihor@gmail.com");
            IWebElement nextButton = driver.FindElement(By.CssSelector("#identifierNext button"));
            nextButton.Click();
            IWebElement passwordInput = driver.FindElement(By.CssSelector("input[type='password']"));
            passwordInput.SendKeys("123password");
            IWebElement signInButton = driver.FindElement(By.CssSelector("#passwordNext button"));
            signInButton.Click();

            // Authorize the application to access your Google account
            IWebElement authorizeButton = driver.FindElement(By.CssSelector("#submit_approve_access button"));
            authorizeButton.Click();

            // Switch back to the original window
            driver.SwitchTo().Window(currentWindow);

            // Assert that the user is logged in
            IWebElement welcomeMessage = driver.FindElement(By.CssSelector("h1.welcome-message"));
            Assert.AreEqual("Welcome, ihor@gmail.com!", welcomeMessage.Text);

        }

        [Test]
        public void InvalidGoogleAccountTests()
        {
            login.LoginUser(username, password);

            // Click the 'Sign up with Google' button
            IWebElement signupButton = driver.FindElement(By.XPath("//button[contains(text(),'Sign up with Google')]"));
            signupButton.Click();

            // Switch to the Google sign-in window
            string currentWindow = driver.CurrentWindowHandle;
            foreach (string windowHandle in driver.WindowHandles)
            {
                if (windowHandle != currentWindow)
                {
                    driver.SwitchTo().Window(windowHandle);
                    break;
                }
            }

            // Enter invalid Google account credentials
            IWebElement emailInput = driver.FindElement(By.CssSelector("input[type='email']"));
            emailInput.SendKeys("invalidemail");
            IWebElement nextButton = driver.FindElement(By.CssSelector("#identifierNext button"));
            nextButton.Click();
            IWebElement passwordInput = driver.FindElement(By.CssSelector("input[type='password']"));
            passwordInput.SendKeys("invalidpassword");
            IWebElement signInButton = driver.FindElement(By.CssSelector("#passwordNext button"));
            signInButton.Click();

            // Assert that an error message is displayed
            IWebElement errorMessage = driver.FindElement(By.CssSelector("#view_container div[role='alert']"));
            Assert.IsTrue(errorMessage.Displayed);
            Assert.AreEqual(errorMessage.Text, "Invalid credentials");
        }

        [Test]
        public void SignUpTest()
        {
            login.LoginUser(username, password);

            // Click the 'Sign up' button
            IWebElement signupButton = driver.FindElement(By.XPath("//button[contains(text(),'Sign up')]"));
            signupButton.Click();

            // Enter valid credentials to sign up
            IWebElement emailInput = driver.FindElement(By.CssSelector("#signup-form input[name='email']"));
            emailInput.SendKeys("example@gmail.com");
            IWebElement passwordInput = driver.FindElement(By.CssSelector("#signup-form input[name='password']"));
            passwordInput.SendKeys("examplepassword");
            IWebElement signupSubmitButton = driver.FindElement(By.CssSelector("#signup-form button[type='submit']"));
            signupSubmitButton.Click();

            // Assert that the 'Resources' page is opened with the expected elements
            IWebElement resourcesTitle = driver.FindElement(By.CssSelector("h1.resources-title"));
            Assert.AreEqual("Resources", resourcesTitle.Text);
            IWebElement addResourceIcon = driver.FindElement(By.CssSelector("div.resources-header i.fa-plus-circle"));
            Assert.IsTrue(addResourceIcon.Displayed);
            IWebElement noDataPlaceholder = driver.FindElement(By.CssSelector("div.resources-list div.no-data"));
            Assert.AreEqual("No data", noDataPlaceholder.Text);
            IWebElement fixedFooter = driver.FindElement(By.CssSelector("footer.fixed-footer"));
            Assert.IsTrue(fixedFooter.Displayed);
        }
    }

}

