using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ControleDeCinema.Testes.Interface.ModuloSessao
{
    public class SessaoFormPageObject
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public SessaoFormPageObject(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
    }
}