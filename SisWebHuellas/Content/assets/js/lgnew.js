if ('serviceWorker' in navigator) {
    // Supported!
    console.log("Supported!");
    navigator.serviceWorker.register('/Content/assets/js/sw.js', { scope: '/Content/assets/js/' }).then(function (reg) {

        if (reg.installing) {
            console.log('[Service worker] installing...');
        } else if (reg.waiting) {
            console.log('[Service worker] installed..');
        } else if (reg.active) {
            console.log('[Service worker] active.');
        } else {
            console.log('La instalación, espera o activación falló.');
        }

    }).catch(function (error) {
        // registration failed
        console.log('El registro del SW falló con ' + error);
    });
} else {
    console.log("No soporta");
}
//if (window.caches) {
//    caches.open('v3');
//    caches.has('v3').then(console.log);
//};

$(document).ready(function () {
    try
    {
        console.log("Desde lgnew.js");
        var x_status_ = "";
        var x_status_ini = "";
        var p_status = true;
        if (localStorage.getItem("ls_online")) {
            x_status_ini = localStorage.getItem("ls_online");
            console.log("estado original: ")
            console.log(x_status_ini);
        } else {
            console.log("No existe nada en el Local Storage");
        }

        if (navigator.onLine) { //Solo funciona para IE (en Chrome siempre muestra TRUE)
            $('#11').html('<label id="_lbl_On" style="color:darkblue;  text-align:center;" >ONLINE</label> ');
            console.log('online');
            x_status_ = "ONLINE>>";

            $.post(
                '/Login/Estatus',
                {
                    p_status: true
                },
                function (response) {
                    //console.log("enviando datos al Contorlador")
                }
                ).fail(function (result) {
                fn_controlarExcepcion(result);
                });

            fn_CargarUsersLS();
            fn_CargarSedesLS();

        } else {
            $('#11').html('<label id="_lbl_Off" style="color:red;  text-align:center;" >OFFLINE</label> ');
            console.log('offline');
            x_status_ = "<<OFFLINE";

            $.post(
                '/Login/Estatus',
                {
                    p_status: false
                },
                function (response) {
                    //console.log("enviando datos al Contorlador")
                }
            ).fail(function (result) {
                fn_controlarExcepcion(result);
                });

            fn_ObtenerUsersLS(false);//añadido 16.06.2021
            fn_ObtenerSedesLS();//añadido 16.06.2021
        }
        localStorage.setItem("ls_online", x_status_);


        if (x_status_ini != "") {
            console.log("nuevo estado: ")
            console.log(localStorage.getItem("ls_online"));
        }
        fn_LimpiarLS();//añadido 15.06
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "lgnew.js | document"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});

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
                , NameFunction: "lgnew.js | fn_LimpiarLS"
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
                    lstUsers.push(newItem);
                }
                localStorage.setItem("Lst_UsersLS", JSON.stringify(lstUsers));
                console.log("Se copiaron al LS los usuarios.")
                if (localStorage.getItem("Lst_UsersLS")) {
                    console.log(JSON.parse(localStorage.getItem("Lst_UsersLS")));
                }
            }
        ).fail(function (result) {
            fn_controlarExcepcion(result);
        });


        var lstmenusUsers = [];//añadido 16.06
        $.post(
            '/UsuarioLogin/ListarMenusUsersJson',
            {},
            function (response) {
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
                    lstmenusUsers.push(newItem);
                }
                localStorage.setItem("Lst_MenusUsersLS", JSON.stringify(lstmenusUsers));
                console.log("Se copiaron al LS los menú.")
                if (localStorage.getItem("Lst_MenusUsersLS")) {
                    console.log(JSON.parse(localStorage.getItem("Lst_MenusUsersLS")));
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
                , NameFunction: "lgnew.js | fn_CargarUsersLS"
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
            { p_estatus: true },//modificado 16.06.2021
            function (response) {
                for (i = 0; i < response.length; i++) {
                    let newItem = {
                        x_IdSede: response[i].Valor
                        , x_DeLocal: response[i].Texto
                    }
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
                , NameFunction: "lgnew.js | fn_CargarSedesLS"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}

//añadido 16.06.2021: Obtener LS y enviarlo al Controller
fn_ObtenerUsersLS = function (Input) {
    try
    {
        var p_status = Input;
        if (localStorage.getItem("Lst_UsersLS") && localStorage.getItem("Lst_MenusUsersLS")) {
            var p_lstUsers = JSON.parse(localStorage.getItem("Lst_UsersLS"));
            var p_lstMenusUsers = JSON.parse(localStorage.getItem("Lst_MenusUsersLS"));
            console.log(p_lstUsers);
            console.log(p_lstMenusUsers);

            $.post(
                '/Login/llenarListas',
                {
                    p_lstUsers: p_lstUsers
                    , p_lstMenusUsers: p_lstMenusUsers
                    , p_status: p_status
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
                , NameFunction: "lgnew.js | fn_ObtenerUsersLS"
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

            $.post(
                '/RegistroHuellas/llenarListas',
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
                , NameFunction: "lgnew.js | fn_ObtenerSedesLS"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}


//copiada el 16.06 desde coreweb.js
fn_controlarExcepcion = function (jqXHR) {
    try
    {
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
                , NameFunction: "lgnew.js | fn_controlarExcepcion"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
//copiada el 16.06 desde coreweb.js
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
                        ////if (window.localStorage.getItem('enrollConected')) {
                        //$('#btn-iniciarMarc').prop('disabled', false);
                        //if (window.localStorage.getItem('modo') * 1 === 1)
                        //    setTimeout(function () { $('#btn-iniciarMarc').click(); }, 75);
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
                , NameFunction: "lgnew.js | fn_verMensaje"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}