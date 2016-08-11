using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortAIO_Updater
{
    public partial class Form1 : Form
    {
        public string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public bool Installed = false;

        public Form1()
        {
            InitializeComponent();

            if (File.Exists(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.dll") && File.Exists(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.Common.dll"))
            {
                Installed = true;
                label1.Text = "Status : PortAIO installed.";
                button1.Text = "Update";
                VersionCheck();
            }
            else
            {
                Installed = false;
                button2.Enabled = false;
                button1.Text = "Download";
                label1.Text = "Status : PortAIO not installed.";
            }
        }

        public bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                    .IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!IsAdministrator())
            {
                MessageBox.Show("Please run PortAIO-Updater as administrator.");
                this.Close();
            }
        }


        public void VersionCheck()
        {
            WebClient client = new WebClient();

            if (Installed)
            {
                FileVersionInfo PortAIOVersion = FileVersionInfo.GetVersionInfo(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.dll");
                FileVersionInfo CommonVersion = FileVersionInfo.GetVersionInfo(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.Common.dll");

                if (PortAIOVersion.FileVersion == null || CommonVersion.FileVersion == null)
                {
                    label1.Text = "Status : Error";
                    MessageBox.Show("PortAIO.dll or PortAIO.Common.dll is corrupt please delete the two .dll's and re-download them.");
                    button1.Enabled = false;
                    return;
                }

                Random random = new Random();
                var PortAIOGitVersion = client.DownloadString("https://raw.githubusercontent.com/berbb/PortAIO-Updater/master/PortAIO.version" + "?random=" + random.Next().ToString());
                var PortAIOCommonGitVersion = client.DownloadString("https://raw.githubusercontent.com/berbb/PortAIO-Updater/master/PortAIO.Common.version" + "?random=" + random.Next().ToString());

                if (PortAIOVersion.FileVersion.Equals(PortAIOGitVersion) && CommonVersion.FileVersion.Equals(PortAIOCommonGitVersion))
                {
                    button1.Enabled = false;
                    label1.Text = "Status : PortAIO installed. (Up to date)";
                }
                else
                {
                    label1.Text = "Status : PortAIO installed. (Outdated)";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Installed) // Update
            {
                if (File.Exists(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.dll") && File.Exists(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.Common.dll"))
                {
                    File.Delete(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.dll");
                    File.Delete(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.Common.dll");
                }
                DownloadFiles();
            }
            else // Download
            {
                button1.Enabled = false;
                DownloadFiles();
            }
        }

        public void DownloadFiles()
        {
            string url1 = @"https://github.com/berbb/PortAIO-Updater/raw/master/PortAIO.dll";
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(new Uri(url1), AppData + @"\EloBuddy\Addons\Libraries\PortAIO.dll");
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = "Downloading PortAIO.dll (" + e.ProgressPercentage + "%)";
        }

        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string url2 = @"https://github.com/berbb/PortAIO-Updater/raw/master/PortAIO.Common.dll";
            using (WebClient wc1 = new WebClient())
            {
                wc1.DownloadProgressChanged += wc1_DownloadProgressChanged;
                wc1.DownloadFileAsync(new Uri(url2), AppData + @"\EloBuddy\Addons\Libraries\PortAIO.Common.dll");
                wc1.DownloadFileCompleted += Wc1_DownloadFileCompleted; ;
            }
        }

        void wc1_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = "Downloading PortAIO.Common.dll (" + e.ProgressPercentage + "%)";
        }

        private void Wc1_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download complete. The program will restart when you exit this box.");
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.dll") && File.Exists(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.Common.dll"))
            {
                File.Delete(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.dll");
                File.Delete(AppData + @"\EloBuddy\Addons\Libraries\PortAIO.Common.dll");
            }
            MessageBox.Show("Successfully deleted. The program will restart when you exit this box.");
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            this.Close();
        }
    }
}
