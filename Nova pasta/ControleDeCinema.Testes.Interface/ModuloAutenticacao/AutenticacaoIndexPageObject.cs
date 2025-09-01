using OpenQA.Selenium;

namespace TesteFacil.Testes.Interface.ModuloDisciplina;
public sealed class AutenticacaoIndexPageObjects
{
    private readonly IWebDriver? driver;

    public AutenticacaoIndexPageObjects(IWebDriver? driver)
    {
        this.driver = driver;
    }

    public AutenticacaoIndexPageObjects IrPara(string enderecoBase)
    {
        driver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "autenticacao/registro"));

        return this;
    }
}