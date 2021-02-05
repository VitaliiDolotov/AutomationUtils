using AutomationUtils.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AutomationUtils.Component
{
    public interface IWebComponent
    {
        RemoteWebDriver Driver { get; set; }

        string Identifier { get; set; }

        By Container { get; }

        By Frame { get; }

        WebDriverExtensions.WaitTime WaitTime { set; }
    }
}
