using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PersusANS.Model
{
    class EProtocolo { }



    class ImpugnacaoRecursos
    {

        public long numeroProcesso { get; set; }

        public ImpugnacaoRecursosInformacoesAdicionais informacoesAdicionais { get; set; }
    }


    class ImpugnacaoRecursosInformacoesAdicionais
    {

        public long numeroAtendimento { get; set; }

        public String competenciaAtendimento { get; set; }

        public String dataFimAtendimento { get; set; }

        //public int instanciaAtendimento { get; set; }
    }



    //Retorno do inicio do procotocolo

    public class ImpugnacaoRecursosRetorno
    {
        public string codigo { get; set; }
        public string numeroProcesso { get; set; }
        public string codOperadora { get; set; }
        public int tipoRegistro { get; set; }
        public string tipoProtocolo { get; set; }
        public string assunto { get; set; }
        public int situacao { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public ImpugnacaoRecursosRetornoInformacoesadicionais informacoesAdicionais { get; set; }
        public ImpugnacaoRecursosRetornoLink link { get; set; }
    }

    public class ImpugnacaoRecursosRetornoInformacoesadicionais
    {
        public string dataFimAtendimento { get; set; }
        public string competenciaAtendimento { get; set; }
        //public string instanciaAtendimento { get; set; }
        public string numeroAtendimento { get; set; }
    }

    public class ImpugnacaoRecursosRetornoLink
    {
        public ImpugnacaoRecursosRetornoParams _params { get; set; }
        public string href { get; set; }
    }

    public class ImpugnacaoRecursosRetornoParams
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }
    //Fim do retorno do inicio do procotocolo


    public class EProtocoloInfo
    {
        public string nome { get; set; }
        public string versao { get; set; }
    }



    public class EProtocoloListar
    {
        public string path { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public int status { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public string unidadeOrigem { get; set; }
        public int iniciaProcesso { get; set; }
        public EProtocoloInformacoesadicionais[] informacoesAdicionais { get; set; }
        public EProtocoloConfiguracaoprocesso configuracaoProcesso { get; set; }
        public EProtocoloTiposinformacoesadicionais[] tiposInformacoesAdicionais { get; set; }
    }

    public class EProtocoloConfiguracaoprocesso
    {
        public int enviaNotificacao { get; set; }
        public int manterAbertoUnidadeOrigem { get; set; }
        public string[] unidadesDestino { get; set; }
    }

    public class EProtocoloInformacoesadicionais
    {
        public string atributo { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public int status { get; set; }
        public int ordem { get; set; }
        public int parametroEntrada { get; set; }
        public int parametroSaida { get; set; }
        public int parametroResumo { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
    }

    public class EProtocoloTiposinformacoesadicionais
    {
        public string atributo { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public int status { get; set; }
        public int ordem { get; set; }
        public int parametroEntrada { get; set; }
        public int parametroSaida { get; set; }
        public int parametroResumo { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
    }



    public class EProtocoloDocumento
    {
        public string numero { get; set; }
        public EProtocoloTipodocumento tipoDocumento { get; set; }
        public string assunto { get; set; }
        public string dataDocumento { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public EProtocoloArquivo arquivo { get; set; }
    }

    public class EProtocoloTipodocumento
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int status { get; set; }
        public DateTime dataCadastro { get; set; }
    }

    public class EProtocoloArquivo
    {
        public string hash { get; set; }
        public string nome { get; set; }
        public int tamanho { get; set; }
        public EProtocoloDocumentoLink link { get; set; }
    }

    public class EProtocoloDocumentoLink
    {
        public EProtocoloDocumentoParams _params { get; set; }
        public string href { get; set; }
    }

    public class EProtocoloDocumentoParams
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }






    //Inicio ListaProtocolosIniciados

    public class EProtocoloProtocoloIniciado
    {
        public string codigo { get; set; }
        public string numeroProcesso { get; set; }
        public String tipoRegistro { get; set; }
        public string tipoProtocolo { get; set; }
        public string assunto { get; set; }
        public String situacao { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public ProtocoloIniciadoResumoinformacoesadicionais resumoInformacoesAdicionais { get; set; }
        public ProtocoloIniciadoLink link { get; set; }
    }

    public class ProtocoloIniciadoResumoinformacoesadicionais
    {
        public string dataFimAtendimento { get; set; }
        public string competenciaAtendimento { get; set; }
        public string instanciaAtendimento { get; set; }
        public string numeroAtendimento { get; set; }
    }

    public class ProtocoloIniciadoLink
    {
        public ProtocoloIniciadoParams _params { get; set; }
        public string href { get; set; }
    }

    public class ProtocoloIniciadoParams
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }

    //Fim ListaProtocolosIniciados





    //Informações EProtocolo


    public class EProtocoloInformacoes
    {
        public string codigo { get; set; }
        public string numeroProcesso { get; set; }
        public string codOperadora { get; set; }
        public int tipoRegistro { get; set; }
        public string tipoProtocolo { get; set; }
        public string assunto { get; set; }
        public int situacao { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public EProtocoloInformacoesInformacoesadicionais informacoesAdicionais { get; set; }
        public EProtocoloInformacoesLink link { get; set; }
        public EProtocoloInformacoesDocumentoprincipal documentoPrincipal { get; set; }
        public EProtocoloInformacoesDocumentoscomplementare[] documentosComplementares { get; set; }
    }

    public class EProtocoloInformacoesInformacoesadicionais
    {
        public string dataFimAtendimento { get; set; }
        public string competenciaAtendimento { get; set; }
        public string instanciaAtendimento { get; set; }
        public string numeroAtendimento { get; set; }
    }

    public class EProtocoloInformacoesLink
    {
        public EProtocoloInformacoesParams _params { get; set; }
        public string href { get; set; }
    }

    public class EProtocoloInformacoesParams
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }

    public class EProtocoloInformacoesDocumentoprincipal
    {
        public string numero { get; set; }
        public EProtocoloInformacoesTipodocumento tipoDocumento { get; set; }
        public string assunto { get; set; }
        public string dataDocumento { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public EProtocoloInformacoesArquivo arquivo { get; set; }
    }

    public class EProtocoloInformacoesTipodocumento
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int status { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
    }

    public class EProtocoloInformacoesArquivo
    {
        public string hash { get; set; }
        public string nome { get; set; }
        public int tamanho { get; set; }
        public EProtocoloInformacoesLink1 link { get; set; }
    }

    public class EProtocoloInformacoesLink1
    {
        public EProtocoloInformacoesParams1 _params { get; set; }
        public string href { get; set; }
    }

    public class EProtocoloInformacoesParams1
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }

    public class EProtocoloInformacoesDocumentoscomplementare
    {
        public string numero { get; set; }
        public EProtocoloInformacoesTipodocumento1 tipoDocumento { get; set; }
        public string assunto { get; set; }
        public string dataDocumento { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public EProtocoloInformacoesArquivo1 arquivo { get; set; }
        public int vinculado { get; set; }
    }

    public class EProtocoloInformacoesTipodocumento1
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int status { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
    }

    public class EProtocoloInformacoesArquivo1
    {
        public string hash { get; set; }
        public string nome { get; set; }
        public int tamanho { get; set; }
        public EProtocoloInformacoesLink2 link { get; set; }
    }

    public class EProtocoloInformacoesLink2
    {
        public EProtocoloInformacoesParams2 _params { get; set; }
        public string href { get; set; }
    }

    public class EProtocoloInformacoesParams2
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }


    //Fim Informações EProtocolo


    //Inicio concluir protocolo

    public class EProtocoloConcluir
    {
        public string codigo { get; set; }
        public string numero { get; set; }
        public string numeroProcesso { get; set; }
        public string codOperadora { get; set; }
        public int tipoRegistro { get; set; }
        public int situacao { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public EProtocoloConcluirInformacoesadicionais informacoesAdicionais { get; set; }
        public EProtocoloConcluirLink link { get; set; }
        public EProtocoloConcluirDocumentoprincipal documentoPrincipal { get; set; }
        public EProtocoloConcluirDocumentoscomplementare[] documentosComplementares { get; set; }
    }

    public class EProtocoloConcluirInformacoesadicionais
    {
        public long numeroAtendimento { get; set; }
        public string competenciaAtendimento { get; set; }
        public string dataFimAtendimento { get; set; }
    }

    public class EProtocoloConcluirLink
    {
        public EProtocoloConcluirParams _params { get; set; }
        public string href { get; set; }
    }

    public class EProtocoloConcluirParams
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }

    public class EProtocoloConcluirDocumentoprincipal
    {
        public string numero { get; set; }
        public EProtocoloConcluirTipodocumento tipoDocumento { get; set; }
        public string assunto { get; set; }
        public string dataDocumento { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public EProtocoloConcluirArquivo arquivo { get; set; }
    }

    public class EProtocoloConcluirTipodocumento
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int status { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
    }

    public class EProtocoloConcluirArquivo
    {
        public string hash { get; set; }
        public string nome { get; set; }
        public int tamanho { get; set; }
        public EProtocoloConcluirLink1 link { get; set; }
    }

    public class EProtocoloConcluirLink1
    {
        public EProtocoloConcluirParams1 _params { get; set; }
        public string href { get; set; }
    }

    public class EProtocoloConcluirParams1
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }

    public class EProtocoloConcluirDocumentoscomplementare
    {
        public string numero { get; set; }
        public EProtocoloConcluirTipodocumento1 tipoDocumento { get; set; }
        public string assunto { get; set; }
        public string dataDocumento { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public EProtocoloConcluirArquivo1 arquivo { get; set; }
        public int vinculado { get; set; }
    }

    public class EProtocoloConcluirTipodocumento1
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int status { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
    }

    public class EProtocoloConcluirArquivo1
    {
        public string hash { get; set; }
        public string nome { get; set; }
        public int tamanho { get; set; }
        public EProtocoloConcluirLink2 link { get; set; }
    }

    public class EProtocoloConcluirLink2
    {
        public EProtocoloConcluirParams2 _params { get; set; }
        public string href { get; set; }
    }

    public class EProtocoloConcluirParams2
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }



    //Fim concluir protocolo
}


