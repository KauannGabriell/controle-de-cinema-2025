using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ControleDeCinema.Testes.Interface.ModuloSessao
{
    public class SessaoFormPageObject
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public SessaoFormPageObject(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public SessaoFormPageObject PreencherFilme(string filme)
        {
            var selectFilme = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("FilmeId")));
            var selectElement = new SelectElement(selectFilme);
            selectElement.SelectByText(filme);
            return this;
        }

        public SessaoFormPageObject PreencherData(string data)
        {
            var inputData = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Data")));
            inputData.Clear();
            inputData.SendKeys(data);
            return this;
        }


        public SessaoFormPageObject PreencherSala(string sala)
        {
            var selectSala = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SalaId")));
            var selectElement = new SelectElement(selectSala);
            selectElement.SelectByText(sala);
            return this;
        }

        public SessaoFormPageObject PreencherHorario(string horario)
        {
            var inputHorario = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Horario")));
            inputHorario.Clear();
            inputHorario.SendKeys(horario);
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