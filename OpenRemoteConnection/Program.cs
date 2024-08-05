using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace Mat.OpenRemoteConnection.UIAutomation
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            try
            {
                ConfigXML configXML = new ConfigXML();
                Service service = new Service();

                Console.WriteLine("Aguarde...\n");

                configXML.TestConfigXML();

                IntPtr handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);

                var hostname = configXML.SelectNodeHostname();
                bool testPings = service.TestPings(hostname);

                if (testPings == true)
                {
                    service.StartProcess(hostname);

                    var processName = "CredentialUIBroker";
                    service.TestProcessRunning(processName);

                    AutomationElement window = service.GetApplicationElement(processName);

                    var windowName = "Segurança do Windows";
                    HideWindow(windowName);

                    service.MoreOption(window);

                    service.DifferenteAccount(window);

                    service.SetUsername(window);

                    service.SetPassword(window);

                    service.ClickOk(window);

                    processName = "CredentialUIBroker";
                    bool processTF = service.ProcessTF(processName);

                    if (processTF == true)
                    {
                        ShowWindow(windowName);
                        window = service.GetApplicationElement(processName);
                        string message = service.ErrorText(window);

                        KillWindowCredentialUI();

                        Console.WriteLine(message);

                        handle = GetConsoleWindow();
                        ShowWindow(handle, SW_SHOW);
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        processName = "mstsc";
                        service.TestProcessRunning(processName);

                        window = service.GetApplicationElement(processName);

                        service.ClickYes(window);
                    }
                }
                else
                {
                    handle = GetConsoleWindow();
                    ShowWindow(handle, SW_SHOW);
                    Thread.Sleep(5000);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Erro: " + exception.Message);

                IntPtr handle = GetConsoleWindow();
                ShowWindow(handle, SW_SHOW);

                Thread.Sleep(5000);
            }
        }

        static void HideWindow(string windowName)
        {
            IntPtr hWnd = FindWindow(null, windowName);

            ShowWindow(hWnd, SW_HIDE);
        }

        static void ShowWindow(string windowName)
        {
            IntPtr hWnd = FindWindow(null, windowName);

            ShowWindow(hWnd, SW_SHOW);
        }

        static void KillWindowCredentialUI()
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c taskkill /IM CredentialUIBroker.exe /F");
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            using (Process process = Process.Start(psi))
            {
                process.WaitForExit();
            }
        }
    }
}
