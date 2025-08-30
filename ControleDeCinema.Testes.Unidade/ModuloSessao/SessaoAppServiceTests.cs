using ControleDeCinema.Dominio.ModuloAutenticacao;
using Moq;
using Microsoft.Extensions.Logging;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Aplicacao.ModuloSessao;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloSala;
using FizzWare.NBuilder;
using ControledeCinema.Dominio.Compartilhado;
using static System.Net.Mime.MediaTypeNames;

namespace ControleDeCinema.Testes.Unidade.ModuloSessao;

[TestClass]
[TestCategory("Teste de unidade da camada de aplicação do módulo sessão")]
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

    private (Filme filme, Sala sala, GeneroFilme genero) CriarObjetosPadrao()
    {
        var genero = Builder<GeneroFilme>.CreateNew().With(g => g.Descricao = "Romance").Build();
        var filme = Builder<Filme>.CreateNew()
            .With(f => f.Titulo = "As venturas de rech e tiago")
            .With(f => f.Duracao = 120)
            .With(f => f.Genero = genero)
            .Build();

        var sala = Builder<Sala>.CreateNew()
            .With(s => s.Capacidade = 100)
            .Build();

        return (filme, sala, genero);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarOk_QuandoSessaoForValida()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime, 30, filme, sala);
        var sessaoTeste = new Sessao(dateTime.AddHours(4), 20, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        var resultado = sessaoAppService?.Cadastrar(sessao);

        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoNumeroMaximoDeIngressosForMaiorQueASala()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime, 110, filme, sala);
        var sessaoTeste = new Sessao(dateTime.AddHours(2), 120, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        var resultado = sessaoAppService?.Cadastrar(sessao);

        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoSessaoForDuplicada()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime, 60, filme, sala);
        var sessaoTeste = new Sessao(dateTime, 60, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        var resultado = sessaoAppService?.Cadastrar(sessao);

        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime, 90, filme, sala);
        var sessaoTeste = new Sessao(dateTime.AddHours(2), 80, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        unitOfWorkMock?
            .Setup(r => r.Commit())
            .Throws(new Exception("Erro Esperado"));

        var resultado = sessaoAppService?.Cadastrar(sessao);

        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Once);
        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.AreEqual("Ocorreu um erro interno do servidor", resultado.Errors.First().Message);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Editar_DeveRetornarOk_QuandoSessaoForValida()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime, 90, filme, sala);
        var sessaoEditada = new Sessao(dateTime.AddHours(2), 80, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessao });

        repositorioSessaoMock?
            .Setup(r => r.Editar(sessao.Id, sessaoEditada))
            .Returns(true);

        var resultado = sessaoAppService?.Editar(sessao.Id, sessaoEditada);

        repositorioSessaoMock?.Verify(r => r.Editar(sessao.Id, sessaoEditada), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Editar_DeveRetornarFalaha_QuandoNumeroMaximoDeIngressosForMaiorQueASala()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();

        var sessao = new Sessao(dateTime, 110, filme, sala);
        var sessaoTeste = new Sessao(dateTime.AddHours(2), 100, filme, sala);
        var sessaoEditada = new Sessao(dateTime.AddHours(4), 120, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        var resultado = sessaoAppService?.Cadastrar(sessao);

        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Editar_DeveRetornarFalha_QuandoSessaoForDuplicada()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();

        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);
        var sessaoTeste = new Sessao(dateTime, 90, filme, sala);
        var sessaoEditada = new Sessao(dateTime, 90, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        var resultado = sessaoAppService?.Editar(sessao.Id, sessaoEditada);

        repositorioSessaoMock?.Verify(r => r.Editar(sessao.Id, sessaoEditada), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Editar_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();

        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);
        var sessaoTeste = new Sessao(dateTime, 90, filme, sala);
        var sessaoEditada = new Sessao(dateTime.AddHours(5), 92, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        repositorioSessaoMock?
            .Setup(r => r.Editar(sessao.Id, sessaoEditada))
            .Returns(true);

        unitOfWorkMock?
            .Setup(r => r.Commit())
            .Throws(new Exception("Erro Esperado"));

        var resultado = sessaoAppService?.Editar(sessao.Id, sessaoEditada);

        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);
        repositorioSessaoMock?.Verify(r => r.Editar(sessao.Id, sessaoEditada), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.AreEqual("Ocorreu um erro interno do servidor", resultado.Errors.First().Message);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Excluir_DeveRetornarOk_QuandoIdSessaoForValido()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);
        var sessaoTeste = new Sessao(dateTime, 90, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        repositorioSessaoMock?
            .Setup(r => r.Excluir(sessao.Id))
            .Returns(true);

        var resultado = sessaoAppService?.Excluir(sessao.Id);

        repositorioSessaoMock?.Verify(r => r.Excluir(sessao.Id), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Excluir_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);
        var sessaoTeste = new Sessao(dateTime, 90, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        repositorioSessaoMock?
            .Setup(r => r.Excluir(sessao.Id))
            .Returns(true);

        unitOfWorkMock?
            .Setup(r => r.Commit())
            .Throws(new Exception("Erro Esperado"));

        var resultado = sessaoAppService?.Excluir(sessao.Id);

        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);
        repositorioSessaoMock?.Verify(r => r.Excluir(sessao.Id), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.AreEqual("Ocorreu um erro interno do servidor", resultado.Errors.First().Message);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void SelecionarPorId_DeveRetornarOk_QuandoIdSessaoForValido()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);
        var sessaoTeste = new Sessao(dateTime, 90, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistroPorId(sessao.Id))
            .Returns(sessao);

        var resultado = sessaoAppService?.SelecionarPorId(sessao.Id);

        repositorioSessaoMock?.Verify(r => r.SelecionarRegistroPorId(sessao.Id), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void SelecionarPorId_DeveRetornarFalha_QuandoSessaoNaoForEncontrada()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistroPorId(sessao.Id))
            .Returns((Sessao?)null);

        var resultado = sessaoAppService?.SelecionarPorId(sessao.Id);

        repositorioSessaoMock?.Verify(r => r.SelecionarRegistroPorId(sessao.Id), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Registro não encontrado", resultado.Errors.First().Message);
    }

    [TestMethod]
    public void SelecionarTodos_DeveRetornarOk_QuandoForUmClienteRequisitando()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessaoTeste = new Sessao(dateTime, 90, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao> { sessaoTeste });

        tenantProviderMock?
            .Setup(t => t.IsInRole("Cliente"))
            .Returns(true);

        var resultado = sessaoAppService?.SelecionarTodos();

        repositorioSessaoMock?.Verify(r => r.SelecionarRegistros(), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

[   TestMethod]
    public void SelecionarTodos_DeveRetornarOk_QuandoForUmaEmpresaRequisitando()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessaoTeste = new Sessao(dateTime, 90, filme, sala);
        var idUsuario = Guid.NewGuid();

        tenantProviderMock?
            .Setup(t => t.IsInRole("Empresa"))
            .Returns(true);

        tenantProviderMock?
            .Setup(t => t.UsuarioId)
            .Returns(idUsuario);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistrosDoUsuario(idUsuario))
            .Returns(new List<Sessao> { sessaoTeste });

        var resultado = sessaoAppService?.SelecionarTodos();

        repositorioSessaoMock?.Verify(r => r.SelecionarRegistrosDoUsuario(idUsuario), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Encerrar_DeveRetornarOk_QuandoSessaoForValida()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistroPorId(sessao.Id))
            .Returns(sessao);

        var resultado = sessaoAppService?.Encerrar(sessao.Id);

        unitOfWorkMock?.Verify(r => r.Commit(), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void VenderIngresso_DeveRetornarOk_QuandoSessaoForValida()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistroPorId(sessao.Id))
            .Returns(sessao);

        var resultado = sessaoAppService?.VenderIngresso(sessao.Id, 20, true);

        unitOfWorkMock?.Verify(r => r.Commit(), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void VenderIngresso_DeveRetornarFalha_QuandoSessaoFoiEncerrada()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);

        sessao.Encerrar();

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistroPorId(sessao.Id))
            .Returns(sessao);

        var resultado = sessaoAppService?.VenderIngresso(sessao.Id, 20, true);

        unitOfWorkMock?.Verify(r => r.Commit(), Times.Never);
        Assert.IsTrue(resultado.IsFailed);
    }
    [TestMethod]
    public void VenderIngresso_DeveRetornarFalha_QuandoAcentoForInvalido()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistroPorId(sessao.Id))
            .Returns(sessao);

        var resultado = sessaoAppService?.VenderIngresso(sessao.Id, 500, true);

        unitOfWorkMock?.Verify(r => r.Commit(), Times.Never);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void VenderIngresso_DeveRetornarFalha_QuandoAcentoEstaOCupado()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var (filme, sala, _) = CriarObjetosPadrao();
        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);

        var ingressoOcupado = sessao.GerarIngresso(20, false);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistroPorId(sessao.Id))
            .Returns(sessao);

        var resultado = sessaoAppService?.VenderIngresso(sessao.Id, 20, true);

        unitOfWorkMock?.Verify(r => r.Commit(), Times.Never);
        Assert.IsTrue(resultado?.IsFailed);
        Assert.AreEqual("Este assento já está ocupado.", resultado?.Errors.First().Message);
    }
}
