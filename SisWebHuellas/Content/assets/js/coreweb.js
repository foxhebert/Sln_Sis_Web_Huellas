

$(document).ready(function () {


    //$.datetimepicker.setLocale('es');


    try
    {
        if (typeof String.prototype.trim !== 'function') { String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ''); } }
        if (typeof Date.prototype.GetHora !== 'function') { Date.prototype.GetHora = function () { if (this === null) return ''; return ('00' + this.getHours()).slice(-2) + ":" + ('00' + this.getMinutes()).slice(-2); } }
        if (typeof Date.prototype.GetFecha !== 'function') { Date.prototype.GetFecha = function () { if (this === null) return ''; return ('00' + this.getDate()).slice(-2) + '/' + ('00' + (this.getMonth() + 1)).slice(-2) + '/' + this.getFullYear(); } }
        if (typeof String.prototype.GetFechaFromJSON !== 'function') { String.prototype.GetFechaFromJSON = function () { if (this === null) return new Date; var fecha = new Date(parseInt(this.substr(6))); return fecha.GetFecha() } }

        //fn_verVersion();//se retira WCF

        if ($('.datatable-personal').length > 0) { fn_disenioTablaPersonal(); fn_cargarPersonal(); }

        if ($('.datatable-marca').length > 0) {
            $('#date_1').datepicker({
                todayBtn: "linked",
                keyboardNavigation: false,
                todayHighlight: true,
                forceParse: false,
                calendarWeeks: true,
                autoclose: true,
                format: "dd/mm/yyyy",
                language: 'es'
            });

            $('#date_2').datepicker({
                todayBtn: "linked",
                keyboardNavigation: false,
                todayHighlight: true,
                forceParse: false,
                calendarWeeks: true,
                autoclose: true,
                format: "dd/mm/yyyy",
                language: 'es'
            });

            var date = new Date();
            var today = new Date(date.getFullYear(), date.getMonth(), date.getDate() - 1);
            $('#date_1').datepicker('setDate', today);
            today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
            $('#date_2').datepicker('setDate', today);

            fn_disenioTablaMarcas();
         //   fn_cargarMarcas();
        }

        $('#enrolar-huella').on('click', function () {





            var codigo = $('#txtCodigo').val();
            if (codigo === "") {
                fn_verMensaje('Ingrese un código', "Registro de Huellas", "warning");
                return;
            }


            //$('#modal-pedido-printxc').css('z-index', '1000');
            $('#modal-pedido-print').css('z-index', '2'); //HGM 12.11.2021
            $('.modal-backdrop').css('z-index', '1');
            $.blockUI({ //  $.unblockUI();
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                },
                message: 'Abriendo Lector...'
            });










            //<<INICIO del comentado para pruebas 16.06.2021
            //if (window.localStorage.getItem('servLocalOK')) {
                    fn_iniciaEnroll();//no comentado para pruebas 16.06.2021 (no validar LS)
            //}
            //<<INICIO del comentado para pruebas 16.06.2021



        });

        $('#btn-iniciarMarc').on('click', function () {
            //<<INICIO del comentado para pruebas 16.06.2021
            //if (window.localStorage.getItem('servLocalOK')) {
              fn_iniciaMarca();//no comentado para pruebas 16.06.2021 (no validar LS)
            //}
            //<<FIN del comentado para pruebas 16.06.2021
        });


        //////HGM FUNCION AÑADIDO PARA PRUEBAS NO USADO EEN EL SISTEMA
        $('#btn-validar-sesion').on('click', function () {

           
            fn_verMensaje("su sesión expiró, Vuelva a iniciar secion.", "Iniciar Marcaciones", "error");
            //window.location.href = "/Login/cerrarSession";// "/Inicio/PaginaPrincipal"; HGM Añadido
            ////////alert('btn-validar-sesion');
            //////var titulo = "Modal Alet"
            //////$.post(
            //////    //'/UsuarioLogin/validarSesion',
            //////    '/Login/Login',                
            //////    {
            //////    },
            //////    function (response) {
            //////        alert('respose');
            //////        if (response !== null) {
            //////            if (!response.exito) {
            //////                alert(123);
            //////                fn_verMensaje(response.message, titulo, 'error')                           
            //////            }
            //////            else {
            //////            }
            //////        }
            //////    }
            //////).fail(function (xhr, textStatus, errorThrown) {
            //////    fn_controlarExcepcion(xhr);
            //////});



        });


        $('#btn-val-silk').on('click', function () {
            fn_verificaConexion();
        });

        if ($('#txtSede').length > 0) // Comentado HGM 12.11.2021
            fn_cargarSedes();

        if ($('#cboEstadoPerso').length > 0) {
            $('#cboEstadoPerso').on('change', function () { fn_cargarMarcas(); });
        }
    
        if ($('#cboEstadoPersoList').length > 0) {
            $('#cboEstadoPersoList').on('change', function () { fn_cargarPersonal(); });
        }
        //if ($('#cboEstadoLectList').length > 0) {
        //    $('#cboEstadoLectList').on('change', function () { fn_cargarLector(); });
        //}

        fn_Estatus();//añadido 15.06.2021


        //var element = document.getElementsByTagName("OBJECT"), index;

        //for (index = element.length - 1; index >= 0; index--) {
        //    element[index].parentNode.removeChild(element[index]);
        //}
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | document"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }

});



////Añadido HGM para redireccionar al Login Jueves 28.10.2021
//$('select').on('change', function (e) {
//    var id = this.selectedIndex;
//    var targetUrl = '/Home/Login' + id.toString();
//    location.href = targetUrl;
//});




