using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;


namespace Dominio.Entidades
{
    [DataContract()]
    [Serializable()]
    public class ZKUsuarios
    {
        #region "Propiedades"

        [DataMember(Order = 1)]
        public int iIdUsuario { get; set; }
        [DataMember(Order = 2)]
        public int iCodUsuario { get; set; }
        [DataMember(Order = 3)]
        public string sNombre { get; set; }
        [DataMember(Order = 4)]
        public string sPassword { get; set; }
        [DataMember(Order = 5)]
        public int iPrivilegio { get; set; }
        [DataMember(Order = 6)]
        public long CardNumber { get; set; }
        [DataMember(Order = 7)]
        public DateTime dFechaHora { get; set; }
        [DataMember(Order = 8)]
        public string Cod_Personal { get; set; }
        [DataMember(Order = 9)]
        public int iCambioNumero { get; set; }
        [DataMember(Order = 10)]
        public int Estado { get; set; }
        [DataMember(Order = 11)]
        public int iModoverificacion { get; set; }
        [DataMember(Order = 12)]
        public int iIdArea { get; set; }
        [DataMember(Order = 13)]
        public int iIdGrupo { get; set; }
        [DataMember(Order = 14)]
        public string strCoUsuarAnt { get; set; }

        //--------------------------------------------------------------
        [DataMember(Order = 15)]
        public string Privilegio { get; set; }
        [DataMember(Order = 16)]
        public string NumHuellas { get; set; }
        [DataMember(Order = 17)]
        public string NumRostros { get; set; }
        [DataMember(Order = 18)]
        public string strDeArea { get; set; }
        [DataMember(Order = 19)]
        public string strDeGrupo { get; set; }
        [DataMember(Order = 20)]
        public string strEstado { get; set; }

        [DataMember(Order = 21)]
        public List<ZKHuellas> Huellas { get; set; }

        [DataMember(Order = 22)]
        public int iIdSede { get; set; }

        [DataMember(Order = 23)]
        public string strSede { get; set; }

        [DataMember(Order = 23)]
        public string strDedos { get; set; }

        #endregion
    }
    [CollectionDataContract()]
    public class ListZKUsuarios : Collection<ZKUsuarios>
    {
    }
}