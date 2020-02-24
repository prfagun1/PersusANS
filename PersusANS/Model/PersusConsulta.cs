using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersusANS.Model
{
    class PersusConsulta { }



    public class PersusConsultaInfo
    {
        public string nome { get; set; }
        public string versao { get; set; }
    }




    public class PersusConsultaABIs
    {
        public string nome { get; set; }
        public DateTime dataDisponibilizacao { get; set; }
    }



    public class PersusConsultaAtendimentosAptosPeticao
    {
        public int id { get; set; }
        public string abi { get; set; }
        public string numeroProcessoAdministrativo { get; set; }
        public string tipoAtendimento { get; set; }
        public string numeroAtendimento { get; set; }
        public string competencia { get; set; }
        public DateTime dataFimAtendimento { get; set; }
        public float saldoDevedorPrincipal { get; set; }
        public int instancia { get; set; }
        public DateTime prazoFinalPeticao { get; set; }
    }




    public class EProtocoloListaProtocolos
    {
        public string codigo { get; set; }
        public string numeroProcesso { get; set; }
        public int tipoRegistro { get; set; }
        public string tipoProtocolo { get; set; }
        public string assunto { get; set; }
        public int situacao { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public EProtocoloListaProtocolosResumoinformacoesadicionais resumoInformacoesAdicionais { get; set; }
        public EProtocoloListaProtocolosLink link { get; set; }
    }

    public class EProtocoloListaProtocolosResumoinformacoesadicionais
    {
        public string dataFimAtendimento { get; set; }
        public string competenciaAtendimento { get; set; }
        public string instanciaAtendimento { get; set; }
        public string numeroAtendimento { get; set; }
    }

    public class EProtocoloListaProtocolosLink
    {
        public EProtocoloListaProtocolosParams _params { get; set; }
        public string href { get; set; }
    }

    public class EProtocoloListaProtocolosParams
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }





}
