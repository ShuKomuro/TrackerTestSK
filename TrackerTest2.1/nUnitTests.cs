using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;


namespace TrackerTest2._1
{
    [TestFixture]
    class nUnitTests
    {
        static IWebDriver driver;
        int inicio = 41;
        int total= 10;
        Methods met;

        [SetUp]
        public void initialize()
        {
            var options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            //options.AcceptInsecureCertificates = true;

            driver = new InternetExplorerDriver(options);
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(60);
            driver.Url = "http://indftp.indconsulting.com/ProductionTrackerUAT/Login.aspx";
            //driver.Url = "http://indftp.indconsulting.com/ProductionTrackerAthena/Login.aspx";
            met = new Methods(driver);
        }

        [TearDown]
        public void finalize()
        {
            driver.Quit();
        }

        //ENTORNO DE PRUEBAS 
        //---------------------------------------------------------------------------------------------------------------------------------------------------

        //DE+++++++++++++++++++++DE+++++++++++++++++++++DE+++++++++++++++++++++DE+++++++++++++++++++++DE+++++++++++++++++++++DE+++++++++++++++++++++DE
        [Test]
        public void Test_01_LoginDE()
        {
            driver.FindElement(By.Name("txtUserName")).SendKeys("gmorales");
            driver.FindElement(By.Name("txtPassword")).SendKeys("tester");
            //driver.FindElement(By.Name("txtPassword")).SendKeys("testing");
            driver.FindElement(By.Name("ImageButton1")).Submit();
        }

        [Test]
        public void Test_02_Unlog()
        {
            //Test_01_LoginDE();
            driver.FindElement(By.Id("lnkLogout")).Click();

        }

        [Test]
        public void Test_03_AddSimpleImages()
        {
            String init_id = met.idFormat();
            Test_01_LoginDE();
            String tr_id = met.Add_Image_Menu_DE();

            for (int i = 1; i <= total; i++)
            {
                String id_inf = (inicio.ToString().Length == 1) ? (init_id + "0" + inicio) : (init_id + "" + inicio);
                driver.FindElement(By.Id("ImageID" + tr_id + i)).SendKeys(id_inf);
                driver.FindElement(By.Id("NoOfOrders" + tr_id + i)).SendKeys("1");
                driver.FindElement(By.Id("Processed" + tr_id + i)).SendKeys("1");
                driver.FindElement(By.Id("Save" + tr_id + i)).Click();
                inicio++;
            }
            System.Threading.Thread.Sleep(2000);
            met.homeBttn();
            String comp = driver.FindElement(By.XPath("//div[@id='cphContent_Panel1'] /span[2]")).Text;
            Assert.AreEqual(comp, "You must select a Project and operational type you going to work with before start with your production.");

        }

        [Test]
        public void Test_04_AddQueryImages()
        {
            String init_id = met.idFormat();
            Test_01_LoginDE();
            String tr_id = met.Add_Image_Menu_DE();
            Assert.AreEqual(3, tr_id.Length);
            for (int i = 1; i <= total; i++)
            {
                String id_inf = (inicio.ToString().Length == 1) ? (init_id + "0" + inicio) : (init_id + "" + inicio);
                driver.FindElement(By.Id("ImageID" + tr_id + i)).SendKeys(id_inf);
                driver.FindElement(By.Id("NoOfOrders" + tr_id + i)).SendKeys("1");
                if (i % 3 == 0)
                {
                    driver.FindElement(By.Id("Processed" + tr_id + i)).SendKeys("0");
                    met.insertarQuery(i, tr_id, inicio);
                }
                else
                {
                    driver.FindElement(By.Id("Processed" + tr_id + i)).SendKeys("1");
                }

                driver.FindElement(By.Id("Save" + tr_id + i)).Click();
                inicio++;
            }
            System.Threading.Thread.Sleep(2000);
            met.homeBttn();
            String comp = driver.FindElement(By.XPath("//div[@id='cphContent_Panel1'] /span[2]")).Text;
            Assert.AreEqual(comp, "You must select a Project and operational type you going to work with before start with your production.");
        }

        [Test]
        public void Test_05_AddImagesWithBreak()
        {
            String init_id = met.idFormat();
            String tr_id = met.Add_Image_Menu_DE();

            for (int i = 1; i <= total; i++)
            {
                String id_inf = (inicio.ToString().Length == 1) ? (init_id + "0" + inicio) : (init_id + "" + inicio);
                driver.FindElement(By.Id("ImageID" + tr_id + i)).SendKeys(id_inf);
                driver.FindElement(By.Id("NoOfOrders" + tr_id + i)).SendKeys("1");
                driver.FindElement(By.Id("Processed" + tr_id + i)).SendKeys("1");
                driver.FindElement(By.Id("Save" + tr_id + i)).Click();
                inicio++;

                if (i % 3 == 0)
                {
                    met.timeOfBreak();
                }
            }
            System.Threading.Thread.Sleep(2000);
            met.homeBttn();
            String comp = driver.FindElement(By.XPath("//div[@id='cphContent_Panel1'] /span[2]")).Text;
            Assert.AreEqual(comp, "You must select a Project and operational type you going to work with before start with your production.");
        }

        //QC+++++++++++++++++++++QC+++++++++++++++++++++QC+++++++++++++++++++++QC+++++++++++++++++++++QC+++++++++++++++++++++QC+++++++++++++++++++++QC
        [Test]
        public void Test_06_LoginQC()
        {
            driver.FindElement(By.Name("txtUserName")).SendKeys("gmoralesQC");
            driver.FindElement(By.Name("txtPassword")).SendKeys("tester");
            //driver.FindElement(By.Name("txtPassword")).SendKeys("testing");
            driver.FindElement(By.Name("ImageButton1")).Submit();
        }
        
