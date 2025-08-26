using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Testes.Integracao.Compartilhado;
using FizzWare.NBuilder;

namespace ControleDeCinema.Testes.Integracao.ModuloSessao
{
    [TestClass]

    [TestCategory("Testes de Integração de Sessão")]
    public sealed class RepositorioSessaoEmOrmTests : TestFixture
    {

        [TestMethod]
        public void Deve_Cadastrar_Sessao_Corretamente()
        {

            // Arrange

            var generoFilme = Builder<GeneroFilme>
                .CreateNew()
                .With(g => g.Descricao = "Ação")
                .Persist();

            var filme = Builder<Filme>
                .CreateNew()
                .With(f => f.Titulo = "Vingadores")
                .With(f => f.Duracao = 120)
                .With(f => f.Lancamento = false)
                .With(f => f.Genero = generoFilme)
                .Persist();

            var sala = Builder<Sala>
                .CreateNew()
                .With(s => s.Numero = 1)
                .With(s => s.Capacidade = 100)
                .Persist();

            DateTime dataInicio = DateTime.Now.AddHours(1);

            var sessao = new Sessao(dataInicio, 50, filme, sala);



            //Act
            repositorioSessao.Cadastrar(sessao);
            dbContext.SaveChanges();
            // Assert   
            var registroSelecionado = repositorioSessao.SelecionarRegistroPorId(sessao.Id);

            Assert.AreEqual(sessao, registroSelecionado);
        }


        [TestMethod]
        public void Deve_CadastrarVarias_Sessoes_Corretamente()
        {

            // Arrange
            var generoFilme = Builder<GeneroFilme>
                 .CreateNew()
                 .With(g => g.Descricao = "Ação")
                 .Persist();

            var filme = Builder<Filme>
                .CreateNew()
                .With(f => f.Titulo = "Vingadores")
                .With(f => f.Duracao = 120)
                .With(f => f.Lancamento = false)
                .With(f => f.Genero = generoFilme)
                .Persist();

            var sala = Builder<Sala>
                .CreateNew()
                .With(s => s.Numero = 1)
                .With(s => s.Capacidade = 100)
                .Persist();

            var sessao = new Sessao(DateTime.Now.AddHours(1), 50, filme, sala);
            var sessao2 = new Sessao(DateTime.Now.AddHours(3), 50, filme, sala);
            var sessao3 = new Sessao(DateTime.Now.AddHours(5), 50, filme, sala);


            List<Sessao> sessoesEsperadas = [sessao, sessao2, sessao3];
            var sessoesEsperadasOrdenadas = sessoesEsperadas
                .OrderBy(d => d.Inicio)
                .ToList();

            //Act
            repositorioSessao.CadastrarEntidades(new List<Sessao> { sessao, sessao2, sessao3 });
            dbContext.SaveChanges();

            var sessoesRecebidas = repositorioSessao.SelecionarRegistros();

            var sessoesRecebidasOrdenadas = sessoesRecebidas
               .OrderBy(d => d.Inicio)
               .ToList();

            // Assert   

            CollectionAssert.AreEqual(sessoesEsperadasOrdenadas, sessoesRecebidasOrdenadas);
        }


