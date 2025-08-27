
using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Aplicacao.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControleDeCinema.Testes.Unidade.ModuloGeneroFilme;

[TestClass]
[TestCategory("Testes de unidade de genero filme")]

public sealed class GeneroFilmeAppServiceTests
{

    private Mock<IRepositorioGeneroFilme>? repositorioGeneroFilmeMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<GeneroFilmeAppService>>? loggerMock;
    private Mock<ITenantProvider>? tenantProviderMock;

    private GeneroFilmeAppService? generoFilmeAppService;


    [TestInitialize]
    public void Setup()
    {
        repositorioGeneroFilmeMock = new Mock<IRepositorioGeneroFilme>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<GeneroFilmeAppService>>();
        tenantProviderMock = new Mock<ITenantProvider>();

         generoFilmeAppService = new GeneroFilmeAppService(
            tenantProviderMock.Object,
            repositorioGeneroFilmeMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object
        );
    }


    [TestMethod]
    public void Cadastrar_DeveRetornarOk_QuandoGeneroFilmeForValido()
    {
        // Arrange

        var generoFilme = new GeneroFilme("Ação");
        var generoFilmeTeste = new GeneroFilme("Teste");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>() { generoFilmeTeste });
        //Act

        var resultado = generoFilmeAppService?.Cadastrar(generoFilme);

        //Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Cadastrar(generoFilme), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }


    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoGeneroFilmeForDuplicado()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");

        var generoFilmeTeste = new GeneroFilme("Ação");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>() { generoFilmeTeste });

        // Act
        var resultado = generoFilmeAppService?.Cadastrar(generoFilme);

        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Cadastrar(generoFilme), Times.Never);

        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Comédia");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>());

        unitOfWorkMock?
            .Setup(r => r.Commit())
            .Throws(new Exception("Erro Esperado"));

        // Act
        var resultado = generoFilmeAppService?.Cadastrar(generoFilme);

        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Cadastrar(generoFilme), Times.Once);

        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);

        Assert.IsNotNull(resultado);
        

        var mensagemErro = resultado.Errors.First().Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);

        Assert.IsTrue(resultado.IsFailed);
    }


    [TestMethod]
    public void Editar_DeveRetornarOk_QuandoGeneroFilmeForValido()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
        var generoFilmeTeste = new GeneroFilme("Teste");
        var generoFilmeEditado = new GeneroFilme("Comédia");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>() { generoFilmeTeste });

        //Act
        var resultado = generoFilmeAppService?.Editar(generoFilme.Id, generoFilmeEditado);

        //Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Editar(generoFilme.Id, generoFilmeEditado), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }


    [TestMethod]
    public void Editar_DeveRetornarFalha_QuandoGeneroFilmeForDuplicado()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
        var generoFilmeTeste = new GeneroFilme("Ação");
        var generoFilmeEditado = new GeneroFilme("Ação");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>() { generoFilmeTeste });

        // Act
        var resultado = generoFilmeAppService?.Editar(generoFilme.Id, generoFilmeEditado);

        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Editar(generoFilme.Id, generoFilmeEditado), Times.Never);

        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Editar_DeveRetornarFalha_QuandoExcecaoForLancada()
    {

        // Arrange
        var generoFilme = new GeneroFilme("Comédia");
        var generoFilmeEditado = new GeneroFilme("Romance");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>());

        unitOfWorkMock?
            .Setup(r => r.Commit())
            .Throws(new Exception("Erro Esperado"));

        // Act
        var resultado = generoFilmeAppService?.Editar(generoFilme.Id, generoFilmeEditado);

        // Assert
        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);
        repositorioGeneroFilmeMock?.Verify(r => r.Editar(generoFilme.Id, generoFilmeEditado), Times.Once);

        Assert.IsNotNull(resultado);

        var mensagemErro = resultado.Errors.First().Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);

        Assert.IsTrue(resultado.IsFailed);

    }

    [TestMethod]
    public void Excluir_DeveRetornarOk_QuandoIdGeneroFilmeForValido()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
      
        //Act
        var resultado = generoFilmeAppService?.Excluir(generoFilme.Id);

        //Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Excluir(generoFilme.Id), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);


        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Excluir_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Comédia");


        unitOfWorkMock?
            .Setup(r => r.Commit())
            .Throws(new Exception("Erro Esperado"));

        // Act
        var resultado = generoFilmeAppService?.Excluir(generoFilme.Id);

        // Assert
        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);
        repositorioGeneroFilmeMock?.Verify(r => r.Excluir(generoFilme.Id), Times.Once);

        Assert.IsNotNull(resultado);

        var mensagemErro = resultado.Errors.First().Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);

        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void SelecionarPorId_DeveRetornarOk_QuandoIdGeneroFilmeForValido()
    {
        // Arrange

        var generoFilme = new GeneroFilme("Ação");
        var generoFilmeTeste = new GeneroFilme("Comedia");
        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistroPorId(generoFilme.Id))
            .Returns(generoFilmeTeste);

        //Act

        var resultado = generoFilmeAppService?.SelecionarPorId(generoFilme.Id);

        //Assert
        repositorioGeneroFilmeMock?.Verify(r => r.SelecionarRegistroPorId(generoFilme.Id), Times.Once);
        

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void SelecionarPorId_DeveRetornarFalha_QuandoGeneroFilmeNaoForEncontrado()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistroPorId(generoFilme.Id))
            .Returns((GeneroFilme?)null);

        // Act
        var resultado = generoFilmeAppService?.SelecionarPorId(generoFilme.Id);

        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.SelecionarRegistroPorId(generoFilme.Id), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);

        var erro = resultado.Errors.First();

        Assert.AreEqual("Registro não encontrado", erro.Message);
      
    }

    [TestMethod]
    public void SelecionarPorId_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Comédia");
        var generoFilmeTeste = new GeneroFilme("Comedia");
        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistroPorId(generoFilme.Id))
            .Returns(generoFilmeTeste);


        repositorioGeneroFilmeMock?
           .Setup(r => r.SelecionarRegistroPorId(generoFilme.Id))
          .Throws(new Exception("Erro Esperado"));

        // Act
        var resultado = generoFilmeAppService?.SelecionarPorId(generoFilme.Id);

        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.SelecionarRegistroPorId(generoFilme.Id), Times.Once);

        Assert.IsNotNull(resultado);

        var mensagemErro = resultado.Errors.First().Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);

        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void SelecionarTodos_DeveRetornarOk_QuandoOCorrerTudoCorretamente()
    {
        // Arrange

        var generoFilme = new GeneroFilme("Ação");
        var generoFilmeTeste = new GeneroFilme("Comedia");
        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme> { generoFilme});

        //Act

        var resultado = generoFilmeAppService?.SelecionarTodos();

        //Assert
        repositorioGeneroFilmeMock?.Verify(r => r.SelecionarRegistros(), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void SelecionarTodos_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
        var generoFilmeTeste = new GeneroFilme("Comedia");
        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme> { generoFilme });


        repositorioGeneroFilmeMock?
          .Setup(r => r.SelecionarRegistros())
          .Throws(new Exception("Erro Esperado"));

        // Act
        var resultado = generoFilmeAppService?.SelecionarTodos();

        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.SelecionarRegistros(), Times.Once);

        Assert.IsNotNull(resultado);

        var mensagemErro = resultado.Errors.First().Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);

        Assert.IsTrue(resultado.IsFailed);
    }



}



