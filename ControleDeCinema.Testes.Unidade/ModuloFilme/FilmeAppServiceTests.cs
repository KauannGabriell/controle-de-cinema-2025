

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
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<ILogger<FilmeAppService>>? loggerMock;
    private Mock<Sessao>? sessaoMock;
    private SessaoAppService? sessaoAppService;

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


        var mensagemErro = resultado.Errors.First().Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }

}

