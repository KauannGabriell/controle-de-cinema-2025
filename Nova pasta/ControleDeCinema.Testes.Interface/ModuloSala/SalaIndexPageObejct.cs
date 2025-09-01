
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

public class SalaIndexPageObject
{
    private readonly IWebDriver? driver;
    private readonly WebDriverWait wait;
    public SalaIndexPageObject(IWebDriver? driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public SalaIndexPageObject IrPara(string enderecoBase)
    {
        driver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "salas"));
        return this;
    }
    public bool ContemSala(string nome)
    {
        return driver.PageSource.Contains(nome);
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