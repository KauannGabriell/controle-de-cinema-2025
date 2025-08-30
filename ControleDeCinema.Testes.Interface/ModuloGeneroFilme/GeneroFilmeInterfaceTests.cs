using ControleDeCinema.Testes.Interface.Compartilhado;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloDisciplina;


[TestClass]
[TestCategory("Testes de Interface de genero filme")]
public sealed class GeneroFilmeInterfaceTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_GeneroFilme_Corretamente()
    {
        driver?.Navigate().GoToUrl($"{enderecoBase}/generos");

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        var botaoCadastrar = wait.Until(drv => drv.FindElement(By.CssSelector("a[data-se='btnCadastrar']")));
        botaoCadastrar.Click();
    }
}

