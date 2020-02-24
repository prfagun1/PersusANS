using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace PersusANS.lib {
    class Metodos {


        public static void SalvaLog(String processo, String texto, String status) {
            String pastaLog = System.Configuration.ConfigurationManager.AppSettings["pastaLog"].ToString();
            String conteudo = "";
            DateTime agora = DateTime.Now;

            String arquivoLog = pastaLog + "\\Persus-" + agora.Day.ToString("00") + "-" + agora.Month.ToString("00") + "-" + agora.Year + ".log";

            if (!Directory.Exists(pastaLog)) Directory.CreateDirectory(pastaLog);

            if (status != "Fim")
            {
                conteudo += "Data: " + DateTime.Now.ToString() + Environment.NewLine;
                conteudo += "Processo: " + processo + Environment.NewLine;
                conteudo += "Status: " + status + Environment.NewLine;
                conteudo += "Texto: " + texto + Environment.NewLine + Environment.NewLine;
            }
            else {
                conteudo = texto + Environment.NewLine + Environment.NewLine;
            }

            File.AppendAllText(arquivoLog, conteudo);

        }


        public static String CalcularHash(string arquivo)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                using (var stream = System.IO.File.OpenRead(arquivo))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }




        public static string GetSHA256HashStream(Stream stream) {
            using (var bufferedStream = new BufferedStream(stream, 1024 * 32)) {
                var sha = new SHA256Managed();
                byte[] checksum = sha.ComputeHash(bufferedStream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
        }

        public static string GetSHA256HashFileName(String filename) {

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }

        }


        public static long GetNumeroProcesso(string xmlpath)
        {

            string numero = null;
            string xmlFile = File.ReadAllText(xmlpath);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlFile);
            XmlNodeList nodeList = xmldoc.GetElementsByTagName("numeroProcesso");
            string Short_Fall = string.Empty;
            foreach (XmlNode node in nodeList)
            {
                numero = node.InnerText;

            }

            numero = numero.Replace("/", "").Replace("-", "");
            return Convert.ToInt64(numero);
        }

        public static String GetDataFimAtentimento(string xmlpath, String atendimento, String competencia)
        {

            string xmlFile = File.ReadAllText(xmlpath);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlFile);
            XmlNodeList nodeList = xmldoc.GetElementsByTagName("prestador");
            string Short_Fall = string.Empty;
            foreach (XmlElement nodePrestador in nodeList)
            {
                XmlNodeList nodeAtendimento = nodePrestador.GetElementsByTagName("atendimento");

                foreach (XmlElement node in nodeAtendimento)
                {

                    String numeroAtendimento = node.GetElementsByTagName("numero")[0].InnerText;
                    String competenciaXML = node.GetElementsByTagName("competencia")[0].InnerText;

                    if (numeroAtendimento == atendimento && competenciaXML == competencia)
                    {
                        DateTime dataFim = Convert.ToDateTime(node.GetElementsByTagName("dataFimAtendimento")[0].InnerText);
                        return dataFim.ToString("dd-MM-yyyy");
                    }
                }


            }
            return null;
        }
    }
}
