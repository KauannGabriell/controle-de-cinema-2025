

using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using Moq;
using Microsoft.Extensions.Logging;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Aplicacao.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Aplicacao.ModuloSessao;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloSala;

namespace ControleDeCinema.Testes.Unidade.ModuloSessao;

[TestClass]
[TestCategory("Teste de unidade de da camada de aplicação do modulo sessao")]
public sealed class SessaoAppServiceTests
{
    private Mock<IRepositorioSessao>? repositorioSessaoMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<ILogger<SessaoAppService>>? loggerMock;
    private SessaoAppService? sessaoAppService;

  
    [TestInitialize]
    public void Setup()
    {
        repositorioSessaoMock = new Mock<IRepositorioSessao>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        tenantProviderMock = new Mock<ITenantProvider>();
        loggerMock = new Mock<ILogger<SessaoAppService>>();

        sessaoAppService = new SessaoAppService(
            tenantProviderMock.Object,
            repositorioSessaoMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object);
    }


    [TestMethod]
    public void Cadastrar_DeveRetornarOk_QuandoSessaoForValida()
    {
        // Arrange
        DateTime dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        GeneroFilme generoFilme = new GeneroFilme("Ação");
        Filme filme = new Filme("Titanic", 120, false, generoFilme);
        Sala sala = new Sala(1, 100);
        var sessao = new Sessao(dateTime, 30, filme, sala);
        var sessaoTeste = new Sessao(dateTime.AddHours(4), 20, filme, sala);


        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });
        //Act
        var resultado = sessaoAppService?.Cadastrar(sessao);

        //Assert
        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }


    [TestMethod]
    public void Cadastrar_DeveRetornarFalaha_QuandoNumeroMaximoDeIngressosForMaiorQueASala()
    {
        // Arrange
        DateTime dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        GeneroFilme generoFilme = new GeneroFilme("Ação");
        Filme filme = new Filme("Titanic", 120, false, generoFilme);
        Sala sala = new Sala(1, 100);

        var sessao = new Sessao(dateTime, 110, filme, sala);
        var sessaoTeste = new Sessao(dateTime.AddHours(2), 120, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });
        //Act
        var resultado = sessaoAppService?.Cadastrar(sessao);

        //Assert
        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Never);

        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoSessaoForDuplicada()
    {
        // Arrange
        DateTime dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        GeneroFilme generoFilme = new GeneroFilme("Ação");
        Filme filme = new Filme("Titanic", 120, false, generoFilme);
        Sala sala = new Sala(1, 100);

        var sessao = new Sessao(dateTime, 60, filme, sala);
        var sessaoTeste = new Sessao(dateTime, 60, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        // Act
        var resultado = sessaoAppService?.Cadastrar(sessao);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Never);

        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        // Arrange

        DateTime dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        GeneroFilme generoFilme = new GeneroFilme("Ação");
        Filme filme = new Filme("Titanic", 120, false, generoFilme);
        Sala sala = new Sala(1, 100);

        var sessao = new Sessao(dateTime, 90, filme, sala);
        var sessaoTeste = new Sessao(dateTime.AddHours(2), 80, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        unitOfWorkMock?
            .Setup(r => r.Commit())
            .Throws(new Exception("Erro Esperado"));

        // Act
        var resultado = sessaoAppService?.Cadastrar(sessao);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Once);

        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);

        Assert.IsNotNull(resultado);


        var mensagemErro = resultado.Errors.First().Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }
}

