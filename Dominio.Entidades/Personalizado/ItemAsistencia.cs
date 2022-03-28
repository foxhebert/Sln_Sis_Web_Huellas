using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;


namespace Dominio.Entidades.Personalizado
{
    [DataContract()]
    [Serializable()]
    public class ItemAsistencia
    {

        #region "Propiedades"
        [DataMember(Order = 1)]
        public int iCodUsuario { get; set; }

        [DataMember(Order = 2)]
        public string sNombre { get; set; }

        [DataMember(Order = 3)]
        public int NumHuellas { get; set; }

        [DataMember(Order = 4)]
        public long CardNumber { get; set; }

        [DataMember(Order = 5)]
        public string Cod_Personal { get; set; }

        [DataMember(Order = 6)]
        public string Fecha { get; set; }

        [DataMember(Order = 7)]
        public string Hora { get; set; }

        [DataMember(Order = 8)]
        public string Sede { get; set; }

        [DataMember(Order = 9)]
        public string strEstado { get; set; }

        [DataMember(Order = 10)]
        public string intFinger { get; set; }

        [DataMember(Order = 9)]
        public string strDispositivo { get; set; }
        #endregion

    }
    [CollectionDataContract()]
    public class ListItemAsistencia : Collection<ItemAsistencia>
    {
    }
}