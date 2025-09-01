
using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloAutenticacao;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

[TestClass]
[TestCategory("Teste de interface do modulo genero filme")]
public sealed class GeneroFilmeInterfaceTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_GeneroFilme()
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

        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver);
        generoFilmeIndex.IrPara(enderecoBase);
        generoFilmeIndex.ClickCadastrar();


        //Act
        var generoFilmeForm = new GeneroFilmeFormPageObject(driver);
        generoFilmeForm
         .PreencherDescricao("Ação")
         .ConfirmarCadastro();

        //Assert
        Assert.IsTrue(generoFilmeIndex.ContemGeneroFilme("Ação"));
    }

    [TestMethod]
    public void Deve_Editar_GeneroFilme()
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
        string enderecoCadastrar = "cadastrar";

        generoFilmeIndex.IrPara(enderecoBase);
        generoFilmeIndex.ClickCadastrar();

        var generoFilmeForm = new GeneroFilmeFormPageObject(driver);
        generoFilmeForm
         .PreencherDescricao("Ação")
         .ConfirmarCadastro();

     

        generoFilmeIndex.IrPara(enderecoBase);
        generoFilmeIndex.ClickEditar();

        //Act
        generoFilmeForm
         .PreencherDescricao("Comédia Editada")
         .ConfirmarCadastro();

        //Assert
        Assert.IsTrue(generoFilmeIndex.ContemGeneroFilme("Comédia Editada"));

    }
    [TestMethod]
    public void Deve_Excluir_GeneroFilme()
    {
        //Arrange
        var autenticacaoIndex = new AutenticacaoIndexPageObjects(driver);

        autenticacaoIndex.IrPara(enderecoBase);
        var autenticacaoForm = new AutenticacaoFormPageObject(driver);
        autenticacaoForm.PreencherEmail()
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

        generoFilmeIndex.IrPara(enderecoBase);

        //Act
        generoFilmeIndex.ClickExcluir();
            generoFilmeForm.ConfirmarExclusao();

        //Assert
        Assert.IsFalse(generoFilmeIndex.ContemGeneroFilme("Ação"));
    }
}
