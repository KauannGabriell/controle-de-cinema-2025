
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloSala;

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
    public bool ContemSala(string numeroSala)
    {
        return driver.PageSource.Contains(numeroSala);
    }

    public SalaFormPageObject ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();
        return new SalaFormPageObject(driver!);
    }

    public SalaFormPageObject ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnEditar']"))).Click();
        return new SalaFormPageObject(driver!);
    }

    public SalaFormPageObject ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnExcluir']"))).Click();
        return new SalaFormPageObject(driver!);
    }

    public void Confirmar()
    {
        var botaoConfirmar = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
            .Until(d =>
            {
                try
                {
                    var element = d.FindElement(By.CssSelector("button[type='submit']"));
                    return (element.Displayed && element.Enabled) ? element : null;
                }
                catch
                {
                    return null;
                }
            });

        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", botaoConfirmar);
        try
        {
            botaoConfirmar.Click();
        }
        catch (ElementClickInterceptedException)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botaoConfirmar);
        }
    }


}