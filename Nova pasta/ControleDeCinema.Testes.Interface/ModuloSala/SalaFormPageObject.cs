using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ControleDeCinema.Testes.Interface.ModuloSala
{
    public class SalaFormPageObject
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public SalaFormPageObject(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public SalaFormPageObject PreencherNumero(string numero)
        {
            var inputNumero = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Numero")));
            inputNumero.Clear();
            inputNumero.SendKeys(numero);
            return this;
        }

        public SalaFormPageObject PreencherCapacidade(string capacidade)
        {
            var inputCapacidade = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Capacidade")));
            inputCapacidade.Clear();
            inputCapacidade.SendKeys(capacidade);
            return this;
        }

        public GeneroFilmeFormPageObject ClickCadastrar()
        {
            driver?.FindElement(By.CssSelector("a[data-se=\"btnCadastrar\"]")).Click();

            return new GeneroFilmeFormPageObject(driver!);
        }

        public GeneroFilmeFormPageObject ClickEditar()
        {
            driver?.FindElement(By.CssSelector(".card a[title='Edição'] ")).Click();

            return new GeneroFilmeFormPageObject(driver!);
        }

        public GeneroFilmeFormPageObject ClickExcluir()
        {
            driver?.FindElement(By.CssSelector(".card a[title='Exclusão'] ")).Click();

            return new GeneroFilmeFormPageObject(driver!);
        }
    }
}
