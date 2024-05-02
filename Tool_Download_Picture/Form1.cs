using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Tool_Download_Picture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ChromeDriver driver;
        private void download_Click(object sender, EventArgs e)
        {
            try
            {
                label3.Text = "Đang tải.......";
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "All Files|*.*";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    string path = save.FileName;
                    bool folderExists = Directory.Exists(path);
                    if (!folderExists)
                        Directory.CreateDirectory(path);
                    var service = ChromeDriverService.CreateDefaultService();
                    service.HideCommandPromptWindow = true;
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--disable-extensions");
                    options.AddArgument("test-type");
                    options.AddArgument("headless");
                    driver = new ChromeDriver(service, options);
                    driver.Navigate().GoToUrl(textBox1.Text);
                    var divChapter = driver.FindElements(By.ClassName("wp-manga-chapter"));
                    int a = 0;
                    for (int i = divChapter.Count - 1; i >= 0; i--)
                    {
                        divChapter = driver.FindElements(By.ClassName("wp-manga-chapter"));
                        var tagChapter = divChapter[i].FindElement(By.TagName("a"));
                        var linkChapter = tagChapter.GetAttribute("href");
                        driver.Navigate().GoToUrl(linkChapter);
                        var divPageChapter = driver.FindElements(By.ClassName("page-break"));
                        string pathchapter = $@"{save.FileName}\chapter {a + 1}";
                        bool folderChapterExists = Directory.Exists(pathchapter);
                        if (!folderChapterExists)
                            Directory.CreateDirectory(pathchapter);
                        for (int k = 0; k < divPageChapter.Count; k++)
                        {
                            var imgPageChapter = divPageChapter[k].FindElement(By.TagName("img"));
                            var linkPictureChapter = imgPageChapter.GetAttribute("src");
                            string name = imgPageChapter.GetAttribute("alt");
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileTaskAsync(new Uri(linkPictureChapter), $@"{save.FileName}\chapter {a + 1}\{name}.png");
                            }
                        }
                        driver.Navigate().GoToUrl(textBox1.Text);
                        a++;
                    }
                    driver.Close();
                }
                label3.Text = "Chỉ tải trên saytruyen hộ em!!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi"+ ex);
            }
            MessageBox.Show("Tải hoàn tất!!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
