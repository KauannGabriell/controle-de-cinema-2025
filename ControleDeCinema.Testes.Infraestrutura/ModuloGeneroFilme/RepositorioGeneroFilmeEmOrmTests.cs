using Docker.DotNet.Models;
using Duobingo.Testes.Integracao.Compartilhado;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Duobingo.Testes.Integracao.ModuloDisciplina
{
    [TestClass]

    [TestCategory("Testes de Integração de disciplina")]
    public sealed class RepositorioDisciplinaEmOrmTests : TestFixture
    {

        [TestMethod]
        public void Deve_Cadastrar_Registro_Corretamente()
        {

            // Arrange
            var disciplina = new Disciplina("Matematica");

            //Act
            repositorioDisciplina.CadastrarRegistro(disciplina);

            dbContext.SaveChanges();
            // Assert   
            var registroSelecionado = repositorioDisciplina.SelecionarRegistroPorId(disciplina.Id);

            Assert.AreEqual(disciplina, registroSelecionado);
        }


        [TestMethod]
        public void Deve_Selecionar_Registros_Corretamente()
        {
            // Padrão AAA

            // Arrange  - Arranjo
            var disciplina1 = new Disciplina("Matematica");
            var disciplina2 = new Disciplina("Portugues");
            var disciplina3 = new Disciplina("Historia");

            repositorioDisciplina.CadastrarRegistro(disciplina1);
            repositorioDisciplina.CadastrarRegistro(disciplina2);
            repositorioDisciplina.CadastrarRegistro(disciplina3);

            dbContext.SaveChanges();

            List<Disciplina> disciplinasEsperadas = [disciplina1, disciplina2, disciplina3];

            var disciplinasEsperadasOrdenadas = disciplinasEsperadas
                .OrderBy(d => d.Nome)
                .ToList();

            // Act - Ação
            var disciplinasRecebidas = repositorioDisciplina.SelecionarRegistros();

            var disciplinasRecebidasOrdenadas = disciplinasRecebidas
                .OrderBy(d => d.Nome)
                .ToList();

            // Assert - Verificação
            CollectionAssert.AreEqual(disciplinasEsperadasOrdenadas, disciplinasRecebidasOrdenadas);
        }

        [TestMethod]
        public void Deve_Editar_Registro_Corretamente()
        {

            // Arrange
            var disciplina = new Disciplina("Matematica");
            repositorioDisciplina.CadastrarRegistro(disciplina);
            dbContext.SaveChanges();

            var disciplinaEditada = new Disciplina("Matematica Avancada");

            //Act

            var conseguiuEditar = repositorioDisciplina.EditarRegistro(disciplina.Id, disciplinaEditada);
            dbContext.SaveChanges();


            //Assert

            var registroSelecionado = repositorioDisciplina.SelecionarRegistroPorId(disciplina.Id);
            Assert.IsTrue(conseguiuEditar);
            Assert.AreEqual(disciplina, registroSelecionado);

        }


        [TestMethod]
        public void Deve_Excluir_Registro_Corretamente()
        {

            // Arrange
            var disciplina = new Disciplina("Matematica");
            repositorioDisciplina.CadastrarRegistro(disciplina);
            dbContext.SaveChanges();

            //Act

            var conseguiuExcluir = repositorioDisciplina.ExcluirRegistro(disciplina.Id);
            dbContext.SaveChanges();

            //Assert

            var registroSelecionado = repositorioDisciplina.SelecionarRegistroPorId(disciplina.Id);
            Assert.IsTrue(conseguiuExcluir);
            Assert.IsNull(registroSelecionado);

        }

    }
}