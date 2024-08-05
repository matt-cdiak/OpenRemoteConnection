using System;
using System.Threading;
using System.Diagnostics;
using System.Windows.Automation;
using System.Net.NetworkInformation;

namespace Mat.OpenRemoteConnection.UIAutomation
{
    class Service
    {
        public bool TestPings(string hostname)
        {
            Ping pingSender = new Ping();

                PingReply reply = pingSender.Send(hostname);

                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine($"Endereço: {reply.Address}");
                    Console.WriteLine($"Tempo de Resposta: {reply.RoundtripTime}ms");
                    Console.WriteLine($"Tamanho dos Dados: {reply.Buffer.Length} bytes");
                    Console.WriteLine($"TTL: {reply.Options.Ttl}\n");

                    return true;
                }
                else
                {
                    Console.WriteLine($"Falha no Ping: {reply.Status}\n");
                    return false;
                }
        }

        public void StartProcess(string hostname)
        {
            ProcessStartInfo mstsc = new ProcessStartInfo
            {
                FileName = @"mstsc",
                Arguments = $"/v:{hostname}",
                UseShellExecute = false
            };
            Process process = Process.Start(mstsc);
            process.WaitForInputIdle();
        }

        public void TestProcessRunning(string processName)
        {
            while (true)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                var processIsRunning = processes.Length > 0;

                if (processIsRunning == false)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    Thread.Sleep(1500);
                    break;
                }
            }
        }

        public bool ProcessTF(string processName)
        {
            Thread.Sleep(500);

            Process[] processes = Process.GetProcessesByName(processName);
            var processIsRunning = processes.Length > 0;

            if (processIsRunning == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public AutomationElement GetApplicationElement(string processName)
        {
            while (true)
            {
                AutomationElement desktop = AutomationElement.RootElement;
                AutomationElementCollection windows = desktop.FindAll(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));

                foreach (AutomationElement window in windows)
                {
                    if (window.Current.ProcessId == Process.GetProcessesByName(processName)[0].Id)
                    {
                        return window;
                    }
                }
            }
        }

        public void MoreOption(AutomationElement window)
        {
            var buttonName = "Mais opções";
            AutomationElement buttonAutomation = window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, buttonName));

            if (buttonAutomation != null)
            {
                InvokePattern invokePattern = (InvokePattern)buttonAutomation.GetCurrentPattern(InvokePattern.Pattern);
                invokePattern.Invoke();
            }
        }

        public void DifferenteAccount(AutomationElement window)
        {
            Condition buttonCondition = new PropertyCondition(AutomationElement.NameProperty, "Alternar para Senha da conta local ou de domínio");
            AutomationElement buttonElement = window.FindFirst(TreeScope.Descendants, buttonCondition);

            if (buttonElement != null)
            {
                object patternObj;
                if (buttonElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out patternObj))
                {
                    SelectionItemPattern selectionItemPattern = (SelectionItemPattern)patternObj;

                    selectionItemPattern.Select();
                }
            }
        }

        public void SetUsername(AutomationElement window)
        {
            ConfigXML configXML = new ConfigXML();
            var domain = configXML.SelectNodeDomain();
            var username = configXML.SelectNodeUsername();

            var userAndDomain = domain + "\\" + username;

            var textboxCondition = new PropertyCondition(AutomationElement.NameProperty, "Nome de usuário");
            var textboxElement = window.FindFirst(TreeScope.Descendants, textboxCondition);

            if (textboxElement != null)
            {
                object patternObj;
                if (textboxElement.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
                {
                    ValuePattern valuePattern = (ValuePattern)patternObj;

                    valuePattern.SetValue(userAndDomain);
                }
            }
        }

        public void SetPassword(AutomationElement window)
        {
            ConfigXML configXML = new ConfigXML();
            var password = configXML.SelectNodePassword();

            var buttonCondition = new PropertyCondition(AutomationElement.NameProperty, "Senha");
            var buttonElement = window.FindFirst(TreeScope.Descendants, buttonCondition);

            if (buttonElement != null)
            {
                object patternObj;
                if (buttonElement.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
                {
                    ValuePattern valuePattern = (ValuePattern)patternObj;

                    valuePattern.SetValue(password);
                }
            }
        }

        public void ClickOk(AutomationElement window)
        {
            var buttonName = "OK";
            var buttonMore = window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, buttonName));

            if (buttonMore != null)
            {
                InvokePattern invokePattern = (InvokePattern)buttonMore.GetCurrentPattern(InvokePattern.Pattern);
                invokePattern.Invoke();
            }
        }

        public string ErrorText(AutomationElement window)
        {
            var messageName = "ErrorText";
            var message = window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, messageName));

            return message.Current.Name;
        }

        public void ClickYes(AutomationElement window)
        {
            var buttonName = "Sim";
            var buttonMore = window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, buttonName));

            if (buttonMore != null)
            {
                InvokePattern invokePattern = (InvokePattern)buttonMore.GetCurrentPattern(InvokePattern.Pattern);
                invokePattern.Invoke();
            }
        }
    }
}
