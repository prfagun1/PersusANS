using Newtonsoft.Json;
using PersusANS.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
As APIs estão documentadas em:
https://www.ans.gov.br/apis/e-protocolo/#api-ProtocoloEletronico-iniciaProtocolo
*/

namespace PersusANS.lib
{
    class ANS
    {

        //ProtocoloEletronico - 14. Concluir Protocolo - POST
        //http://<host>/e-protocolo/operadoras/:codOperadora/:tipoRegistro/:tipoProtocolo/:assunto/concluidas
        public static bool ConcluirEProtocolo(String url, String token, String codigoOperadora, String codProtocolo, int instancia)
        {
            String urlPath = "";

            if (instancia == 1)
            {
                urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/impugnacoes/concluidas";
            }
            else
            {
                urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/recursos/concluidas";
            }

            String parametros = "Bearer " + token;

            var client = new RestClient(url);
            var request = new RestRequest(urlPath, Method.POST);

            request.AddHeader("authorization", parametros);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");


            String parametro = "{\"codProtocolo\": " + codProtocolo + "}";
            request.AddParameter("application/json", parametro, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);


            try
            {
                Model.EProtocoloConcluir eprotocoloConcluir = JsonConvert.DeserializeObject<Model.EProtocoloConcluir>(response.Content);
                lib.Metodos.SalvaLog("Conclui protocolo", response.Content, "OK");
                return true;
            }
            catch (Exception erro)
            {
                lib.Metodos.SalvaLog("Conclui protocolo", erro.ToString(), "Erro");
                return false;
            }

        }


        //ProtocoloEletronico - 10. Listar Protocolos - GET
        //http://<host>/e-protocolo/operadoras/:codOperadora/:tipoRegistro/:tipoProtocolo/:assunto
        public static List<Model.EProtocoloProtocoloIniciado> ListarProtocolos(String url, String token, String codigoOperadora, int instancia)
        {
            String urlPath = "";
            if (instancia == 1)
            {
                urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/impugnacoes";
            }
            else
            {
                urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/recursos";
            }

            String parametros = "Bearer " + token;

            var client = new RestClient(url);
            var request = new RestRequest(urlPath, Method.GET);

            request.AddHeader("authorization", parametros);
            IRestResponse response = client.Execute(request);

            List<Model.EProtocoloProtocoloIniciado> protocolosIniciados = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<Model.EProtocoloProtocoloIniciado>>(response.Content);

            for (int i = 0; i < protocolosIniciados.Count; i++)
            {
                switch (protocolosIniciados[i].situacao)
                {
                    case "1":
                        protocolosIniciados[i].situacao = "Em andamento";
                        break;
                    case "2":
                        protocolosIniciados[i].situacao = "Finalizado";
                        break;
                    case "3":
                        protocolosIniciados[i].situacao = "Cancelado";
                        break;
                    case "4":
                        protocolosIniciados[i].situacao = "Expirado";
                        break;
                }

                if (protocolosIniciados[i].tipoRegistro == "1")
                {
                    protocolosIniciados[i].tipoRegistro = "Notificação";
                }
                else
                {
                    protocolosIniciados[i].tipoRegistro = "Petição";
                }
            }

            return protocolosIniciados;
        }

        //http://<host>/e-protocolo/operadoras/:codOperadora/:tipoRegistro/:tipoProtocolo/:assunto

        public static List<Model.EProtocoloInformacoes> InformacoesProtocolo(String url, String token, String codigoOperadora, String codigoAtendimento)
        {
            String urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/impugnacoes-recursos/" + codigoAtendimento;
            String parametros = "Bearer " + token;

            var client = new RestClient(url);
            var request = new RestRequest(urlPath, Method.GET);

            request.AddHeader("authorization", parametros);
            IRestResponse response = client.Execute(request);

            List<Model.EProtocoloInformacoes> informacoesProtocolo = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<Model.EProtocoloInformacoes>>(response.Content);

            return informacoesProtocolo;
        }


