using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Dominio.Entidades
{
    public class TG_USUARIO
    {
        [DataMember] public int intIdUsuar { get; set; }
        [DataMember] public bool bitTipoUsuar { get; set; }
        [DataMember] public string strUsUsuar { get; set; }
        [DataMember] public string strCoPassw { get; set; }
        [DataMember] public string strNoUsuar { get; set; }
        [DataMember] public bool bitFlAdmin { get; set; }
        [DataMember] public bool bitPrimerPassw { get; set; }
        [DataMember] public Int16? tinFlEstado { get; set; }
        [DataMember] public string strMotivoEst { get; set; }
        [DataMember] public DateTime? dttFchBloqueo { get; set; }
        [DataMember] public DateTime? dttFchUltPass { get; set; }
        [DataMember] public DateTime? dttFchCaduca { get; set; }
        [DataMember] public int intIdPersonal { get; set; }
        [DataMember] public bool bitFlActivo { get; set; }
        [DataMember] public bool bitFlEliminado { get; set; }
        [DataMember] public int intIdUsuarReg { get; set; }
        [DataMember] public DateTime? dttFeReg { get; set; }
        [DataMember] public int intIdUsuarModif { get; set; }
        [DataMember] public DateTime? dttFeModif { get; set; }
        [DataMember] public string strstrDesPerfil { get; set; }
        [DataMember] public string strDesEmp { get; set; }
        [DataMember] public string strEstadoActivo { get; set; }
        [DataMember] public int intIdPerfil { get; set; }
        [DataMember] public string imgFoto { get; set; }
        //extras login
        [DataMember] public string strUserName { get; set; }
        [DataMember] public string strNomPerfil { get; set; }
        [DataMember] public int intIdSesion { get; set; }
        [DataMember] public int intIdSoft { get; set; }
        [DataMember] public int intCodValida { get; set; }
        [DataMember] public string strDetalleValida { get; set; }
    }
}