        [Test]
        public void Test_07_NormalProcess_QC()
        {
            Test_06_LoginQC();
            int num = met.process_Image_Menu("QC");
            //Console.WriteLine("Numero de LINEAS: " + num);
            for (int i = 1; i <= total; i++)
            {
                for (int j = 1; j <= num; j++)
                {
                    driver.FindElement(By.Id("imgYes" + j)).Click();
                    //Console.WriteLine("NUMERO DE I EN QC: " + i);
                }
                driver.FindElement(By.Id("done")).Click();
                //Console.WriteLine("NUMERO DE J EN QC: " + j);
            }
            System.Threading.Thread.Sleep(2000);
            driver.FindElement(By.XPath("//body/div[5]/div[3]/div[1]//button")).Click();
            met.homeBttn();
            String comp = driver.FindElement(By.XPath("//div[@id='cphContent_Panel1'] /span[2]")).Text;
            Assert.AreEqual(comp, "You must select a Project and operational type you going to work with before start with your production.");
        }

        public void Test_08_Query_Process_QC()
        {
            Test_06_LoginQC();
            int num = met.process_Image_Menu("QC");
            Console.WriteLine("Numero de LINEAS: " + num);
            for (int j = 0; j < total; j++)
            {
                for (int i = 1; i <= num; i++)
                {
                    driver.FindElement(By.Id("imgYes" + i)).Click();
                }
                driver.FindElement(By.Id("done")).Click();
            }
            System.Threading.Thread.Sleep(2000);
            met.homeBttn();
            String comp = driver.FindElement(By.XPath("//div[@id='cphContent_Panel1'] /span[2]")).Text;
            Assert.AreEqual(comp, "You must select a Project and operational type you going to work with before start with your production.");
        }

        public void Test_09_BreaksProcess_QC()
        {
            Test_06_LoginQC();
            int num = met.process_Image_Menu("QC");
            Console.WriteLine("Numero de LINEAS: " + num);
            for (int j = 0; j < total; j++)
            {
                if (j % 3 == 0)
                {
                    met.timeOfBreak();
                }
                for (int i = 1; i <= num; i++)
                {
                    driver.FindElement(By.Id("imgYes" + i)).Click();
                }
                driver.FindElement(By.Id("done")).Click();
            }
            System.Threading.Thread.Sleep(2000);
        }

        //INDQC+++++++++++++++++++++INDQC+++++++++++++++++++++INDQC+++++++++++++++++++++INDQC+++++++++++++++++++++INDQC+++++++++++++++++++++INDQC+++++++++++++++++++++INDQC

        [Test]
        public void Test_500_LoginINDQC()
        {
            driver.FindElement(By.Name("txtUserName")).SendKeys("gmoralesNQC");
            driver.FindElement(By.Name("txtPassword")).SendKeys("tester");
            //driver.FindElement(By.Name("txtPassword")).SendKeys("testing");
            driver.FindElement(By.Name("ImageButton1")).Submit();
        }

        [Test]
        public void Test_501_ProcessImageINDQC()
        {
            Test_500_LoginINDQC();
            met.process_Image_Menu("IND QC");

            for (int i = 1; i <= total; i++)
            {
                driver.FindElement(By.Id("btnCheckAll")).Click();
                driver.FindElement(By.Id("done")).Click();
                //Console.WriteLine("Valor de i en NQC: "+ i);
            }
            System.Threading.Thread.Sleep(2000);
            driver.FindElement(By.XPath("//body/div[5]/div[3]/div[1]//button")).Click();
            met.homeBttn();
            String comp = driver.FindElement(By.XPath("//div[@id='cphContent_Panel1'] /span[2]")).Text;
            Assert.AreEqual(comp, "You must select a Project and operational type you going to work with before start with your production.");
        }

        [Test]
        public void Test_400_FlushQueue()
        {
            Test_01_LoginDE();
            int op = 1; ;
            driver.FindElement(By.Id("TopMenu_LstMaster")).Click();
            driver.FindElement(By.Id("TopMenu_lstClearQC")).Click();
            SelectElement selBox = new SelectElement(driver.FindElement(By.Id("cphContent_ddlOpsCenter")));
            met.flushQueue(selBox, op);
            driver.FindElement(By.Id("cphContent_btnSearch")).Click();
            driver.FindElement(By.Id("cphContent_btnDelete")).Click();
            driver.FindElement(By.Id("cphContent_btnConfirm")).Click();
            driver.FindElement(By.Id("cphContent_btnSearch")).Click();

            String texTab = driver.FindElement(By.XPath("//table[@id='cphContent_grdQueue']   //td[contains(text(),'No records found')]")).Text;
            Console.Write("wea watbla: " + texTab);
            Assert.AreEqual("No records found", texTab);
        }

        [Test]
        public void Test_901_Prueba()
        {
            Test_06_LoginQC();
            met.process_Image_Menu("QC");
            //System.Threading.Thread.Sleep(5000);
            driver.FindElement(By.XPath("//body/div[5]/div[3]/div[1]//button")).Click();
            //Test_02_Unlog();
            //met.Add_Image_Menu_DE();
            //met.homeBttn();
            //String comp = driver.FindElement(By.XPath("//div[@id='cphContent_Panel1'] /span[2]")).Text;
            //Assert.AreEqual(comp, "You must select a Project and operational type you going to work with before start with your production.");
        }

        [Test]
        public void Test_900_Normal_Path()
        {
            Test_03_AddSimpleImages();
            Test_02_Unlog();
            Test_07_NormalProcess_QC();
            Test_02_Unlog();
            Test_501_ProcessImageINDQC();
        }

    }
}