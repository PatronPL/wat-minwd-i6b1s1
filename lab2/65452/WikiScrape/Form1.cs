using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WikiScrape
{
    public partial class Form1 : Form
    {
        private IWebDriver driver;

        public Form1()
        {
            InitializeComponent();
        }

        public void Scrape(string word)
        {
            var url = "https://pl.wiktionary.org/wiki/" + word;
            driver = new ChromeDriver
            {
                Url = url
            };

            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                    By.CssSelector("dd.lang-en.fldt-znaczenia"))
                );
                var elements = driver.FindElements(By.CssSelector("dd.lang-en.fldt-znaczenia"));
                foreach (var element in elements)
                {
                    translations.Text += element.Text + System.Environment.NewLine;
                }
                try
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                        By.CssSelector("img.thumbimage"))
                    );
                    var image = driver.FindElement(By.CssSelector("img.thumbimage"));
                    var imgSrc = image.GetAttribute("src");
                    using (WebClient client = new WebClient())
                    {
                        pictureBox1.ImageLocation = imgSrc;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                catch (Exception)
                {
                }
            }
            catch(Exception)
            {
                translations.Text = "Nie znaleziono";
            }
            finally
            { 
                driver.Quit();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            translations.Text = "";
            pictureBox1.ImageLocation = null;
            Scrape(wordInput.Text);
        }
    }
}
