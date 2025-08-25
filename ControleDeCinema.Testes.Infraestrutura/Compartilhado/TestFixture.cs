

using ControleDeCinema.Infraestrutura.Orm.Compartilhado;
using System.ComponentModel;

namespace Duobingo.Testes.Integracao.Compartilhado
{

    [TestClass]
    public abstract class TestFixture
    {
        protected ControleDeCinemaDbContext dbContext;
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

            dbContext = TesteDbContextFactory.CriarDbContext(container.GetConnectionString());
            ConfigurarTabelas(dbContext);

            repositorioQuestoes = new RepositorioQuestoesEmOrm(dbContext);
            repositorioDisciplina = new RepositorioDisciplinaEmOrm(dbContext);
            repositorioMateria = new RepositorioMateriaEmOrm(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Disciplina>(repositorioDisciplina.CadastrarRegistro);
            BuilderSetup.SetCreatePersistenceMethod<Materia>(repositorioMateria.CadastrarRegistro);
            BuilderSetup.SetCreatePersistenceMethod<Questoes>(repositorioQuestoes.CadastrarRegistro);
        }


        public static void ConfigurarTabelas(duobingoDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            dbContext.Questoes.RemoveRange(dbContext.Questoes);
            dbContext.Materias.RemoveRange(dbContext.Materias);
            dbContext.Disciplinas.RemoveRange(dbContext.Disciplinas);

            dbContext.SaveChanges();
        }

    }
}