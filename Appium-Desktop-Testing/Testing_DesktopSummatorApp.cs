using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace Appium_Desktop_Testing
{
    public class Tests
    {

        private WindowsDriver<WindowsElement> driver;
        private const string AppiumServer = "http://[::1]:4723/wd/hub";
        private AppiumOptions options;

        [SetUp]
        public void Setup()
        {
            this.options = new AppiumOptions() { PlatformName = "Windows"};
          //  options.AddAdditionalCapability("Url", AppiumServer);
            options.AddAdditionalCapability(MobileCapabilityType.App, @"C:\Users\Lenovo\Downloads\WindowsFormsApp.exe");
            this.driver = new WindowsDriver<WindowsElement>( new Uri(AppiumServer), options);
           
        }
        [TearDown]
        public void close()
        {
            driver.Quit();
        }
        [Test]
        public void Test_DesktopSummator_PositivIntegers()
        {
            var num1 = driver.FindElementByAccessibilityId("textBoxFirstNum");
            num1.Click();
            num1.SendKeys("5");
            var num2 = driver.FindElementByAccessibilityId("textBoxSecondNum");
            num2.Click();
            num2.SendKeys("25");
            var calcButton = driver.FindElementByAccessibilityId("buttonCalc");
            calcButton.Click();

            var result = driver.FindElementByAccessibilityId("textBoxSum").Text;
            Assert.AreEqual("30", result);

        }

        [Test]
        public void Test_DesktopSummator_PositivDesimalNumbers()
        {
            var num1 = driver.FindElementByAccessibilityId("textBoxFirstNum");
            num1.Click();
            num1.SendKeys("5.5");
            var num2 = driver.FindElementByAccessibilityId("textBoxSecondNum");
            num2.Click();
            num2.SendKeys("25.2");
            var calcButton = driver.FindElementByAccessibilityId("buttonCalc");
            calcButton.Click();

            var result = driver.FindElementByAccessibilityId("textBoxSum").Text;
            Assert.AreEqual("30.7", result);

        }

        [Test]
        public void Test_DesktopSummator_NegativNumbers()
        {
            var num1 = driver.FindElementByAccessibilityId("textBoxFirstNum");
            num1.Click();
            num1.SendKeys("-5.5");
            var num2 = driver.FindElementByAccessibilityId("textBoxSecondNum");
            num2.Click();
            num2.SendKeys("-5.2");
            var calcButton = driver.FindElementByAccessibilityId("buttonCalc");
            calcButton.Click();

            var result = driver.FindElementByAccessibilityId("textBoxSum").Text;
            Assert.AreEqual("-10.7", result);

        }

        [Test]
        public void Test_DesktopSummator_InvalidAndEmptyData()
        {
            var num1 = driver.FindElementByAccessibilityId("textBoxFirstNum");
            num1.Click();
            num1.SendKeys("");
            var num2 = driver.FindElementByAccessibilityId("textBoxSecondNum");
            num2.Click();
            num2.SendKeys("hi");
            var calcButton = driver.FindElementByAccessibilityId("buttonCalc");
            calcButton.Click();

            var result = driver.FindElementByAccessibilityId("textBoxSum").Text;
            Assert.AreEqual("error", result);

        }
    }
}