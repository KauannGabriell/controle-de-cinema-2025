
using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloAutenticacao;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;

[TestClass]
[TestCategory("Teste de interface do modulo filme")]
public sealed class FilmeInterfaceTests : TestFixture
{

    [TestMethod]
    public void Deve_Cadastrar_Filme()
    {

        //Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObjects(driver);
        autenticacaoIndex.IrPara(enderecoBase);
        var autenticacaoForm = new AutenticacaoFormPageObject(driver);
        autenticacaoForm
         .PreencherEmail()
         .PreencherSenha("gV12345678")
         .PreencherConfimarSenha("gV12345678")
         .PreencherTipoCliente("Empresa")
         .Confirmar();

        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver);
        generoFilmeIndex.IrPara(enderecoBase);
        generoFilmeIndex.ClickCadastrar();

        var generoFilmeForm = new GeneroFilmeFormPageObject(driver);
        generoFilmeForm
         .PreencherDescricao("Ação")
         .ConfirmarCadastro();

        var filmeIndex = new FilmeIndexPageObject(driver);
        filmeIndex.IrPara(enderecoBase);
        filmeIndex.ClickCadastrar();

        //Act
        var filmeForm = new FilmeFormPageObject(driver);

        filmeForm
         .PreencherTitulo("Vingadores")
         .PreencherDuracao("02:30")
         .SelecionarGenero("Ação")
         .ConfirmarCadastro();


        //Assert
        Assert.IsTrue(filmeIndex.ContemFilme("Vingadores"));
    }

    [TestMethod]
    public void Deve_Editar_Filme()
    {

        //Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObjects(driver);
        autenticacaoIndex.IrPara(enderecoBase);
        var autenticacaoForm = new AutenticacaoFormPageObject(driver);
        autenticacaoForm
         .PreencherEmail()
         .PreencherSenha("gV12345678")
         .PreencherConfimarSenha("gV12345678")
         .PreencherTipoCliente("Empresa")
         .Confirmar();

        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver);
        generoFilmeIndex.IrPara(enderecoBase);
        generoFilmeIndex.ClickCadastrar();

        var generoFilmeForm = new GeneroFilmeFormPageObject(driver);
        generoFilmeForm
         .PreencherDescricao("Ação")
         .ConfirmarCadastro();

        var filmeIndex = new FilmeIndexPageObject(driver);
        filmeIndex.IrPara(enderecoBase);
        filmeIndex.ClickCadastrar();

        var filmeForm = new FilmeFormPageObject(driver);

        filmeForm
         .PreencherTitulo("Vingadores")
         .PreencherDuracao("02:30")
         .SelecionarGenero("Ação")
         .ConfirmarCadastro();

        //Act
        filmeIndex.ClickEditar();

        filmeForm
         .PreencherTitulo("Vingadores 2")
         .PreencherDuracao("02:40")
         .SelecionarGenero("Ação")
         .ConfirmarCadastro();

        //Assert
        Assert.IsTrue(filmeIndex.ContemFilme("Vingadores 2"));
    }

    [TestMethod]
    public void Deve_Excluir_Filme()
    {
        //Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObjects(driver);
        autenticacaoIndex.IrPara(enderecoBase);
        var autenticacaoForm = new AutenticacaoFormPageObject(driver);
        autenticacaoForm
         .PreencherEmail()
         .PreencherSenha("gV12345678")
         .PreencherConfimarSenha("gV12345678")
         .PreencherTipoCliente("Empresa")
         .Confirmar();

        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver);
        generoFilmeIndex.IrPara(enderecoBase);
        generoFilmeIndex.ClickCadastrar();

        var generoFilmeForm = new GeneroFilmeFormPageObject(driver);
        generoFilmeForm
         .PreencherDescricao("Ação")
         .ConfirmarCadastro();

        var filmeIndex = new FilmeIndexPageObject(driver);
        filmeIndex.IrPara(enderecoBase);
        filmeIndex.ClickCadastrar();

        var filmeForm = new FilmeFormPageObject(driver);

        filmeForm
         .PreencherTitulo("Vingadores")
         .PreencherDuracao("02:30")
         .SelecionarGenero("Ação")
         .ConfirmarCadastro();

        //Act

        filmeIndex.ClickExcluir();
        filmeForm.ConfirmarExclusao();
        
        //Assert
        Assert.IsFalse(filmeIndex.ContemFilme("Vingadores"));
    }
}