using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloAutenticacao;
using ControleDeCinema.Testes.Interface.ModuloFilme;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using ControleDeCinema.Testes.Interface.ModuloSala;

namespace ControleDeCinema.Testes.Interface.ModuloSessao;

[TestClass]
[TestCategory("Teste de interface do modulo sessão")]
public sealed class SessaoInterfaceTests : TestFixture
{

    [TestMethod]
    public void Deve_Cadastrar_Sessao()
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

        var salaIndex = new SalaIndexPageObject(driver);
        salaIndex.IrPara(enderecoBase);
        salaIndex.ClickCadastrar();
        var salaForm = new SalaFormPageObject(driver);
            
        var sessaoIndex = new SessaoIndexPageObject(driver);
        sessaoIndex.IrPara(enderecoBase);
        sessaoIndex.ClickCadastrar();


        //Act
        var sessaoForm = new SessaoFormPageObject(driver);
        sessaoForm
         .PreencherHorario("14:00")
         .PreencherData("25/12/2024")
         .PreencherFilme("Vingadores")
         .PreencherSala("10")
         .ConfirmarCadastro();

        //Assert
        Assert.IsTrue(sessaoIndex.ContemSessao("25/12/2024", "14:00"));

    }
}
