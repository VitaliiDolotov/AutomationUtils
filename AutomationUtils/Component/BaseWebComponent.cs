using System;
using AutomationUtils.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AutomationUtils.Component
{
    public abstract class BaseWebComponent : IWebComponent
    {
        protected IWebElement Component;

        public IWebElement Instance
        {
            get { return Component ??= Construct(); }
        }

        public RemoteWebDriver Driver { get; set; }

        public WebDriverExtensions.WaitTime WaitTime { get; set; }

        public string Identifier { get; set; }

        public string ParentElementSelector { get; set; }

        protected abstract IWebElement Construct();

        public bool Displayed
        {
            get
            {
                try
                {
                    return Instance.Displayed();
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
