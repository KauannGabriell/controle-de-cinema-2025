using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
public class GeneroFilmeFormPageObject
{
    private readonly IWebDriver? driver;
    private readonly WebDriverWait wait;
    public GeneroFilmeFormPageObject(IWebDriver? driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }
    public GeneroFilmeFormPageObject PreencherDescricao(string descricao)
    {
        IWebElement inputDescricao = driver?.FindElement(By.Id("Descricao"));

        inputDescricao.Clear();
        inputDescricao.SendKeys(descricao);

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

