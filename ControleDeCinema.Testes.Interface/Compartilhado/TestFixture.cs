using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ControleDeCinema.Testes.Interface.Compartilhado;

[TestClass]
public sealed class TestFixture
{
      protected static IWebDriver? driver;
    


    private static void InicializarWebDriver()
    {
        driver = new ChromeDriver();
    }

    private static void EncerrarWebDriver()
    {
        driver?.Quit();
        driver?.Dispose();
    }
}

