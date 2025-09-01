using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
namespace ControleDeCinema.Testes.Interface.ModuloAutenticacao;

public class AutenticacaoFormPageObject
{
    private readonly IWebDriver? driver;
    private readonly WebDriverWait wait;
    public AutenticacaoFormPageObject(IWebDriver? driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.FindElement(By.CssSelector("form")).Displayed);
    }

    public AutenticacaoFormPageObject PreencherEmail()
    {
        var emailAleatorio = Guid.NewGuid().ToString("N").Substring(0, 8); 
    
        var inputEmail = driver?.FindElement(By.Id("Email"));
        inputEmail?.Clear();
        inputEmail?.SendKeys($"usuario_{emailAleatorio}@teste.com");
        return this;
    }

    public AutenticacaoFormPageObject PreencherSenha(string senha)
    {
        var inputSenha = driver?.FindElement(By.Id("Senha"));
        inputSenha?.Clear();
        inputSenha?.SendKeys(senha);
        return this;
    }

    public AutenticacaoFormPageObject PreencherConfimarSenha(string confirmarSenha)
    {
        var inputConfirmarSenha = driver?.FindElement(By.Id("ConfirmarSenha"));
        inputConfirmarSenha?.Clear();
        inputConfirmarSenha?.SendKeys(confirmarSenha);
        return this;
    }

    public AutenticacaoFormPageObject PreencherTipoCliente(string tipoCliente)
    {
        var selectTipoCliente = new SelectElement(driver?.FindElement(By.Id("Tipo")));
        selectTipoCliente.SelectByText(tipoCliente);
        return this;
    }

    public void Confirmar()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        var botaoConfirmar = driver?.FindElement(By.CssSelector("button[type='submit']")); botaoConfirmar?.Click();
    }
}


