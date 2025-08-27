
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;

namespace ControleDeCinema.Testes.Unidade.ModuloGeneroFIlme;


[TestClass]
[TestCategory("Testes de unidade de sessão filme")]
public sealed class SessaoTests
{

    private Sessao? sessao;


    [TestMethod]
    public void Deve_Atualizar_Sessao()
    {

        //Arrange
        DateTime dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        DateTime dateTimeAtualizado = new DateTime(2025, 06, 10, 20, 30, 00);
        var numeroIngressos = 5;

        GeneroFilme generoFilme = new GeneroFilme("Ação");
        Filme filme = new Filme("Titanic", 120, false, generoFilme);
        Sala sala = new Sala(1, 100);

        sessao = new Sessao(dateTime, numeroIngressos, filme, sala);
        Sessao SessaoEditada = new Sessao(dateTimeAtualizado, numeroIngressos, filme, sala);

        //Act
        sessao.AtualizarRegistro(SessaoEditada);

        //Assert
        Assert.AreEqual(sessao.Inicio, SessaoEditada.Inicio);
    }

    [TestMethod]
    public void Deve_Gerar_Ingresso_Corretamente()
    {
        //Arrange
        var generoFilme = new GeneroFilme("Ação");
        var filme = new Filme("Aventuras do Rech", 120, true, generoFilme);
        var dataSessao = DateTime.Now;
        var sala = new Sala(1, 100);
        var sessao = new Sessao(dataSessao, 20, filme, sala);

        int numeroAssento = 5;
        bool meiaEntrada = false;

        //Act
        var ingressoGerado = sessao.GerarIngresso(numeroAssento, meiaEntrada);

        //Assert
        Assert.AreEqual(sessao, ingressoGerado.Sessao);
        Assert.IsTrue(sessao.Ingressos.Contains(ingressoGerado));
        Assert.AreEqual(numeroAssento, ingressoGerado.NumeroAssento);
        Assert.AreEqual(meiaEntrada, ingressoGerado.MeiaEntrada);
        Assert.AreEqual(19, sessao.ObterQuantidadeIngressosDisponiveis());
    }

    [TestMethod]
    public void Deve_Obter_Assentos_Disponiveis()
    {
        //Arrange
        var generoFilme = new GeneroFilme("Ação");
        var filme = new Filme("Aventuras do Rech", 120, true, generoFilme);
        var dataSessao = DateTime.Now;
        var sala = new Sala(1, 100);
        var sessao = new Sessao(dataSessao, 20, filme, sala);

        bool meiaEntrada = false;

        sessao.GerarIngresso(1, meiaEntrada);
        sessao.GerarIngresso(2, meiaEntrada);
        sessao.GerarIngresso(3, meiaEntrada);

        // Act  
        var assentosDisponiveis = sessao.ObterAssentosDisponiveis();

        //Assert
        var assentosEsperados = Enumerable.Range(4, 17).ToArray();
        CollectionAssert.AreEqual(assentosEsperados, assentosDisponiveis);
    }

    [TestMethod]
    public void Deve_Obter_QuantidadeDeIngressos_Disponiveis()
    {
        //Arrange
        var generoFilme = new GeneroFilme("Ação");
        var filme = new Filme("Aventuras do Rech", 120, true, generoFilme);
        var dataSessao = DateTime.Now;
        var sala = new Sala(1, 100);
        var sessao = new Sessao(dataSessao, 20, filme, sala);

        bool meiaEntrada = false;

        sessao.GerarIngresso(1, meiaEntrada);
        sessao.GerarIngresso(2, meiaEntrada);
        sessao.GerarIngresso(3, meiaEntrada);

        // Act  
        sessao.ObterQuantidadeIngressosDisponiveis();

        //Assert
        Assert.AreEqual(17, sessao.ObterQuantidadeIngressosDisponiveis());
    }

}
