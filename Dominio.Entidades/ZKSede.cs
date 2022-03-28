using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Dominio.Entidades
{
    [DataContract()]
    [Serializable()]
    public class ZKSede
    {
        #region "Propiedades"
        [DataMember(Order = 1)]
        public int intIdSede { get; set; }

        [DataMember(Order = 2)]
        public string strCoLocal { get; set; }

        [DataMember(Order = 3)]
        public string strDeLocal { get; set; }

        [DataMember(Order = 4)]
        public string strDireccion { get; set; }

        [DataMember(Order = 5)]
        public bool bitActivo { get; set; }

        [DataMember(Order = 6)]
        public string strEstadoActivo { get; set; }

        [DataMember(Order = 7)]
        public string strTiempoEntreMarca { get; set; }

        #endregion
    }
    [CollectionDataContract()]
    public class ListZKSede : Collection<ZKSede>
    {
    }
}
