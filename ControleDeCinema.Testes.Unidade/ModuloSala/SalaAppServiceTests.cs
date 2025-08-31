using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Aplicacao.ModuloFilme;
using ControleDeCinema.Aplicacao.ModuloSala;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControleDeCinema.Testes.Unidade.ModuloSala;

[TestClass]
[TestCategory("Teste de unidade de da camada de aplicação do modulo sala")]
public sealed class SalaAppServiceTests
{

    private Mock<IRepositorioSala>? repositorioSalaMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<SalaAppService>>? loggerMock;
    private Mock<ITenantProvider>? tenantProviderMock;

    private SalaAppService? salaAppService;


    [TestInitialize]
    public void Setup()
    {
        repositorioSalaMock = new Mock<IRepositorioSala>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<SalaAppService>>();
        tenantProviderMock = new Mock<ITenantProvider>();

        salaAppService = new SalaAppService(
           tenantProviderMock.Object,
           repositorioSalaMock.Object,
           unitOfWorkMock.Object,
           loggerMock.Object
       );
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarOk_QuandoSalaForValida()
    {
        // Arrange

        var sala = new Sala(1,30);
        var salaTeste = new Sala(4,20);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>() { salaTeste });
        //Act
        var resultado = salaAppService?.Cadastrar(sala);

        //Assert
        repositorioSalaMock?.Verify(r => r.Cadastrar(sala), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoSalaForDuplicada()
    {
        // Arrange

        var sala = new Sala(1,30);
        var salaTeste = new Sala(1,30);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>() { salaTeste });

        // Act
        var resultado = salaAppService?.Cadastrar(sala);

        // Assert
        repositorioSalaMock?.Verify(r => r.Cadastrar(sala), Times.Never);

        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        // Arrange

        var sala = new Sala(1,50);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>());

        unitOfWorkMock?
            .Setup(r => r.Commit())
            .Throws(new Exception("Erro Esperado"));

        // Act
        var resultado = salaAppService?.Cadastrar(sala);

        // Assert
        repositorioSalaMock?.Verify(r => r.Cadastrar(sala), Times.Once);

        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsFalse(resultado.IsSuccess);

        var mensagemErro = resultado.Errors.First().Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Editar_DeveRetornarOk_QuandoSalaForValida()
    {
        // Arrange

        var sala = new Sala(1,30);
        var salaTeste = new Sala(2,15);
        var salaEditado = new Sala(3,25);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>() { salaTeste });

        //Act
        var resultado = salaAppService?.Editar(sala.Id, salaEditado);

        //Assert
        repositorioSalaMock?.Verify(r => r.Editar(sala.Id, salaEditado), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }
}    
