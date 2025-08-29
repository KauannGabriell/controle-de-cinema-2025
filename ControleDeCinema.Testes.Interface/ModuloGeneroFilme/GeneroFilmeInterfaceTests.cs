using ControleDeCinema.Testes.Interface.Compartilhado;
using OpenQA.Selenium;

namespace ControleDeCinema.Testes.Interface.ModuloDisciplina;


[TestClass]
[TestCategory("Testes de Interface de genero filme")]
public sealed class GeneroFilmeInterfaceTests : TestFixture
{

    [TestMethod]\
    public void Test()
    {
        driver?.Navigate().GoToUrl("https://localhost:7131/generos");

        var botaoCadastrar = driver?.FindElement(By.CssSelector("a[data-se='btnCadastrar']"));

        botaoCadastrar?.Click();
    }
}

