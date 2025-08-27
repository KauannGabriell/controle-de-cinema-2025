using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Integracao.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeCinema.Testes.Infraestrutura.ModuloSala
{
    [TestClass]

    [TestCategory("Testes de Integração de Sala")]
    public sealed class RepositorioSalaEmOrmTests : TestFixture
    {

        [TestMethod]
        public void Deve_Cadastrar_Sala_Corretamente()
        {

            // Arrange
            var sala = new Sala(2,60);

            //Act
            repositorioSala.Cadastrar(sala);

            dbContext.SaveChanges();
            // Assert   
            var registroSelecionado = repositorioSala.SelecionarRegistroPorId(sala.Id);

            Assert.AreEqual(sala, registroSelecionado);
        }


        [TestMethod]
        public void Deve_CadastrarVarias_Salas_Corretamente()
        {

            // Arrange

            var sala = new Sala(2, 50);
            var sala2 = new Sala(4, 30);
            var sala3 = new Sala(3, 25);

            

            List<Sala> salasEsperadas = [sala, sala2, sala3];
            var salasEsperadasOrdenadas = salasEsperadas
                .OrderBy(d => d.Numero)
                .ToList();

            //Act
            repositorioSala.CadastrarEntidades(new List<Sala> { sala, sala2, sala3 });
            dbContext.SaveChanges();

            var salasRecebidas = repositorioSala.SelecionarRegistros();

            var salasRecebidasOrdenadas = salasRecebidas
               .OrderBy(d => d.Numero)
               .ToList();

            // Assert   

            CollectionAssert.AreEqual(salasEsperadasOrdenadas, salasRecebidasOrdenadas);
        }


        [TestMethod]
        public void Deve_Selecionar_Salas_Corretamente()
        {
            // Padrão AAA

            // Arrange  - Arranjo

            var sala = new Sala(2, 50);
            var sala2 = new Sala(4, 30);
            var sala3 = new Sala(3, 25);

            repositorioSala.CadastrarEntidades(new List<Sala> { sala, sala2, sala3 });
            dbContext.SaveChanges();

            List<Sala> salasEsperadas = [sala, sala2, sala3];

            var salasEsperadasOrdenadas = salasEsperadas
                .OrderBy(d => d.Numero)
                .ToList();

            // Act - Ação
            var salasRecebidas = repositorioSala.SelecionarRegistros();

            var salasRecebidasOrdenadas = salasRecebidas
                .OrderBy(d => d.Numero)
                .ToList();

            // Assert - Verificação
            CollectionAssert.AreEqual(salasEsperadasOrdenadas, salasRecebidasOrdenadas);
        }

        [TestMethod]
        public void Deve_Editar_Sala_Corretamente()
        {

            // Arrange

            var sala = new Sala(2, 60);

            repositorioSala.Cadastrar(sala);
            dbContext.SaveChanges();

            var salaEditada = new Sala(1,50);

            //Act

            var conseguiuEditar = repositorioSala.Editar(sala.Id, salaEditada);
            dbContext.SaveChanges();


            //Assert

            var registroSelecionado = repositorioSala.SelecionarRegistroPorId(sala.Id);
            Assert.IsTrue(conseguiuEditar);
            Assert.AreEqual(sala, registroSelecionado);

        }


        [TestMethod]
        public void Deve_Excluir_Sala_Corretamente()
        {

            // Arrange

            var sala = new Sala(1, 50);

            repositorioSala.Cadastrar(sala);
            dbContext.SaveChanges();

            //Act

            var conseguiuExcluir = repositorioSala.Excluir(sala.Id);
            dbContext.SaveChanges();

            //Assert

            var registroSelecionado = repositorioSala.SelecionarRegistroPorId(sala.Id);
            Assert.IsTrue(conseguiuExcluir);
            Assert.IsNull(registroSelecionado);

        }

    }
}
