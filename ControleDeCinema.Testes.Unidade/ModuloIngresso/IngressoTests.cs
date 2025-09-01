using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Dominio.ModuloSala;

namespace ControleDeCinema.Testes.Unidade.ModuloIngresso;

[TestClass]
[TestCategory("Testes de unidade de ingresso")]
public sealed class FilmeTests
{

    private Ingresso? filme;


    [TestMethod]
    public void Deve_Atualizar_Ingresso()
    {
        var dateTime = new DateTime(2024, 06, 10, 20, 30, 00);
        var generoFilme = new GeneroFilme("Ação");
        var filme = new Filme("Titanic", 120, false, generoFilme);
        var sala = new Sala(1, 100);
        var sessao = new Sessao(dateTime.AddHours(5), 90, filme, sala);
        var ingresso = new Ingresso(10, false, sessao);

        var dateTime2 = new DateTime(2024, 06, 10, 20, 30, 00);
        var generoFilme2 = new GeneroFilme("Ação");
        var filme2 = new Filme("Titanic", 120, false, generoFilme);
        var sala2 = new Sala(1, 100);
        var sessao2 = new Sessao(dateTime.AddHours(19), 90, filme, sala);

        Ingresso ingressoAtualizado = new Ingresso(20, false, sessao2);
        //Act
        ingresso.AtualizarRegistro(ingressoAtualizado);
        //Assert
        Assert.AreEqual(ingresso.Sessao, ingressoAtualizado.Sessao);
    }
}