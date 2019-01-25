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
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Interactions;

namespace AutomateIdahoUploads
{
    public class UploadTask
    {
        IWebDriver driver;

        public void inputCompletions(List<Completion> completions)
        {
            driver = new ChromeDriver(@"../../../packages/Selenium.Chrome.WebDriver.2.43/driver");
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

                try
                {

                    IWebElement courseField = wait.Until<IWebElement>(d => d.FindElement(By.CssSelector(".input-sm.form-control.input-s-sm")));
                    courseField.SendKeys(courseNumber);
                    Thread.Sleep(3000);
                    courseField.SendKeys(Keys.Return);
                    Thread.Sleep(3000);
                    IWebElement firstVisibleCourse = wait.Until<IWebElement>(d => d.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/section[1]/section[1]/section[1]/section[1]/section[1]/section[1]/div[1]/section[1]/section[1]/section[1]/section[1]/section[1]/div[1]/ul[1]/li[1]/small[1]/span[2]")));
                    if(courseNumber == firstVisibleCourse.GetAttribute("innerHTML"))
                    {
                        IWebElement manageRoster = wait.Until<IWebElement>(d => d.FindElement(By.PartialLinkText("Manage")));
                        manageRoster.Click();
                        IWebElement addButton = wait.Until<IWebElement>(d => d.FindElement(By.LinkText("Add")));
                        addButton.Click();
                    }
                    else
                    {
                        throw new Exception();
                    }
                } catch
                {
                    string errorInfo = String.Format("The following completion encountered an error: {0} | {1} | {2} | {3}", completion.course, completion.date, completion.license, completion.name);
                    Logger logger = new Logger();
                    StreamWriter sw = new StreamWriter(@"log.txt", true);
                    logger.writeErrorsToLog(errorInfo, sw);
                    sw.Close();
                    //then go back to the previous page
                    driver.Navigate().Refresh();
                    Thread.Sleep(3000);
                    continue;
                }

                try
                {
                    Thread.Sleep(3000);
                    IWebElement stateField = wait.Until<IWebElement>(d => d.FindElement(By.Id("state")));
                    stateField.Click();
                    stateField.SendKeys(Keys.ArrowDown);
                    stateField.SendKeys(Keys.Return);
                    Thread.Sleep(3000);

                    IWebElement licenseField = wait.Until<IWebElement>(d => d.FindElement(By.Id("licenseNumber")));
                    licenseField.SendKeys(license);
                } catch
                {
                    string errorInfo = String.Format("The following completion encountered an error: {0} | {1} | {2} | {3}", completion.course, completion.date, completion.license, completion.name);
                    Logger logger = new Logger();
                    StreamWriter sw = new StreamWriter(@"log.txt", true);
                    logger.writeErrorsToLog(errorInfo, sw);
                    sw.Close();
                    //then go back to the previous page
                    IWebElement goBack = driver.FindElement(By.Id("btnPrev"));
                    courseButton.Click();
                    Thread.Sleep(3000);
                    continue;
                }

                try
                {
                    IWebElement dateCompletedField = wait.Until<IWebElement>(d => d.FindElement(By.Id("dateCompleted")));
                    dateCompletedField.Click();
                    int dateLength = 10;
                    while (dateLength > 0)
                    {
                        dateCompletedField.SendKeys(Keys.Backspace);
                        dateLength--;
                    }

                    dateCompletedField.SendKeys(String.Format("{0:MM/dd/yyyy}", completionDate));
                    dateCompletedField.SendKeys(Keys.Enter);

                    IWebElement scrollField = wait.Until<IWebElement>(d => d.FindElement(By.Id("formNav")));
                    scrollField.Click();

                    Actions actions = new Actions(driver);
                    actions.SendKeys(OpenQA.Selenium.Keys.End).Build().Perform();

                    Thread.Sleep(3000);

                    IWebElement saveButton = wait.Until<IWebElement>(d => d.FindElement(By.CssSelector(".btn.btn-primary.btn-s-xs.btn-sm")));
                    saveButton.Click();

                    Thread.Sleep(3000);

                    IWebElement postButton = wait.Until<IWebElement>(d => d.FindElement(By.LinkText("Post")));
                    postButton.Click();

                    Thread.Sleep(3000);

                    courseButton.Click();
                } catch
                {
                    string errorInfo = String.Format("The following completion encountered an error: {0} | {1} | {2} | {3}", completion.course, completion.date, completion.license, completion.name);
                    Logger logger = new Logger();
                    StreamWriter sw = new StreamWriter(@"log.txt", true);
                    logger.writeErrorsToLog(errorInfo, sw);
                    sw.Close();
                    //then go back to the previous page
                    IWebElement goBack = driver.FindElement(By.Id("btnPrev"));
                    courseButton.Click();
                    Thread.Sleep(3000);
                    continue;
                } finally
                {
                    courseButton.Click();
                    Thread.Sleep(3000);
                }
            }
        }
    }
}

