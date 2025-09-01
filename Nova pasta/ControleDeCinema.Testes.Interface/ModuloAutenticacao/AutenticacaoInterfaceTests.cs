
using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloAutenticacao;

namespace ControleDeCinema.Testes.Interface.ModuloAutenticacao;

[TestClass]
[TestCategory("Teste de interface do modulo autenticação")]
public sealed class AutenticacaoInterfaceTests : TestFixture
{

    [TestMethod]
    public void Deve_Cadastrar_ContaEmpresarial()
    {
        var autenticacaoIndex = new AutenticacaoIndexPageObjects(driver);

        autenticacaoIndex.IrPara(enderecoBase);

        var autenticacaoForm = new AutenticacaoFormPageObject(driver);

        autenticacaoForm
         .PreencherEmail()
         .PreencherSenha("gV12345678")
         .PreencherConfimarSenha("gV12345678")
         .PreencherTipoCliente("Empresa")
         .Confirmar();
    }

}
