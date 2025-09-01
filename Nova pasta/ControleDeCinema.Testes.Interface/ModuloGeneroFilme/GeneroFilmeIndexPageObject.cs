
using OpenQA.Selenium;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

public class GeneroFilmeIndexPageObject
{
    private readonly IWebDriver? driver;
    public GeneroFilmeIndexPageObject(IWebDriver? driver)
    {
        this.driver = driver;
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
        driver?.FindElement(By.CssSelector("a[data-se=\"btnCadastrar\"]")).Click();

        return new GeneroFilmeFormPageObject(driver!);
    }

    public GeneroFilmeFormPageObject ClickEditar()
    {
        driver?.FindElement(By.CssSelector(".card a[title='Edição'] ")).Click();

        return new GeneroFilmeFormPageObject(driver!);
    }

    public GeneroFilmeFormPageObject ClickExcluir()
    {
        driver?.FindElement(By.CssSelector(".card a[title='Exclusão'] ")).Click();

        return new GeneroFilmeFormPageObject(driver!);
    }

}

