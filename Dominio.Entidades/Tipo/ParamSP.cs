namespace Dominio.Entidades.Tipo
{
    public class ParamSP
    {
        public string strNomParam { get; set; }
        public object strValParam { get; set; }
        public enParamIO enuDirParam { get; set; }

        public int intLongitud { get; set; }

        public bool blnEsEstructura { get; set; }
        public string strEstructuraNombre { get; set; }

        /*
        public string strNomParam { get; set; }
        public object strValParam { get; set; }
        public enParamIO enuDirParam { get; set; }
        public int intLongitud { get; set; }

        */
    }

    public enum enParamIO
    {
        Entrada = 1,
        Salida = 2,
        EntSal = 3
    }
}