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

        public void Build()
        {
            if (!(Props.ParentSelector is null) && !(Props.Parent is null))
            {
                throw new Exception("Not allowed to use Parent element selector and Parent element together");
            }

            ParentSelector = Props.ParentSelector;
            Parent = Props.Parent;

            WaitTime = Props.WaitTime;

            var waitSec = int.Parse(WaitTime.GetValue());

            #region Parent element

            if (!(Props.ParentSelector is null))
            {
                if (!Driver.IsElementExists(Props.ParentSelector, WaitTime))
                {
                    //throw new Exception($"Unable to find Parent element with '{Props.ParentSelector}' selector");
                    return;
                }

                Parent = Driver.FindElement(ParentSelector);
            }

            #endregion

            var selector = Construct();

            if (Parent is null)
            {
                Driver.WaitForElementDisplayCondition(selector, Props.Displayed, waitSec);
            }
            else
            {
                Driver.WaitForElementInElementDisplayCondition(Parent, selector, Props.Displayed, waitSec);
            }

            if (Props.Displayed)
            {
                Component = Parent is null ? Driver.FindElement(selector) : Parent.FindElement(selector);
                PageFactory.InitElements(Component, this);
            }
        }

        public IWebElement Instance => Props.Displayed ? Component : null;

        public RemoteWebDriver Driver { get; set; }

        public WebDriverExtensions.WaitTime WaitTime { get; set; }

        public string Identifier { get; set; }

        protected By ParentSelector { get; set; }

        protected IWebElement Parent { get; set; }

        public By Container { get; }
    }

    public class Properties
    {
        public By ParentSelector = null;

        public IWebElement Parent = null;

        public bool Displayed = true;

        public WebDriverExtensions.WaitTime WaitTime = WebDriverExtensions.WaitTime.Medium;
    }
}
