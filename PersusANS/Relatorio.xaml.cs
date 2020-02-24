using System;
using System.Windows;
using System.Windows.Input;

namespace PersusANS
{
    /// <summary>
    /// Lógica interna para Relatorio.xaml
    /// </summary>
    public partial class Relatorio : Window  {
        private String token;
        private String url;
        private String codigoOperadora;


        public Relatorio()  {
            InitializeComponent();

            this.InicializaRelatorios();

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            gResultado.Visibility = Visibility.Hidden;
            tResultado.Visibility = Visibility.Hidden;

            this.CarregaParametros();

        }

        private void InicializaRelatorios() {

            cRelatorio.Items.Insert(0, "Verificar se o sistema está o no ar");
            cRelatorio.Items.Insert(1, "Listar ABIs");
            cRelatorio.Items.Insert(2, "Listar protocolos");
            cRelatorio.Items.Insert(3, "Listar atendimentos aptos para petição");
        }

        public void pesquisar() {
            String json = "";
            String resultado = "";

            switch (cRelatorio.SelectedValue)
            {
                case "Verificar se o sistema está o no ar":
                    tResultado.Visibility = Visibility.Visible;
                    gResultado.Visibility = Visibility.Hidden;
                    try
                    {
                        json = lib.ANS.GetVersaoEProtocolo(url, token);
                        Model.EProtocoloInfo info = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Model.EProtocoloInfo>(json);
                        resultado = "O serviço está no ar." + Environment.NewLine + Environment.NewLine;
                        resultado += "Nome do serviço: " + info.nome + Environment.NewLine;
                        resultado += "Versão da API: " + info.versao;
                    }
                    catch
                    {
                        resultado = "Erro ao acessar o serviço.";
                    }
                    tResultado.Text = resultado;
                    break;

                case "Listar ABIs":
                    tResultado.Visibility = Visibility.Hidden;
                    gResultado.Visibility = Visibility.Visible;
                    gResultado.ItemsSource = lib.ANS.ListaABIs(url, token);
                    break;

                case "Listar protocolos":
                    tResultado.Visibility = Visibility.Hidden;
                    gResultado.Visibility = Visibility.Visible;

                    //Busca o relatório para as duas instâncias
                    var listagem = lib.ANS.ListarProtocolos(url, token, codigoOperadora, 1);
                    listagem.AddRange(lib.ANS.ListarProtocolos(url, token, codigoOperadora, 2));
                    gResultado.ItemsSource = listagem;
                    break;

                case "Listar atendimentos aptos para petição":
                    tResultado.Visibility = Visibility.Hidden;
                    gResultado.Visibility = Visibility.Visible;
                    gResultado.ItemsSource = lib.ANS.ListaAtendimentosAptosPeticao(url, token, codigoOperadora);
                    break;
            }

        }

        private void CarregaParametros() {
            //Busca parametros do arquivo de configuração
            try
            {
                token = System.Configuration.ConfigurationManager.AppSettings["token"].ToString();
                url = System.Configuration.ConfigurationManager.AppSettings["url"].ToString();
                codigoOperadora = System.Configuration.ConfigurationManager.AppSettings["codigoOperadora"].ToString();
            }
            catch (Exception erro)
            {
                tResultado.Text = "Erro ao buscar parâmetros do arquivo de configuração: " + erro.ToString();
                return;
            }

        }

        private void bPesquisar_Click(object sender, RoutedEventArgs e) {
            try
            {
                this.pesquisar();
            }
            catch(Exception erro) {
                tResultado.Text = "Erro ao realizar pesquisa: " + erro.ToString();
            }
        }


        private void bVoltar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }


    }
}
