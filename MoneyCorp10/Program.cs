using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;



namespace MoneyCorp10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set the browser setting to maximize when opened
            ChromeOptions MyOptions = new ChromeOptions();
            MyOptions.AddArgument("--start-maximized");



            // Launch browser
            IWebDriver driver = new ChromeDriver(MyOptions);
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;



            // Navigate to the URL
            driver.Navigate().GoToUrl("https://www.moneycorp.com/en-gb/");
            System.Threading.Thread.Sleep(3000);



            // Verification
            Assert.AreEqual(driver.Title, "Moneycorp | Global Payments");



            // Click on UK
            driver.FindElement(By.Id("language-dropdown-flag")).Click();
            System.Threading.Thread.Sleep(2000);



            // Click on USA
            driver.FindElement(By.XPath("//a[@aria-label='USA English']")).Click();
            System.Threading.Thread.Sleep(2000);



            // Verification region change
            String FlagSRC = driver.FindElement(By.XPath("/ html[1] / body[1] / section[1] / header[1] / div[1] / div[1] / div[3] / div[2] / div[1] / ul[1] / div[1] / button[1] / span[1] / img[1]")).GetAttribute("src");
            Assert.IsTrue(FlagSRC.Contains("united-states-of-america."));



            /*// Clear cookies
            ReadOnlyCollection<System.Net.Cookie> cookies = driver.Manage().Cookies.AllCookies;
            driver.Manage().Cookies.DeleteAllCookies();
            foreach (Cookie tempCookie in cookies)
            {
                driver.Manage().Cookies.AddCookie(tempCookie);
            }*/

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);



            // Click Find out more
            try
            {
                driver.FindElement(By.XPath("//a[@href='/en-us/business/foreign-exchange-solutions/']//span[@class='ignoreScrollEvents'][normalize-space()='Find out more']")).Click();
            }
            catch (Exception)
            {
                IWebElement FindOutMorebutton = driver.FindElement(By.XPath("//a[@href='/en-us/business/foreign-exchange-solutions/']//span[@class='ignoreScrollEvents'][normalize-space()='Find out more']"));
                IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;
                executor.ExecuteScript("arguments[0].click();", FindOutMorebutton);
            }



            System.Threading.Thread.Sleep(2000);



            // Verification Foreign Exchange Solutions page
            Assert.AreEqual(driver.Title, "Foreign Exchange Solutions | moneycorp USA");



            /*// Clear cookies
            driver.Manage().Cookies.DeleteAllCookies();
            foreach (Cookie tempCookie in cookies)
            {
                driver.Manage().Cookies.AddCookie(tempCookie);
            }*/



            // Search for international payments
            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("body > section:nth-child(4) > header:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(1) > form:nth-child(1) > input:nth-child(1)")).SendKeys("international payments");
            System.Threading.Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("div[class='u-flex u-flex-align-center u-flex-justify-end'] input:nth-child(2)")).Click();



            // Verification Search results page
            String DivClass = driver.FindElement(By.XPath("/ html[1] / body[1] / section[1] / section[1] / div[2]")).GetAttribute("class");
            Assert.IsTrue(DivClass.Contains("container search-results"));



            // Verification Search result link
            IWebElement ResultsWrapper = driver.FindElement(By.CssSelector("body > section[class='main'] div[class='results-wrapper']"));
            ICollection<IWebElement> Results = ResultsWrapper.FindElements(By.TagName("a"));

            foreach (var link in Results)
            {
                Assert.IsTrue(link.GetAttribute("href").StartsWith("https://www.moneycorp.com/en-us/"));



            }



            Console.WriteLine("Verified all links start with https://www.moneycorp.com/en-us/");



            // Close the browse
            driver.Close();
        }
    }

}