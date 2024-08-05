using System;
using System.Xml;
using System.IO;
using ConsolePassword;

namespace Mat.OpenRemoteConnection.UIAutomation
{
    class ConfigXML
    {
        private string xmlDocPath = "config.xml";

        public XmlDocument GetXmlDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();

            return xmlDoc;
        }

        public void TestConfigXML()
        {
            var xmlDoc = GetXmlDoc();

            if (!(File.Exists(xmlDocPath)))
            {
                var consoleHostname = GetHostname();
                var consoleDomain = GetDomain();
                var consoleUsername = GetUsername();
                var consolePassword = GetPassword();

                CreateNodes(xmlDoc);

                xmlDoc.Load(xmlDocPath);

                AddValueNodeHostname(xmlDoc, consoleHostname);
                AddValueNodeDomain(xmlDoc, consoleDomain);
                AddValueNodeUsername(xmlDoc, consoleUsername);
                AddValueNodePassword(xmlDoc, consolePassword);
            }
        }

        private string GetHostname()
        {
            Console.Write("Digite o hostname: ");
            var consoleHostname = Console.ReadLine();

            return consoleHostname;
        }

        private string GetDomain()
        {
            Console.Write("Digite o domínio: ");
            var consoleDomain = Console.ReadLine();

            return consoleDomain;
        }

        private string GetUsername()
        {
            Console.Write("Digite o usuário: ");
            var consoleUsername = Console.ReadLine();

            return consoleUsername;
        }

        private string GetPassword()
        {
            Console.Write("Digite a senha: ");
            string consolePassword = PasswordReader.ReadPassword();

            return consolePassword;
        }

        public void CreateNodes(XmlDocument xmlDoc)
        {
            XmlElement configNode;
            XmlElement hostnameNode;
            XmlElement loginNode;
            XmlElement usernameNode;
            XmlElement passwordNode;
            XmlElement domainNode;

            configNode = xmlDoc.CreateElement("Config");
            xmlDoc.AppendChild(configNode);

            hostnameNode = xmlDoc.CreateElement("Hostname");
            configNode.AppendChild(hostnameNode);

            loginNode = xmlDoc.CreateElement("Login");
            configNode.AppendChild(loginNode);

            domainNode = xmlDoc.CreateElement("Domain");
            loginNode.AppendChild(domainNode);

            usernameNode = xmlDoc.CreateElement("Username");
            loginNode.AppendChild(usernameNode);

            passwordNode = xmlDoc.CreateElement("Password");
            loginNode.AppendChild(passwordNode);

            xmlDoc.Save(xmlDocPath);
        }

        private void AddValueNodeHostname(XmlDocument xmlDoc, string consoleHostname)
        {
            XmlNode hostnameNode = xmlDoc.SelectSingleNode("/Config/Hostname");

            hostnameNode.InnerText = consoleHostname;

            xmlDoc.Save(xmlDocPath);
        }

        private void AddValueNodeDomain(XmlDocument xmlDoc, string consoleDomain)
        {
            XmlNode domainNode = xmlDoc.SelectSingleNode("/Config/Login/Domain");

            domainNode.InnerText = consoleDomain;

            xmlDoc.Save(xmlDocPath);
        }

        private void AddValueNodeUsername(XmlDocument xmlDoc, string consoleUsername)
        {
            XmlNode usernameNode = xmlDoc.SelectSingleNode("/Config/Login/Username");

            var secureUsername = consoleUsername.ToSecureString();

            string encryptUsername = secureUsername.EncryptString();

            usernameNode.InnerText = encryptUsername;

            xmlDoc.Save(xmlDocPath);
        }

        private void AddValueNodePassword(XmlDocument xmlDoc, string consolePassword)
        {
            XmlNode passwordNode = xmlDoc.SelectSingleNode("/Config/Login/Password");

            var securePassword = consolePassword.ToSecureString();

            string encryptPassword = securePassword.EncryptString();

            passwordNode.InnerText = encryptPassword;

            xmlDoc.Save(xmlDocPath);
        }

        public string SelectNodeHostname()
        {
            var xmlDoc = GetXmlDoc();

            xmlDoc.Load(xmlDocPath);

            XmlNode hostnameNode = xmlDoc.SelectSingleNode("/Config/Hostname");

            var hostname = hostnameNode.InnerText;

            return hostname;
        }

        public string SelectNodeDomain()
        {
            var xmlDoc = GetXmlDoc();

            xmlDoc.Load(xmlDocPath);

            XmlNode domainNode = xmlDoc.SelectSingleNode("/Config/Login/Domain");

            var domain = domainNode.InnerText;

            return domain;
        }

        public string SelectNodeUsername()
        {
            var xmlDoc = GetXmlDoc();

            xmlDoc.Load(xmlDocPath);

            XmlNode usernameLoginNode = xmlDoc.SelectSingleNode("/Config/Login/Username");

            var encryptUsername = usernameLoginNode.InnerText;

            var secureUsername = encryptUsername.DecryptString();

            var username = secureUsername.ToInsecureString();

            return username;
        }

        public string SelectNodePassword()
        {
            var xmlDoc = GetXmlDoc();

            xmlDoc.Load(xmlDocPath);

            XmlNode passwordLoginNode = xmlDoc.SelectSingleNode("/Config/Login/Password");

            var encryptPassword = passwordLoginNode.InnerText;

            var securePassword = encryptPassword.DecryptString();

            var password = securePassword.ToInsecureString();

            return password;
        }
    }
}
