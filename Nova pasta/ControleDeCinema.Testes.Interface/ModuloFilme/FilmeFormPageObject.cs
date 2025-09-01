using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;
public class FilmeFormPageObject
{
    private readonly IWebDriver? driver;
    private readonly WebDriverWait wait;
    public FilmeFormPageObject(IWebDriver? driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public FilmeFormPageObject PreencherTitulo(string titulo)
    {
        IWebElement inputTitulo = driver?.FindElement(By.Id("Titulo"));
        inputTitulo.Clear();
        inputTitulo.SendKeys(titulo);
        return this;
    }

    public FilmeFormPageObject PreencherDuracao(string duracao)
    {
        IWebElement inputDuracao = driver?.FindElement(By.Id("Duracao"));
        inputDuracao.Clear();
        inputDuracao.SendKeys(duracao);
        return this;
    }

    public FilmeFormPageObject SelecionarGenero(string genero)
    {
        var selectElement = new SelectElement(driver.FindElement(By.Id("GeneroId")));
        selectElement.SelectByText(genero);
        return this;
    }

    public void ConfirmarCadastro()
    {
        ClicarBotao("button.btn-primary[type='submit']");
    }
    public void ConfirmarExclusao()
    {
        ClicarBotao("button.btn-danger[type='submit']");
    }
    public void ConfirmarEdicao()
    {
        ClicarBotao("button.btn-primary[type='submit']");
    }
    private void ClicarBotao(string seletor)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        var botao = wait.Until(d =>
        {
            try
            {
                var element = d.FindElement(By.CssSelector(seletor));
                return (element.Displayed && element.Enabled) ? element : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        });
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", botao);
        try
        {
            botao.Click();
        }
        catch (ElementClickInterceptedException)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botao);
        }
    }


}