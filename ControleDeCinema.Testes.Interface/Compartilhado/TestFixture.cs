using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ControleDeCinema.Testes.Interface.Compartilhado;

[TestClass]
public abstract class TestFixture
{
      protected static IWebDriver? driver;

    [TestInitialize]
    public void ConfigurarTestes()
    {
        InicializarWebDriver();
    }

    [TestCleanup]
    public void EncerrarTestes()
        {
        EncerrarWebDriver();
    }

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

