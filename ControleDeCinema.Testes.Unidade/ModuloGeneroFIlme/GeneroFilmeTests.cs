
using ControleDeCinema.Dominio.ModuloGeneroFilme;

namespace ControleDeCinema.Testes.Unidade.ModuloGeneroFIlme;


[TestClass]
[TestCategory("Testes de unidade de genero filme")]
public sealed  class GeneroFilmeTests
{

    private GeneroFilme? generoFilme;


    [TestMethod]
    public void Deve_Atualizar_GeneroFilme()
    {

        //Arrange
        generoFilme = new GeneroFilme("Ação");
        GeneroFilme generoFilmeAtualizado = new GeneroFilme("Comédia");
        //Act
        generoFilme.AtualizarRegistro(generoFilmeAtualizado);

        //Assert
        Assert.AreEqual(generoFilme.Descricao, generoFilmeAtualizado.Descricao);
    }
}