        //http://<host>/e-protocolo/info
        public static String GetVersaoEProtocolo(String url, String token)
        {
            String urlPath = "e-protocolo/v2/info";
            String parametros = "Bearer " + token;

            var client = new RestClient(url);
            var request = new RestRequest(urlPath, Method.GET);

            request.AddHeader("authorization", parametros);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }



        //Atendimento - Consultar para petição - GET
        public static List<Model.PersusConsultaAtendimentosAptosPeticao> ListaAtendimentosAptosPeticao(String url, String token, String codigoOperadora)
        {
            String urlPath = "persus-consulta/operadoras/" + codigoOperadora + "/atendimentos/aptos-peticao";
            String parametros = "Bearer " + token;

            var client = new RestClient(url);
            var request = new RestRequest(urlPath, Method.GET);

            request.AddHeader("authorization", parametros);
            IRestResponse response = client.Execute(request);

            List<Model.PersusConsultaAtendimentosAptosPeticao> atendimentos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<Model.PersusConsultaAtendimentosAptosPeticao>>(response.Content);
            return atendimentos.OrderByDescending(x => x.numeroAtendimento).ToList();
        }

        //http://<host>/persus-consulta/abis
        public static List<Model.PersusConsultaAtendimentosAptosPeticao> ListaABIs(String url, String token)
        {
            String urlPath = "persus-consulta/abis";
            String parametros = "Bearer " + token;

            var client = new RestClient(url);
            var request = new RestRequest(urlPath, Method.GET);

            request.AddHeader("authorization", parametros);
            IRestResponse response = client.Execute(request);

            List<Model.PersusConsultaAtendimentosAptosPeticao> abis = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<Model.PersusConsultaAtendimentosAptosPeticao>>(response.Content);
            return abis.OrderByDescending(x => x.prazoFinalPeticao).ToList();

        }




        //Lê os arquivos para buscar os dados e enviar
        //Exemplo de nome de arquivo: 4316106592908-102016-Nome do documento.pdf
        public static String IniciaEProtocolo(String url, String token, String codigoOperadora, long numeroProcesso, int instancia, String XMLABI, FileInfo arquivo, FileInfo[] pastaPDFDocumentosComplementares)
        {
            long numeroAtendimento;
            String competenciaAtendimento;
            String codigoProtocoloEletronico;
            String retorno;
            Boolean retornoDocumento;


            String[] dados = arquivo.Name.Split('-');
            numeroAtendimento = Convert.ToInt64(dados[0]);
            competenciaAtendimento = dados[1];

            String dataFimAtendimento = lib.Metodos.GetDataFimAtentimento(XMLABI, numeroAtendimento.ToString(), competenciaAtendimento);

            //Inicia o protocolo


            codigoProtocoloEletronico = EnviaEProtocolo(url, token, codigoOperadora, numeroProcesso, dataFimAtendimento, numeroAtendimento, competenciaAtendimento, instancia);
            if (codigoProtocoloEletronico == null)
            {
                moveArquivo(arquivo.DirectoryName, arquivo.Name, true);
                return "Erro no envio do e-protocolo do arquivo " + arquivo.Name + Environment.NewLine;
            }

            //Adiciona documento principal

            retornoDocumento = EnviaDocumentoPrincipalEprotocolo(url, token, codigoOperadora, codigoProtocoloEletronico, numeroAtendimento, competenciaAtendimento, arquivo, instancia);
            if (!retornoDocumento)
            {
                moveArquivo(arquivo.DirectoryName, arquivo.Name, true);
                return "Erro ao adicionar documento principal no protocolo + " + codigoProtocoloEletronico + ": verificar log de erro para maiores detalhes" + Environment.NewLine;
            }

            //Adiciona documentos secundários
            foreach (FileInfo arquivoComplementar in pastaPDFDocumentosComplementares)
            {
                retornoDocumento = EnviaDocumentoComplementarEprotocolo(url, token, codigoOperadora, codigoProtocoloEletronico, numeroAtendimento, competenciaAtendimento, arquivoComplementar, instancia);
                if (!retornoDocumento)
                {
                    return "Erro ao adicionar documento complementar no protocolo + " + codigoProtocoloEletronico + ": " + "verificar log de erro para maiores detalhes" + Environment.NewLine;
                }
            }

            //Finaliza e-protocolo
            retornoDocumento = ConcluirEProtocolo(url, token, codigoOperadora, codigoProtocoloEletronico, instancia);
            if (!retornoDocumento)
            {
                moveArquivo(arquivo.DirectoryName, arquivo.Name, true);
                return "Conclui protocolo + " + codigoProtocoloEletronico + ": verificar log de erro para maiores detalhes" + Environment.NewLine;
            }


            moveArquivo(arquivo.DirectoryName, arquivo.Name, false);

            lib.Metodos.SalvaLog("", "Protocolo " + codigoProtocoloEletronico + " finalizado com sucesso ", "Fim");
            lib.Metodos.SalvaLog("", "-----------------------------------------------------------------------------------------------------", "Fim");

            return "Arquivo " + arquivo.Name + " enviado com sucesso, código protocolo " + codigoProtocoloEletronico + Environment.NewLine;
        }

