

using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Infraestrutura.Orm.Compartilhado;
using ControleDeCinema.Infraestrutura.Orm.ModuloFilme;
using ControleDeCinema.Infraestrutura.Orm.ModuloGeneroFilme;
using ControleDeCinema.Infraestrutura.Orm.ModuloSala;
using ControleDeCinema.Infraestrutura.Orm.ModuloSessao;
using FizzWare.NBuilder;
using System.ComponentModel;
using Testcontainers.MsSql;

namespace Duobingo.Testes.Integracao.Compartilhado
{

    [TestClass]
    public abstract class TestFixture
    {
        protected ControleDeCinemaDbContext dbContext;
        protected RepositorioGeneroFilmeEmOrm repositorioGenero;
        protected RepositorioIngressoEmOrm repositorioIngresso;
        protected RepositorioFilmeEmOrm repositorioFilme;
        protected RepositorioSessaoEmOrm repositorioSessao;
        protected RepositorioSalaEmOrm repositorioSala;

        private static MsSqlContainer container;
        public TestFixture()
        {

        }

        private static async Task IniciarBancoDadosAsync()
        {
            await container.StartAsync();
        }
        private static async Task EncerrarBancoDadosAsync()
        {
            await EncerrarBancoDadosAsync();
        }

        [AssemblyInitialize]
        public static async Task Setup(TestContext _)
        {
            container = new MsSqlBuilder()
              .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
              .WithName("Duobingo-DB-Containers")
              .WithCleanUp(true)
              .Build();

            await IniciarBancoDadosAsync();
        }


        [AssemblyCleanup]
        public static async Task Teardown()
        {
            await EncerrarBancoDadosAsync();
        }

        [TestInitialize]
        public void ConfigurarTestes()
        {
            if (container is null)
                throw new Exception("O banco dados não foi inicializado");

            dbContext = ControleDeCinemaDbContextFactory.CriarDbContext(container.GetConnectionString());
            ConfigurarTabelas(dbContext);


            BuilderSetup.SetCreatePersistenceMethod<GeneroFilme>(repositorioGenero.Cadastrar);
            BuilderSetup.SetCreatePersistenceMethod<Filme>(repositorioFilme.Cadastrar);
            BuilderSetup.SetCreatePersistenceMethod<Sessao>(repositorioSessao.Cadastrar);
            BuilderSetup.SetCreatePersistenceMethod<Sala>(repositorioSala.Cadastrar);
        }


        public static void ConfigurarTabelas(ControleDeCinemaDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();
            dbContext.Filmes.RemoveRange(dbContext.Filmes);
            dbContext.GenerosFilme.RemoveRange(dbContext.GenerosFilme);
            dbContext.Sessoes.RemoveRange(dbContext.Sessoes);
            dbContext.Salas.RemoveRange(dbContext.Salas);
            dbContext.Ingressos.RemoveRange(dbContext.Ingressos);
            dbContext.SaveChanges();
        }

    }
}