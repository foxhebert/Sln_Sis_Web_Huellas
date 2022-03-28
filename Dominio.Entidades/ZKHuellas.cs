using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;


namespace Dominio.Entidades
{
    [DataContract()]
    [Serializable()]
    public class ZKHuellas
    {
        #region "Propiedades"

        [DataMember(Order = 1)]
        public int iIdHuella { get; set; }
        [DataMember(Order = 2)]
        public int iIdUsuario { get; set; }
        [DataMember(Order = 3)]
        public int iFingerNumber { get; set; }
        [DataMember(Order = 4)]
        public string Huella { get; set; }
        [DataMember(Order = 5)]
        public int nLngHuella { get; set; }
        [DataMember(Order = 6)]
        public DateTime dFechaHora { get; set; }
        [DataMember(Order = 7)]
        public int ifingernumberzk5000 { get; set; }
        [DataMember(Order = 8)]
        public string Huella10 { get; set; }
        [DataMember(Order = 9)]
        public int nLngHuella10 { get; set; }

        #endregion
    }
    [CollectionDataContract()]
    public class ListZKHuellas : Collection<ZKHuellas>
    {
    }
}
