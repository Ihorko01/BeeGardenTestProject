using OpenQA.Selenium;

namespace BeeGardenTestProject
{
    public class LoginTests: TestBase
    {

        [Test]
        public void LayoutVerifying()
        {

            // Navigate to the login page
            driver.Navigate().GoToUrl("http://localhost:3000/login");

            // Assert the position of the 'Heard' title
            IWebElement heardTitle = driver.FindElement(By.XPath("//h1[contains(text(),'Heard')]"));

            // Assert the position of the 'Upload photos' text
            IWebElement uploadText = driver.FindElement(By.XPath("//h2[contains(text(),'Upload photos of your book')]"));
            IWebElement uploadParent = uploadText.FindElement(By.XPath("./.."));

            // Assert the position of the 'Sign up with Google' button
            IWebElement signupButton = driver.FindElement(By.XPath("//button[contains(text(),'Sign up with Google')]"));
            IWebElement signupParent = signupButton.FindElement(By.XPath("./.."));

            // Assert the position of the 'Log in with Google' button
            IWebElement loginButton = driver.FindElement(By.XPath("//button[contains(text(),'Log in with Google')]"));
            IWebElement loginParent = loginButton.FindElement(By.XPath("./.."));
        }

        [Test]
        public void LoginEmptyErrorVerifying()
        {
            // Assert presence of page elements
            IWebElement usernameInput = driver.FindElement(By.Id("username"));
            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            // Verify page element layout
            Assert.IsTrue(usernameInput.Displayed);
            Assert.IsTrue(passwordInput.Displayed);
            Assert.IsTrue(loginButton.Displayed);

            // Perform form validation
            usernameInput.SendKeys("");
            passwordInput.SendKeys("");
            loginButton.Click();

            IWebElement errorLabel = driver.FindElement(By.Id("error-label"));
            Assert.IsTrue(errorLabel.Displayed);
            Assert.AreEqual(errorLabel.Text, "Invalid login or password.");
        }

        [Test]
        public void LoginIncorrectUserNameVerifying()
        {
            // Assert presence of page elements
            IWebElement usernameInput = driver.FindElement(By.Id("username"));
            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            // Verify page element layout
            Assert.IsTrue(usernameInput.Displayed);
            Assert.IsTrue(passwordInput.Displayed);
            Assert.IsTrue(loginButton.Displayed);

            // Perform form validation
            usernameInput.SendKeys("@#!#!90");
            passwordInput.SendKeys("pswrd_0");
            loginButton.Click();

            IWebElement errorLabel = driver.FindElement(By.Id("error-label"));
            Assert.IsTrue(errorLabel.Displayed);
            Assert.AreEqual(errorLabel.Text, "Please enter valid characters only");
        }

        [Test]
        public void LoginIncorrectPasswordVerifying()
        {
            // Assert presence of page elements
            IWebElement usernameInput = driver.FindElement(By.Id("username"));
            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("login-button"));

            // Verify page element layout
            Assert.IsTrue(usernameInput.Displayed);
            Assert.IsTrue(passwordInput.Displayed);
            Assert.IsTrue(loginButton.Displayed);

            // Perform form validation
            usernameInput.SendKeys("testuser@gmail.com");
            passwordInput.SendKeys("pswrd_0");
            loginButton.Click();

            IWebElement errorLabel = driver.FindElement(By.Id("error-label"));
            Assert.IsTrue(errorLabel.Displayed);
            Assert.AreEqual(errorLabel.Text, "Invalid login or password.");
        }
    }
}
