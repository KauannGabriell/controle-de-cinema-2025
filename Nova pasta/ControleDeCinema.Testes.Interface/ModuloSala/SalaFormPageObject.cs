using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme
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

        public SalaFormPageObject PreencherNome(string nome)
        {
            var inputNome = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Número")));
            inputNome.Clear();
            inputNome.SendKeys(nome);
            return this;
        }
        public SalaFormPageObject PreencherCapacidade(string capacidade)
        {
            var inputCapacidade = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Capacidade")));
            inputCapacidade.Clear();
            inputCapacidade.SendKeys(capacidade);
            return this;
        }


        public void ConfirmarCadastro()
        {
            ClicarBotao("button.btn-primary[type='submit']");
        }

        public void ConfirmarExclusao()
        {
            ClicarBotao("button.btn-danger[type='submit']");
        }

        private void ClicarBotao(string seletor)
        {
            var botao = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(seletor)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", botao);
            try
            {
                botao.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", botao);
            }
        }
    }
}