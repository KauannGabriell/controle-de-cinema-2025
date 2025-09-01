using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleDeCinema.Testes.Interface.ModuloSessao;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloSessao;

public class SessaoIndexPageObject
{
    private readonly IWebDriver? driver;
    private readonly WebDriverWait wait;
    public SessaoIndexPageObject(IWebDriver? driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }
    public SessaoIndexPageObject IrPara(string enderecoBase)
    {
        driver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "sessoes"));
        return this;
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

    public bool ContemSessao(string horario, string data)
    {
        if (!driver.PageSource.Contains(horario) || !driver.PageSource.Contains(data))
            throw new Exception("Sessão não encontrada");

        return true;
    }
}
