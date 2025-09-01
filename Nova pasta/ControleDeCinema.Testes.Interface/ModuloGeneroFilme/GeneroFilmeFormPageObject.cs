using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme
{
    public class GeneroFilmeFormPageObject
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public GeneroFilmeFormPageObject(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public GeneroFilmeFormPageObject PreencherDescricao(string descricao)
        {
            var inputDescricao = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Descricao")));
            inputDescricao.Clear();
            inputDescricao.SendKeys(descricao);
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
