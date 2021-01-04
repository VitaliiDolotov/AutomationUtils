using System;
using AutomationUtils.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SeleniumExtras.PageObjects;

namespace AutomationUtils.Component
{
    public abstract class BaseWebComponent : IWebComponent
    {
        public Properties Props = new Properties();

        protected IWebElement Component;

        protected abstract By Construct();

        public IWebElement Instance
        {
            get
            {
                ParentElementSelector = Props.ParentElementSelector;
                WaitTime = Props.WaitTime;

                var waitSec = int.Parse(WaitTime.GetValue());

                var selector = Construct();
                Driver.WaitForElementDisplayCondition(selector, Props.Displayed, waitSec);

                if (Props.Displayed)
                {
                    Component = Driver.FindElement(selector);
                    //In case we have ParentElement
                    if (!string.IsNullOrEmpty(ParentElementSelector))
                    {
                        if (Driver.IsElementDisplayed(By.XPath(ParentElementSelector), WaitTime))
                        {
                            PageFactory.InitElements(Driver.FindElement(By.XPath(ParentElementSelector)), this);
                        }
                        else
                        {
                            throw new Exception($"Unable to init component with '{ParentElementSelector}' parentElement selector");
                        }
                    }
                    else
                    {
                        PageFactory.InitElements(Driver, this);
                    }
                    return Component;
                }
                else
                {
                    return null;
                }
            }
        }

        public RemoteWebDriver Driver { get; set; }

        public WebDriverExtensions.WaitTime WaitTime { get; set; }

        public string Identifier { get; set; }

        public string ParentElementSelector { get; set; }

        public string Container { get; }

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

    public class Properties
    {
        public string ParentElementSelector = string.Empty;

        public bool Displayed = true;

        public WebDriverExtensions.WaitTime WaitTime = WebDriverExtensions.WaitTime.Medium;
    }
}
