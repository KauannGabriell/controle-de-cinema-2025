using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleDeCinema.Testes.Interface.ModuloSessao;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloSessao;

public class SessaoIndexPageObejct
{
    private readonly IWebDriver? driver;
    private readonly WebDriverWait wait;
    public SessaoIndexPageObejct(IWebDriver? driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }
    public SessaoIndexPageObejct IrPara(string enderecoBase)
    {
        driver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "sessoes"));
        return this;
    }

    public bool ContemSessao(string filme, string sala, string horario)
    {
        return driver.PageSource.Contains(filme) && driver.PageSource.Contains(sala) && driver.PageSource.Contains(horario);
    }

    public SessaoFormPageObject ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();
        return new SessaoFormPageObject(driver!);
    }

    public SessaoFormPageObject ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnEditar']"))).Click();
        return new SessaoFormPageObject(driver!);
    }

    public SessaoFormPageObject ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnExcluir']"))).Click();
        return new SessaoFormPageObject(driver!);
    }
}
