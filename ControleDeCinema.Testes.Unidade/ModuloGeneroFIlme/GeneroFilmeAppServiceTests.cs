
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
}