        [TestMethod]
        public void Deve_Selecionar_Sessoes_Corretamente()
        {

            // Arrange  
            var generoFilme = Builder<GeneroFilme>
                  .CreateNew()
                  .With(g => g.Descricao = "Ação")
                  .Persist();

            var filme = Builder<Filme>
                .CreateNew()
                .With(f => f.Titulo = "Vingadores")
                .With(f => f.Duracao = 120)
                .With(f => f.Lancamento = false)
                .With(f => f.Genero = generoFilme)
                .Persist();

            var sala = Builder<Sala>
                .CreateNew()
                .With(s => s.Numero = 1)
                .With(s => s.Capacidade = 100)
                .Persist();

            var sessao = new Sessao(DateTime.Now.AddHours(1), 50, filme, sala);
            var sessao2 = new Sessao(DateTime.Now.AddHours(3), 50, filme, sala);
            var sessao3 = new Sessao(DateTime.Now.AddHours(5), 50, filme, sala);

            repositorioSessao.Cadastrar(sessao);
            repositorioSessao.Cadastrar(sessao2);
            repositorioSessao.Cadastrar(sessao3);


            dbContext.SaveChanges();

            List<Sessao> sessoesEsperadas = [sessao, sessao2, sessao3];

            var sessoesEsperadasOrdenadas = sessoesEsperadas
                .OrderBy(d => d.Inicio)
                .ToList();

            // Act 
            var sessoesRecebidas = repositorioSessao.SelecionarRegistros();

            var sessoesRecebidasOrdenadas = sessoesRecebidas
                .OrderBy(d => d.Inicio)
                .ToList();

            // Assert 
            CollectionAssert.AreEqual(sessoesEsperadasOrdenadas, sessoesRecebidasOrdenadas);
        }

        [TestMethod]
        public void Deve_Editar_Sessao_Corretamente()
        {

            // Arrange
            var generoFilme = Builder<GeneroFilme>
                    .CreateNew()
                    .With(g => g.Descricao = "Ação")
                    .Persist();

            var filme = Builder<Filme>
                    .CreateNew()
                    .With(f => f.Titulo = "Vingadores")
                    .With(f => f.Duracao = 120)
                    .With(f => f.Lancamento = false)
                    .With(f => f.Genero = generoFilme)
                    .Persist();

            var filmeEditado = Builder<Filme>
                  .CreateNew()
                  .With(f => f.Titulo = "Planeta dos Gorilas Albinos")
                  .With(f => f.Duracao = 120)
                  .With(f => f.Lancamento = true)
                  .With(f => f.Genero = generoFilme)
                  .Build();

            var sala = Builder<Sala>
                    .CreateNew()
                    .With(s => s.Numero = 1)
                    .With(s => s.Capacidade = 100)
                    .Persist();

            var sessao = new Sessao(DateTime.Now.AddHours(1), 50, filme, sala);
            repositorioSessao.Cadastrar(sessao);
            dbContext.SaveChanges();

            var sessaoEditada = new Sessao(DateTime.Now.AddHours(1), 50, filmeEditado, sala);

            //Act

            var conseguiuEditar = repositorioSessao.Editar(sessao.Id, sessaoEditada);
            dbContext.SaveChanges();


            //Assert

            var registroSelecionado = repositorioSessao.SelecionarRegistroPorId(sessao.Id);
            Assert.IsTrue(conseguiuEditar);
            Assert.AreEqual(sessao, registroSelecionado);

        }


        [TestMethod]
        public void Deve_Excluir_Sessao_Corretamente()
        {

            // Arrange
            var generoFilme = Builder<GeneroFilme>
                 .CreateNew()
                 .With(g => g.Descricao = "Ação")
                 .Persist();

            var filme = Builder<Filme>
                .CreateNew()
                .With(f => f.Titulo = "Vingadores")
                .With(f => f.Duracao = 120)
                .With(f => f.Lancamento = false)
                .With(f => f.Genero = generoFilme)
                .Persist();

            var sala = Builder<Sala>
                .CreateNew()
                .With(s => s.Numero = 1)
                .With(s => s.Capacidade = 100)
                .Persist();

            var sessao = new Sessao(DateTime.Now.AddHours(1), 50, filme, sala);
            repositorioSessao.Cadastrar(sessao);
            dbContext.SaveChanges();

            //Act

            var conseguiuExcluir = repositorioSessao.Excluir(sessao.Id);
            dbContext.SaveChanges();

            //Assert

            var registroSelecionado = repositorioSessao.SelecionarRegistroPorId(sessao.Id);
            Assert.IsTrue(conseguiuExcluir);
            Assert.IsNull(registroSelecionado);

        }
    }
}