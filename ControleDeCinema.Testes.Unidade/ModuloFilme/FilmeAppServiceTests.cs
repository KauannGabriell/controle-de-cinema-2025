

using ControleDeCinema.Aplicacao.ModuloSessao;
using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloSessao;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using ControleDeCinema.Aplicacao.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Infraestrutura.Orm.ModuloFilme;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Aplicacao.ModuloFilme;

namespace ControleDeCinema.Testes.Unidade.ModuloFilme;
[TestClass]
[TestCategory("Teste de unidade de da camada de aplicação do modulo filme")]
public sealed class FilmeAppServiceTests
{

    private Mock<IRepositorioFilme>? repositorioFilmeMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<FilmeAppService>>? loggerMock;
    private Mock<ITenantProvider>? tenantProviderMock;

    private FilmeAppService? filmeAppService;


    [TestInitialize]
    public void Setup()
    {
        repositorioFilmeMock = new Mock<IRepositorioFilme>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<FilmeAppService>>();
        tenantProviderMock = new Mock<ITenantProvider>();

        filmeAppService = new FilmeAppService(
           tenantProviderMock.Object,
           repositorioFilmeMock.Object,
           unitOfWorkMock.Object,
           loggerMock.Object
       );
    }


    [TestMethod]
    public void Cadastrar_DeveRetornarOk_QuandoFilmeForValido()
    {
        // Arrange

        var generoFilme = new GeneroFilme("Ação");
        var filme = new Filme("Titanic", 120, false, generoFilme);
        var filmeTeste = new Filme("Teste", 70, false, generoFilme);

        repositorioFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filmeTeste });
        //Act
        var resultado = filmeAppService?.Cadastrar(filme);

        //Assert
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoFilmeForDuplicado()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
        var filme = new Filme("Titanic", 120, false, generoFilme);
        var filmeTeste = new Filme("Titanic", 120, false, generoFilme);

        repositorioFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filmeTeste });

        // Act
        var resultado = filmeAppService?.Cadastrar(filme);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Never);

        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
        var filme = new Filme("Titanic", 120, false, generoFilme);

        repositorioFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>());

        unitOfWorkMock?
            .Setup(r => r.Commit())
            .Throws(new Exception("Erro Esperado"));

        // Act
        var resultado = filmeAppService?.Cadastrar(filme);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Once);

        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.IsSuccess);

        var mensagemErro = resultado.Errors.First().Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Editar_DeveRetornarOk_QuandoFilmeForValido()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");

        var filme = new Filme("Titanic", 120, false, generoFilme);
        var filmeTeste = new Filme("Superman", 70, false, generoFilme);
        var filmeEditado = new Filme("Sombras da Vida", 80, false, generoFilme);

        repositorioFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filmeTeste });

        //Act
        var resultado = filmeAppService?.Editar(filme.Id, filmeEditado);

        //Assert
        repositorioFilmeMock?.Verify(r => r.Editar(filme.Id, filmeEditado), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Editar_DeveRetornarFalha_QuandoFilmeForDuplicado()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");

        var filme = new Filme("Titanic", 120, false, generoFilme);
        var filmeTeste = new Filme("Titanic", 120, false, generoFilme);
        var filmeEditado = new Filme("Titanic", 120, false, generoFilme);

        repositorioFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filmeTeste });

        // Act
        var resultado = filmeAppService?.Editar(filme.Id, filmeEditado);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Editar(filme.Id, filmeEditado), Times.Never);

        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Editar_DeveRetornarFalha_QuandoExcecaoForLancada()
    {

        // Arrange
        var generoFilme = new GeneroFilme("Ação");

        var filme = new Filme("Titanic", 120, false, generoFilme);
        var filmeEditado = new Filme("Superman", 70, false, generoFilme);

        repositorioFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>());

        unitOfWorkMock?
            .Setup(r => r.Commit())
            .Throws(new Exception("Erro Esperado"));

        // Act
        var resultado = filmeAppService?.Editar(filme.Id, filmeEditado);

        // Assert
        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);
        repositorioFilmeMock?.Verify(r => r.Editar(filme.Id, filmeEditado), Times.Once);

        Assert.IsNotNull(resultado);

        var mensagemErro = resultado.Errors.First().Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }
}

