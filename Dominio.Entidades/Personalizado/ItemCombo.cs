using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Dominio.Entidades.Personalizado
{
    [DataContract()]
    [Serializable()]
    public class ItemCombo
    {
        #region "Propiedades"
        [DataMember(Order = 1)]
        public object Valor { get; set; }

        [DataMember(Order = 2)]
        public object Texto { get; set; }
        #endregion
    }

    [CollectionDataContract()]
    public class ListItemCombo : Collection<ItemCombo>
    {
    }
}
