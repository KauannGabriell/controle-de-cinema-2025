using ControleDeCinema.Aplicacao.ModuloSessao;
using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloSessao;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;

namespace ControleDeCinema.Testes.Unidade.ModuloSessao;

[TestClass]
[TestCategory("Teste de unidade de da camada de aplicação do modulo ingresso")]
public sealed class IngressoAppServiceTests
{
    private Mock<IRepositorioIngresso>? repositorioIngressoMock;
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<ILogger<IngressoAppService>>? loggerMock;
    private IngressoAppService? ingressoAppService;

    [TestInitialize]
    public void Setup()
    {
        repositorioIngressoMock = new Mock<IRepositorioIngresso>();
        tenantProviderMock = new Mock<ITenantProvider>();
        loggerMock = new Mock<ILogger<IngressoAppService>>();

        ingressoAppService = new IngressoAppService(
            tenantProviderMock.Object,
            repositorioIngressoMock.Object,
            loggerMock.Object);
    }

    [TestMethod]
    public void SelecionarTodos_DeveRetornarOk_QuandoIngressoForValido()
    {
        // Arrange
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var generoFilme = new GeneroFilme("Ação");
        var filme = new Filme("Titanic", 120, false, generoFilme);
        var sala = new Sala(1, 100);

        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);

        var ingresso = new Ingresso(10, false, sessao);
        var ingressoTeste  = new Ingresso(20, false, sessao);
        var idUsuario = Guid.NewGuid();
        
        repositorioIngressoMock?
            .Setup(r => r.SelecionarRegistros(idUsuario))
            .Returns(new List<Ingresso> {ingressoTeste  });

        tenantProviderMock?
            .Setup(t => t.UsuarioId)
            .Returns(idUsuario);

        //Act
        var resultado = ingressoAppService?.SelecionarTodos();


        //Assert
        repositorioIngressoMock?.Verify(r => r.SelecionarRegistros(idUsuario), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void SelecionarTodos_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        // Arrange
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var generoFilme = new GeneroFilme("Ação");
        var filme = new Filme("Titanic", 120, false, generoFilme);
        var sala = new Sala(1, 100);

        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);

        var ingresso = new Ingresso(10, false, sessao);
        var ingressoTeste = new Ingresso(20, false, sessao);
        var idUsuario = Guid.NewGuid();

        repositorioIngressoMock?
            .Setup(r => r.SelecionarRegistros(idUsuario))
            .Throws(new Exception("Erro Esperado"));

        tenantProviderMock?
            .Setup(t => t.UsuarioId)
            .Returns(idUsuario);

        //Act
        var resultado = ingressoAppService?.SelecionarTodos();


        //Assert
        var mensagemErro = resultado.Errors.First().Message;
        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }
}