//#region AÑADIDOS 2021
//función nueva añadida 15.06.2021
fn_Estatus = function () {
    try
    {
        //---------------------------------------------------------------------------
        //añadido 15.06.2021 <<INICIO
        console.log("Desde coreweb.js");
            if (navigator.onLine) { //Solo funciona para IE (en Chrome siempre muestra TRUE)
                $('#11').html('<label id="_lbl_On" style="color:darkblue;  text-align:center;" >ONLINE</label> ');
                console.log('online');
                x_status_ = "ONLINE>>";

                //A) primero enviar AllRH del LS -> SQL
                fn_EnviarAllRH_LS();

                $('#11').html('<label id="_lbl_On" style="color:darkblue;  text-align:center;" >ONLINE</label> ');
                //fn_CargarUsersLS();
                //fn_CargarSedesLS();
            } else {
                $('#11').html('<label id="_lbl_Off" style="color:red;  text-align:center;" >OFFLINE</label> ');
                console.log('offline');
                x_status_ = "<<OFFLINE";
                //fn_ObtenerSedesLS();//añadido 16.06.2021
            }
        localStorage.setItem("ls_online", x_status_);
        fn_menu();
        fn_LimpiarLS();
        //añadido 15.06.2021 >>FIN
        //---------------------------------------------------------------------------
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_Estatus"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
//función nueva añadida 15.06.2021
fn_EnviarAllRH_LS = function () {
    try
    {
        //A) primero enviar AllRH del LS -> SQL
        if (localStorage.getItem("ls_TRegPersonal") != null && parseInt(localStorage.getItem("ls_TRegPersonal")) > 0) {
            $('#11').html('<label id="_lbl_On" style="color:darkblue;  text-align:center;" >ONLINE: ...Sincronizando Registros de Personal... label> ');
            var TotalRegOffline_ = localStorage.getItem("ls_TRegPersonal")
            var i = 1;
            while (i <= TotalRegOffline_) {
                var clave_ = "x_personalOffline_" + i.toString();

                if (localStorage.getItem(clave_)) {
                    personal_ = JSON.parse(localStorage.getItem(clave_));
                    console.log("Persona " + i.toString() + ": ");
                    console.log(personal_);

                    //Llamar al método del Controlador para enviarlo a la BD y luego si es OK el registro eliminar del LS
                    $.post(
                        '/RegistroHuellas/RegistraUsuario',
                        {
                            x_idUsuario: personal_.x_idUsuario //idUsuario
                            , x_codigo: personal_.x_codigo//codigo
                            , x_nombres: personal_.x_nombres//nombre
                            , x_tarjeta: personal_.x_tarjeta//numTar
                            , x_codPersonal: personal_.x_codPersonal//codPer
                            , x_sede: personal_.x_sede//sede
                            , x_estado: personal_.x_estado//estado
                            , x_dedos: personal_.x_dedos//dedos
                            , x_huellas: personal_.x_huellas//huellas
                            , x_dedosAnt: personal_.x_dedosAnt//dedosAnt
                        },
                        function (response) {
                            if (response !== null) {
                                if (!response.exito) {
                                    console.log("Error en Enrolamiento de Personal: con la clave " + clave_);
                                }
                                else {
                                    console.log("Enrolamiento de Personal: " + response.message + "de la clave " + clave_);
                                    localStorage.removeItem(clave_);
                                }
                            }
                        }
                    ).fail(function (xhr, textStatus, errorThrown) {
                        console.log("Fail POST RegistroHuellas/RegistraUsuario: en la clave " + clave_);
                    });
                } else {
                    console.log("No existe nada en el Local Storage para la cleve: " + clave_);
                }
                i = i + 1;
            }
        }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_EnviarAllRH_LS"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
//función nueva añadida 15.06.2021
fn_LimpiarLS = function () {
    try
    {
        //Limpiar Contador de Registro de Marcas en Local Storage
        if (localStorage.getItem("ls_TRegPersonal") != null && parseInt(localStorage.getItem("ls_TRegPersonal")) > 0) {
            console.log("RegHuellas_Pendientes_LS: ");
            console.log(localStorage.getItem("ls_TRegPersonal"));
            //<< Validar si se eliminaron todos los registros en LS
            var TotalRegOffline_ = localStorage.getItem("ls_TRegPersonal")
            var i = 1;
            var x = 0;//TotalRegOffline_;
            while (i <= TotalRegOffline_) {
                var clave_ = "x_personalOffline_" + i.toString();
                if (!localStorage.getItem(clave_)) {
                    x = x + 1;
                }
                i = i + 1;
            }
            if (x == TotalRegOffline_) {//Si solo si todos los registros eliminados son igual al total
                localStorage.removeItem("ls_TRegPersonal");
            }
        }
        //Limpiar Contador de Registro de Marcas en Local Storage
        if (localStorage.getItem("ls_TRegMarca") != null && parseInt(localStorage.getItem("ls_TRegMarca")) > 0) {
            console.log("Marcaciones_Pendientes_LS: ");
            console.log(localStorage.getItem("ls_TRegMarca"));
            //<< Validar si se eliminaron todos los registros en LS
            var TotalRegOffline_ = localStorage.getItem("ls_TRegMarca")
            var i = 1;
            var x = 0;//TotalRegOffline_;
            while (i <= TotalRegOffline_) {
                var clave_ = "x_marcaOffline_" + i.toString();
                if (!localStorage.getItem(clave_)) {
                    x = x + 1;
                }
                i = i + 1;
            }
            if (x == TotalRegOffline_) {//Si solo si todos los registros eliminados son igual al total
                localStorage.removeItem("ls_TRegMarca");
            }
            }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_LimpiarLS"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
//función nueva añadida 15.06.2021
fn_CargarSedesLS = function () {
    try
    {
        var lstSedes = [];//añadido 15.06
        $.post(
            '/UsuarioLogin/ListarSedeJson',
            {p_estatus:true },//modificado 16.06.2021
            function (response)
            {
                console.log(response);
                for (i = 0; i < response.length; i++) {
                    let newItem = {
                        x_IdSede: response[i].Valor
                        , x_DeLocal: response[i].Texto
                    }
                    console.log(newItem);
                    lstSedes.push(newItem);
                }
                localStorage.setItem("Lst_SedesLS", JSON.stringify(lstSedes));
                console.log("Se copiaron al LS las Sedes.")
                if (localStorage.getItem("Lst_SedesLS")) {
                    console.log(JSON.parse(localStorage.getItem("Lst_SedesLS")));
                }
            }
            ).fail(function (result) {
                fn_controlarExcepcion(result);
            });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_CargarSedesLS"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
//función nueva añadida 16.06.2021
fn_ObtenerSedesLS = function () {
    try
    {
        if (localStorage.getItem("Lst_SedesLS")) {
            var p_lstsedes_ = JSON.parse(localStorage.getItem("Lst_SedesLS"));
            console.log(p_lstsedes_);
            $.post(
                '/UsuarioLogin/llenarListas',
                {
                    p_lst_: p_lstsedes_
                },
                function (response) {
                    //console.log("enviando datos al Contorlador")
                }
            ).fail(function (result) {
                fn_controlarExcepcion(result);
            });

        }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_ObtenerSedesLS"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
//función nueva añadida 16.06.2021
fn_CargarUsersLS = function () {
    try
    {
        var lstUsers = [];//añadido 15.06
        $.post(
            '/UsuarioLogin/ListarUsersJson',
            {},
            function (response) {
                //añadido 15.06.2021 
                console.log(response);
                for (i = 0; i < response.length; i++) {
                    let newItem = {
                        iIdSesion: response[i].iIdSesion
                        , sNombreSesion: response[i].sNombreSesion
                        , sPasswordSesion: response[i].sPasswordSesion
                        , bitFlSist: response[i].bitFlSist
                        , iPermiso: response[i].iPermiso
                        , iRestaurado: response[i].iRestaurado
                        , sPermisoMenu: response[i].sPermisoMenu
                        , intIdLocal: response[i].intIdLocal
                        , dttFecReg: response[i].dttFecReg
                        , intEsAdmin: response[i].intEsAdmin
                        , bitFlActivo: response[i].bitFlActivo
                        , intIdSede: response[i].intIdSede
                        , strSede: response[i].strSede
                    }
                    //console.log(newItem);
                    lstUsers.push(newItem);
                }
                localStorage.setItem("Lst_UsersLS", JSON.stringify(lstUsers));

                if (localStorage.getItem("Lst_UsersLS")) {
                    var p_ = JSON.parse(localStorage.getItem("Lst_UsersLS"));
                    console.log(p_);
                }
                //        //añadido 15.06.2021 << FIN
            }
        ).fail(function (result) {
            fn_controlarExcepcion(result);
        });


        var lstmenusUsers = [];//añadido 16.06
        $.post(
            '/UsuarioLogin/ListarMenusUsersJson',
            {},
            function (response) {
                //añadido 15.06.2021 
                console.log(response);
                for (i = 0; i < response.length; i++) {
                    let newItem = {
                        intIdMenus: response[i].IntIdMenus
                        , strCoMenus: response[i].StrCoMenus
                        , strNoMenus: response[i].StrNoMenus
                        , strDeMenus: response[i].StrDeMenus
                        , strCoMenusRelac: response[i].StrCoMenusRelac
                        , intOrden: response[i].IntOrden
                        , strControlador: response[i].StrControlador
                        , strAccion: response[i].StrAccion
                        , strIcono: response[i].StrIcono
                        , IntAsing: response[i].IntAsing
                        , iIdSesion: response[i].iIdSesion
                    }
                    //console.log(newItem);
                    lstmenusUsers.push(newItem);
                }
                localStorage.setItem("Lst_MenusUsersLS", JSON.stringify(lstmenusUsers));

                if (localStorage.getItem("Lst_MenusUsersLS")) {
                    var p_ = JSON.parse(localStorage.getItem("Lst_MenusUsersLS"));
                    console.log(p_);
                }
                //        //añadido 15.06.2021 << FIN
            }
        ).fail(function (result) {
            fn_controlarExcepcion(result);
            });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_CargarUsersLS"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
fn_menu = function () {
    try
    {
        console.log("....");
        //añadido 18.06.2021
        $.post(
            '/Login/Menu_',
            {},
            function (response) {
                console.log(response);
                var idmenu = "#m_" + response;
                //$(idmenu).prop('style', 'color: black; font-weight: bold;background: gainsboro; padding-right: 10px;padding-left: 10px;');//no funciona esta linea en IE.
                $(idmenu).css('color', 'black').css('font-weight', 'bold').css('background', 'gainsboro').css('padding-right', '10px').css('padding-left', '10px');
           
            }
        ).fail(function (result) {
            });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_menu"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}


//#endregion AÑADIDOS 2021


var data_edt = null;
var row_del = null;
var _varTablaPersonal;
var _varTablaMarca;
var _varTablaLogin;
var lstHuellas = null;
var _tableLanguaje = {
    lengthMenu: 'Mostrar _MENU_ Items',
    info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
    infoEmpty: 'No hay Items para mostrar',
    search: 'Buscar: ',
    sSearchPlaceholder: '',
    zeroRecords: 'No se encontraron registros coincidentes',
    infoFiltered: '(Filtrado de _MAX_ totales Items)',
    paginate: {
        previous: 'Anterior',
        next: 'Siguiente'
    }
};

checkIt = function (evt) {

    evt = (evt) ? evt : window.event
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false
    }



    return true
}

//se retira WCF 22.06.2021
//fn_verVersion = function () {
//    try
//    {
//        $.post(
//            '/RegistroHuellas/ObtenerVersion',
//            {},
//            function (response) {
//                $('#smlVersion').html(response);
//            }
//        ).fail(function (result) {
//            fn_controlarExcepcion(result);
//            //alert('ERROR ' + result.status + ' ' + result.statusText);
//            });
//    }
//    catch (e)
//    {
//        $.post(
//            '/Login/LogsJs',
//            {
//                Excep: e
//                , NameFunction: "coreweb.js | fn_verVersion"
//            },
//            function (response) {
//                console.log("Error js en TXt log")
//            }
//        ).fail(function (result) {
//        });
//        console.error();
//    }
//}



let arreglosSedes = [];




fn_limpiarMensajes = function () {
    $('#ambiance-notification').html('');
}
fn_verMensaje = function (x_mensaje, x_title, x_type, x_tiempo) {
    try
    {
        if (typeof x_tiempo === 'undefined')
            x_tiempo = 2;

        var color = 'green';
        if (x_type === 'error')
            color = 'red';
        if (x_type === 'warning')
            color = 'orange';

        $.alert({
            title: x_title,
            content: x_mensaje,
            type: color,
            escapeKey: true,
            autoClose: 'ok|' + (x_tiempo * 1000),
            buttons: {
                ok: {
                    text: 'Aceptar',
                    action: function () {

                        //AÑADIDO HGM 
                        if (x_mensaje.indexOf("sesión expiró") != -1) {  //Bloque para condicionar comparando el texto - HGM 28.10.2021 Para redireccionar "AUTOMATICAMENTE" al loguin cuando haya terminado la sesión

                            window.location.href = "/Login/cerrarSession";
                        }
                        else{

                             ////if (window.localStorage.getItem('enrollConected')) {
                            $('#btn-iniciarMarc').prop('disabled', false);
                            if (window.localStorage.getItem('modo') * 1 === 1)
                            setTimeout(function () { $('#btn-iniciarMarc').click(); }, 75);

                        }

                    
                    }
                }
            }
        });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_verMensaje"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
fn_table_draw = function (table) {
    table.draw(false);//'page');
    //table.fnStandingRedraw();
}
fn_controlarExcepcion = function (jqXHR) {
    try
    {
        window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
        var mensaje = "";
        if (jqXHR.status === 0) {
            mensaje = 'No hay conexión a la Red.';
        } else if (jqXHR.status === 404) {
            mensaje = 'La página solicitada no está disponible.';
        } else if (jqXHR.status === 500) {
            if (window.localStorage.getItem('modo') * 1 === 1) {
                mensaje = 'No es posible conectar con el servicio web.<br>Las marcaciones se realizarán de forma offline';
                $('#btn-iniciarMarc').prop('disabled', false);
            }
            else
                mensaje = 'No es posible conectar con el servicio web.';
        } else if (textStatus === 'parsererror') {
            mensaje = 'Error en el análisis del JSON solicitado.';
        } else if (textStatus === 'timeout') {
            mensaje = 'Tiempo de espera superado.';
        } else if (textStatus === 'abort') {
            mensaje = 'Solicitud Ajax abortado.';
        } else {
            mensaje = 'Error reportado' + jqXHR.responseText;
        }
        fn_verMensaje(mensaje, "Registro de Huellas", "error", 5);
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_controlarExcepcion"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
fn_iniListHuellas = function () {
    try
    {
        lstHuellas = new Array();

        for (var i = 0; i < 10; i++) {
            lstHuellas[i] = {};              // creates a new object
            lstHuellas[i].intDedo = i + 1;
            lstHuellas[i].strHuella = "";
            lstHuellas[i].lnHuella = 0;
        }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_iniListHuellas"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
fn_cargarSedes = function () {
    try
    {
        //añadido 16.06.2021 
        var p_estatus = true;
        if (navigator.onLine) {
            p_estatus = true;
        }
        else {
            p_estatus = false;
            fn_ObtenerSedesLS();//añadido 16.06.2021
        }

        $.post(
            '/UsuarioLogin/ListarSedeJson',
            { p_estatus: p_estatus},//modificado
            function (response) {


                //let arreglosSedes = [];
                for (i = 0; i < response.length; i++) { //HGM añadido 27.10.2021
                    //alert(response[i].Texto);
                    arreglosSedes.push(response[i].Texto + '_' + response[i].Valor);
                }

                console.log('sedes' + arreglosSedes );



                $('#txtSede').autoComplete({
                    minChars: 0,
                    source: function (term, suggest) {
                        term = term.toLowerCase();
                        var suggestions = [];
                        for (i = 0; i < response.length; i++)
                            if (~(response[i].Texto).toLowerCase().indexOf(term))
                                suggestions.push(response[i]);
                        suggest(suggestions);
                        console.log(suggestions);
                    },
                    renderItem: function (item, search) {

                        search = search.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
                        var re = new RegExp("(" + search.split(' ').join('|') + ")", "gi");


                        //alert(item.Texto);


                        return '<div class="autocomplete-suggestion" data-sede="' + item.Texto + '" data-id="' + item.Valor + '" data-val="' + search + '">' + item.Texto.replace(re, "<b>$1</b>") + '</div>';

                      
                    },
                    onSelect: function (e, term, item) {
                        $('#txtSede').val(item.data('sede')); //HGM
                        $('#hddSede').val(item.data('id'));
                    }
                });

            }
        ).fail(function (result) {
            fn_controlarExcepcion(result);
            });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_cargarSedes"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}

//=== MARCACIONES ================================================================================================================================
//#region Marcas
$('#btn-filtrar-marcas').on('click', function () {

    if ($('#date_1').val() == "" && $('#date_2').val() == "") {

        fn_verMensaje("Seleccionar el Rango de Fechas.", "Consulta de Marcaciones", 'info')
    }
    else if ($('#date_1').val() == "") {

        fn_verMensaje("Seleccionar Fecha de Inicio.", "Consulta de Marcaciones", 'info');
        $('#date_1').focus();
    }
    else if ($('#date_2').val() == "") {

        fn_verMensaje("Seleccionar Fecha de Fin.", "Consulta de Marcaciones", 'info');
        $('#date_2').focus();
    }
    else {

        fn_cargarMarcas();


    }



});


//$('.datepicker').datepicker({
//    format: 'mm/dd/yyyy',
//    startDate: '-3d'
//});


//function checkDateDifference(startDate, endDate) {
//    startDate = $.datepicker.parseDate('mm/dd/yy', startDate);
//    endDate = $.datepicker.parseDate('mm/dd/yy', endDate);

//    var difference = (endDate - startDate) / (86400000);
//    alert(difference)
//    if (difference < 0) {
//        showError("The start date must come before the end date.");
//        return false;
//    }
//    return true;

//}

///7Lenguaje Formato Datepicker
$(".datepicker").datepicker({
    format: 'dd/mm/yyyy',
    language: 'ru'
});



//function compararFechas() {
//    var date_received = $("#date_1").datepicker('getDate');
//    var date_completed = $("#date_2").datepicker('getDate');

//    var diff = date_completed - date_received;
//    var days = diff / 1000 / 60 / 60 / 24;
//    //return days;
//}


fn_cargarMarcas = function () {




    //Bloque Añadido HGM 02.11.2021 para comparar las fechas
    var dateDesde   = $("#date_1").datepicker('getDate'); //Recoje el valor del input como fecha
    var dateHasta   = $("#date_2").datepicker('getDate');
    if (Date.parse(dateDesde) > Date.parse(dateHasta) || Date.parse(dateDesde) == Date.parse(dateHasta)  ) {
        fn_verMensaje("La Fecha de Fin debe ser posterior a la Fecha de Inicio", "Filtro de Fechas", 'error') 
        return false;
    }


    //$("#id_date_received, #id_date_completed").datepicker();
    //checkDateDifference("01/11/2021","01/12/2021");
    //var datep = $('#date_1').datetimepicker({
    //     // dateFormat: 'dd-mm-yy',
    //     format: 'DD/MM/YYYY HH:mm:ss',
    // });
    //var date_received = $("#date_1").datepicker('getDate');
    //alert(compararFechas());
    //var dateDatePicker = $.datepicker.formatDate("dd/mm/yy", new Date("09/01/2014")) < $.datepicker.formatDate("dd/mm/yy", new Date("10/01/2014")); // Returns true
    //alert($.datepicker.formatDate("dd/mm/yy", new Date("09/01/2014")) < $.datepicker.formatDate("dd/mm/yy", new Date("10/01/2014")););
    //$.datepicker.formatDate("dd/mm/yy", new Date("10/01/2014")) < $.datepicker.formatDate("dd/mm/yy", new Date("10/01/2014")); // Returns false
    //$.datepicker.formatDate("dd/mm/yy", new Date("11/01/2014")) < $.datepicker.formatDate("dd/mm/yy", new Date("10/01/2014")); // Returns false



    window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
    try
    {
        var date_Start = $('#date_1').val();
        var date_End = $('#date_2').val();
        var active = $('#cboEstadoPerso option:selected').val();
        if (date_Start === null || date_End === null)
            return;
        /******************************************************************************
        Se comento ya que no coge los valores como fecha
        if (date_Start > date_End) {
            fn_verMensaje("La Fecha de Fin debe ser posterior a la Fecha de Inicio", "Filtro de Fechas", 'error') //18.06.2021
            return;
        }
        *******************************************************************************/


        $('.sticky-top').css('z-index', '1');//HGM 12.11.2021
        $.blockUI({ //  $.unblockUI(); //Añadido 12.11.2021  88888
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            message: 'Cargando...'
        });



        if (date_Start !== '' && date_End !== '') {
            var sede = $('#idse').val() * 1;
            $.post(
                '/Asistencia/FiltrarAsistenciaJSON',
                { x_fechaIni: date_Start, x_fechaFin: date_End, x_criterio: '', x_filtro: '', x_estado: active, x_idSede:sede },
                function (response) {
                    console.log(response);
                    fn_disenioTablaMarcas(response);
                    $.unblockUI(); //Añadido 12.11.2021
                }
            ).fail(function (result) {
                fn_controlarExcepcion(result);
                //alert('ERROR ' + result.status + ' ' + result.statusText);
                $.unblockUI(); //Añadido 12.11.2021
            });
        }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_cargarMarcas"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
        $.unblockUI(); //Añadido 12.11.2021
    }
}
fn_disenioTablaMarcas = function (data) {
    try
    {
        if (data !== undefined) {
            var count = 0;
            if (typeof _varTablaMarca !== 'undefined')
                _varTablaMarca.destroy();
            _varTablaMarca = $('.datatable-marca').DataTable({
                data: data,
                columns: [
                    { data: 'iCodUsuario' },
                    { data: 'Fecha' },
                    { data: 'Hora' },
                    { data: 'sNombre' },
                    { data: 'CardNumber' },
                    { data: 'strDispositivo' },
                    { data: 'NumHuellas' },
                    { data: 'intFinger' },
                    //{ data: 'Sede' },
                ],
                lengthMenu: [10, 25, 50],
                order: [[1, 'desc'], [2, 'desc']],
                language: _tableLanguaje,
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (aData.strEstado === "Inactivo")
                        $(nRow).css('color', 'red').css('background','#ffeded4d');
                },
                dom: 'lBfrtip',
                buttons: [{
                    extend: 'excelHtml5',
                    title: 'Listado de Marcaciones',
                    extension: '.xlsx',
                    text: 'Exportar a Excel  '
                }]
            });
        } else {
            _varTablaMarca = $('.datatable-marca').DataTable({
                columns: [
                    { data: 'iCodUsuario' },
                    { data: 'Fecha' },
                    { data: 'Hora' },
                    { data: 'sNombre' },
                    { data: 'CardNumber' },
                    { data: 'strDispositivo' },
                    { data: 'NumHuellas' },
                    { data: 'intFinger' },
                    //{ data: 'Sede' },
                ],
                lengthMenu: [10, 25, 50],
                order: [[1, 'desc'], [2, 'desc']],
                language: _tableLanguaje,
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (aData.strEstado === "Inactivo")
                        $(nRow).css('color', 'red').css('background', '#ffeded4d');
                },
                dom: 'lBfrtip',
                buttons: [{
                    extend: 'excelHtml5',
                    title: 'Listado de Marcaciones',
                    extension: '.xlsx',
                    text: 'Exportar a Excel  '
                }]
            });
        }
        fn_formatLinkExport();
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_disenioTablaMarcas"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
//#endregion Marcas


//=== PERSONAL ================================================================================================================================
//#region Personal
var tr;
var estActi = 0;

fn_cargarPersonal = function () {

    try
    {
        

        fn_limpiarForm();
        var sede = $('#idse').val() * 1;
        var active = $('#cboEstadoPersoList option:selected').val();

        $('.sticky-top').css('z-index', '1');//HGM 12.11.2021

        $.ajax({
            url: '/RegistroHuellas/ListarPersonalJson',
            type: 'POST',
            data: {
                x_idSede: sede, x_estado: active
            },
            beforeSend: function () {
                $.blockUI({
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#000',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        opacity: .5,
                        color: '#fff'
                    },
                    message: 'Cargando...'
                });
            },


            success: function (response) {

                //============================================================

                fn_disenioTablaPersonal(response);

                //============================================================

         },
            complete: function () {
                $.unblockUI();
            }
    });










        ////$.post(
        ////    '/RegistroHuellas/ListarPersonalJson',
        ////    {
        ////        x_idSede: sede, x_estado: active
        ////    },
        ////    function (response) {
        ////        fn_disenioTablaPersonal(response);
        ////    }
        ////).fail(function (result) {
        ////    fn_controlarExcepcion(result);
        ////    //alert('ERROR ' + result.status + ' ' + result.statusText);
        ////    });




























    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_cargarPersonal"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
fn_disenioTablaPersonal = function (data) {
    try
    {
        var buttons = '<button class="btn btn-sm btn-dark personal_edit" title="Editar Personal"><i class="fa fa-pencil"></i> Editar </button>&nbsp;';
        buttons += '<button class="btn btn-sm " id="btnUpEstado"><i class="fe"></i></button>&nbsp;';
        if (window.localStorage.getItem('esadm') * 1 === 1)
            buttons += '<button class="btn btn-sm btn-danger personal_delete " title="Eliminar Personal"><i class="fa fa-trash-o"></i></button>';

        var colHdd = [
            {
                targets: [0],
                visible: false,
                searchable: false
            },
            {
                bSortable: false,
                targets: -1,
                data: null,
                defaultContent: '<div style="width:140px !important">' + buttons + '</div>'
            }];
        if (data !== undefined) {
            var count = 0;
            if (typeof _varTablaPersonal !== 'undefined')
                _varTablaPersonal.destroy();


            //alert('cargado');



            _varTablaPersonal = $('.datatable-personal').DataTable({
                data: data,
                columns: [
                    { data: 'iIdUsuario' },
                    { data: 'iCodUsuario' },
                    { data: 'Cod_Personal' },
                    { data: 'sNombre' },

                    { data: 'dFechaHora', render: function (data, type, row) { return data.GetFechaFromJSON(); } },

                    { data: 'strEstado' },
                    { data: 'NumHuellas' },
                    { data: 'CardNumber' },
                    { data: 'strSede' },
                    { data: '' },
                ],
                lengthMenu: [10, 25, 50],
                order: [[3, 'asc']],
                language: _tableLanguaje,
                columnDefs: colHdd,
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var estado = aData.strEstado === 'ACTIVO';
                    //modificaciones 18.06.2021
                    if (estado) {
                        $(nRow).find(".btn").addClass('btn-success').addClass('personal_inactivar');
                        $(nRow).find("#btnUpEstado").prop('title', 'Inactivar Personal');
                        //$(nRow).find(".btn").addClass('btn-warning').prop('title', 'Inactivar Usuario').addClass('personal_inactivar');
                        $(nRow).find(".fe").addClass('fe-user-minus');

                        // btn-warning
                    }
                    else {
                        $(nRow).find(".btn").addClass('btn-warning').addClass('personal_activar');
                        $(nRow).find("#btnUpEstado").prop('title', 'Activar Personal');

                        //$(nRow).find(".btn").addClass('btn-success').prop('title', 'Activar Usuario').addClass('personal_activar');
                        $(nRow).find(".fe").addClass('fe-user-check');
                        $(nRow).css('color', 'red').css('background', '#ffeded4d');
                    }
                }
                , dom: 'lBfrtip',
                buttons: [{
                    extend: 'excelHtml5',
                    title: 'Listado de Personal',
                    extension: '.xlsx',
                    text: 'Exportar a Excel  '
                    , exportOptions: {
                        columns: [1, 2, 3, 4, 5, 6, 7, 8] //columns: ':visible' (solo en caso de exportar vsiibles)
                    }
                    //,customize: function (xlsx) {
                    //    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                    //    var col = $('col', sheet);
                    //    col.each(function () {
                    //        $(this).attr('width', 20);
                    //    });
                    //}   
                }]
            });
        } else {
            _varTablaPersonal = $('.datatable-personal').DataTable({
                //lengthMenu: [10, 25, 50],
                order: [[3, 'asc']],
                language: _tableLanguaje,
                columnDefs: colHdd,
                dom: 'lBfrtip',
                buttons: [{
                    extend: 'excelHtml5',
                    title: 'Listado de Personal',
                    extension: '.xlsx',
                    text: 'Exportar a Excel  '
                    , exportOptions: {
                        columns: [1, 2, 3, 4, 5, 6, 7, 8]
                    }
                    //, customize: function (xlsx) {
                    //    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                    //    var col = $('col', sheet);
                    //    col.each(function () {
                    //        $(this).attr('width', 20);
                    //    });
                    //}  
                }]
            });
        }
        fn_formatLinkExport();
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_disenioTablaPersonal"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}

fn_formatLinkExport = function () {
    $('.dt-buttons').css('float', 'right').css('margin', '1rem 0rem').css('padding', '10px 0px 9px 0px').append('<span style="color:#21a366"><i class="fa fa-file-excel-o" ></i></span>');
    $('.dt-buttons a').css('color', '#495057').css('text-decoration', 'none');
    $('.dt-buttons a:hover').css('color', '#495057').css('text-decoration', 'none');
}

$('#btn-nuev-pers').on('click', function () {



    var date = new Date();
    $('#txtFeReg').val(date.GetFecha());
    $('#txtHoReg').val(date.GetHora());
    $('#chkActivo').prop('checked', true);
    $('#chkActivo').prop('disabled', true);
    $('#exampleModalCenterTitlePer').html('NUEVO PERSONAL');
    $('#modal-pedido-print').modal({
        show: true,
        keyboard: false,
        backdrop: 'static'
    });


    $('#modal-pedido-print').css('z-index', '100');
    $('.modal-backdrop').css('z-index', '1');

    fn_limpiarForm();
    zkonline.CheckFinger = '0000000000';//añadido 23.06.2021
    //llevar combo Sedes
});
$('.datatable-personal').on('draw.dt', function () {
    try
    {
        var btn = 0;//añadido 18.06.2021
        $(".personal_edit").unbind("click");
        $(".personal_delete").unbind("click");
        $(".personal_activar").unbind("click");
        $(".personal_inactivar").unbind("click");

        $('.personal_edit').on('click', function () {


            window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
            btn = 2;//añadido 18.06.2021
            fn_iniListHuellas();
            row_del = _varTablaPersonal.row($(this).parents('tr'));
            var data = row_del.data();

            var fecha = new Date(parseInt(data.dFechaHora.substr(6)));
            $('#hddIdUsuario').val(data.iIdUsuario);
            $('#txtCodigo').val(data.iCodUsuario);
            $('#txtCodigo').prop('disabled', true);
            $('#txtFeReg').val(fecha.GetFecha());
            $('#txtHoReg').val(fecha.GetHora());
            $('#txtCodPerso').val(data.Cod_Personal);
            $('#txtNombres').val(data.sNombre);
            $('#txtNumTar').val(data.CardNumber);
            $('#txtSede').val(data.iIdSede);
            $('#txtNumHuellas').val(data.NumHuellas);
            $('#chkActivo').prop('checked', data.Estado === 1).prop('disabled', false);
            $('#exampleModalCenterTitlePer').html('EDITAR PERSONAL');
            $('#modal-pedido-print').modal({
                show: true,
                keyboard: false,
                backdrop: 'static'
            });

            fn_obtenerHuellas();


            //alert(123);

            //$('#modal-pedido-printxc').css('z-index', '1000');
            $('#modal-pedido-print').css('z-index', '10000'); //HGM 12.11.2021
            //$('.modal-backdrop').css('z-index', '10000');
        })

        $('.personal_delete').on('click', function () {
            btn = 3; //añadido 18.06.2021
            row_del = _varTablaPersonal.row($(this).parents('tr'));
            window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)

            $.confirm({
                title: 'Eliminar Personal',
                content: '¿Está seguro de eliminar el registro de forma permanente? <b>Las marcas realizadas por este personal también serán eliminadas</b>',
                buttons: {
                    si: {
                        keys: ['s'],
                        action: function () {
                            $.post(
                                '/RegistroHuellas/EliminarRegistro',
                                {
                                    x_idUsuario: row_del.data().iIdUsuario
                                },
                                function (response) {
                                    if (response !== null) {
                                        if (!response.exito) {
                                            fn_verMensaje(response.message, "Eliminar Personal", 'error')//modificado 18.06.2021
                                            //fn_verMensaje(response.message, "Registro de Huellas", 'error')
                                            //alert(response.message);
                                        }
                                        else {
                                            row_del.remove();
                                            row_del = null;
                                            fn_table_draw(_varTablaPersonal);
                                            //fn_verMensaje(response.message, "Registro de Huellas", response.type);
                                            fn_verMensaje(response.message, "Eliminar Personal", response.type);//modificado 18.06.2021
                                            //fn_limpiarForm();
                                        }
                                    }
                                }
                            ).fail(function (xhr, textStatus, errorThrown) {
                                fn_controlarExcepcion(xhr);
                                //var errorMessage = xhr.status + ': ' + xhr.statusText
                                //alert(errorMessage);
                            });
                        }
                    },
                    no: function () {
                    }
                }
            });
        });

        $(".personal_activar").on("click", function () {
            if (btn == 0) {
                tr = $(this).parents('tr');
                row_del = _varTablaPersonal.row($(this).parents('tr'));
                var data = row_del.data();
                fn_CambiarEstadoPersonal(data.iIdUsuario, 1);
            }
            btn = 0;
        });
        $(".personal_inactivar").on("click", function () {
            if (btn == 0) {
                tr = $(this).parents('tr');
                row_del = _varTablaPersonal.row($(this).parents('tr'));
                var data = row_del.data();
                fn_CambiarEstadoPersonal(data.iIdUsuario, 0);
            }
            btn = 0;
        });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | datatable-personal"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});
$('#btn-save-pers').on('click', function () {

    try
    {
        window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)

        var codigo = $('#txtCodigo').val() * 1;
        if (codigo === "" || codigo === 0) {
            fn_verMensaje('Ingrese un código', "Personal", "warning");//modificado 18.06.2021
            return;
        }

        var codPer = $('#txtCodPerso').val();
        if (codPer === "") {
            fn_verMensaje('Ingrese el Código de personal.', "Personal", "warning");//modificado 18.06.2021
            return;
        }

        var nombre = $('#txtNombres').val();
        if (nombre === "") {
            fn_verMensaje('Ingrese un nombre', "Personal", "warning");//modificado 18.06.2021
            return;
        }
        var numTar = $('#txtNumTar').val();

        var d = new Array();
        var h = new Array();

        for (i = 0; i < 10; i++) {
            if (lstHuellas[i].lnHuella > 0 && lstHuellas[i].strHuella !== "") {
                d.push(lstHuellas[i].intDedo);
                h.push(lstHuellas[i].strHuella);
            }
        }
        $('#hddDedos').val(d);
        $('#hddHuellas').val(h);
        var dedos = $('#hddDedos').val();
        var huellas = $('#hddHuellas').val();
        var sede = $('#txtSede').val();
        var estado = $('#chkActivo').prop('checked') ? 1 : 0;
        var idUsuario = $('#hddIdUsuario').val();
        //añadido 18.06.2021
        var titulo = "Nuevo Personal"
        if (idUsuario > 0) {
            titulo = "Editar Personal"
        }
        var dedosAnt = "";
        fn_verificaIE();
        //if (blnesIE) {//comentado 23.06.2021
            if (zkonline.CheckFinger !== null) {
                dedosAnt = zkonline.CheckFinger;
            }
        //}//comentado 23.06.2021
        var x_status_ = "";
        //añadir opcion de Validar Navigator.OFFLINE >> almacenamiento local en Cache Storage 14.06.2021
        if (navigator.onLine) {
            //---------------------------------------------------------------------------
            //fn_Estatus();//añadido 15.06.2021
            fn_EnviarAllRH_LS();

            //luego enviar el nuevo registro.
            //---------------------------------------------------------------------------
            $.post(
                '/RegistroHuellas/RegistraUsuario',
                {
                    x_idUsuario: idUsuario
                    , x_codigo: codigo
                    , x_nombres: nombre
                    , x_tarjeta: numTar
                    , x_codPersonal: codPer
                    , x_sede: sede
                    , x_estado: estado
                    , x_dedos: dedos
                    , x_huellas: huellas
                    , x_dedosAnt: dedosAnt
                },
                function (response) {
                    if (response !== null) {
                        if (!response.exito) {
                            fn_verMensaje(response.message, titulo, 'error')//18.06.2021
                            //alert(response.message);
                        }
                        else {
                            if ($('#hddIdUsuario').val() * 1 === 0)
                                _varTablaPersonal.row.add(response.entidad);
                            else
                                row_del.data(response.entidad);

                            fn_table_draw(_varTablaPersonal);
                            fn_verMensaje(response.message, titulo, response.type);//18.06.2021
                            fn_limpiarForm();
                            zkonline.CheckFinger = '0000000000';//añadido 23.06.2021
                            $('#modal-pedido-print').modal('hide');
                        }
                    }
                    //$("#zkonline").removeAttr("classid");//añadido 24.06.2021 QA_driver
                }
            ).fail(function (xhr, textStatus, errorThrown) {
                fn_controlarExcepcion(xhr);
                //$("#zkonline").removeAttr("classid");//añadido 24.06.2021 QA_driver
            });
            //---------------------------------------------------------------------------
        }
        else {
            //---------------------------------------------------------------------------
            //añadido 15.06.2021 <<INICIO
            x_status_ = "<<OFFLINE";
            localStorage.setItem("ls_online", x_status_);
            console.log('offline');
            $('#11').html('<label id="_lbl_Off" style="color:red;  text-align:center;" >OFFLINE</label> ');
            var TotalRegOffline = 0;
            if (localStorage.getItem("ls_TRegPersonal")) {
                var TotalRegOffline = parseInt(localStorage.getItem("ls_TRegPersonal"));
            }
            //aqui almacenar en Local Storage
            let personalNew = {
                x_idUsuario: idUsuario
                , x_codigo: codigo
                , x_nombres: nombre
                , x_tarjeta: numTar
                , x_codPersonal: codPer
                , x_sede: sede
                , x_estado: estado
                , x_dedos: dedos
                , x_huellas: huellas
                , x_dedosAnt: dedosAnt
            }
            //generar Clave
            var j = TotalRegOffline + 1;
            var clave = "x_personalOffline_" + j.toString();
            localStorage.setItem(clave, JSON.stringify(personalNew));

            if (localStorage.getItem(clave)) {
                localStorage.setItem("ls_TRegPersonal", j);
                fn_verMensaje("El personal se registró exitosamente (inLS).", "Registro de Huellas", "success");
                $('#modal-pedido-print').modal('hide');
                //$("#zkonline").removeAttr("classid");//añadido 24.06.2021 QA_driver
            }
            //añadido 15.06.2021 >>FIN
            //---------------------------------------------------------------------------
        }
        //$("#zkonline").attr('classid', '');
        localStorage.clear(); //adicionado pruebas QA 24.06.2021.1844 ES
        try {
            //CollectGarbage(); //Comentado HGM 02.11.2021
        }
        catch (err) {
            ////alert('collectgarbage' + err.description);  //Comentado HGM 02.11.2021
        }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | btn-save-pers"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});


$('#btn-close-').on('click', function () {
    try {
        //$("#zkonline").removeAttr("classid");//añadido 24.06.2021 QA_driver
        try {
            //CollectGarbage(); //Comentado HGM 02.11.2021
        }
        catch (err) {
            //alert('collectgarbage' + err.description); //Comentado HGM 02.11.2021
        }
    }
    catch (e) {
    $.post(
        '/Login/LogsJs',
        {
            Excep: e
            , NameFunction: "coreweb.js | btn-close-"
        },
        function (response) {
            console.log("Error js en TXt log")
        }
    ).fail(function (result) {
    });
    console.error();
}
});
$('#btn-close-x').on('click', function () {
    try {
        //$("#zkonline").removeAttr("classid");//añadido 24.06.2021 QA_driver

        try {
            //CollectGarbage(); //Comentado HGM 02.11.2021
        }
        catch (err) {
            //alert('collectgarbage' + err.description);//Comentado HGM 02.11.2021
        }
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | btn-close-x"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});

fn_limpiarForm = function () {

    fn_limpiarMensajes();
    $("#hddIdUsuario").val('0');
    $("#txtCodigo").val('');
    $("#txtNombres").val('');
    $("#txtNumTar").val('0');
    $("#txtCodPerso").val('');
    $("#hddDedos").val('');
    $("#hddHuellas").val('');
    $("#txtNumHuellas").val('0');
    $('#txtCodigo').prop('disabled', false);
    fn_iniListHuellas();
    $('#txtSede').val(0);
    //zkonline.CheckFinger = '0000000000';//añadido 23.06.2021

}
fn_obtenerHuellas = function () {
    try
    {
        zkonline.CheckFinger = '0000000000';//añadido 23.06.2021
        var idUsuario = $('#hddIdUsuario').val();
        $.post(
            '/RegistroHuellas/ObtenerHuellasUsuario',
            {
                x_idUsuario: idUsuario
            },
            function (response) {
                for (i = 0; i < response.length; i++) {
                    if (response[i] === "1")
                        lstHuellas[i].lnHuella = 10;
                }
                zkonline.CheckFinger = response;
            }
        ).fail(function (xhr, textStatus, errorThrown) {
            fn_controlarExcepcion(xhr);
            //var errorMessage = xhr.status + ': ' + xhr.statusText
            //alert(errorMessage);
            });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_obtenerHuellas"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
fn_CambiarEstadoPersonal = function (usuario, estado) {
    try
    {
        window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
        estActi = estado;
        var titulo = 'Inactivar Personal';
        var preg = "¿Está seguro de inactivar el registro de forma permanente?";
        if (estado == 1) {
            titulo = 'Activar Personal';
            preg = "¿Está seguro de activar el registro de forma permanente?";
        }
        $.confirm({
            title: titulo,
            content: preg,
            buttons: {
                si: {
                    keys: ['s'],
                    action: function () {
                        $.post(
                            '/RegistroHuellas/CambiarEstadoRegistro',
                            {
                                x_idUsuario: row_del.data().iIdUsuario,
                                x_estado: estado
                            },
                            function (response) {
                                if (response !== null) {
                                    if (!response.exito) {
                                        fn_verMensaje(response.message, titulo, 'error') //18.06.2021
                                        //alert(response.message);
                                    }
                                    else {
                                        debugger;
                                        if ($('#cboEstadoPersoList').val() == 2) {
                                            if (estActi === 0)
                                                $(tr).css('color', 'red').css('background', '#ffeded4d');
                                            else
                                                $(tr).css('color', '').css('background', '');
                                            row_del.data(response.entidad);
                                        }
                                        else {
                                            row_del.remove();
                                            row_del = null;
                                        }

                                        fn_table_draw(_varTablaPersonal);
                                        fn_verMensaje(response.message, titulo, response.type);//18.06.2021
                                    }
                                }
                            }
                        ).fail(function (xhr, textStatus, errorThrown) {
                            fn_controlarExcepcion(xhr);
                            //var errorMessage = xhr.status + ': ' + xhr.statusText
                            //alert(errorMessage);
                        });
                    }
                },
                no: function () {
                }
            }
        });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_CambiarEstadoPersonal"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
//#endregion Personal


//=== USUARIO ==============================================================================================================================
//#region Usuario
var operacion = 0;
fn_cargarLogin = function () {

    $('.sticky-top').css('z-index', '1');//HGM 12.11.2021
    $.blockUI({ //  $.unblockUI(); //Añadido 12.11.2021  88888
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        },
        message: 'Cargando...'
    });

    try
    {
        fn_limpiarFormLogin();
        $.post(
            '/UsuarioLogin/ListarLoginJson',
            {},
            function (response) {
                fn_disenioTablaLogin(response);
                $.unblockUI();
            }
        ).fail(function (result) {
            fn_controlarExcepcion(result);
            //alert('ERROR ' + result.status + ' ' + result.statusText);
            $.unblockUI();
            });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_cargarLogin"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
        $.unblockUI();
    }
}
fn_disenioTablaLogin = function (data) {
    try
    {
        var colHdd = [
            {
                targets: [0],
                visible: false,
                searchable: false
            },
            {
                targets: -1,
                data: null,
                defaultContent: '<div style="width:230px !important">' +
                    '<button class="btn btn-sm btn-dark login_edit" title="Editar Usuario"><i class="fa fa-pencil"></i> Editar </button>&nbsp;' +
                    '<button class="btn btn-sm btn-secondary login_pwd " title="Cambiar Clave"><i class="fa fa-keyboard-o"></i> Cambiar clave</button>&nbsp;' +
                    '<button class="btn btn-sm btn-danger login_delete " title="Eliminar Usuario"><i class="fa fa-trash-o"></i></button></div>'
         }];



        var colHdd_HGM = [
            {
                targets: [0],
                visible: false,
                searchable: false,
                targets: 0, // your case first column
                className: "text-center",
                width: "4%"
            },
            {
            targets: [3],
            visible: true,
            searchable: false,
            //className: "justify-content-center",
            width: "6%",     
            //"ordering": false HGM.18.11.2021
            bSortable: false // HGM https://stackoverflow.com/questions/16335928/how-to-remove-sorting-option-from-datatables

            },

            {
                targets: -1,
                data: null,
                defaultContent: '<div style="width:100% !important; text-align:center;" >' + //HGM
                '<button class="btn btn-sm btn-dark login_edit" title="Editar Usuario"><i class="fa fa-pencil"></i> Editar </button>&nbsp;' +
                '<button class="btn btn-sm btn-secondary login_pwd " title="Cambiar Clave"><i class="fa fa-keyboard-o"></i> Cambiar clave</button>&nbsp;' +
                '<button class="btn btn-sm btn-danger login_delete " title="Eliminar Usuario"><i class="fa fa-trash-o"></i></button></div>'
            }];



        if (data !== undefined) {
            var count = 0;
            if (typeof _varTablaLogin !== 'undefined')
                _varTablaLogin.destroy();
            _varTablaLogin = $('.datatable-login').DataTable({
                data: data,
                columns: [
                    { data: 'iIdSesion'     },
                    { data: 'sNombreSesion' },
                    { data: 'strSede'       },
                    { data: ''              },
                ],
                lengthMenu: [10, 25, 50],
                order: [[1, 'asc']],
                language: _tableLanguaje,
                columnDefs: colHdd_HGM ,
                dom: 'lBfrtip',
                buttons: [{
                    extend: 'excelHtml5',
                    title: 'Listado de Usuarios',
                    extension: '.xlsx',
                    text: 'Exportar a Excel  '
                    , exportOptions: {
                        columns: [1, 2]
                    }
                    //, customize: function (xlsx) {
                    //    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                    //    var col = $('col', sheet);
                    //    col.each(function () {
                    //        $(this).attr('width', 15);
                    //    });
                    //}  
                }]

            });
        } else {
            _varTablaLogin = $('.datatable-login').DataTable({
                lengthMenu: [10, 25, 50],
                order: [[2, 'asc']],
                language: _tableLanguaje,
                columnDefs: colHdd,
                dom: 'lBfrtip',
                buttons: [{
                    extend: 'excelHtml5',
                    title: 'Listado de Usuarios',
                    extension: '.xlsx',
                    text: 'Exportar a Excel  '
                    , exportOptions: {
                        columns: [1, 2]
                    }
                    //, customize: function (xlsx) {
                    //    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                    //    var col = $('col', sheet);
                    //    col.each(function () {
                    //        $(this).attr('width', 15);
                    //    });
                    //}  
                }]
            });
        }
        fn_formatLinkExport();
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_disenioTablaLogin"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}


/////////Añadido pra limpiar campo 26.10.2021 HGM
////function limpiarControles() {
////    alerte(
////    $('#txtNombre').val("");
////    $("#txtpwd1").val('');
////    $("#txtpwd2").val('');
////}




///////////////////////////////////////////////////////////////////HGM
$('#btn-nuev-login').on('click', function () {
    try
    {
        fn_obtenerMenuUsuario(0);
        operacion = 0;
        $('#hddIdSesion').val(0);
        $('#txtNombre').val("").prop('disabled', false);
        /////Añadido pra limpiar campo 26.10.2021 HGM
        $("#txtpwd1").val('');
        $("#txtpwd2").val('');
        $('.cl-pwd').show();
        $('.cl-sede').show();
        $('#hddSede').val(0);
        $('#txtSede').val('');
        $('#fstSede').prop('disabled', false);

        $('#exampleModalCenterTitle').html("REGISTRO DE USUARIO");
        $('#modal-nuevo-login').modal({
            show: true,
            keyboard: false,
            backdrop: 'static'
        });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                  Excep: e
                , NameFunction: "coreweb.js | btn-nuev-login"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
})
$('.datatable-login').on('draw.dt', function () {
    try
    {
        $(".login_edit").unbind("click");
        $(".login_delete").unbind("click");
        $(".login_pwd").unbind("click");

        $('.login_edit').on('click', function () {
            operacion = 1;
            row_del = _varTablaLogin.row($(this).parents('tr'));
            var data = row_del.data();
            fn_obtenerMenuUsuario(data.iIdSesion);
            $('#hddIdSesion').val(data.iIdSesion);
            $('#txtNombre').val(data.sNombreSesion).prop('disabled', true);
            $('.cl-pwd').hide();
            $('.cl-sede').show();
            $('#hddSede').val(data.intIdSede);
            $('#txtSede').val(data.strSede);
            $('#cboSede').html('');//HGM
            $('#cboSede').append('<option value="' + data.iIdSede + '">' + data.strSede + '</option>');//HGM
            $('#fstSede').prop('disabled', data.intEsAdmin === 1);

            $('#exampleModalCenterTitle').html("REGISTRO DE USUARIO");
            $('#modal-nuevo-login').modal({
                show: true,
                keyboard: false,
                backdrop: 'static'
            });
        })
        $('.login_pwd').on('click', function () {


            $('#txtNombre').val('');
            $("#txtpwd1").val('');
            $("#txtpwd2").val('');



            operacion = 2;
            row_del = _varTablaLogin.row($(this).parents('tr'));
            var data = row_del.data();
            $('#hddIdSesion').val(data.iIdSesion);
            $('#txtNombre').val(data.sNombreSesion).prop('disabled', true);
            $('.cl-pwd').show();
            $('.cl-sede').hide();

            $('#hddSede').val(data.iIdSede);
            $('#txtSede').val(data.strSede);
            $('#cboSede').html('');//HGM
            $('#cboSede').append('<option value="' + data.iIdSede + '">' + data.strSede + '</option>');//HGM


            $('#exampleModalCenterTitle').html("CAMBIO DE CLAVE");
            $('#modal-nuevo-login').modal({
                show: true,
                keyboard: false,
                backdrop: 'static'
            });
        })
        $('.login_delete').on('click', function () {
            window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
            row_del = _varTablaLogin.row($(this).parents('tr'));
            if (row_del.data().sNombreSesion == $('#spn_user').html()) {
                fn_verMensaje("El usuario tiene la sesión iniciada.","Eliminar Usuario","error");
            }
            else
                fn_eliminarLogin(row_del.data().iIdSesion);
        });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | datatable-login"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});
$('#btn-save-login').on('click', function () {


    //alert('HGM añadido 27.10.2021');
    let existeSede = 0;
    for (i = 0; i < arreglosSedes.length; i++) {
        
        var sedeSelec = arreglosSedes[i].split("_");
        //
        if (sedeSelec[0] == ($('#txtSede').val().trim()) ) {
            //$('#txtSede').val(item.data('sede')); //HGM
            $('#hddSede').val(sedeSelec[1]);
            existeSede = 1;
        }

    }


    if ($('#txtNombre').val().trim()=== "") {
        fn_verMensaje('Ingrese un Login', "Usuarios", "warning");
        return;
    }

    //if ($('#txtpwd1').val().trim() == "" ) {
    //    fn_verMensaje('Ingrese una clave. ', "Usuarios", "warning");
    //    return;
    //}

    //if ($('#txtpwd2').val().trim() == "") {
    //    fn_verMensaje('Repetir su clave. ', "Usuarios", "warning");
    //    return;
    //}

    //if ($('#txtpwd2').val() == "") {
    //    fn_verMensaje('Repetir Clave.', "Usuarios", "warning");
    //    return;
    //}

    if ($('#txtSede').val().trim() === "") {
        fn_verMensaje('Asignar una Sede', "Usuarios", "warning"); ñ
        $('#txtSede').focus();
        return;
    }

    if (existeSede == 0 ) {

        fn_verMensaje('La Sede asignada no existe.', "Usuarios", "warning");
        $('#txtSede').val('');
        $('#txtSede').focus();
        return;

    }




    try
    {
        window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
        debugger;
   
        var login = $('#txtNombre').val().trim();
        var sede = $('#txtSede').val().trim();

        if (login === "") {
            fn_verMensaje('Ingrese un Login', "Usuarios", "warning");
            return;
        }
        if (sede === "") {
            fn_verMensaje('Asignar una Sede', "Usuarios", "warning");
            return;
        }
        var idSesion = $('#hddIdSesion').val();
        var sede = $('#hddSede').val();
        var pw1 = "";
        var pw2 = "";
        var menus = "";
        var titulo = "Nuevo Usuario";
        if (operacion == 1) {
            titulo = "Editar Usuario";
        }else if (operacion == 2) {
                titulo = "Cambiar Clave";       
                pw1 = $('#txtpwd1').val();
                pw2 = $('#txtpwd2').val();

                if (pw1 == "") {
                    fn_verMensaje('Ingrese una Clave.', "Usuarios", "warning");
                    return;
                }

                if ($('#txtpwd2').val() == "") {
                    fn_verMensaje('Repetir Clave.', "Usuarios", "warning");
                    return;
                }
        }
        if (operacion !== 1) {
            pw1 = $('#txtpwd1').val();
            pw2 = $('#txtpwd2').val();

            if (pw1 == "") {
                fn_verMensaje('Ingrese una Clave.', "Usuarios", "warning");
                return;
            }

            if (pw2 == "") {
                fn_verMensaje('Repetir Clave.', "Usuarios", "warning");
                return;
            }
            if (pw1 !== pw2) {
                fn_verMensaje('La clave no coincide.', "Usuarios", "warning");
                return;
            }
        }
        if (operacion !== 2) {
            if (sede === 0) {
                fn_verMensaje('Debe seleccionar una sede.', "Usuarios", "warning");
                return;
            }
            menus = '';
            var favorite = [];
            $.each($("input[name='chkSede']:checked"), function () {
                favorite.push($(this).val());
            });
            menus = favorite.join(",");
            if (menus === "") {
                fn_verMensaje('Debe seleccionar un menú para este usuario.', "Registro de Usuarios", "error");
                return;
            }
        }
        if (operacion === 0) {
            //alert('operacion === 0')


            if ($('#txtpwd1').val().trim() == "") {
                fn_verMensaje('Ingrese una clave. ', "Usuarios", "warning");
                return;
            }

            if ($('#txtpwd2').val().trim() == "") {
                fn_verMensaje('Repetir su clave. ', "Usuarios", "warning");
                return;
            }


            $.post(
                '/UsuarioLogin/RegistrarLogin',
                {
                     x_iIdSesion: idSesion
                    , x_sNombreSesion: login
                    , x_sPasswordSesion: pw1
                    , x_intIdSede: sede  
                    , x_menus: menus
                },
                function (response) {
                    if (response !== null) {
                        if (!response.exito) {
                            fn_verMensaje(response.message, titulo, 'error')
                        }
                        else {
                            _varTablaLogin.row.add(response.entidad);

                            fn_table_draw(_varTablaLogin);
                            fn_verMensaje(response.message, titulo, response.type);
                            fn_limpiarFormLogin();
                            $('#modal-nuevo-login').modal('hide');
                        }
                    }
                }
            ).fail(function (xhr, textStatus, errorThrown) {
                fn_controlarExcepcion(xhr);
            });
        }
        if (operacion === 1) {



            $.post(
                '/UsuarioLogin/CambiarSede',
                {
                    x_iIdSesion: idSesion
                    , x_intIdSede: sede
                    , x_menus: menus
                },
                function (response) {
                    if (response !== null) {
                        if (!response.exito) {
                            fn_verMensaje(response.message, titulo, 'error')
                        }
                        else {
                                row_del.data(response.entidad);

                            fn_table_draw(_varTablaLogin);
                            fn_verMensaje(response.message, titulo, response.type);
                            fn_limpiarFormLogin();
                            $('#modal-nuevo-login').modal('hide');
                        }
                    }
                }
            ).fail(function (xhr, textStatus, errorThrown) {
                fn_controlarExcepcion(xhr);
            });
        }
        if (operacion === 2) {
            $.post(
                '/UsuarioLogin/CambiarPassword',
                {
                    x_iIdSesion: idSesion
                    , x_sPasswordSesion: pw1
                },
                function (response) {
                    if (response !== null) {
                        if (!response.exito) {
                            fn_verMensaje(response.message, titulo, 'error')
                        }
                        else {
                            fn_verMensaje(response.message, titulo, response.type);
                            fn_limpiarFormLogin();
                            $('#modal-nuevo-login').modal('hide');
                        }
                    }
                }
            ).fail(function (xhr, textStatus, errorThrown) {
                fn_controlarExcepcion(xhr);
            });
        }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | btn-save-login"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});
fn_obtenerMenuUsuario = function (idUsuario) {
    try
    {
        $('#chkLstMenu').html('');
        $.post(
            '/UsuarioLogin/ListarMenuUsuarioJson',
            {
                x_idSesion: idUsuario
            },
            function (response) {
                var ckklst = $('#chkLstMenu');
                for (i = 0; i < response.length; i++) {
                    var dato = response[i].Valor.split('-');
                    ckklst.append('<div class="form-check"><input type="checkbox" class="form-check-input" name="chkSede" id="chk_' + dato[0] + '" ' + (dato[1] > 0 ? 'checked' : '') + ' value="' + dato[0] + '"><label class="form-check-label" for="chk_' + dato[0] + '">' + response[i].Texto + '</label></div>');
                }
            }
        ).fail(function (xhr, textStatus, errorThrown) {
            fn_controlarExcepcion(xhr);
            });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_obtenerMenuUsuario"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
fn_limpiarFormLogin = function () {

    fn_limpiarMensajes();
    $("#hddIdSesion").val('0');
    $("#txtNombre").val('');
    $("#txtpwd1").val('');
    $("#txtpwd2").val('');
    $('#txtSede').val(0);
    window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
}
fn_eliminarLogin = function (id) {
    try
    {
        $.confirm({
        title: 'Eliminar Usuario',
        content: '¿Está seguro de eliminar el registro de forma permanente?',
        buttons: {
            si: {
                keys: ['s'],
                action: function () {
                    $.post(
                        '/UsuarioLogin/EliminarLogin',
                        {
                            x_iIdSesion: id
                        },
                        function (response) {
                            window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
                            if (response !== null) {
                                if (!response.exito) {
                                   
                                    fn_verMensaje(response.message, "Eliminar Usuario", 'error')
                                    // alert(response.message);
                                }
                                else {
                                    row_del.remove();
                                    row_del = null;
                                    fn_table_draw(_varTablaLogin);
                                    fn_verMensaje(response.message, "Eliminar Usuario", response.type);
                                    fn_limpiarFormLogin();
                                }
                            }
                        }
                    ).fail(function (xhr, textStatus, errorThrown) {
                        fn_controlarExcepcion(xhr);
                        //var errorMessage = xhr.status + ': ' + xhr.statusText
                        //alert(errorMessage);
                    });
                }
            },
            no: function () {
            }
        }
        });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_eliminarLogin"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}




//=== TERMINAL ==============================================================================================================================
fn_cargarLector = function () {


    $('.sticky-top').css('z-index', '1');//HGM 12.11.2021
    $.blockUI({ //  $.unblockUI(); //Añadido 12.11.2021  88888
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        },
        message: 'Cargando...'
    });

    try {

        var active = $('#cboEstadoLectList option:selected').val();
        console.log("entro...");
        //fn_limpiarFormLector();
        $.post(
            '/Lector/ListarLectorJson',
            { x_filtro:''
              , x_estado: active
              ,x_IdTerminal:0
            },
            function (response) {
                console.log(response)
                fn_disenioTablaLector(response);
                $.unblockUI();
            }
        ).fail(function (result) {
            fn_controlarExcepcion(result);
            $.unblockUI();
        });
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_cargarLector"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
        $.unblockUI();
    }
}
fn_disenioTablaLector = function (data) {
    try {
        var

        buttons = '<button class="btn btn-sm btn-dark sedes_edit" title="Editar Lectosr"><i class="fa fa-pencil"></i> Editar </button>&nbsp;';
        buttons += '<button class="btn btn-sm "  id="btnUpEstado" style="width:9%; height:26px;"><svg style="width:100%;"  aria-hidden="true" focusable="false" data-prefix="fas" data-icon="fingerprint" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="svg-inline--fa fa-fingerprint fa-w-16"><path fill="currentColor" d="M256.12 245.96c-13.25 0-24 10.74-24 24 1.14 72.25-8.14 141.9-27.7 211.55-2.73 9.72 2.15 30.49 23.12 30.49 10.48 0 20.11-6.92 23.09-17.52 13.53-47.91 31.04-125.41 29.48-224.52.01-13.25-10.73-24-23.99-24zm-.86-81.73C194 164.16 151.25 211.3 152.1 265.32c.75 47.94-3.75 95.91-13.37 142.55-2.69 12.98 5.67 25.69 18.64 28.36 13.05 2.67 25.67-5.66 28.36-18.64 10.34-50.09 15.17-101.58 14.37-153.02-.41-25.95 19.92-52.49 54.45-52.34 31.31.47 57.15 25.34 57.62 55.47.77 48.05-2.81 96.33-10.61 143.55-2.17 13.06 6.69 25.42 19.76 27.58 19.97 3.33 26.81-15.1 27.58-19.77 8.28-50.03 12.06-101.21 11.27-152.11-.88-55.8-47.94-101.88-104.91-102.72zm-110.69-19.78c-10.3-8.34-25.37-6.8-33.76 3.48-25.62 31.5-39.39 71.28-38.75 112 .59 37.58-2.47 75.27-9.11 112.05-2.34 13.05 6.31 25.53 19.36 27.89 20.11 3.5 27.07-14.81 27.89-19.36 7.19-39.84 10.5-80.66 9.86-121.33-.47-29.88 9.2-57.88 28-80.97 8.35-10.28 6.79-25.39-3.49-33.76zm109.47-62.33c-15.41-.41-30.87 1.44-45.78 4.97-12.89 3.06-20.87 15.98-17.83 28.89 3.06 12.89 16 20.83 28.89 17.83 11.05-2.61 22.47-3.77 34-3.69 75.43 1.13 137.73 61.5 138.88 134.58.59 37.88-1.28 76.11-5.58 113.63-1.5 13.17 7.95 25.08 21.11 26.58 16.72 1.95 25.51-11.88 26.58-21.11a929.06 929.06 0 0 0 5.89-119.85c-1.56-98.75-85.07-180.33-186.16-181.83zm252.07 121.45c-2.86-12.92-15.51-21.2-28.61-18.27-12.94 2.86-21.12 15.66-18.26 28.61 4.71 21.41 4.91 37.41 4.7 61.6-.11 13.27 10.55 24.09 23.8 24.2h.2c13.17 0 23.89-10.61 24-23.8.18-22.18.4-44.11-5.83-72.34zm-40.12-90.72C417.29 43.46 337.6 1.29 252.81.02 183.02-.82 118.47 24.91 70.46 72.94 24.09 119.37-.9 181.04.14 246.65l-.12 21.47c-.39 13.25 10.03 24.31 23.28 24.69.23.02.48.02.72.02 12.92 0 23.59-10.3 23.97-23.3l.16-23.64c-.83-52.5 19.16-101.86 56.28-139 38.76-38.8 91.34-59.67 147.68-58.86 69.45 1.03 134.73 35.56 174.62 92.39 7.61 10.86 22.56 13.45 33.42 5.86 10.84-7.62 13.46-22.59 5.84-33.43z" class=""></path></svg></button>&nbsp;';
        buttons += '<button class="btn btn-sm btn-danger sedes_delete " title="Eliminar Lector"><i class="fa fa-trash-o"></i></button>';

        var colHdd = [
            {
                targets: [0],
                visible: false,
                searchable: false
            },
            {
                bSortable: false ,
                targets: -1,
                data: null,
                defaultContent: '<div style="width:100% !important; text-align:center;">' + buttons + '</div>'
            }
            ,
            {
                targets: [4],
                visible: true,
                searchable: false,
                //width: "26%",
                bSortable: false // HGM https://stackoverflow.com/questions/16335928/how-to-remove-sorting-option-from-datatables
            },


        ];



        //var colHdd_HGM = [
        //    {
        //        targets: [0],
        //        visible: false,
        //        searchable: false,
        //        targets: 0, // your case first column
        //        className: "text-center",
        //        width: "4%"
        //    },
        //    {
        //        targets: [3],
        //        visible: true,
        //        searchable: false,
        //        //className: "justify-content-center",
        //        width: "6%",
        //        //"ordering": false HGM.18.11.2021
        //        bSortable: false // HGM https://stackoverflow.com/questions/16335928/how-to-remove-sorting-option-from-datatables

        //    },

        //    {
        //        targets: -1,
        //        data: null,
        //        defaultContent: '<div style="width:100% !important; text-align:center;" >' + //HGM
        //        '<button class="btn btn-sm btn-dark login_edit" title="Editar Usuario"><i class="fa fa-pencil"></i> Editar </button>&nbsp;' +
        //        '<button class="btn btn-sm btn-secondary login_pwd " title="Cambiar Clave"><i class="fa fa-keyboard-o"></i> Cambiar clave</button>&nbsp;' +
        //        '<button class="btn btn-sm btn-danger login_delete " title="Eliminar Usuario"><i class="fa fa-trash-o"></i></button></div>'
        //}];








        if (data !== undefined) {
            var count = 0;
            if (typeof _varTablaLector !== 'undefined')
                _varTablaLector.destroy();
            _varTablaLector = $('.datatable-lector').DataTable({
                data: data,
                columns: [
                    { data: 'iIdTerminal' },
                    { data: 'iNumero' },
                    { data: 'sSerie' },
                    { data: 'sDescripcion' },
                    { data: 'strActivo' },
                    { data: '' },
                ],
                lengthMenu: [10, 25, 50],
                order: [[1, 'asc']],
                language: _tableLanguaje,
                columnDefs: colHdd,
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var estado = aData.bActivo === 1;
                    if (estado) {
                        $(nRow).find(".btn").addClass('btn-success').addClass('lector_inactivar');
                        $(nRow).find("#btnUpEstado").prop('title', 'Inactivar Lector');
                         $(nRow).find(".fe").addClass('fe-user-minus');
                    }
                    else {
                        $(nRow).find(".btn").addClass('btn-warning').addClass('lector_activar');
                        $(nRow).find("#btnUpEstado").prop('title', 'Activar Lector');
                        $(nRow).find(".fe").addClass('fe-user-check');
                        $(nRow).css('color', 'red').css('background', '#ffeded4d');
                    }
                }
                , dom: 'lBfrtip',
                buttons: [{
                    extend: 'excelHtml5',
                    title: 'Listado de Lectores de Huella',
                    extension: '.xlsx',
                    text: 'Exportar a Excel  '
                    , exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                }]

            });
        } else {
            _varTablaLector = $('.datatable-lector').DataTable({
                lengthMenu: [10, 25, 50],
                order: [[2, 'asc']],
                language: _tableLanguaje,
                columnDefs: colHdd,
                dom: 'lBfrtip',
                buttons: [{
                    extend: 'excelHtml5',
                    title: 'Listado de Lectores de Huella',
                    extension: '.xlsx',
                    text: 'Exportar a Excel  '
                    , exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                }]
            });
        }
        fn_formatLinkExport();
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_disenioTablaLector"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }

    fn_limpiarFormLector = function () {
        //fn_limpiarMensajes();
        $("#txtCodigo").val('');
        window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
    }
    fn_eliminarLector = function (id) {
        try {
            $.confirm({
                title: 'Eliminar Usuario',
                content: '¿Está seguro de eliminar el registro de forma permanente?',
                buttons: {
                    si: {
                        keys: ['s'],
                        action: function () {
                            $.post(
                                '/UsuarioLogin/EliminarLogin',
                                {
                                    x_iIdSesion: id
                                },
                                function (response) {
                                    window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
                                    if (response !== null) {
                                        if (!response.exito) {

                                            fn_verMensaje(response.message, "Eliminar Usuario", 'error')
                                            // alert(response.message);
                                        }
                                        else {
                                            row_del.remove();
                                            row_del = null;
                                            fn_table_draw(_varTablaLogin);
                                            fn_verMensaje(response.message, "Eliminar Usuario", response.type);
                                            fn_limpiarFormLogin();
                                        }
                                    }
                                }
                            ).fail(function (xhr, textStatus, errorThrown) {
                                fn_controlarExcepcion(xhr);
                                //var errorMessage = xhr.status + ': ' + xhr.statusText
                                //alert(errorMessage);
                            });
                        }
                    },
                    no: function () {
                    }
                }
            });
        }
        catch (e) {
            $.post(
                '/Login/LogsJs',
                {
                    Excep: e
                    , NameFunction: "coreweb.js | fn_eliminarLogin"
                },
                function (response) {
                    console.log("Error js en TXt log")
                }
            ).fail(function (result) {
            });
            console.error();
        }
    }
}
$('#btn-nuev-lect').on('click', function () {
    try {
        $("#txtNum").val('');
        $("#txtSerie").val('');
        $("#txtDescrip").val('');
        //$('#txtNum').prop('disabled', false);
        $("#txtIdTermEdit").val(0);

        operacion = 0;
        $('#exampleModalCenterTitle').html("REGISTRO DE LECTOR");
        $('#modal-lector-reg').modal({
            show: true,
            keyboard: false,
            backdrop: 'static'
        });
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | btn-nuev-lect"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
})
$('.datatable-lector').on('draw.dt', function () {
    try {
        var btn = 0;//añadido 18.06.2021
        $(".lector_edit").unbind("click");
        $(".lector_delete").unbind("click");
        $(".lector_activar").unbind("click");
        $(".lector_inactivar").unbind("click");

        $('.lector_edit').on('click', function () {
            window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
            $("#txtNum").val('');
            $("#txtSerie").val('');
            $("#txtDescrip").val('');
            $("#txtIdTermEdit").val('');
            btn = 2;//añadido 18.06.2021
            row_del = _varTablaLector.row($(this).parents('tr'));
            var data = row_del.data();

            $("#txtNum").val(data.iNumero);
            $("#txtSerie").val(data.sSerie);
            $("#txtDescrip").val(data.sDescripcion);
            //$('#txtNum').prop('disabled', true);
            $("#txtIdTermEdit").val(data.iIdTerminal); //enviar PK

            $('#exampleModalCenterTitlePer').html('EDITAR LECTOR');
            $('#modal-lector-reg').modal({
                show: true,
                keyboard: false,
                backdrop: 'static'
            });
        })

        $('.lector_delete').on('click', function () {
            btn = 3; //añadido 18.06.2021
            row_del = _varTablaLector.row($(this).parents('tr'));
            window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)

            $.confirm({
                title: 'Eliminar Lector',
                content: '¿Está seguro de eliminar el registro de forma permanente?',
                //content: '¿Está seguro de eliminar el registro de forma permanente? <b>Las marcas realizadas por este personal también serán eliminadas</b>',
                buttons: {
                    si: {
                        keys: ['s'],
                        action: function () {
                            $.post(
                                '/Lector/EliminarLector',
                                {
                                    x_idTerminal: row_del.data().iIdTerminal
                                },
                                function (response) {
                                    if (response !== null) {
                                        if (!response.exito) {
                                            fn_verMensaje(response.message, "Eliminar Lector", 'error')//modificado 18.06.2021
                                        }
                                        else {
                                            row_del.remove();
                                            row_del = null;
                                            fn_table_draw(_varTablaLector);
                                            fn_verMensaje(response.message, "Eliminar Lector", response.type);//modificado 18.06.2021
                                        }
                                    }
                                }
                            ).fail(function (xhr, textStatus, errorThrown) {
                                fn_controlarExcepcion(xhr);
                            });
                        }
                    },
                    no: function () {
                    }
                }
            });
        });

        $(".lector_activar").on("click", function () {
            if (btn == 0) {
                tr = $(this).parents('tr');
                row_del = _varTablaLector.row($(this).parents('tr'));
                var data = row_del.data();
                fn_CambiarEstadoLector(data.iIdTerminal, 1);
            }
            btn = 0;
        });
        $(".lector_inactivar").on("click", function () {
            if (btn == 0) {
                tr = $(this).parents('tr');
                row_del = _varTablaLector.row($(this).parents('tr'));
                var data = row_del.data();
                fn_CambiarEstadoLector(data.iIdTerminal, 0);
            }
            btn = 0;
        });
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | datatable-personal"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});
$('#btn-save-lect').on('click', function () {
    try {
        window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
        var numero = $("#txtNum").val();
        var serie = $("#txtSerie").val().trim();
        var descripcion = $("#txtDescrip").val().trim();
        var idTerminal = $("#txtIdTermEdit").val(); //tomar el id a editar
        if (numero === "") {
            fn_verMensaje('Asigne un Número al lector', "Número", "warning");
            return;
        }
        if (serie === "") {
            fn_verMensaje('Ingrese la Serie del lector', "Serie", "warning");
            return;
        }
        if (descripcion === "") {
            fn_verMensaje('Ingrese una Descripción', "Descripción", "warning");
            return;
        }

        var titulo = "Nuevo Lector";
        if (idTerminal != 0) {
            titulo = "Editar Lector";
        }

            $.post(
                '/Lector/RegistraLector',
                {
                    IdTerminal: idTerminal
                    , x_numero: numero
                    , x_serie: serie
                    , x_descripcion: descripcion
                },
                function (response) {
                    if (response !== null) {
                        if (!response.exito) {
                            fn_verMensaje(response.message, titulo, 'error')
                        }
                        else {
                            //_varTablaLector.row.add(response.entidad);
                             //fn_table_draw(_varTablaLector);
                            fn_verMensaje(response.message, titulo, response.type);
                            fn_cargarLector();//añadido 24.06.2021
                            $('#modal-lector-reg').modal('hide');
                        }
                    }
                }
            ).fail(function (xhr, textStatus, errorThrown) {
                fn_controlarExcepcion(xhr);
            });

    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | btn-save-lect"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});
$('#cboEstadoLectList').on('change', function () { fn_cargarLector(); });
fn_CambiarEstadoLector = function (usuario, estado) {
    try {
        window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
        estActi = estado;
        var titulo = 'Inactivar Lector';
        var preg = "¿Está seguro de inactivar el registro de forma permanente?";
        if (estado == 1) {
            titulo = 'Activar Lector';
            preg = "¿Está seguro de activar el registro de forma permanente?";
        }
        $.confirm({
            title: titulo,
            content: preg,
            buttons: {
                si: {
                    keys: ['s'],
                    action: function () {
                        $.post(
                            '/Lector/CambiarEstadoLector',
                            {
                                x_idTerminal: row_del.data().iIdTerminal,
                                x_estado: estado
                            },
                            function (response) {
                                if (response !== null) {
                                    if (!response.exito) {
                                        fn_verMensaje(response.message, titulo, 'error') //18.06.2021
                                    }
                                    else {
                                        debugger;
                                        if ($('#cboEstadoLectList').val() == 2) {
                                            if (estActi === 0)
                                                $(tr).css('color', 'red').css('background', '#ffeded4d');
                                            else
                                                $(tr).css('color', '').css('background', '');
                                            row_del.data(response.entidad);
                                        }
                                        else {
                                            row_del.remove();
                                            row_del = null;
                                        }

                                        fn_table_draw(_varTablaLector);
                                        fn_verMensaje(response.message, titulo, response.type);//18.06.2021
                                    }
                                }
                            }
                        ).fail(function (xhr, textStatus, errorThrown) {
                            fn_controlarExcepcion(xhr);
                        });
                    }
                },
                no: function () {
                }
            }
        });
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_CambiarEstadoLector"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}





//////////////////////////////////////////////////////////////////
// VALIDACION DE CARACTERES DE CODIGO - BASADO EN PERFIL
//////////////////////////////////////////////////////////////////
function validarCodigoAll(evt) {
    //onkeypress = "validarCodigoAll(event)"
    //let k = event ? event.which : window.event.keyCode;
    //if (k == 32) return false;

    var theEvent = evt || window.event;
    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|[a-z]|[A-Z]|\_|\/|\-/; //Números, Letras ---> a-z,A-Z, _, - sin espacio, slash, guion, guion bajo
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }

}

//Funcion PEsonalizada solo Codigo de Personal
function validarCodigoPersonal(evt) {
    //onkeypress = "validarCodigoPersonal(event)"
    //let k = event ? event.which : window.event.keyCode;
    //if (k == 32) return false;

    var theEvent = evt || window.event;
    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|[a-z]|[A-Z]|\_|\/|\-/; //Números, Letras ---> a-z,A-Z, _, - sin espacio, slash, guion, guion bajo
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }

}

//Funcion PEsonalizada solo Codigo de Personal
function validarPersonal(evt) {
    //onkeypress = "validarPersonal(event)"
    //let k = event ? event.which : window.event.keyCode;
    //if (k == 32) return false;

    var theEvent = evt || window.event;
    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|[a-z]|[A-Z]|\_|\/|\-|\ /; //Números, Letras ---> a-z,A-Z, _, - sin espacio, slash, guion, guion bajo
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }

}

function validarControlesFecha(evt) {
    //onkeypress = "validarControlesFecha(event)"
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    ////var regex = /[0-9]|[a-z]|[A-Z]|\_|\-/; //Numeros, Letras ---> a-z,A-Z, _ , -
    var regex = /[0-9]|\//; //Numeros, Letras ---> a-z,A-Z, _ , -
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }


}
function mostrarClave_1() {

    const el = document.getElementById("buttomshowpass_1");
    const x = document.getElementById("txtpwd1");
    if (x.type === "password") {
        x.type = "text";
        el.innerHTML = '<i class="bi bi-eye-slash"></i>';
    } else {
        x.type = "password";
        el.innerHTML = '<i class="bi bi-eye"></i>';
    }

}
function mostrarClave_2() {

    const el = document.getElementById("buttomshowpass_2");
    const x = document.getElementById("txtpwd2");
    if (x.type === "password") {
        x.type = "text";
        el.innerHTML = '<i class="bi bi-eye-slash"></i>';
    } else {
        x.type = "password";
        el.innerHTML = '<i class="bi bi-eye"></i>';
    }

}
$('#txtpwd1').bind('input', function () {
    var current = $('#txtpwd1').val();
    var newData = current.replace("x", "");
    newData = newData.replace("x", ""); // small x if you want
    $('#txtpwd1').val(newData);
});
/////////////PARA EL OJIO DE LOS INPUTS CLAVE
function mostrarClave_1() {

    const el = document.getElementById("buttomshowpass_1");
    const x = document.getElementById("txtpwd1");
    if (x.type === "password") {
        x.type = "text";
        el.innerHTML = '<i class="bi bi-eye-slash"></i>';
    } else {
        x.type = "password";
        el.innerHTML = '<i class="bi bi-eye"></i>';
    }

}
function mostrarClave_2() {

    const el = document.getElementById("buttomshowpass_2");
    const x = document.getElementById("txtpwd2");
    if (x.type === "password") {
        x.type = "text";
        el.innerHTML = '<i class="bi bi-eye-slash"></i>';
    } else {
        x.type = "password";
        el.innerHTML = '<i class="bi bi-eye"></i>';
    }

}
$('#id_Password').bind('input', function () {
    var current = $('#id_Password').val();
    var newData = current.replace("x", "");
    newData = newData.replace("x", ""); // small x if you want
    $('#id_Password').val(newData);
});


function txt_strDireccion(evt) {

    var theEvent = evt || window.event;
    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|[a-z]|[A-Z]|\_|\/|\-|\ |\#|\&|\°/; //Números, Letras ---> a-z,A-Z, _, - sin espacio, slash, guion, guion bajo
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }

}





/*====================================================================================================
=========   MANTENIMIENTO EDES:  LISTAR SEDES
======================================================================================================*/
//fn_cargarLector = function () {
$('#cboEstadoSedesList').on('change', function () { fn_cargarSedes2(); });

fn_cargarSedes2 = function () {

    $('.sticky-top').css('z-index', '1');//HGM 12.11.2021
    $.blockUI({ //  $.unblockUI(); //Añadido 12.11.2021  88888
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        },
        message: 'Cargando...'
    });

    try {
        var active = $('#cboEstadoSedesList option:selected').val();// cboEstadoLectList 
        //fn_limpiarFormLector();
        $.post(
            //'/Lector/ListarLectorJson',
            '/Sedes/ListarSedesJson',
            {
                  x_filtro: ''
                , x_estado: active
                , x_intIdSede: 0
            },
            function (response) {

                console.log(response)
                fn_disenioTablaSedes(response);
                $.unblockUI();
            }
        ).fail(function (result) {
            //alert()
            fn_controlarExcepcion(result);
        });
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_cargarLector"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
        $.unblockUI();
    }
}

fn_disenioTablaSedes = function (data) {


    console.log(data); 
    try {   //Iconos desde https://fontawesome.com/v4.7/icon/home
        var buttons = '<button class="btn btn-sm btn-dark sedes_edit" title="Editar Sede"><i class="fa fa-pencil"></i> Editar </button>&nbsp;';
            buttons += '<button class="btn btn-sm " id="btnUpEstadoS"><i class="fa fa-home"></i></button>&nbsp;';
            buttons += '<button class="btn btn-sm btn-danger sedes_delete " title="Eliminar Sede"><i class="fa fa-trash-o"></i></button>';

        var colHdd = [
            {
                targets: [0],
                visible: false,
                searchable: false
            },
            {
                bSortable: false,
                targets: -1,
                data: null,
                defaultContent: '<div style="width:140px !important">' + buttons + '</div>'
            }];



        if (data !== undefined) {
            var count = 0;
            if (typeof _varTablaSedes !== 'undefined')
                _varTablaSedes.destroy();
            _varTablaSedes = $('.datatable-sedes').DataTable({
                data: data,
                columns: [
                    { data: 'intIdSede' },
                    { data: 'strCoLocal' },
                    { data: 'strDeLocal' },
                    { data: 'strDireccion' },
                    { data: 'strEstadoActivo' },
                    { data: '' },

                ],
                lengthMenu: [10, 25, 50],
                order: [[1, 'asc']],
                language: _tableLanguaje,
                columnDefs: colHdd,
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var estado = aData.bitActivo === true;
                    if (estado) {
                        $(nRow).find(".btn").addClass('btn-success').addClass('sedes_inactivar');
                        $(nRow).find("#btnUpEstadoS").prop('title', 'Inactivar Sede');
                        $(nRow).find(".fe").addClass('fe-user-minus');
                    }
                    else {
                        $(nRow).find(".btn").addClass('btn-warning').addClass('sedes_activar');
                        $(nRow).find("#btnUpEstadoS").prop('title', 'Activar Sede');
                        $(nRow).find(".fe").addClass('fe-user-check');
                        //$(nRow).css('color', 'red').css('background', '#ffeded4d');
                        $(nRow).css('color', 'red').css('background', '#ffeded4d');
                    }
                }
                , dom: 'lBfrtip',

                buttons: [{
                    extend: 'excelHtml5',
                    title: 'Listado de Sedes',
                    extension: '.xlsx',
                    text: 'Exportar a Excel  '
                    , exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                }]

            });
        } else {
            _varTablaSedes = $('.datatable-sedes').DataTable({
                lengthMenu: [10, 25, 50],
                order: [[2, 'asc']],
                language: _tableLanguaje,
                columnDefs: colHdd,
                dom: 'lBfrtip',
                buttons: [{
                    extend: 'excelHtml5',
                    title: 'Listado de Sedes',
                    extension: '.xlsx',
                    text: 'Exportar a Excel  '
                    , exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                }]
            });
        }
        fn_formatLinkExport();
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_disenioTablaSedes"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }

    //////fn_limpiarFormLector = function () {
    //////    //fn_limpiarMensajes();
    //////    $("#txtCodigo").val('');
    //////    window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
    //////}


    //fn_eliminarLector = function (id) {
    fn_eliminarSedes = function (id) {
        try {
            $.confirm({
                title: 'Eliminar Sede',
                content: '¿Está seguro de eliminar el registro de Sede de forma permanente111?',
                buttons: {
                    si: {
                        keys: ['s'],
                        action: function () {

                            $.post(
                                '/UsuarioLogin/EliminarLogin',
                                {
                                    x_iIdSesion: id
                                },
                                function (response) {
                                    //window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
                                    if (response !== null) {
                                        if (!response.exito) {

                                            fn_verMensaje(response.message, "Eliminar Sede", 'error')
                                            // alert(response.message);
                                        }
                                        else {
                                            row_del.remove();
                                            row_del = null;
                                            fn_table_draw(_varTablaSedes);
                                            fn_verMensaje(response.message, "Eliminar Sede", response.type);
                                            fn_limpiarFormLogin(); ///////////////////////////////////////////////
                                        }
                                    }
                                }
                            ).fail(function (xhr, textStatus, errorThrown) {
                                fn_controlarExcepcion(xhr);
                                //var errorMessage = xhr.status + ': ' + xhr.statusText
                                //alert(errorMessage);
                            });
                        }
                    },
                    no: function () {
                    }
                }
            });
        }
        catch (e) {
            $.post(
                '/Login/LogsJs',
                {
                    Excep: e
                    , NameFunction: "coreweb.js | fn_eliminarLogin"
                },
                function (response) {
                    console.log("Error js en TXt log")
                }
            ).fail(function (result) {
            });
            console.error();
        }
    }
}

$('.datatable-sedes').on('draw.dt', function () {
    try {
        var btn = 0;//añadido 18.06.2021
        $(".sedes_edit").unbind("click");
        $(".sedes_delete").unbind("click");
        $(".sedes_activar").unbind("click");
        $(".sedes_inactivar").unbind("click");

        $('.sedes_edit').on('click', function () {

            var colocal = $("#txt_strCoLocal").val();
            var delocal = $("#txt_strDeLocal").val().trim();
            var dilocal = $("#txt_strDireccion").val().trim();
            var intIdSede = $("#txt_intIdSede").val(); //tomar el id a editar


            btn = 2;//añadido 18.06.2021
            row_del = _varTablaSedes.row($(this).parents('tr'));
            var data = row_del.data();

            $("#txt_strCoLocal").val(data.strCoLocal);
            $("#txt_strDeLocal").val(data.strDeLocal);
            $("#txt_strDireccion").val(data.strDireccion);
            $("#txt_intIdSede").val(data.intIdSede); //enviar PK

            $('#exampleModalCenterTitlePer').html('EDITAR SEDE');
            $('#modal-sedes-reg').modal({
                show: true,
                keyboard: false,
                backdrop: 'static'
            });
        })

        $('.sedes_delete').on('click', function () {
            //alert('.sedes_delete');
            btn = 3; //añadido 18.06.2021
            row_del = _varTablaSedes.row($(this).parents('tr'));
            //window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)

            $.confirm({
                title: 'Eliminar Sede',
                content: '¿Está seguro de eliminar la sede  "' + row_del.data().strDeLocal + '" de forma permanente?',
                //content: '¿Está seguro de eliminar el registro de forma permanente? <b>Las marcas realizadas por este personal también serán eliminadas</b>',
                buttons: {
                    si: {
                        keys: ['s'],
                        action: function () {
                            $.post(
                                //'/Lector/EliminarLector',
                                '/Sedes/EliminarSedes',
                                {
                                    x_intIdSede: row_del.data().intIdSede
                                },
                                function (response) {
                                    if (response !== null) {
                                        if (!response.exito) {
                                            fn_verMensaje(response.message, "Eliminar Sede", 'error')//modificado 18.06.2021
                                        }
                                        else {
                                            row_del.remove();
                                            row_del = null;
                                            fn_table_draw(_varTablaSedes);
                                            fn_verMensaje(response.message, "Eliminar Sede", response.type);//modificado 18.06.2021
                                        }
                                    }
                                }
                            ).fail(function (xhr, textStatus, errorThrown) {
                                fn_controlarExcepcion(xhr);
                            });
                        }
                    },
                    no: function () {
                    }
                }
            });
        });

        $(".sedes_activar").on("click", function () {
            if (btn == 0) {
                tr = $(this).parents('tr');
                row_del = _varTablaSedes.row($(this).parents('tr'));
                var data = row_del.data();
                //fn_CambiarEstadoLector(data.intIdSede, 1);
                fn_CambiarEstadoSedes(data.intIdSede, 1);
            }
            btn = 0;
        });


        $(".sedes_inactivar").on("click", function () {
            //alert(".sedes_inactivar");
            if (btn == 0) {
                tr = $(this).parents('tr');
                row_del = _varTablaSedes.row($(this).parents('tr'));
                var data = row_del.data();
                //fn_CambiarEstadoLector(data.iIdTerminal, 0);//
                fn_CambiarEstadoSedes(data.intIdSede, 0);
            }
            btn = 0;
        });
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | datatable-personal"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});

//$('#btn-nuev-lect').on('click', function () {
$('#btn-nuev-sedes').on('click', function () {

    try {
        $("#txt_strCoLocal").val('');
        $("#txt_strDeLocal").val('');
        $("#txt_strDireccion").val('');
        $("#txt_intIdSede").val(0);// $("#txtIdTermEdit").val(0);

        operacion = 0;
        $('#exampleModalCenterTitle').html("REGISTRO DE SEDES");
        $('#modal-sedes-reg').modal({
            show: true,
            keyboard: false,
            backdrop: 'static'
        });
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | btn-nuev-lect"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
})


$('#btn-save-sedes').on('click', function () {
    //alert('#btn-save-sedes');
    try {
        //window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
        var colocal = $("#txt_strCoLocal").val();
        var delocal = $("#txt_strDeLocal").val().trim();
        var dilocal = $("#txt_strDireccion").val().trim();
        var intIdSede = $("#txt_intIdSede").val(); //tomar el id a editar
        if (colocal === "") {
            fn_verMensaje('Asigne un código a la Sede', "Códigoo", "warning");
            return;
        }
        if (delocal === "") {
            fn_verMensaje('Ingrese la descripción de la Sede', "Descripción", "warning");
            $("#txt_strCoLocal").focus();
            return;
        }
        //if (dilocal === "") {
        //    fn_verMensaje('Ingrese una Dirección', " Dirección", "warning");
        //    $("#txt_strDeLocal").focua()
        //    return;
        //}

        var titulo = "Nueva Sede";
        if (intIdSede != 0) {
            titulo = "Editar Sede";
        }


        //alert(intIdSede);
        $.post(
            '/Sedes/RegistraSedes',
            {
                //  IdTerminal: idTerminal
                //, x_numero: numero
                //, x_serie: serie
                //, x_descripcion: descripcion

                _intIdSede: intIdSede , _strCoLocal: colocal, _strDeLocal:delocal,  _strDireccion: dilocal, _bitActivo: 1, _strEstadoActivo: 'Activo'


            },
            function (response) {
                if (response !== null) {
                    if (!response.exito) {
                        fn_verMensaje(response.message, titulo, 'error')


                        //fn_cargarSedes2();

                    }
                    else {

                        fn_verMensaje(response.message, titulo, response.type);
                        //fn_cargarLector();

                        $("#txt_strCoLocal").val("");
                        $("#txt_strDeLocal").val("");
                        $("#txt_strDireccion").val("");
                        $("#txt_intIdSede").val(""); 
                        $('#modal-sedes-reg').modal('hide');
                        fn_cargarSedes2();

                    }
                }
            }
        ).fail(function (xhr, textStatus, errorThrown) {
            fn_controlarExcepcion(xhr);
        });

    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | btn-save-lect"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }

});



fn_CambiarEstadoSedes = function (usuario, estado) {
    try {
        //window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
        estActi = estado;
        var titulo = 'Inactivar Sede';
        var preg = "¿Está seguro de inactivar el registro de forma permanente?";
        if (estado == 1) {
            titulo = 'Activar Sede';
            preg = "¿Está seguro de activar el registro de forma permanente?";
        }
        $.confirm({
            title: titulo,
            content: preg,
            buttons: {
                si: {
                    keys: ['s'],
                    action: function () {
                        $.post(
                            //'/Lector/CambiarEstadoLector',
                            '/Sedes/CambiarEstadoSedes',
                            {
                                x_intIdSede: row_del.data().intIdSede,
                                x_estado: estado
                            },
                            function (response) {
                                if (response !== null) {
                                    if (!response.exito) {
                                        fn_verMensaje(response.message, titulo, 'error') //18.06.2021
                                    }
                                    else {
                                        debugger;
                                        if ($('#cboEstadoSedesList').val() == 2) {
                                            if (estActi === 0)
                                                $(tr).css('color', 'red').css('background', '#ffeded4d');
                                            else
                                                $(tr).css('color', '').css('background', '');
                                            row_del.data(response.entidad);
                                        }
                                        else {
                                            row_del.remove();
                                            row_del = null;
                                        }

                                        fn_table_draw(_varTablaSedes);
                                        fn_verMensaje(response.message, titulo, response.type);//18.06.2021
                                    }
                                }
                            }
                        ).fail(function (xhr, textStatus, errorThrown) {
                            fn_controlarExcepcion(xhr);
                        });
                    }
                },
                no: function () {
                }
            }
        });
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | fn_CambiarEstadoSedes"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}



























