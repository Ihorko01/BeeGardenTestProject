using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGardenTestProject
{
    public class AudioDetailsTests: TestBase
    {
        WebDriverWait wait;
        Login login;
        string username = "testuser@gmail.com";
        string password = "Password!1";

        [SetUp]
        public void Setup()
        {
            login = new Login();
        }
        [Test]
        public void TestAudioDetailsPageDisplaysCorrectDateTimeFormat()
        {
            login.LoginUser(username, password);

            // navigate to the audio details page for the uploaded audio
            driver.Navigate().GoToUrl("https://localhost:3000/audio/1234");

            // verify that the uploaded date is displayed in the format "dd/mm/yyyy"
            string expectedDateFormat = "dd/MM/yyyy";
            string actualDateFormat = driver.FindElement(By.ClassName("uploaded-date")).Text;

            Assert.AreEqual(expectedDateFormat, actualDateFormat, "The uploaded date is not displayed in the correct format.");

            // verify that the uploaded time is displayed in the format "hh:mm:ss AM/PM"
            string expectedTimeFormat = "hh:mm:ss tt";
            string actualTimeFormat = driver.FindElement(By.ClassName("uploaded-time")).Text;

            Assert.AreEqual(expectedTimeFormat, actualTimeFormat, "The uploaded time is not displayed in the correct format.");
        }

        [Test]
        public void TestImageIsDispalyedOnAudioPage()
        {
            login.LoginUser(username, password);

            // Navigate to the audio details page for the uploaded audio
            driver.Navigate().GoToUrl("https://localhost:3000/audio/1234");

            // Verify that the source image is displayed on the page
            IWebElement sourceImage = driver.FindElement(By.CssSelector(".source-image"));
            Assert.IsTrue(sourceImage.Displayed);

            // Click on the source image to open it in a new tab
            string sourceImageUrl = sourceImage.GetAttribute("src");
            Actions actions = new Actions(driver);
            actions.MoveToElement(sourceImage).ContextClick().Build().Perform();
            actions.SendKeys(Keys.ArrowDown).SendKeys(Keys.ArrowDown).SendKeys(Keys.Enter).Build().Perform();

            // Switch to the new tab and verify that the image is displayed
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            Assert.AreEqual("https://localhost:3000/source-images/abcd.jpg", driver.Url);
            IWebElement image = driver.FindElement(By.TagName("img"));
            Assert.IsTrue(image.Displayed);
        }

        [Test]
        public void TestAudioPlaybackAndStopping()
        {
            login.LoginUser(username, password);

            // Navigate to the audio details page for the uploaded audio
            driver.Navigate().GoToUrl("http://localhost:3000/audio");

            // Find the play/stop icon and click it to start playing the audio
            var playStopButton = driver.FindElement(By.Id("play-stop-button"));
            playStopButton.Click();

            // Wait for the audio to start playing
            System.Threading.Thread.Sleep(2000); // Replace with appropriate wait method

            // Click the play/stop icon again to stop the audio
            playStopButton.Click();

            // Assert that the audio has stopped playing
            var isPlaying = playStopButton.GetAttribute("class").Contains("playing");
            Assert.IsFalse(isPlaying, "Audio is still playing after clicking the play/stop button again");
        }

        [Test]
        public void TestAudioEditingAndRegeneration()
        {
            login.LoginUser(username, password);

            // Navigate to the audio details page for the uploaded audio
            driver.Navigate().GoToUrl("http://localhost:3000/audio");

            // Find the Edit button next to the generated text and click it
            var editButton = driver.FindElement(By.Id("edit-button"));
            editButton.Click();

            // Wait for the text editor to become visible and enter new text
            var textEditor = driver.FindElement(By.Id("text-editor"));
            Thread.Sleep(1000); // Replace with appropriate wait method
            textEditor.Clear();
            textEditor.SendKeys("New text for audio generation");

            // Click the Regenerate Audio button to generate new audio with the edited text
            var regenerateButton = driver.FindElement(By.Id("regenerate-audio-button"));
            regenerateButton.Click();

            // Wait for the new audio to be generated
            Thread.Sleep(5000); // Replace with appropriate wait method

            // Click the play/stop icon to play the new audio
            var playStopButton = driver.FindElement(By.Id("play-stop-button"));
            playStopButton.Click();

            // Assert that the new audio is playing
            var isPlaying = playStopButton.GetAttribute("class").Contains("playing");
            Assert.IsTrue(isPlaying, "New audio is not playing after clicking the play/stop button");
        }

        [Test]
        public void TestAudioPlayerBehavior()
        {
            login.LoginUser(username, password);

            // Navigate to the page with the audio player
            driver.Navigate().GoToUrl("http://localhost:3000/audio");

            // Find the play button in the audio player and click it
            var playButton = driver.FindElement(By.Id("play-button"));
            playButton.Click();

            // Wait for a few seconds
            Thread.Sleep(5000); // Replace with appropriate wait method

            // Find the pause button in the audio player and click it
            var pauseButton = driver.FindElement(By.Id("play-stop-button"));
            pauseButton.Click();

            // Find the play button again and click it
            playButton = driver.FindElement(By.Id("play-stop-button"));
            playButton.Click();

            // Wait for the audio to start playing again
            Thread.Sleep(2000); // Replace with appropriate wait method

            // Assert that the audio is playing
            var isPlaying = playButton.GetAttribute("class").Contains("playing");
            Assert.IsTrue(isPlaying, "Audio is not playing after clicking the play button again");
        }

        [Test]
        public void TestAudioPlayerSpeedx1_5()
        {
            login.LoginUser(username, password);

            // Navigate to the page with the audio player
            driver.Navigate().GoToUrl("http://localhost:3000/audio-player");

            // Find the play button in the audio player and click it
            var playButton = driver.FindElement(By.Id("play-stop-button"));
            playButton.Click();

            // Find the speed dropdown in the audio player and click it
            var speedDropdown = driver.FindElement(By.Id("speed-dropdown"));
            speedDropdown.Click();

            // Find the option for 1.5x speed and click it
            var speed15xOption = driver.FindElement(By.XPath("//option[@value='1.5']"));
            speed15xOption.Click();

            // Wait for the audio to start playing at the new speed
            Thread.Sleep(2000);

            // Assert that the audio is playing at 1.5x speed
            var playbackRate = driver.FindElement(By.CssSelector("audio")).GetAttribute("playbackRate");
            Assert.AreEqual("1.5", playbackRate, "Audio is playing at 1.5x speed");
        }

        [Test]
        public void TestAudioPlayerSpeedx2()
        {
            login.LoginUser(username, password);

            // Navigate to the page with the audio player
            driver.Navigate().GoToUrl("http://localhost:3000/audio-player");

            // Find the play button in the audio player and click it
            var playButton = driver.FindElement(By.Id("play-button"));
            playButton.Click();

            // Find the speed dropdown in the audio player and click it
            var speedDropdown = driver.FindElement(By.Id("speed-dropdown"));
            speedDropdown.Click();

            // Find the option for 2.0x speed and click it
            var speed20xOption = driver.FindElement(By.XPath("//option[@value='2.0']"));
            speed20xOption.Click();

            // Wait for the audio to start playing at the new speed
            wait.Until(driver => driver.FindElement(By.CssSelector("audio")).GetAttribute("playbackRate") == "2.0");

            // Assert that the audio is playing at 2.0x speed
            var playbackRate = driver.FindElement(By.CssSelector("audio")).GetAttribute("playbackRate");
            Assert.AreEqual("2.0", playbackRate, "Audio is playing at 2.0x speed");
        }

        [Test]
        public void TestAppBackgrounding()
        {
            login.LoginUser(username, password);

            // Navigate to the app page
            driver.Navigate().GoToUrl("https://localhost:3000/app");

            // Play an audio
            IWebElement playButton = driver.FindElement(By.CssSelector("#audio-player button.play"));
            playButton.Click();

            // Minimize window or press the device's home button to send the app to the background.
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.SwitchTo().Window(driver.WindowHandles.First());

            // Open another app or navigate to the home screen.
            // This depends on the device's OS and cannot be automated with Selenium.

            // Wait for a few seconds and then return to the application.
            Thread.Sleep(TimeSpan.FromSeconds(5));
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            // Verify that the audio is still playing
            IWebElement pauseButton = driver.FindElement(By.CssSelector("#audio-player button.pause"));
            Assert.IsTrue(pauseButton.Displayed, "The audio player should be playing after returning to the app.");
        }

        [Test]
        public void UpdateText()
        {
            login.LoginUser(username, password);
            driver.Navigate().GoToUrl("http://localhost:3000/audio-player");

            // Click on the 'Edit' button for the text
            IWebElement editButton = driver.FindElement(By.CssSelector(".text-container .edit-button"));
            editButton.Click();

            // Update the text with valid input
            IWebElement textarea = driver.FindElement(By.CssSelector(".text-container textarea"));
            textarea.Clear();
            textarea.SendKeys("This is a valid input for the text field.");

            // Click on the 'Save' button
            IWebElement saveButton = driver.FindElement(By.CssSelector(".text-container .save-button"));
            saveButton.Click();

            // Verify that the text has been updated
            IWebElement updatedText = driver.FindElement(By.CssSelector(".text-container .text"));
            Assert.AreEqual("This is a valid input for the text field.", updatedText.Text);
        }

        [Test]
        public void VerifySourceImageIsDisplayedAndClickable()
        {
            // Navigate to the audio details page for the uploaded audio
            driver.Navigate().GoToUrl("http://localhost:3000/audio/123");

            // Verify that the source image is displayed on the page
            IWebElement sourceImage = driver.FindElement(By.CssSelector("img.source-image"));
            Assert.IsTrue(sourceImage.Displayed);

            // Click on the source image to open it in a new tab
            string sourceImageSrc = sourceImage.GetAttribute("src");
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("window.open(arguments[0]);", sourceImageSrc);

            // Switch to the new tab and verify that the image is loaded
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            IWebElement sourceImageInNewTab = driver.FindElement(By.CssSelector("img"));
            Assert.IsTrue(sourceImageInNewTab.Displayed);
        }

        [Test]
        public void DeleteUploadedAudio()
        {
            // Navigate to the audio details page for the uploaded audio
            driver.Navigate().GoToUrl("http://localhost:3000/audio/123");

            // Click on the "Delete" button
            IWebElement deleteButton = driver.FindElement(By.CssSelector("button.delete-button"));
            deleteButton.Click();

            // Confirm the deletion
            IAlert confirmDeletionAlert = null;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                confirmDeletionAlert = wait.Until(ExpectedConditions.AlertIsPresent());
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Confirmation alert did not appear within 10 seconds.");
            }
            Assert.AreEqual("Are you sure you want to delete this audio?", confirmDeletionAlert.Text);
            confirmDeletionAlert.Accept();

            // Verify that the audio is no longer listed
            bool isAudioDeleted = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.audio-details")));
            Assert.IsTrue(isAudioDeleted);
        }

        [Test]
        public void TestAppBackgroundSwitching()
        {
            login.LoginUser(username, password);

            // Navigate to the app page
            driver.Navigate().GoToUrl("https://localhost:3000/app");

            // Play an audio
            IWebElement playButton = driver.FindElement(By.CssSelector("#audio-player button.play"));
            playButton.Click();

            // Wait for a few seconds and then return to the application.
            Thread.Sleep(TimeSpan.FromSeconds(5));
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            // Verify that the audio is still playing
            IWebElement pauseButton = driver.FindElement(By.CssSelector("#audio-player button.pause"));
            Assert.IsTrue(pauseButton.Displayed, "The audio player should be playing after returning to the app.");
        }
    }
}
