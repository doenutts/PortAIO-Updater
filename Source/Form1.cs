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

        private void Form1_Load(object sender, EventArgs e)
        {
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

                string url1 = @"https://github.com/berbb/PortAIO-Updater/raw/master/PortAIO.dll";
                WebClient client1 = new WebClient();
                client1.DownloadFileAsync(new Uri(url1), AppData + @"\EloBuddy\Addons\Libraries\PortAIO.dll");

                string url2 = @"https://github.com/berbb/PortAIO-Updater/raw/master/PortAIO.Common.dll";
                WebClient client2 = new WebClient();
                client2.DownloadFileAsync(new Uri(url2), AppData + @"\EloBuddy\Addons\Libraries\PortAIO.Common.dll");

                client2.DownloadFileCompleted += new AsyncCompletedEventHandler(client_UpdateFileCompleted);
            }
            else // Download
            {
                button1.Enabled = false;
                string url1 = @"https://github.com/berbb/PortAIO-Updater/raw/master/PortAIO.dll";
                WebClient client1 = new WebClient();                
                client1.DownloadFileAsync(new Uri(url1), AppData + @"\EloBuddy\Addons\Libraries\PortAIO.dll");

                string url2 = @"https://github.com/berbb/PortAIO-Updater/raw/master/PortAIO.Common.dll";
                WebClient client2 = new WebClient();
                client2.DownloadFileAsync(new Uri(url2), AppData + @"\EloBuddy\Addons\Libraries\PortAIO.Common.dll");

                client2.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            }
        }

        void client_UpdateFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Successfully Updated. The program will restart when you exit this box.");
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            this.Close();
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Successfully downloaded. The program will restart when you exit this box.");
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
