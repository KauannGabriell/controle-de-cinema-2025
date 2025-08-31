using ControleDeCinema.Dominio.ModuloSala;

namespace ControleDeCinema.Testes.Unidade.ModuloSala;

[TestClass]
[TestCategory("Testes de unidade de sala")]
public sealed class SalaTests
{

    private Sala? sala;


    [TestMethod]
    public void Deve_Atualizar_Sala()
    {

        //Arrange

        sala = new Sala(1,20);
        Sala salaAtualizada = new Sala(2, 30);
        //Act
        sala.AtualizarRegistro(salaAtualizada);

        //Assert
        Assert.AreEqual(sala.Numero, salaAtualizada.Numero);
    }
}
