
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

public class GeneroFilmeIndexPageObject
{
    private readonly IWebDriver? driver;
    private readonly WebDriverWait wait;
    public GeneroFilmeIndexPageObject(IWebDriver? driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }
    public GeneroFilmeIndexPageObject IrPara(string enderecoBase)
    {
        driver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "generos"));
        return this;
    }

    public bool ContemGeneroFilme(string descricao)
    {
        return driver.PageSource.Contains(descricao);

    }

    public GeneroFilmeFormPageObject ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();
        return new GeneroFilmeFormPageObject(driver!);
    }

    public GeneroFilmeFormPageObject ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnEditar']"))).Click();
        return new GeneroFilmeFormPageObject(driver!);
    }

    public GeneroFilmeFormPageObject ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnExcluir']"))).Click();
        return new GeneroFilmeFormPageObject(driver!);
    }

}

