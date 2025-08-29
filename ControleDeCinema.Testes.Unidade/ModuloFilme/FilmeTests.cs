using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloFilme;


namespace ControleDeCinema.Testes.Unidade.ModuloFilme;

[TestClass]
[TestCategory("Testes de unidade de genero filme")]
public sealed class FilmeTests
{

    private Filme? filme;


    [TestMethod]
    public void Deve_Atualizar_Filme()
    {

        //Arrange
        GeneroFilme? generoFilme = new GeneroFilme("Ação");

        filme = new Filme("Sonic 2",20,false,generoFilme);
        Filme filmeAtualizado = new Filme("Velozes & Furiosos 1", 50, false, generoFilme);
        //Act
        filme.AtualizarRegistro(filmeAtualizado);

        //Assert
        Assert.AreEqual(filme.Titulo, filmeAtualizado.Titulo);
    }
}