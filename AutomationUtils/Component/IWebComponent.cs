using AutomationUtils.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AutomationUtils.Component
{
    interface IWebComponent
    {
        RemoteWebDriver Driver { get; set; }

        string Identifier { get; set; }

        string ParentElementSelector { get; set; }

        WebDriverExtensions.WaitTime WaitTime { set; }
    }
}
