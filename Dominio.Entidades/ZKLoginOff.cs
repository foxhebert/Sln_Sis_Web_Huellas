using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;


namespace Dominio.Entidades
{
    [DataContract()]
    [Serializable()]
    public class ZKLoginOff
    {
        #region "Propiedades"

        [DataMember(Order = 1)]
        public int iIdSesion { get; set; }
        [DataMember(Order = 2)]
        public string sNombreSesion { get; set; }
        [DataMember(Order = 3)]
        public string sPasswordSesion { get; set; }
        [DataMember(Order = 4)]
        public bool bitFlSist { get; set; }
        [DataMember(Order = 5)]
        public int iPermiso { get; set; }
        [DataMember(Order = 6)]
        public int iRestaurado { get; set; }
        [DataMember(Order = 7)]
        public string sPermisoMenu { get; set; }
        [DataMember(Order = 8)]
        public int intIdSede { get; set; }
        [DataMember(Order = 9)]
        public DateTime dttFecReg { get; set; }
        [DataMember(Order = 10)]
        public int intEsAdmin { get; set; }
        [DataMember(Order = 11)]
        public bool bitFlActivo { get; set; }

        [DataMember(Order = 11)]
        public string strSede { get; set; }

        //[DataMember(Order = 12)]
        //public ListZKMenu OpcionesMenu { get; set; }
        [DataMember(Order = 12)]
        public ListZKMenuOff OpcionesMenuOff { get; set; }
        #endregion
    }
    [CollectionDataContract()]
    public class ListZKLoginOff : Collection<ZKLoginOff>
    {
    }
}