        private static void moveArquivo(String pasta, String arquivo, Boolean erro)
        {
            if (erro)
            {
                File.Move(pasta + "\\" + arquivo, pasta + "\\Erro\\" + arquivo);
            }
            else
            {
                File.Move(pasta + "\\" + arquivo, pasta + "\\Enviados\\" + arquivo);
            }
        }


        //http://<host>/e-protocolo/operadoras/999999/peticoes/ressarcimento/impugnacoes-recursos
        public static String EnviaEProtocolo(String url, String token, String codigoOperadora, long numeroProcesso, String dataFimAtendimento, long numeroAtendimento, string competenciaAtendimento, int instancia)
        {
            String urlPath = "";

            if (instancia == 1)
            {
                urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/impugnacoes";
            }
            else
            {
                urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/recursos";
            }


            String parametros = "Bearer " + token;

            Model.ImpugnacaoRecursos ImpugnacaoRecursos = new Model.ImpugnacaoRecursos();
            Model.ImpugnacaoRecursosInformacoesAdicionais impugnacaoRecursosInformacoesAdicionais = new Model.ImpugnacaoRecursosInformacoesAdicionais();


            ImpugnacaoRecursos.numeroProcesso = numeroProcesso;

            impugnacaoRecursosInformacoesAdicionais.numeroAtendimento = numeroAtendimento;
            impugnacaoRecursosInformacoesAdicionais.competenciaAtendimento = competenciaAtendimento;
            impugnacaoRecursosInformacoesAdicionais.dataFimAtendimento = dataFimAtendimento;
            //impugnacaoRecursosInformacoesAdicionais.instanciaAtendimento = instancia;
            ImpugnacaoRecursos.informacoesAdicionais = impugnacaoRecursosInformacoesAdicionais;

            string output = JsonConvert.SerializeObject(ImpugnacaoRecursos);

            var client = new RestClient(url);
            var request = new RestRequest(urlPath, Method.POST);

            request.AddHeader("Authorization", parametros);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddParameter("application/json", output, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            try
            {
                Model.ImpugnacaoRecursosRetorno impugnacaoRecursosRetorno = JsonConvert.DeserializeObject<Model.ImpugnacaoRecursosRetorno>(response.Content);
                lib.Metodos.SalvaLog("Envia e-protocolo", response.Content, "OK");
                return impugnacaoRecursosRetorno.codigo;
            }
            catch(Exception e)
            {
                lib.Metodos.SalvaLog("Erro ao iniciar e-protocolo", response.Content, "Erro");
                return null;
            }


        }



        //http://<host>/e-protocolo/operadoras/:codOperadora/:tipoRegistro/:tipoProtocolo/:assunto/:codProtocolo/documentos/principais
        //Exemplo: http://<host>/e-protocolo/operadoras/999999/peticoes/ressarcimento/impugnacoes-recursos/99999998/documentos/principais
        public static bool EnviaDocumentoPrincipalEprotocolo(String url, String token, String codigoOperadora, String codigoProtocoloEletronico, long numeroAtendimento, String competenciaAtendimento, FileInfo fileName, int instancia)
        {
            String urlPath = "";

            if (instancia == 1)
            {
                urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/impugnacoes/" + codigoProtocoloEletronico + "/documentos/principais";
            }
            else
            {
                urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/recursos/" + codigoProtocoloEletronico + "/documentos/principais";
            }

            String parametros = "Bearer " + token;

            String hash = lib.Metodos.GetSHA256HashFileName(fileName.FullName);
            String nomeArquivo = fileName.Name;
            String assunto = "Impugnação referente ao atendimento " + numeroAtendimento + " - " + competenciaAtendimento.Substring(0, 2) + "/" + competenciaAtendimento.Substring(2);


            var client = new RestClient(url);
            var request = new RestRequest(urlPath, Method.POST);

            request.AddHeader("authorization", parametros);
            request.AddParameter("hash", hash);
            request.AddParameter("nomeArquivo", nomeArquivo);
            request.AddParameter("assunto", assunto);
            request.AddFile("arquivo", fileName.FullName);

            IRestResponse response = client.Execute(request);

            //Testa se retornou um json valido

            try
            {
                Model.EProtocoloDocumento retorno = JsonConvert.DeserializeObject<Model.EProtocoloDocumento>(response.Content);
                lib.Metodos.SalvaLog("Adiciona documento principal", response.Content, "OK");
                return true;
            }
            catch (Exception erro)
            {
                lib.Metodos.SalvaLog("Adiciona documento principal", "Mensagem ANS: " + response.Content + Environment.NewLine + "Erro programa: " + erro.ToString(), "Erro");
                return false;
            }

        }



        //http://<host>/e-protocolo/operadoras/:codOperadora/:tipoRegistro/:tipoProtocolo/:assunto/:codProtocolo/documentos/complementares
        public static bool EnviaDocumentoComplementarEprotocolo(String url, String token, String codigoOperadora, String codigoProtocoloEletronico, long numeroAtendimento, String competenciaAtendimento, FileInfo fileName, int instancia)
        {
            String urlPath = "";

            if (instancia == 1)
            {
                urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/impugnacoes/" + codigoProtocoloEletronico + "/documentos/complementares";
            }
            else
            {
                urlPath = "e-protocolo/v2/operadoras/" + codigoOperadora + "/peticoes/ressarcimento/recursos/" + codigoProtocoloEletronico + "/documentos/complementares";
            }

            String parametros = "Bearer " + token;

            String hash = lib.Metodos.GetSHA256HashFileName(fileName.FullName);
            String nomeArquivo = fileName.Name;
            String assunto = fileName.Name.Replace("." + fileName.Extension, "");


            var client = new RestClient(url);
            var request = new RestRequest(urlPath, Method.POST);

            request.AddHeader("authorization", parametros);
            request.AddParameter("hash", hash);
            request.AddParameter("nomeArquivo", nomeArquivo);
            request.AddParameter("assunto", assunto);
            request.AddParameter("tipoDocumento", "10");
            request.AddParameter("dataDocumento", fileName.LastWriteTime.ToString("yyyy-MM-dd"));
            request.AddFile("arquivo", fileName.FullName);

            IRestResponse response = client.Execute(request);

            try
            {
                Model.EProtocoloDocumento retorno = JsonConvert.DeserializeObject<Model.EProtocoloDocumento>(response.Content);
                lib.Metodos.SalvaLog("Adiciona documento complementar - " + nomeArquivo, response.Content, "OK");
                return true;
            }
            catch (Exception erro)
            {
                lib.Metodos.SalvaLog("Adiciona documento complementar - " + nomeArquivo, erro.ToString(), "Erro");
                return false;
            }

        }
    }
}
