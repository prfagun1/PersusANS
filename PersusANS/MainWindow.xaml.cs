using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PersusANS{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window{
        public MainWindow() {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private int posicaoAssinatura;

        private void bLocalizarPastaPDF_Click(object sender, RoutedEventArgs e) {
            FolderBrowserDialog navegarPasta = new FolderBrowserDialog();
            DialogResult response = navegarPasta.ShowDialog();
            if (response.ToString() == "OK") {
                tPastaPDF.Text = navegarPasta.SelectedPath;
                tPastaAssinados.Text = navegarPasta.SelectedPath + "\\Assinados";
            }
        }

        private void bEnviarDados_Click(object sender, RoutedEventArgs e)  {


            String token;
            String url;
            String codigoOperadora;
            String pastaLog;
            long numeroProcesso;

 //Busca parametros do arquivo de configuração
            try {
                token           = System.Configuration.ConfigurationManager.AppSettings["token"].ToString();
                url             = System.Configuration.ConfigurationManager.AppSettings["url"].ToString();
                codigoOperadora = System.Configuration.ConfigurationManager.AppSettings["codigoOperadora"].ToString();
                pastaLog        = System.Configuration.ConfigurationManager.AppSettings["pastaLog"].ToString();
            }
            catch (Exception erro) {
                tResultado.Text = "Erro ao buscar parâmetros do arquivo de configuração: " + erro.ToString();
                return;
            }

            try
            {
                numeroProcesso = lib.Metodos.GetNumeroProcesso(tXMLABI.Text);
            }
            catch(Exception erro) {
                tResultado.Text = "Erro ao ler o arquivo ABI: " + erro.ToString();
                return;
            }

            int instancia = Convert.ToInt32(((ComboBoxItem)cInstancia.SelectedItem).Tag.ToString());

            String pastaPDF = tPastaAssinados.Text;
            String pastaDocumentosComplementares = tPastatDocumentosComplementares.Text;
            String XMLABI = tXMLABI.Text;

            Thread thread = new Thread(() => IniciaEnvio(pastaPDF, pastaDocumentosComplementares, url, token, codigoOperadora, numeroProcesso, instancia, XMLABI));
            thread.Start();

        }


        public async Task UpdateText(String texto)
        {
            await this.Dispatcher.BeginInvoke(new Action(() => {
                tResultado.Text += texto;
            }), DispatcherPriority.Normal);
        }


        private async void IniciaEnvio(String pastaPDF, String pastaDocumentosComplementares, String url, String token, String codigoOperadora, long numeroProcesso, int instancia, String XMLABI)
        {

            DirectoryInfo pastaPDFInfo = new DirectoryInfo(pastaPDF);
            FileInfo[] pastaPDFFiles = pastaPDFInfo.GetFiles("*.pdf");

            DirectoryInfo pastaPDFDocumentosComplementaresInfo = new DirectoryInfo(pastaDocumentosComplementares);
            FileInfo[] pastaPDFDocumentosComplementares = pastaPDFDocumentosComplementaresInfo.GetFiles("*.pdf");

//Cria diretórios para guardar erros e arquivos enviados
            if(!Directory.Exists(pastaPDF + "\\Enviados")) Directory.CreateDirectory(pastaPDF + "\\Enviados");
            if (!Directory.Exists(pastaPDF + "\\Erro"))    Directory.CreateDirectory(pastaPDF + "\\Erro");



            foreach (FileInfo arquivo in pastaPDFFiles)
            {
                try
                {
                    await UpdateText(lib.ANS.IniciaEProtocolo(url, token, codigoOperadora, numeroProcesso, instancia, XMLABI, arquivo, pastaPDFDocumentosComplementares));
                }
                catch(Exception erro) {
                    await UpdateText("Erro encontrado: " + erro.ToString());
                }

            }
        }





        private void validaLocalAssinatura(object sender, RoutedEventArgs e) {

            System.Windows.Controls.RadioButton local = (System.Windows.Controls.RadioButton)sender;
            switch (local.Name)
            {
                case "localAssinatura1":
                    posicaoAssinatura = 1;
                    break;
                case "localAssinatura2":
                    posicaoAssinatura = 2;
                    break;
                case "localAssinatura3":
                    posicaoAssinatura = 3;
                    break;
                case "localAssinatura4":
                    posicaoAssinatura = 4;
                    break;
                case "localAssinatura5":
                    posicaoAssinatura = 5;
                    break;
                case "localAssinatura6":
                    posicaoAssinatura = 6;
                    break;
            }
        }

        private void bLocalizarPastaAssinados_Click(object sender, RoutedEventArgs e){
            FolderBrowserDialog navegarPasta = new FolderBrowserDialog();
            DialogResult response = navegarPasta.ShowDialog();
            if (response.ToString() == "OK") {
                tPastaAssinados.Text = navegarPasta.SelectedPath;
            }
        }

        private void bLocalizarDocumentosComplementares_Click(object sender, RoutedEventArgs e) {
            FolderBrowserDialog navegarPasta = new FolderBrowserDialog();
            DialogResult response = navegarPasta.ShowDialog();

            if (response.ToString() == "OK") {
                tPastatDocumentosComplementares.Text = navegarPasta.SelectedPath;
            }

        }

        private void bAssinarDocumento_Click(object sender, RoutedEventArgs e)   {
            this.AssinaDocumentos();
        }


        private void GetListaCertificados(object sender, RoutedEventArgs e) {
            lib.PDF pdf = new lib.PDF();
            foreach (String certificado in pdf.getCertList()) {
                ComboboxItem item = new ComboboxItem();
                item.Text = certificado.Replace("CN=", "").Replace("OU=", "").Replace("DC=", "").Replace("O=", "").Replace("C=", "");
                item.Value = certificado;
                cCertificado.Items.Add(item);
            }
        }


        private void AssinaDocumentos() {
            tResultado.Text = "";

            lib.PDF pdf = new lib.PDF();

            //lib Methods method = new Methods();
            ComboboxItem item = (ComboboxItem)cCertificado.SelectedItem;
            String resultOK = "Local dos arquivos assinados: " + tPastaPDF.Text + "\\Assinados" + Environment.NewLine + Environment.NewLine;
            String resultError = "";
            String result = "";


            result += pdf.signPDF(item.Value.ToString(), tPastaPDF.Text, posicaoAssinatura);
            String[] resultado = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < resultado.Length; i++) {
                if (resultado[i].EndsWith(" - Erro")){
                    resultError += resultado[i] + Environment.NewLine;
                }
                else{
                    resultOK += resultado[i] + Environment.NewLine;
                }
            }

            if (resultError != ""){
                tResultado.Text = "Os documentos abaixo apresentaram erros:" + Environment.NewLine;
                tResultado.Text += resultError;
            }
            else {
                tResultado.Text = "Nenhum documento apresentou erro.";
            }

            tResultado.Text += resultOK;
        }

        private void bRelatorios_Click(object sender, RoutedEventArgs e)
        {
            Relatorio relatorio = new Relatorio();
            relatorio.Show();
            this.Close();
        }

        private void bLocalizarXMLABI_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog navegarPasta = new OpenFileDialog();
            DialogResult response = navegarPasta.ShowDialog();

            if (response.ToString() == "OK")
            {
                tXMLABI.Text = navegarPasta.FileName;
            }
        }
    }

    public class ComboboxItem {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString() {
            return Text;
        }
    }
}
