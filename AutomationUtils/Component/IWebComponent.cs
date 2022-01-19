using AutomationUtils.Extensions;
using AutomationUtils.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AutomationUtils.Component
{
    public interface IWebComponent : IContextContainer
    {
        WebDriver Driver { get; set; }

        string Identifier { get; set; }

        // TODO Uncomment when logic will be implemented
        // By Frame { get; }

        WebDriverExtensions.WaitTime WaitTime { set; }
    }
}
