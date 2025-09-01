using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using OpenQA.Selenium;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;

public class FilmeIndexPageObject
{
    private readonly IWebDriver? driver;

    public FilmeIndexPageObject(IWebDriver? driver)
    {
        this.driver = driver;
    }

    public FilmeIndexPageObject IrPara(string enderecoBase)
    {
        driver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "filmes"));
        return this;
    }

    public bool ContemFilme(string titulo)
    {
        return driver.PageSource.Contains(titulo);
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