using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Aplicacao.ModuloFilme;
using Microsoft.Extensions.Logging;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using Moq;


namespace ControleDeCinema.Testes.Unidade.ModuloFilme;

[TestClass]
[TestCategory("Testes de unidade de filme")]

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
}
