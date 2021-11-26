using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using NUnit.Framework;

using OpenQA.Selenium.Support.UI;

namespace TrackerTest2._1
{
    class Methods
    {
        IWebDriver driver;

        public Methods(IWebDriver driver)
        {
            this.driver = driver;
        }
        public void insertarQuery(int i, String tr_id, int ini) 
        {
            Random rnd = new Random();
            IWebElement txtqw = driver.FindElement(By.Id("QueriesCount" + tr_id + i));
            txtqw.SendKeys("1");
            txtqw.SendKeys(Keys.Tab);
            int rng = rnd.Next(1, 39);
            IWebElement check = driver.FindElement(By.XPath("//table[@id='tblQueryList']//tbody/tr[" + rng + "]"));
            check.FindElement(By.Id("chkQuery_" + rng)).Click();
            check.FindElement(By.Id("txtDrug_" + rng)).SendKeys("drg " + ini);

            driver.FindElement(By.Id("btnQueriesConfirm")).Click();
        }

        public void flushQueue(SelectElement selBox, int op)
        {
            switch (op)
            {
                case 1://REGULAR TEST
                    selBox.SelectByText("Chrysanthos");
                    System.Threading.Thread.Sleep(1000);
                    selBox = new SelectElement(driver.FindElement(By.Id("cphContent_ddlProject")));
                    selBox.SelectByText("Buffalo");
                    System.Threading.Thread.Sleep(1000);
                    selBox = new SelectElement(driver.FindElement(By.Id("cphContent_ddlProcess")));
                    selBox.SelectByText("Refills");
                    System.Threading.Thread.Sleep(1000);
                    break;

                case 2://REGULAR
                    break;

                case 3://ATHENA TEST
                    selBox.SelectByText("Athena");
                    //System.Threading.Thread.Sleep(1000);
                    selBox = new SelectElement(driver.FindElement(By.Id("cphContent_ddlProject")));
                    selBox.SelectByText("TestingGroup");
                    //System.Threading.Thread.Sleep(1000);
                    selBox = new SelectElement(driver.FindElement(By.Id("cphContent_ddlProcess")));
                    selBox.SelectByText("A01 Process");
                    //System.Threading.Thread.Sleep(1000);
                    break;

                case 4://ATHENA 
                    break;

            }

        }

        public void timeOfBreak()
        {
            driver.FindElement(By.Id("btnBreakIn")).Click();
            System.Threading.Thread.Sleep(60000);
            //60000 - 1 minuto
            //180000 - 3 minuto
            driver.FindElement(By.Id("btnBreakOut")).Click();
        }

        public String Add_Image_Menu_DE()
        {
            driver.FindElement(By.Id("TopMenu_lstDEHome")).Click();
            SelectElement selBoxDE = new SelectElement(driver.FindElement(By.Id("ddlOperationalType")));
  
            selBoxDE.SelectByText("DE");

            selBoxDE = new SelectElement(driver.FindElement(By.Id("ddlProject")));
            selBoxDE.SelectByText("Buffalo");

            driver.FindElement(By.Id("cphContent_btnGo")).Submit();
            System.Threading.Thread.Sleep(1500);
            driver.SwitchTo().Alert().Accept();
            IWebElement tr_id = driver.FindElement(By.XPath("//td[@id='tdDynamicTable']//table"));
            return tr_id.GetAttribute("id");

        }

        public int process_Image_Menu(String typeOf)
        {
            driver.FindElement(By.Id("TopMenu_lstDEHome")).Click();
            SelectElement selBoxDE = new SelectElement(driver.FindElement(By.Id("ddlOperationalType")));
            selBoxDE.SelectByText(typeOf);

            driver.FindElement(By.Id("cphContent_btnGo")).Submit();
            System.Threading.Thread.Sleep(1500);
            driver.SwitchTo().Alert().Accept();
            int attempts = 0;
            bool flag = false;
            SelectElement sel = new SelectElement(driver.FindElement(By.Id("ddlProject")));
            while (attempts <= 10)
            {
                try
                {
                    sel.SelectByText("Buffalo");
                    flag = true;
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error feo: " + e);
                }
                attempts++;
            }

            Assert.IsTrue(flag);

            
            
            sel = new SelectElement(driver.FindElement(By.Id("ddlProcess")));
            sel.SelectByIndex(1);
            System.Threading.Thread.Sleep(1500);
            int numOfSpaces = driver.FindElements(By.XPath("//td[@id='tdDynamicTable']//table/tbody/tr[5]/td")).Count;
            return (numOfSpaces-1);
        }

        public void homeBttn()
        {
            driver.FindElement(By.LinkText("Home")).Click();
        }


       public String idFormat()
        { 
            String year = "" + DateTime.Today.Year;
            year = "" + year[2] + year[3];
            String day = (DateTime.Today.Day.ToString().Length == 1) ? ("0" + DateTime.Today.Day) : ("" + DateTime.Today.Day);
            return "88" + day + DateTime.Today.Month + year;
        }
    }
}
