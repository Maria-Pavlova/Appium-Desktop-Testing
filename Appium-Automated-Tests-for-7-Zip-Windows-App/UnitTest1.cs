using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System;
using System.IO;
using System.Threading;

namespace Appium_Automated_Tests_for_7_Zip_Windows_App
{
    public class Tests
    {
        private WindowsDriver<WindowsElement> driver;
        private WindowsDriver<WindowsElement> driverRoot;
        private const string AppiumServerUrl = "http://[::1]:4723/wd/hub";
        private const string Path7Zip = @"C:\Program Files\7-Zip\";
        private string workDir;

        [OneTimeSetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions() { PlatformName = "Windows"};
            appiumOptions.AddAdditionalCapability("app", @"C:\Program Files\7-Zip\7zFM.exe");
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUrl), appiumOptions);

            var appiumOptionsRoot = new AppiumOptions() { PlatformName = "Windows" };
            appiumOptionsRoot.AddAdditionalCapability("app", "Root");
            driverRoot = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUrl), appiumOptionsRoot);


            workDir = Directory.GetCurrentDirectory() + @"\workDir";
            if (Directory.Exists(workDir))
                Directory.Delete(workDir, true);
            Directory.CreateDirectory(workDir);

        }
        [OneTimeTearDown]
        public void Quit()
        {
            driver.Quit();
        }

        [Test]
        public void Test_7Zip()
        {
            var textBoxLocation = driver.FindElementByXPath("/Window/Pane/Pane/ComboBox/Edit");
                textBoxLocation.SendKeys(Path7Zip + Keys.Enter);
           
            driver.FindElementByClassName("SysListView32").SendKeys(Keys.Control + "a");

            var addButton = driver.FindElementByName("Add");
            addButton.Click();

            Thread.Sleep(2000);
            // Create an archive
            var windowCreateArchive = driverRoot.FindElementByName("Add to Archive");

            var textBoxArchiveName = windowCreateArchive.FindElementByXPath("/Window[@Name=\"Add to Archive\"]/ComboBox/Edit[@Name=\"Archive:\"]");
          
            string archiveName = workDir + @"\" + "archive.7z";
            textBoxArchiveName.SendKeys(archiveName);

            var textBoxArchiveFormat = windowCreateArchive.FindElementByXPath("/Window[@Name=\"Add to Archive\"]/ComboBox[@Name=\"Archive format:\"]");
            textBoxArchiveFormat.SendKeys("7z");

            var textBoxCompressionLevel = windowCreateArchive.FindElementByXPath("/Window[@Name=\"Add to Archive\"]/ComboBox[@Name=\"Compression level:\"]");
            textBoxCompressionLevel.SendKeys("End");

            var textBoxCompressionMethod = windowCreateArchive.FindElementByXPath("/Window[@Name=\"Add to Archive\"]/ComboBox[@Name=\"Compression method:\"]");
           textBoxCompressionMethod.SendKeys("Home");

            var textBoxDictionarySize = windowCreateArchive.FindElementByXPath("/Window[@Name=\"Add to Archive\"]/ComboBox[@Name=\"Compression method:\"]");
            textBoxDictionarySize.SendKeys("Home");

            var buttonOK = windowCreateArchive.FindElementByXPath("/Window/Button[@Name=\"OK\"]");
            buttonOK.Click();

            // Extract the archive
            textBoxLocation.SendKeys(archiveName + Keys.Enter);

            var extractButton = driver.FindElementByName("Extract");
            extractButton.Click();

            var buttonExtractOK = driver.FindElementByName("OK");
            buttonExtractOK.Click();

            Thread.Sleep(1000);

            // Assert that files are the same
            
            FileAssert.AreEqual(workDir + @"\7zFM.exe", Path7Zip + @"\7zFM.exe");

            foreach(string fileOriginal in Directory.EnumerateFiles(Path7Zip, "*.*", SearchOption.AllDirectories))
            {
                var fileNameOnly = fileOriginal.Replace(Path7Zip, "");
                var fileCopy = workDir + @"\" + fileNameOnly;
                FileAssert.AreEqual(fileOriginal, fileCopy);
            }
        }
    }
}