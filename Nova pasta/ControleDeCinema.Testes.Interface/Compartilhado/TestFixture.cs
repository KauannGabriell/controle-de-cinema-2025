using ControleDeCinema.Infraestrutura.Orm.Compartilhado;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TesteFacil.Testes.Interface.Compartilhado;

[TestClass]
public abstract class TestFixture
{
    protected static IWebDriver? driver;
    protected ControleDeCinemaDbContext? dbContext;

    protected static string enderecoBase = "https://localhost:7131";
    private static string connectionString = "Host=localhost;Port=1234;Database=ControleCinema;Username=postgres;Password=123456";

    [TestInitialize]
    public void ConfigurarTestes()
    {
        dbContext = ControleDeCinemaDbContextFactory.CriarDbContext(connectionString);

        ConfigurarTabelas(dbContext);

        InicializarWebDriver();
    }

    [TestCleanup]
    public void Cleanup()
    {
        EncerrarWebDriver();
    }

    private static void InicializarWebDriver()
    {
        var options = new ChromeOptions();

        driver = new ChromeDriver(options);
    }

    private static void EncerrarWebDriver()
    {
        driver?.Quit();
        driver?.Dispose();
    }

    private static void ConfigurarTabelas(ControleDeCinemaDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        dbContext.Ingressos.RemoveRange(dbContext.Ingressos);
        dbContext.Sessoes.RemoveRange(dbContext.Sessoes);
        dbContext.Salas.RemoveRange(dbContext.Salas);
        dbContext.Filmes.RemoveRange(dbContext.Filmes);
        dbContext.GenerosFilme.RemoveRange(dbContext.GenerosFilme);

        dbContext.UserRoles.RemoveRange(dbContext.UserRoles);
        dbContext.Users.RemoveRange(dbContext.Users);

        dbContext.SaveChanges();
    }
}