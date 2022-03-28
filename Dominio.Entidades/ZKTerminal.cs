using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Dominio.Entidades
{
    [DataContract()]
    [Serializable()]
    public class ZKTerminal
    {
        #region "Propiedades"

        [DataMember(Order = 1)]
        public int iIdTerminal { get; set; }

        [DataMember(Order = 2)]
        public int iNumero { get; set; }

        [DataMember(Order = 3)]
        public string sSerie { get; set; }

        [DataMember(Order = 4)]
        public string sDescripcion { get; set; }

        [DataMember(Order = 5)]
        public int bActivo { get; set; }

        [DataMember(Order = 6)]
        public string strActivo { get; set; }

        #endregion
    }
    [CollectionDataContract()]
    public class ListZKTerminal : Collection<ZKTerminal>
    {
    }
}
