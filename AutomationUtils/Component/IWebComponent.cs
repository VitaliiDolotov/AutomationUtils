using AutomationUtils.Extensions;
using AutomationUtils.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AutomationUtils.Component
{
    public interface IWebComponent : IContextContainer
    {
        RemoteWebDriver Driver { get; set; }

        string Identifier { get; set; }

        By Frame { get; }

        WebDriverExtensions.WaitTime WaitTime { set; }

        void CheckAutomationClass();
    }
}
