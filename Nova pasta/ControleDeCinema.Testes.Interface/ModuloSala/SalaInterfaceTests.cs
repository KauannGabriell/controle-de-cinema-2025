using TesteFacil.Testes.Interface.Compartilhado;
using TesteFacil.Testes.Interface.ModuloDisciplina;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

[TestClass]
[TestCategory("Teste de interface do modulo sala")]
public sealed class SalaInterfaceTests : TestFixture
{

    [TestMethod]
    public void Deve_Cadastrar_Sala()
    {
        //Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObjects(driver);
        autenticacaoIndex.IrPara(enderecoBase);
        var autenticacaoForm = new AutenticacaoFormPageObject(driver);
        autenticacaoForm
         //Não estava conseguindo apagar a tabela dos usuarios então fiz um metodo para criar um email aleatorio
         .PreencherEmail()
         .PreencherSenha("gV12345678")
         .PreencherConfimarSenha("gV12345678")
         .PreencherTipoCliente("Empresa")
         .Confirmar();

        var salaIndex = new SalaIndexPageObject(driver);
        salaIndex.IrPara(enderecoBase);
        salaIndex.ClickCadastrar();

        //Act
        var salaForm = new SalaFormPageObject(driver);

        salaForm
         .PreencherNumero("10")
         .PreencherCapacidade("100");

        salaIndex
         .Confirmar();
        //Assert
        Assert.IsTrue(salaIndex.ContemSala("10"));

    }
    [TestMethod]
    public void Deve_Editar_Sala()
    {
        //Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObjects(driver);
        autenticacaoIndex.IrPara(enderecoBase);
        var autenticacaoForm = new AutenticacaoFormPageObject(driver);
        autenticacaoForm
         //Não estava conseguindo apagar a tabela dos usuarios então fiz um metodo para criar um email aleatorio
         .PreencherEmail()
         .PreencherSenha("gV12345678")
         .PreencherConfimarSenha("gV12345678")
         .PreencherTipoCliente("Empresa")
         .Confirmar();
        var salaIndex = new SalaIndexPageObject(driver);
        salaIndex.IrPara(enderecoBase);
        salaIndex.ClickCadastrar();
        var salaForm = new SalaFormPageObject(driver);
        salaForm
         .PreencherNumero("10")
         .PreencherCapacidade("100")
         .ClickCadastrar();

        //Act
        salaIndex.ClickEditar();

        salaForm
         .PreencherNumero("30")
         .PreencherCapacidade("150");

        salaIndex.ClickCadastrar();

        //Assert
        Assert.IsTrue(salaIndex.ContemSala("30"));
    }
    [TestMethod]
    public void Deve_Excluir_Sala()
    {
        //Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObjects(driver);
        autenticacaoIndex.IrPara(enderecoBase);
        var autenticacaoForm = new AutenticacaoFormPageObject(driver);
        autenticacaoForm
         //Não estava conseguindo apagar a tabela dos usuarios então fiz um metodo para criar um email aleatorio
         .PreencherEmail()
         .PreencherSenha("gV12345678")
         .PreencherConfimarSenha("gV12345678")
         .PreencherTipoCliente("Empresa")
         .Confirmar();
        var salaIndex = new SalaIndexPageObject(driver);
        salaIndex.IrPara(enderecoBase);
        salaIndex.ClickCadastrar();
        var salaForm = new SalaFormPageObject(driver);
        salaForm
         .PreencherNumero("10")
         .PreencherCapacidade("100")
         .ClickCadastrar();

        //Act
        salaIndex.ClickExcluir();

        //Assert
        Assert.IsFalse(salaIndex.ContemSala("10"));
    }
}