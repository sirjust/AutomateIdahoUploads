using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using AutomateWashingtonUploads;

namespace AutomateIdahoUploads
{
    public class UploadTask
    {
        IWebDriver driver;

        public void inputCompletions(List<Completion> completions)
        {
            driver = new ChromeDriver(@"C:\Users\SirJUST\source\repos\AutomateIdahoUploads\packages\Selenium.Chrome.WebDriver.2.43\driver");
            driver.Url = "https://launchpad.cebroker.com/login";
            driver.Manage().Window.Maximize();
            LoginInfo loginInfo = new LoginInfo();
            string myUserName = loginInfo.id;
            string myPassword = loginInfo.password;
            IWebElement usernameInput = driver.FindElement(By.Id("username"));
            IWebElement passwordInput = driver.FindElement(By.Id("password"));

            usernameInput.SendKeys(myUserName);
            passwordInput.SendKeys(myPassword);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IWebElement login = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//button[@type='submit']//div[@class='btn-content']")));
            login.Click();

            IWebElement courseButton = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//aside[@id='nav']//section//span[@data-bind='html: title'][contains(text(),'Courses')]")));
            courseButton.Click();

            foreach (Completion completion in completions)
            {
                // from here we loop through each completion
                string courseNumber = completion.course;
                string license = completion.license;
                string dateString = completion.date;
                string[] splitUpDate = dateString.Split('-');
                int year = int.Parse(splitUpDate[0]);
                int month = int.Parse(splitUpDate[1]);
                int day = int.Parse(splitUpDate[2]);

                DateTime completionDate = new DateTime(year, month, day);

                //try
                //{

                IWebElement courseField = wait.Until<IWebElement>(d=>d.FindElement(By.CssSelector(".input-sm.form-control.input-s-sm")));
                courseField.SendKeys(courseNumber);
                courseField.SendKeys(Keys.Return);

                IWebElement anchor = wait.Until<IWebElement>(d => d.FindElement(By.PartialLinkText("Manage")));
                anchor.Click();
                //}
                //catch
                //{
                //    string errorInfo = String.Format("The following completion encountered an error: {0} | {1} | {2} | {3}", completion.course, completion.date, completion.license, completion.name);
                //    Logger logger = new Logger();
                //    StreamWriter sw = new StreamWriter(@"log.txt", true);
                //    logger.writeErrorsToLog(errorInfo, sw);
                //    sw.Close();
                //    //then go back to the previous page
                //    IWebElement goBack = driver.FindElement(By.Id("btnPrev"));
                //    goBack.Click();
                //    Thread.Sleep(3000);
                //    continue;
                //}

                //    // if the completion date is incorrect for the course the program will log it and go to the next completion
                //    // this occurs with older electrical courses, where the completion date is out of the range of course validity
                //    try
                //    {
                //        IWebElement dateInput = wait.Until<IWebElement>(d=> d.FindElement(By.Id("txtComplDt")));
                //        dateInput.SendKeys(String.Format("{0:MM/dd/yyyy}", completionDate));
                //    }
                //    catch
                //    {
                //        string errorInfo = String.Format("The following completion encountered an error: {0} | {1} | {2} | {3}", completion.course, completion.date, completion.license, completion.name);
                //        Logger logger = new Logger();
                //        StreamWriter sw = new StreamWriter(@"log.txt", true);
                //        logger.writeErrorsToLog(errorInfo, sw);
                //        sw.Close();
                //        //then go back to the previous page
                //        IWebElement goBack = driver.FindElement(By.Id("btnPrev"));
                //        goBack.Click();
                //        Thread.Sleep(3000);
                //        continue;
                //    }

                //    IWebElement createRoster = wait.Until<IWebElement>(d=> d.FindElement(By.Id("btnGetRoster")));
                //    createRoster.Click();

                //    try
                //    {
                //        IWebElement inputLicense = wait.Until<IWebElement>(d=> d.FindElement(By.Id("txtLicense")));
                //        inputLicense.SendKeys(license);
                //        IWebElement findLicensee = wait.Until<IWebElement>(d=> d.FindElement(By.Id("btnPeople")));
                //        findLicensee.Click();

                //        // next we have to submit the roster
                //        IWebElement addToRoster = wait.Until<IWebElement>(d=> d.FindElement(By.Id("btnTransferToRoster")));
                //        addToRoster.Click();
                //    }
                //    catch
                //    {
                //        string errorInfo = String.Format("The following completion encountered an error: {0} | {1} | {2} | {3}",completion.course, completion.date, completion.license, completion.name);
                //        Logger logger = new Logger();
                //        StreamWriter sw = new StreamWriter(@"log.txt", true);
                //        logger.writeErrorsToLog(errorInfo, sw);
                //        sw.Close();
                //    }
                //    finally
                //    {
                //        //then go back to the previous page
                //        IWebElement goBack = wait.Until<IWebElement>(d=> d.FindElement(By.Id("btnPrev")));
                //        goBack.Click();
                //    }
                //    //loop again until the end
                //}
                //driver.Close();
            }
        }
    }
}

