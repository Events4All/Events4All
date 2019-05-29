using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Events4All.Business.ENotifications;
using Events4All.Business.Resource;
using System.Threading.Tasks;

namespace Events4All.Tests
{
    /// <summary>
    /// Summary description for EmailNotificationTest
    /// </summary>
    [TestClass]
    public class EmailNotificationTest
    {
        [TestMethod]
        public void EmailNotificationTestPass()
        {
            EmailNotification EN = new EmailNotification();
            Task<bool> result =  EN.SendEmailAsync("jrhos97@gmail.com", "Hello", "test");

            Assert.AreEqual(result, false);
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }
    }
}
