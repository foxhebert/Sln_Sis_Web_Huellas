using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Dominio.Entidades
{
    public class ZKMenuOff
    {
        [DataMember(Order = 1)]
        public int IntIdMenus { get; set; }
        [DataMember(Order = 2)]
        public string StrCoMenus { get; set; }
        [DataMember(Order = 3)]
        public string StrNoMenus { get; set; }
        [DataMember(Order = 4)]
        public string StrDeMenus { get; set; }
        [DataMember(Order = 5)]
        public string StrNoFormu { get; set; }
        [DataMember(Order = 6)]
        public string StrVariable { get; set; }
        [DataMember(Order = 7)]
        public string StrCoMenusRelac { get; set; }
        [DataMember(Order = 8)]
        public int IntOrden { get; set; }
        [DataMember(Order = 9)]
        public int IntFlEstad { get; set; }
        [DataMember(Order = 10)]
        public string StrControlador { get; set; }
        [DataMember(Order = 11)]
        public string StrAccion { get; set; }
        [DataMember(Order = 12)]
        public string StrIcono { get; set; }
        [DataMember(Order = 13)]
        public int IntAsing { get; set; }
        [DataMember(Order = 14)]
        public int iIdSesion { get; set; }
    }
    [CollectionDataContract()]
    public class ListZKMenuOff : Collection<ZKMenuOff>
    {
    }
}