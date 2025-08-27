using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Testes.Integracao.Compartilhado;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeCinema.Testes.Infraestrutura.ModuloFilme
{
    [TestClass]

    [TestCategory("Testes de Integração de Genero de filme")]
    public sealed class RepositorioFilmeEmOrmTests : TestFixture
    {

        [TestMethod]
        public void Deve_Cadastrar_Filme_Corretamente()
        {

            // Arrange
            var generoFilme = Builder<GeneroFilme>
                .CreateNew()
                .With(g => g.Descricao = "Ficção")
                .Persist();

            var filme = Builder<Filme>
                .CreateNew()
                .With(f => f.Titulo = "A Chegada")
                .With(f => f.Duracao = 120)
                .With(f => f.Lancamento = false)
                .With(f => f.Genero = generoFilme)
                .Persist();

            //Act
            repositorioFilme.Cadastrar(filme);

            dbContext.SaveChanges();
            // Assert   
            var registroSelecionado = repositorioFilme.SelecionarRegistroPorId(filme.Id);

            Assert.AreEqual(filme, registroSelecionado);
        }


        [TestMethod]
        public void Deve_CadastrarVarios_Filme_Corretamente()
        {

            // Arrange

            var generoFilme = Builder<GeneroFilme>
                .CreateNew()
                .With(g => g.Descricao = "Ficção")
                .Persist();

            var filme = new Filme("A chegada", 60, false, generoFilme);
            var filme2 = new Filme("Homem Aranha", 123, false, generoFilme);
            var filme3 = new Filme("Batman", 130, false, generoFilme);

            List<Filme> filmesEsperados = [filme, filme2, filme3];
            var filmesEsperadosOrdenados = filmesEsperados
                .OrderBy(d => d.Titulo)
                .ToList();

            //Act
            repositorioFilme.CadastrarEntidades(new List<Filme> { filme, filme2, filme3 });
            dbContext.SaveChanges();

            var filmesRecebidos = repositorioFilme.SelecionarRegistros();

            var filmesRecebidosOrdenados = filmesRecebidos
               .OrderBy(d => d.Titulo)
               .ToList();

            // Assert   

            CollectionAssert.AreEqual(filmesEsperadosOrdenados, filmesRecebidosOrdenados);
        }


        [TestMethod]
        public void Deve_Selecionar_Filmes_Corretamente()
        {
            // Padrão AAA

            // Arrange  - Arranjo
            var generoFilme = Builder<GeneroFilme>
                .CreateNew()
                .With(g => g.Descricao = "Terror")
                .Persist();

            var filme = new Filme("O Iluminado", 60, false, generoFilme);
            var filme2 = new Filme("Corra", 123, false, generoFilme);
            var filme3 = new Filme("Armadilha", 130, false, generoFilme);

            repositorioFilme.CadastrarEntidades(new List<Filme> { filme, filme2, filme3 });
            dbContext.SaveChanges();

            List<Filme> filmeEsperados = [filme, filme2, filme3];

            var filmesEsperadosOrdenados = filmeEsperados
                .OrderBy(d => d.Titulo)
                .ToList();

            // Act - Ação
            var filmesRecebidos = repositorioFilme.SelecionarRegistros();

            var filmesRecebidosOrdenados = filmesRecebidos
                .OrderBy(d => d.Titulo)
                .ToList();

            // Assert - Verificação
            CollectionAssert.AreEqual(filmesEsperadosOrdenados, filmesRecebidosOrdenados);
        }

        [TestMethod]
        public void Deve_Editar_Filme_Corretamente()
        {

            // Arrange

            var generoFilme = Builder<GeneroFilme>
                .CreateNew()
                .With(g => g.Descricao = "Terror")
                .Persist();

            var filme = new Filme("O Iluminado", 60, false, generoFilme);
            repositorioFilme.Cadastrar(filme);
            dbContext.SaveChanges();

            var filmeEditado = new Filme("O Exorcista", 76, false, generoFilme);

            //Act

            var conseguiuEditar = repositorioFilme.Editar(filme.Id, filmeEditado);
            dbContext.SaveChanges();


            //Assert

            var registroSelecionado = repositorioFilme.SelecionarRegistroPorId(filme.Id);
            Assert.IsTrue(conseguiuEditar);
            Assert.AreEqual(filme, registroSelecionado);

        }


        [TestMethod]
        public void Deve_Excluir_Genero_Corretamente()
        {

            // Arrange
            var generoFilme = Builder<GeneroFilme>
                 .CreateNew()
                 .With(g => g.Descricao = "Terror")
                 .Persist();

            var filme = new Filme("O Iluminado", 60, false, generoFilme);
            repositorioFilme.Cadastrar(filme);
            dbContext.SaveChanges();

            //Act

            var conseguiuExcluir = repositorioFilme.Excluir(filme.Id);
            dbContext.SaveChanges();

            //Assert

            var registroSelecionado = repositorioFilme.SelecionarRegistroPorId(filme.Id);
            Assert.IsTrue(conseguiuExcluir);
            Assert.IsNull(registroSelecionado);

        }

    }
}
