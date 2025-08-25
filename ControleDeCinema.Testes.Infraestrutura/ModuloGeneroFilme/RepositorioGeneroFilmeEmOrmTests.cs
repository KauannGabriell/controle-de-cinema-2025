using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Integracao.Compartilhado;
using Docker.DotNet.Models;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ControleDeCinema.Testes.Integracao.ModuloGeneroFilme
{
    [TestClass]

    [TestCategory("Testes de Integração de Genero de filme")]
    public sealed class RepositorioGeneroFilmeEmOrmTests : TestFixture
    {

        [TestMethod]
        public void Deve_Cadastrar_GeneroFilme_Corretamente()
        {

            // Arrange
            var genero = new GeneroFilme("Ação");

            //Act
            repositorioGenero.Cadastrar(genero);

            dbContext.SaveChanges();
            // Assert   
            var registroSelecionado = repositorioGenero.SelecionarRegistroPorId(genero.Id);

            Assert.AreEqual(genero, registroSelecionado);
        }


        [TestMethod]
        public void Deve_Selecionar_Generos_Corretamente()
        {
            // Padrão AAA

            // Arrange  - Arranjo
            var genero = new GeneroFilme("Ação");
            var genero2 = new GeneroFilme("Romance");
            var genero3 = new GeneroFilme("Terror");

            repositorioGenero.Cadastrar(genero);
            repositorioGenero.Cadastrar(genero2);
            repositorioGenero.Cadastrar(genero3);


            dbContext.SaveChanges();

            List<GeneroFilme> generosEsperados = [genero, genero2, genero3];

            var generosEsperadosOrdenados = generosEsperados
                .OrderBy(d => d.Descricao)
                .ToList();

            // Act - Ação
            var generosRecebidos = repositorioGenero.SelecionarRegistros();

            var generosRecebidosOrdenados = generosRecebidos
                .OrderBy(d => d.Descricao)
                .ToList();

            // Assert - Verificação
            CollectionAssert.AreEqual(generosEsperadosOrdenados, generosRecebidosOrdenados);
        }

        [TestMethod]
        public void Deve_Editar_Genero_Corretamente()
        {

            // Arrange
            var genero = new GeneroFilme("Suspense");
            repositorioGenero.Cadastrar(genero);
            dbContext.SaveChanges();

            var generoEditado = new GeneroFilme("Romance");

            //Act

            var conseguiuEditar = repositorioGenero.Editar(genero.Id, generoEditado);
            dbContext.SaveChanges();


            //Assert

            var registroSelecionado = repositorioGenero.SelecionarRegistroPorId(genero.Id);
            Assert.IsTrue(conseguiuEditar);
            Assert.AreEqual(genero, registroSelecionado);

        }


        [TestMethod]
        public void Deve_Excluir_Registro_Corretamente()
        {

            // Arrange
            var genero = new GeneroFilme("Comédia");
            repositorioGenero.Cadastrar(genero);
            dbContext.SaveChanges();

            //Act

            var conseguiuExcluir = repositorioGenero.Excluir(genero.Id);
            dbContext.SaveChanges();

            //Assert

            var registroSelecionado = repositorioGenero.SelecionarRegistroPorId(genero.Id);
            Assert.IsTrue(conseguiuExcluir);
            Assert.IsNull(registroSelecionado);

        }

    }
}