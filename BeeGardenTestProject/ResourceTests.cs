using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGardenTestProject
{
    public class ResourceTests: TestBase
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
        public void AddResourceWithValidData() 
        {
            login.LoginUser(username, password);

            // Log in to the app with valid credentials
            IWebElement loginButton = driver.FindElement(By.XPath("//button[contains(text(),'Log in')]"));
            loginButton.Click();
            IWebElement emailInput = driver.FindElement(By.CssSelector("#login-form input[name='email']"));
            emailInput.SendKeys("ihor@gmail.com");
            IWebElement passwordInput = driver.FindElement(By.CssSelector("#login-form input[name='password']"));
            passwordInput.SendKeys("123password");
            IWebElement loginSubmitButton = driver.FindElement(By.CssSelector("#login-form button[type='submit']"));
            loginSubmitButton.Click();

            // Navigate to the Add Resource page
            IWebElement addResourceButton = driver.FindElement(By.CssSelector("div.resources-header i.fa-plus-circle"));
            addResourceButton.Click();

            // Enter valid data to create a resource
            IWebElement titleInput = driver.FindElement(By.CssSelector("#add-resource-form input[name='title']"));
            titleInput.SendKeys("Valid title");
            IWebElement descriptionInput = driver.FindElement(By.CssSelector("#add-resource-form textarea[name='description']"));
            descriptionInput.SendKeys("Valid description");
            IWebElement fileInput = driver.FindElement(By.CssSelector("#add-resource-form input[type='file']"));
            fileInput.SendKeys("\\images\\image.jpg");
            IWebElement createButton = driver.FindElement(By.CssSelector("#add-resource-form button[type='submit']"));
            createButton.Click();

            // Assert that the resource is added successfully and saved in the database
            IWebElement resourceTitle = driver.FindElement(By.CssSelector("div.resources-list .resource-item .title"));
            Assert.AreEqual("Valid title", resourceTitle.Text);
        }

        [Test]
        public void AddResource_InvalidTitle_ErrorDisplayed()
        {
            login.LoginUser(username, password);

            // Navigate to Add Resource page
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Enter invalid Title with less than 5 characters
            IWebElement titleField = driver.FindElement(By.Id("title"));
            titleField.SendKeys("abc");

            // Enter valid Description
            IWebElement descriptionField = driver.FindElement(By.Id("description"));
            descriptionField.SendKeys("Valid description");

            // Upload valid image
            IWebElement fileInput = driver.FindElement(By.Id("file-input"));
            fileInput.SendKeys("\\images\\image.jpg");

            // Click on Create button
            IWebElement createButton = driver.FindElement(By.Id("create-button"));
            createButton.Click();

            // Check if error message is displayed
            IWebElement errorMessage = driver.FindElement(By.CssSelector(".error-message"));
            Assert.AreEqual("Title should have at least 5 characters.", errorMessage.Text);
        }

        [Test]
        public void AddResource_TitleTooLong_ErrorDisplayed()
        {
            login.LoginUser(username, password);

            // Navigate to Add Resource page
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Enter Title with more than 40 characters
            IWebElement titleField = driver.FindElement(By.Id("title"));
            titleField.SendKeys("This is a very long title that exceeds the maximum number of characters allowed.");

            // Enter valid Description
            IWebElement descriptionField = driver.FindElement(By.Id("description"));
            descriptionField.SendKeys("Valid description");

            // Upload valid image
            IWebElement fileInput = driver.FindElement(By.Id("file-input"));
            fileInput.SendKeys("C:/path/to/image.jpg");

            // Click on Create button
            IWebElement createButton = driver.FindElement(By.Id("create-button"));
            createButton.Click();

            // Check if error message is displayed
            IWebElement errorMessage = driver.FindElement(By.CssSelector(".error-message"));
            Assert.AreEqual("Title should not exceed 40 characters.", errorMessage.Text);
        }

        [Test]
        public void AddResource_InvalidImageFormat_ErrorDisplayed()
        {
            login.LoginUser(username, password);

            // Navigate to Add Resource page
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Enter valid Title
            IWebElement titleField = driver.FindElement(By.Id("title"));
            titleField.SendKeys("Valid Title");

            // Enter valid Description
            IWebElement descriptionField = driver.FindElement(By.Id("description"));
            descriptionField.SendKeys("Valid description");

            // Upload invalid image in gif format
            IWebElement fileInput = driver.FindElement(By.Id("file-input"));
            fileInput.SendKeys("\\images\\image.gif");

            // Click on Create button
            IWebElement createButton = driver.FindElement(By.Id("create-button"));
            createButton.Click();

            // Check if error message is displayed
            IWebElement errorMessage = driver.FindElement(By.CssSelector(".error-message"));
            Assert.AreEqual("Image should be in jpg, jpeg, or png format.", errorMessage.Text);
        }

        [Test]
        public void AddResourcePage_TitleFieldIsRequired()
        {
            login.LoginUser(username, password);

            // Navigate to the Add Resource page
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Leave the Title field blank
            driver.FindElement(By.Name("description")).SendKeys("Valid description");
            driver.FindElement(By.Name("image")).SendKeys("\\images\\valid_image.jpg");

            // Click on the Create button
            driver.FindElement(By.XPath("//button[text()='Create']")).Click();

            // Verify that an error message is displayed indicating that the Title field is required
            string expectedErrorMessage = "Title is required";
            IWebElement errorMessage = driver.FindElement(By.CssSelector(".error-message"));
            Assert.AreEqual(expectedErrorMessage, errorMessage.Text);
        }

        [Test]
        public void AddResourceWithoutImage()
        {
            login.LoginUser(username, password);

            // Navigate to the Add Resource page
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Fill in the Title and Description fields
            string title = "Test Resource";
            string description = "This is a test resource";
            driver.FindElement(By.Name("title")).SendKeys(title);
            driver.FindElement(By.Name("description")).SendKeys(description);

            // Click on the Create button
            driver.FindElement(By.CssSelector(".create-button")).Click();

            // Verify that an error message is displayed indicating that the image is required
            string errorMessage = driver.FindElement(By.CssSelector(".error-message")).Text;
            Assert.AreEqual("Image is required", errorMessage);
        }

        [Test]
        public void AddResourceWithLargeImageSizeTest()
        {
            login.LoginUser(username, password);

            // Navigate to Add Resource page
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Enter valid title and description
            string validTitle = "Sample Title";
            string validDescription = "Sample Description";
            IWebElement titleInput = driver.FindElement(By.Id("title"));
            titleInput.SendKeys(validTitle);
            IWebElement descriptionInput = driver.FindElement(By.Id("description"));
            descriptionInput.SendKeys(validDescription);

            // Upload an image with large file size
            string largeImagePath = "\\images\\image.jpg";
            IWebElement imageInput = driver.FindElement(By.Id("image"));
            imageInput.SendKeys(largeImagePath);

            // Click on Create button
            IWebElement createButton = driver.FindElement(By.Id("create-button"));
            createButton.Click();

            // Verify that an error message is displayed indicating that the image file size should be smaller
            IWebElement errorMessage = driver.FindElement(By.CssSelector(".error-message"));
            string expectedErrorMessage = "The image file size should be smaller.";
            Assert.AreEqual(expectedErrorMessage, errorMessage.Text);
        }

        [Test]
        public void AddResource_TitleAlreadyExists_ErrorDisplayed()
        {
            login.LoginUser(username, password);

            // Navigate to Add Resource page
            driver.Navigate().GoToUrl("http://localhost:3000/add-resource");

            // Enter existing title
            var titleField = driver.FindElement(By.Id("title"));
            titleField.SendKeys("Existing Title");

            // Enter valid description
            var descriptionField = driver.FindElement(By.Id("description"));
            descriptionField.SendKeys("This is a valid description.");

            // Upload valid image
            var imageField = driver.FindElement(By.Id("image"));
            imageField.SendKeys("\\images\\valid_image.jpg");

            // Click on Create button
            var createButton = driver.FindElement(By.Id("create-button"));
            createButton.Click();

            // Check for error message
            var errorMessage = driver.FindElement(By.CssSelector(".error-message"));
            Assert.That(errorMessage.Text, Is.EqualTo("Title already exists in the system."));
        }

        [Test]
        public void AddResourceWithShortDescription()
        {
            login.LoginUser(username, password);

            // Navigate to Add Resource page
            driver.Navigate().GoToUrl("http://localhost:3000/add-resource");

            // Enter valid title
            IWebElement titleField = driver.FindElement(By.Id("title"));
            titleField.SendKeys("Valid Title");

            // Enter short description
            IWebElement descField = driver.FindElement(By.Id("description"));
            descField.SendKeys("Short");

            // Upload valid image
            IWebElement fileInput = driver.FindElement(By.Id("image"));
            fileInput.SendKeys("\\images\\image.jpg");

            // Click on Create button
            IWebElement createButton = driver.FindElement(By.Id("create-button"));
            createButton.Click();

            // Verify resource is added successfully and saved in DB
            IWebElement successMessage = driver.FindElement(By.CssSelector(".success-message"));
            Assert.AreEqual("Resource added successfully.", successMessage.Text);

            // Verify short description warning message is displayed
            IWebElement warningMessage = driver.FindElement(By.CssSelector(".warning-message"));
            Assert.AreEqual("Note: The description is very short and needs to be expanded.", warningMessage.Text);
        }

        [Test]
        public void AddResource_InvalidTitle()
        {
            login.LoginUser(username, password);

            // Navigate to Add Resource page
            driver.Navigate().GoToUrl("http://localhost:3000/add-resource");

            // Enter invalid title
            string invalidTitle = "Resource &"; // contains invalid character '&'
            IWebElement titleField = driver.FindElement(By.Id("title"));
            titleField.SendKeys(invalidTitle);

            // Enter valid description
            string description = "This is a valid description.";
            IWebElement descriptionField = driver.FindElement(By.Id("description"));
            descriptionField.SendKeys(description);

            // Upload valid image
            string imagePath = "\\images\\image.jpg"; // replace with actual path to image
            IWebElement imageUpload = driver.FindElement(By.Id("image-upload"));
            imageUpload.SendKeys(imagePath);

            // Click Create button
            IWebElement createButton = driver.FindElement(By.Id("create-button"));
            createButton.Click();

            // Verify error message is displayed
            IWebElement errorMessage = driver.FindElement(By.ClassName("error-message"));
            string expectedErrorMessage = "Title contains invalid characters.";
            Assert.AreEqual(expectedErrorMessage, errorMessage.Text);
        }

        [Test]
        public void VerifyCorrectResourceTitleDisplayed()
        {
            login.LoginUser(username, password);

            // Navigate to Resource Single page
            driver.Navigate().GoToUrl("https://localhost:3000/resource/single");

            // Get the expected resource title from the database or test data
            string expectedTitle = "Example Resource Title";

            // Get the actual resource title displayed on the page
            string actualTitle = driver.FindElement(By.CssSelector(".resource-title")).Text;

            // Compare the expected and actual titles
            Assert.AreEqual(expectedTitle, actualTitle, "The title of Resource is not matched with title on 'Resources' page");
        }

        [Test]
        public void VerifyResourceDescription()
        {
            login.LoginUser(username, password);

            // Navigate to the Resource Single page
            driver.Navigate().GoToUrl("https://localhost:3000/resources/single");

            // Find the description element and get its text
            IWebElement descriptionElement = driver.FindElement(By.CssSelector(".resource-description"));
            string actualDescription = descriptionElement.Text;

            // Verify that the actual description matches the expected description
            string expectedDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam at massa neque.";
            Assert.AreEqual(expectedDescription, actualDescription, "The description of Resource does not match the expected description.");
        }

        [Test]
        public void UploadPhotoWithText()
        {
            login.LoginUser(username, password);

            // 1. Navigate to the Resource Single page.
            //    (Assuming we already have a resource available on the page)
            driver.Navigate().GoToUrl("https://localhost:3000/resources/single");

            // 2. Click on the Upload Photo button in the Available audios section.
            var uploadPhotoButton = driver.FindElement(By.CssSelector(".available-audios .upload-photo-btn"));
            uploadPhotoButton.Click();

            // 3. Select a valid photo file in png, jpg, or jpeg format.
            var photoInput = driver.FindElement(By.CssSelector(".available-audios .photo-input"));
            photoInput.SendKeys("\\images\\photo.png");

            // 4. Enter a text for the photo.
            var photoTextInput = driver.FindElement(By.CssSelector(".available-audios .photo-text-input"));
            photoTextInput.SendKeys("This is a test photo");

            // 5. Click on the Generate Audio button.
            var generateAudioButton = driver.FindElement(By.CssSelector(".available-audios .generate-audio-btn"));
            generateAudioButton.Click();

            // Wait for the audio to be generated
            var audioElement = new WebDriverWait(driver, TimeSpan.FromSeconds(60)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".available-audios audio")));

            // Assert that the generated audio is added to the Available audios section
            Assert.IsTrue(audioElement.Displayed);
        }

        [Test]
        public void DeleteResourceAndRelatedData()
        {
            login.LoginUser(username, password);

            // Navigate to the Resource Single page
            driver.Navigate().GoToUrl("http://localhost:3000/resource/123");

            // Click on the Delete button in the Resource info section
            var deleteButton = driver.FindElement(By.CssSelector(".resource-info .delete-button"));
            deleteButton.Click();

            // Verify that a confirmation message is displayed
            var confirmationMessage = driver.FindElement(By.CssSelector(".modal-content"));
            Assert.That(confirmationMessage.Displayed, Is.True, "Confirmation message is not displayed");

            // Click on the Confirm button
            var confirmButton = driver.FindElement(By.CssSelector(".modal-content .confirm-button"));
            confirmButton.Click();

            // Wait for the resource to be deleted
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlContains("http://localhost:3000/resources"));

            // Verify that the resource is deleted successfully
            var resourceTitle = driver.FindElements(By.CssSelector(".resource-list .title")).FirstOrDefault(e => e.Text == "Resource 123");
            Assert.That(resourceTitle, Is.Null, "Resource is not deleted");

            // Verify that related audios and photos are deleted as well
            var audioTitle = driver.FindElements(By.CssSelector(".audio-list .title")).FirstOrDefault(e => e.Text == "Audio 123");
            Assert.That(audioTitle, Is.Null, "Related audio is not deleted");

            var photoTitle = driver.FindElements(By.CssSelector(".photo-list .title")).FirstOrDefault(e => e.Text == "Photo 123");
            Assert.That(photoTitle, Is.Null, "Related photo is not deleted");
        }

        [Test]
        public void GenerateEnglishAudioFromEnglishText()
        {
            login.LoginUser(username, password);

            // 1. Navigate to the Resource Single page.
            driver.Navigate().GoToUrl("https://localhost:3000/resource/1234");

            // 2. Click on the Upload Photo button in the Available audios section.
            IWebElement uploadPhotoBtn = driver.FindElement(By.Id("upload-photo-btn"));
            uploadPhotoBtn.Click();

            // 3. Select a valid photo with text in English file in png, jpg, or jpeg format.
            string photoFilePath = "C:\\photos\\testphoto.png";
            IWebElement photoInput = driver.FindElement(By.Id("photo-input"));
            photoInput.SendKeys(photoFilePath);

            // 4. Enter a text for the photo.
            string photoText = "This is a test photo";
            IWebElement photoTextInput = driver.FindElement(By.Id("photo-text-input"));
            photoTextInput.SendKeys(photoText);

            // 5. Click on the Generate Audio button.
            IWebElement generateAudioBtn = driver.FindElement(By.Id("generate-audio-btn"));
            generateAudioBtn.Click();

            // Assert that the generated audio is in English and generated text is in English
            string generatedAudioLang = driver.FindElement(By.Id("generated-audio-lang")).Text;
            string generatedTextLang = driver.FindElement(By.Id("generated-text-lang")).Text;

            Assert.AreEqual("English", generatedAudioLang);
            Assert.AreEqual("English", generatedTextLang);
        }

        [Test]
        public void VerifyUploadedTimeDisplayed()
        {
            // Navigate to the page where the uploaded time is expected to be displayed
            driver.Navigate().GoToUrl("https://localhost:3000/resource/1234");

            // Upload a file
            IWebElement fileUpload = driver.FindElement(By.Id("fileUpload"));
            fileUpload.SendKeys("\\testfile.txt");

            // Get the current time
            DateTime currentTime = DateTime.Now;

            // Check that the uploaded time is displayed on the page
            IWebElement uploadedTime = driver.FindElement(By.Id("uploadedTime"));
            Assert.AreEqual(currentTime.ToString("F"), uploadedTime.Text);
        }



        [Test]
        public void GenerateAudioWithUkrainianText()
        {
            login.LoginUser(username, password);

            // 1. Navigate to the Resource Single page.
            driver.Navigate().GoToUrl("http://localhost:3000/resource/12345");

            // 2. Click on the Upload Photo button in the Available audios section.
            IWebElement uploadPhotoButton = driver.FindElement(By.Id("upload-photo-button"));
            uploadPhotoButton.Click();

            // 3. Select a valid photo file with text in Ukrainian in png, jpg, or jpeg format.
            string photoPath = "path/to/ukrainian/photo.jpg";
            IWebElement fileInput = driver.FindElement(By.Id("file-input"));
            fileInput.SendKeys(photoPath);

            // 4. Enter a text for the photo.
            string photoText = "Це український текст";
            IWebElement photoTextInput = driver.FindElement(By.Id("photo-text-input"));
            photoTextInput.SendKeys(photoText);

            // 5. Click on the Generate Audio button.
            IWebElement generateAudioButton = driver.FindElement(By.Id("generate-audio-button"));
            generateAudioButton.Click();

            // Assert that the generated audio is in Ukrainian and generated text is in Ukrainian
            IWebElement audioPlayer = driver.FindElement(By.ClassName("audio-player"));
            string audioSrc = audioPlayer.GetAttribute("src");
            string generatedText = driver.FindElement(By.ClassName("generated-text")).Text;

            Assert.IsTrue(audioSrc.Contains("ukr"), "The generated audio is in Ukrainian.");
            Assert.AreEqual(photoText, generatedText, "The generated text is in Ukrainian.");
        }

        [Test]
        public void AudioPlayerIsDisplayed()
        {
            login.LoginUser(username, password);

            driver.Navigate().GoToUrl("http://localhost:3000/resource/12345");

            IWebElement audioPlayer = driver.FindElement(By.Id("audio-player"));
            bool isDisplayed = audioPlayer.Displayed;
            Assert.IsTrue(isDisplayed, "Audio player is not displayed on the page");
        }

        [Test]
        public void UpdateResourceInfo()
        {
            login.LoginUser(username, password);
            driver.Navigate().GoToUrl("http://localhost:3000/resource/12345");

            // Click on the 'Edit' button in the Resource Info section
            IWebElement editButton = driver.FindElement(By.CssSelector(".resource-info .edit-button"));
            editButton.Click();

            // Update the description with valid input (less than or equal to 200 characters)
            IWebElement descriptionInput = driver.FindElement(By.CssSelector(".resource-info .description-input"));
            descriptionInput.Clear();
            descriptionInput.SendKeys("This is a valid description that is less than or equal to 200 characters.");

            // Click on the 'Save' button
            IWebElement saveButton = driver.FindElement(By.CssSelector(".resource-info .save-button"));
            saveButton.Click();

            // Verify that the description has been updated
            IWebElement updatedDescription = driver.FindElement(By.CssSelector(".resource-info .description"));
            Assert.AreEqual("This is a valid description that is less than or equal to 200 characters.", updatedDescription.Text);
        }

        [Test]
        public void InvalidPhotoFileFormat_ErrorDisplayed()
        {
            login.LoginUser(username, password);

            // Navigate to the Resource Single page
            driver.Navigate().GoToUrl("https://localhost:3000/resource/single");

            // Click on the Upload Photo button
            IWebElement uploadButton = driver.FindElement(By.CssSelector(".available-audios .upload-photo"));
            uploadButton.Click();

            // Select an invalid photo file format
            IWebElement fileInput = driver.FindElement(By.CssSelector("#upload-photo-modal input[type='file']"));
            fileInput.SendKeys("path/to/invalid/file.pdf");

            // Click on the Generate Audio button
            IWebElement generateButton = driver.FindElement(By.CssSelector("#upload-photo-modal button.generate-audio"));
            generateButton.Click();

            // Verify that an error message is displayed
            IWebElement errorMessage = driver.FindElement(By.CssSelector("#upload-photo-modal .error-message"));
            Assert.AreEqual("File format is not supported.", errorMessage.Text);
        }

        [Test]
        public void AddResource_WrongImageFormat()
        {
            login.LoginUser(username, password);

            // Navigate to Add Resource page
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Enter valid Title
            IWebElement titleField = driver.FindElement(By.Id("title"));
            titleField.SendKeys("Valid Title");

            // Enter valid Description
            IWebElement descriptionField = driver.FindElement(By.Id("description"));
            descriptionField.SendKeys("Valid description");

            // Upload invalid image in gif format
            IWebElement fileInput = driver.FindElement(By.Id("file-input"));
            fileInput.SendKeys("\\images\\image.gif");

            // Click on Create button
            IWebElement createButton = driver.FindElement(By.Id("create-button"));
            createButton.Click();

            // Check if error message is displayed
            IWebElement errorMessage = driver.FindElement(By.CssSelector(".error-message"));
            Assert.AreEqual("Image should be in jpg, jpeg, or png format.", errorMessage.Text);
        }

        [Test]
        public void AddResourceWithoutImage_ErrorDisplayed()
        {
            login.LoginUser(username, password);

            // Navigate to the Add Resource page
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Fill in the Title and Description fields
            string title = "Test Resource";
            string description = "This is a test resource";
            driver.FindElement(By.Name("title")).SendKeys(title);
            driver.FindElement(By.Name("description")).SendKeys(description);

            // Click on the Create button
            driver.FindElement(By.CssSelector(".create-button")).Click();

            // Verify that an error message is displayed indicating that the image is required
            string errorMessage = driver.FindElement(By.CssSelector(".error-message")).Text;
            Assert.AreEqual("Image is required", errorMessage);
        }

        [Test]
        public void AddResourcePage_WithoutTitle()
        {
            login.LoginUser(username, password);

            // Navigate to the Add Resource page
            driver.Navigate().GoToUrl("https://localhost:3000/add-resource");

            // Leave the Title field blank
            driver.FindElement(By.Name("description")).SendKeys("Valid description");
            driver.FindElement(By.Name("image")).SendKeys("\\images\\valid_image.jpg");

            // Click on the Create button
            driver.FindElement(By.XPath("//button[text()='Create']")).Click();

            // Verify that an error message is displayed indicating that the Title field is required
            string expectedErrorMessage = "Title is required";
            IWebElement errorMessage = driver.FindElement(By.CssSelector(".error-message"));
            Assert.AreEqual(expectedErrorMessage, errorMessage.Text);
        }
    }
}
