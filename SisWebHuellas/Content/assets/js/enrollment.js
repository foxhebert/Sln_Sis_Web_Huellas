
//Funcion de prueba (comentable/eliminable)
$('#btn-validar-sesion-prueba').on('click', function () {
    //debugger;
    alert('tn-validar-sesion-prueba');
    //fn_iniciaMarca_callback();
    //$('altInstall').remove();
    data = new Object();
    data.Result = true;
    fn_iniciaMarca_callback(data);
});



var localserver = false;
var blnConectado = false;
var strSN = "";
var blnesIE = false;
var nInten = 0;
$(document).ready(function () {
        fn_verificaIE();
        fn_limpiarForm();
        fn_validaServ();//comentado 24.06.2021
        fn_Estatus();//añadido 17.06.2021
    $('#spn_down_set').click(function () {
            if (window.localStorage.getItem('initDown') == null)
                window.localStorage.setItem('initDown', 1);

            fn_validaServ();//comentado 24.06.2021
            fn_descargaInst();//añadido 25.06.2021
            ////añadido pruebas 09.06.2021 ES - inicio
            //$.post(
            //    '/RegistroHuellas/EjecutarBat',
            //    {},
            //    function (response) {
            //        console.log(response);
            //    }
            //).fail(function (result) {
            //});
            ////añadido pruebas 09.06.2021 ES - fin - no descomentar

    });



});


//Pagina Cargada completamente. Es lo opuesto a Document ready
document.addEventListener("DOMContentLoaded", function () {
    consultarDescargaZKOnline();
    ////if (typeof zkonline.InitSensor == 'undefined') {
    ////}
    //alert("Descargar ZKOnline");
});


//REMOVER EL BOTON DE "Descargar ZKOnline" CUANDO YA SE HA
//DESCARGADO UNA VEZ EL INSTALADOR AÑADIDO HGM 02.11.2021
function consultarDescargaZKOnline() {

    try {
        $.post(
            '/Login/ConsultarDescargaDriver',
            {},
            function (response) {

                if (response !== null) {

                    if (!response.exito) {                     
                        //$('#div_down_set').remove();
                        //$("#altInstall").css("text-decoration-line", "none");                

                    }
                    else {
                        //alert();
                        $('#div_down_set').show();
                    }
                }

            }
        ).fail(function (result) {
        });

    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_descargaInst"
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
fn_Estatus = function () {
    try
    {
        console.log("Desde enrollment.js");
        if (navigator.onLine) { //Solo funciona para IE (en Chrome siempre muestra TRUE)
            $('#11').html('<label id="_lbl_On" style="color:darkblue;  text-align:center;" >ONLINE</label> ');
            console.log('online');
            x_status_ = "ONLINE>>";

            //B) primero enviar AllMarcas del LS -> SQL
            fn_EnviarAllMarcas_LS();

            $('#11').html('<label id="_lbl_On" style="color:darkblue;  text-align:center;" >ONLINE</label> ');
        } else {
            $('#11').html('<label id="_lbl_Off" style="color:red;  text-align:center;" >OFFLINE</label> ');
            console.log('offline');
            x_status_ = "<<OFFLINE";
            ////fn_ObtenerSedesLS();//añadido 16.06.2021
        }
        localStorage.setItem("ls_online", x_status_);

        fn_LimpiarLS();
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_Estatus"
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
fn_EnviarAllMarcas_LS = function () {
    try
    {
        //B) primero enviar AllMarcas del LS -> SQL
        if (localStorage.getItem("ls_TRegMarca") != null && parseInt(localStorage.getItem("ls_TRegMarca")) > 0)
        {
            $('#11').html('<label id="_lbl_On" style="color:darkblue;  text-align:center;" >ONLINE: ...Sincronizando Registros de Marcaciones... label> ');
            var TotalRegOffline_ = localStorage.getItem("ls_TRegMarca")
            var i = 1;
            while (i <= TotalRegOffline_) {
                var clave_ = "x_marcaOffline_" + i.toString();

                if (localStorage.getItem(clave_))
                {
                    personal_ = JSON.parse(localStorage.getItem(clave_));
                    console.log("Marca " + i.toString() + ": ");
                    console.log(personal_);

                        //Llamar al método del Controlador para enviarlo a la BD y luego si es OK el registro eliminar del LS
                        $.post(
                            '/Asistencia/RegistraMarca_',
                            {
                                x_huella: personal_.x_huella
                                , x_sSerie: personal_.x_sSerie
                                , x_idLocal: personal_.x_idLocal //dato adicional
                                , x_FechaHora: personal_.x_FechaHora //dato adicional
                                , x_Pre: true //dato adicional
                            },
                            function (response) {
                                if (response !== null) {
                                    if (!response.exito) {
                                        console.log("Error en Marcaciones: con la clave " + clave_);
                                    }
                                    else {
                                        console.log("Marcaciones: " + response.message + "de la clave " + clave_);
                                        localStorage.removeItem(clave_);
                                    }
                                }
                            }
                        ).fail(function (xhr, textStatus, errorThrown) {
                            console.log("Fail POST Asistencia/RegistraMarca_: en la clave " + clave_);
                        });
                } else
                {
                    console.log("No existe nada en el Local Storage para la cleve: " + clave_);
                }
                i = i + 1;
            }
        }
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_EnviarAllMarcas_LS"
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
        if (localStorage.getItem("ls_TRegPersonal") != null && parseInt(localStorage.getItem("ls_TRegPersonal")) > 0)
        {
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
            if (x == TotalRegOffline_)
            {//Si solo si todos los registros eliminados son igual al total
                localStorage.removeItem("ls_TRegPersonal");
            }
        }

        //Limpiar Contador de Registro de Marcas en Local Storage
        if (localStorage.getItem("ls_TRegMarca") != null && parseInt(localStorage.getItem("ls_TRegMarca")) > 0)
        {
            console.log("Marcaciones_Pendientes_LS: ");
            console.log(localStorage.getItem("ls_TRegMarca"));
            //<< Validar si se eliminaron todos los registros en LS
            var TotalRegOffline_ = localStorage.getItem("ls_TRegMarca")
            var i = 1;
            var x = 0;//TotalRegOffline_;
            while (i <= TotalRegOffline_)
            {
                var clave_ = "x_marcaOffline_" + i.toString();
                if (!localStorage.getItem(clave_)) {
                    x = x + 1;
                }
                i = i + 1;
            }
            if (x == TotalRegOffline_)
            {//Si solo si todos los registros eliminados son igual al total
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
                , NameFunction: "enrollment.js | fn_LimpiarLS"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}



fn_verificaIE = function () {
    try
    {
	    if (window.localStorage.getItem('esIE') == null) {
		    var esIE = /*@cc_on!@*/false || !!document.documentMode;
		    window.localStorage.setItem('esIE',esIE ? 1: 0);
	    }
        if (window.localStorage.getItem('esIE') == 0) {
            //<INICIO comentado ES 11.06.2021>
            $('#btn-iniciarMarc').remove();
            $('#btn-val-silk').remove();
            $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021
            $('#altConector').addClass('alert-warning').html('Use Internet Explorer').prop('title', 'Usar Internet Explorer para interactuar con el enrolador.');
            $('#enrolar-huella').remove();
		    //<FIN comentado ES 11.06.2021>
            return;
        }
        //$('#altConector').html("").removeClass('alert-success').removeClass('alert-danger');  //comentado 11.06.2021
        $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_verificaIE"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
fn_iniciaMarca = function () {
    try
    {
        if (!window.localStorage.getItem('servLocalOK'))
            //return false;//comentado 11.06.2021

        if (window.localStorage.getItem('modo') * 1 === 1) {
            data = new Object();
            data.Result = true;
            fn_iniciaMarca_callback(data);
        }
        else
        {
            $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021
            //$('#altConector').html("").removeClass('alert-success').removeClass('alert-danger');

            var valuesAddress = $('body').data('base') + "Pause";
                ajax = $.ajax({
                    type: "GET",
                    dataType: "jsonp",
                    url: valuesAddress,
                    async: false,
                    jsonpCallback: 'fn_iniciaMarca_callback',
                    error: function (xhr, status, error) {
                        if (xhr.status) {
                            window.localStorage.setItem('servLocalOK', false)
                            return;
                        }
                    }
                });
        }

        ////añadido para pruebas 11.06.2021
        data = new Object();
        data.Result = true;
        fn_iniciaMarca_callback(data);
        ////añadido para pruebas 11.06.2021

    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_iniciaMarca"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}






fn_iniciaMarca_callback = function (data)
{
    try
    {
        if (!data.Result) {
            fn_verMensaje(data.Message, "Registro de Huellas", 5);
            return;
        }
        //$("#zkonline").prop('classid', "clsid:A318A9AC-E75F-424C-9364-6B40A848FC6B"); //añadido 24.06.2021 QA_driver
        window.localStorage.setItem('enrollPaused', 1);

        if (typeof zkonline.InitSensor !== 'undefined') {

            console.log("InitSensor !=undefined (IF)");
            zkonline.InitSensor();
            console.log("SensorSN");
            strSN = zkonline.SensorSN;//comentado 16.06.2021
            //strSN ="BXUC182260404";//añadido en duro el nro de Serie del Lector (borrar)
            if (strSN.trim() !== "") { //comentado ES 11.06.2021
                console.log("strSN.trim()!=  (IF)");
                $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021
                $('#altConector').addClass('alert-success').html('Serial: ' + strSN).prop('title', 'Enrolador conectado y listo para usarse.');
                if (window["zkonline"]) {//comentado ES 11.06.2021
                    console.log("window[zkonline] (IF)");
                    window.localStorage.setItem('modo', 1);//le indica que debe levantar el lector dsps del mensaje
                    zkonline.DefaultWindowClose = 1000;
                    zkonline.SetVerHint = "Registrar Marcación";
                    zkonline.SetLanguageFile('zkonline.es')
                    zkonline.CheckFinger = '0000000000';
                    zkonline.FPEngineVersion = "10";
                    if (zkonline.GetVerTemplate()) //evento disparado por el lector al reconocer la huella
                    {//comentado ES 11.06.2021
                        var strHuella = zkonline.VerifyTemplate;//comentado ES 11.06.2021
                        console.log("Huella10: zkonline.VerifyTemplate");//añadido 21.06 pruebas
                        console.log(strHuella);//añadido 21.06 pruebas

                        //var strHuella = 'TSFTUzIxAAAEYmAECAUHCc7QAAAoY5EBAAAAhI8qXmL+APwOdwAgAHpvOgAAAVYNsgA8Y4gNnAAsAVwPIWIOAdcLSACPAe1plgBKAYgPhwCwYvcPMgBTASUBW2JmAfoOegCxAYBtrgCdAJIP8gBtYvcOfgD+AEAPTGIPAeAPPwAzAOBvYwDJAAgM4wAHY9cLXQBLAT8OJ2IuAdYMqAD7AZVtvQDWAB0OegApY5oPRwBkATsFcGKSAH8MNQC3AGpthgBfAIkHnQDsYmQLTwDiADwMgGLdAIwPSAD/AdlsoADXAJQPQQBOY4AOigC2AE0PiWJUAX8OPQCdAfZnkQBfAXwPrwBqYwMPXwB5AUQHrWJ0AJIMcACdAAhmQJrpHhEbLPQ9mYMY+HdNeWCBGWhfkB4NEQrrEO5jbQORg+4AnAzxm5jxWQh5CST5ppDC/LuSsPNU+rmFnfvJeQ3+SIRxZZOMqgJWD+J30OiC76d7sQsD+X7rQI2F5lH/KZN9jcwH1hDGpFuN6GGkgZn3CRHkH8FhbwC2+t76nAmdmNT74Yvjkt6PSW1rBU8UJQxMBkF1L/rT+lN6GXoHBU57hX4S+oIOsA1YaomDoJcgF76eXBDt+z4JIPTFhfPs7XtlC1MOIWYA+aX6Qv7n9qZoNQelAOH45Re1lmgJuP/R9nT+EGnggI2A4YAEif2dKwKGClsXRIa37eob5/zsyysDJCIAAkYdqAjFi0Lhw19kDgAomvD7NUH9wP5VB8WEZuuHeAMAjGLWwAtiHWzxW/9L7cBeYQEybXTBCcU6aZb+QDgEADW3bX5hAa54GsIKxXCU5JJ3YAsAcFB9gKPAwWkJAHhQCfsmVBEACpfghcD6nP49///ATMsAq/uXcML/wsMFw8SjwQ4AE6vkjv80UMHABAC1oN9dF2LXp5NEwsC5dY8NBwCGs4+JBg0E5LmMiMCdhQUFBOy5E0sFAGQAACxpAVzJd8TAAP7EFMAMAGTKAzlT+51SwhUA3NRfYnCdxcDCwMPCBcJmoxEAoNWadwHBxqOSwHIDAGETA/lkAVnZa8PBpQoE4NiTw5jBwakEBNzbIHMMAGEZAy6jTXEHAKXc21ZOawGK3xpYRQcFBBflhp8WANwusME7wHKAwsLDOnzEnQIAfPyTxsMQPWNbcP8MEF7E9P+d/VjBxDYX1d8G+ERSg8KIwVF0xGYRPQRWVgzVSA++/v77+/1XtQcUJBJWT5IEEOcVUqPBDRCbLJwFwMenxJTAbgoQoC0HmXnBWhUQ2vSQ+iFlwcTDxMIHe/s4AhBjMmbAyBBMVd/BH1nA/wfAFnLMP4w0e8IBqWkYChCmPoz/hsrHo0MDEIJHhgcHFPdGjGzG/gPVgkjhwwUQnUwMpwcU6VOAwH7BBtWTUWE3wwgQi1hFeGJkEY5dfcDBkgYU9F4D/v+QBNWOZhhYCRBtbfo6T2ZmEUhyBmUH1YBwn0dsChDQgkPBXhbBCBDEiH2HwPum'; //añadido para pruebas ES 11.06.2021
                        $('#btn-iniciarMarc').prop('disabled', true);

                        //añadir opcion de Validar Navigator.OFFLINE >> almacenamiento local en Cache Storage 14.06.2021
                        var idLocal = 0;
                        if ($('#idse').val() != null || $('#idse').val() != "") {
                            idLocal = $('#idse').val(); //variable nueva que normalmente la coloca el Controlador
                        }
                        console.log("IdSede:");
                        console.log(idLocal);
                        var Now_ = new Date();
                        fechahora = Now_.GetFecha() + ' ' + Now_.getHours() + ':' + Now_.getMinutes() + ':' + Now_.getSeconds() + '.' + Now_.getMilliseconds();
                        if (navigator.onLine) {
                            //---------------------------------------------------------------------------
                            fn_EnviarAllMarcas_LS();;//añadido 15.06.2021

                            //luego enviar el nuevo registro actual Online
                            //---------------------------------------------------------------------------
                            $.post(
                                '/Asistencia/RegistraMarca_',
                                {
                                    x_huella: strHuella
                                    , x_sSerie: strSN //añadido para pruebas ES 11.06.2021
                                    , x_idLocal: idLocal //dato adicional añadido 16.06
                                    , x_FechaHora: fechahora //dato adicional añadido 16.06
                                    , x_Pre: false //dato adicional
                                },
                                //{ x_huella: strHuella, x_sSerie: zkonline.SensorSN },//comentado ES 11.06.2021
                                function (response) {
                                    console.log(response);
                                    if (response !== null) {
                                        if (response.exito) {
                                            $('#btn-filtrar-marcas').click();
                                        }
                                        tipo = 1;
                                        if (response.type !== "success")
                                            tipo = 0;
                                        
                                        fn_noticaLc(tipo, response.message);
                                        window.localStorage.setItem('modo', 1);//le indica que debe levantar el lector dsps del mensaje
                                        fn_verMensaje(response.message, "Marcaciones", response.type);
                                    }
                                }
                            ).fail(function (xhr, textStatus, errorThrown) {
                                fn_controlarExcepcion(xhr);
                                fn_ReanudaMarca();
                                if (xhr.status === 500) {
                                    $('#btn-iniciarMarc').prop('disabled', true);
                                }
                                //fn_validarHuellaLocal(strHuella, zkonline.SensorSN);
                            });

                            //---------------------------------------------------------------------------
                        } else {
                            //---------------------------------------------------------------------------
                            ////eliminar la siguiente linea:
                            //strHuella = 'TZNTUzIxAAAE0NMECAUHCc7QAAAo0ZEBAAAAhH0wjtDBAOoPowAcAObfqQCwAGYPcQCy0GUPagCcAMUPZtDsAAoPoABCAFPfVAChABgNXQBx0F8PQADwANUMstByAFsPxgCxAF7dXABlAGoJUwBF0QEPvwA+Ab4P29BVAXMKggBwAOzfhADjAIsPTgCm0NYPcgDiAM8PhNCPAHgPTwB6AJPfSgDOAJcPWgAM0fgPSQCWAMEIzNCFAFUMRQBMAPLYUwBtAOgFuAAq0QIPUgA0AcwMsNBCAGkIlQDYAHfecQDGAA4PbgC30NcPpgDkACkPcdDuAIsPtgAuAHPfsgCOANgPpgCL0PwOUwCTAMMNWdB+AO4K5QAWANzfjAAiAYUPFAAV0W0G6wAAAS0JntBDAGkPywCcAffdpwB0AQgB+AQJD1sU1Pu9b9dYOEEtB66QqJJ1A3ZWLAT56hH+uIFZ0ff92fk5gcv8IcQIFGEaUQnnBIY5bwk+eT957ABB2miHqQBhBEeEeVRYf1JvHvzzfHpWN4geCoZ+QIGiKnpvwQBtETzefVFcAeqo/fGQYuo8rYN5gd0B8KJ1q4h/NQCygDeJsdA3ieKK4nSXDWJVcQMxEJ3sLPydrsj+vfSl8LgLhVDA/er0ZYFfBP4vcwtvh4MLXANTwL74Sg0zDIoXUfpoJ7EHTHF9dho9SQ+ldBZgaYN9UdwBmYypl6OBPVGYhm1/sfw8+DHA8Qe18HEIAPA52eTq5vMXYfp2ZVXXjDKE/oR69GMgA3xWEyty3APWKVoNhYE+eDLsZ8QuCUuPgfJEwAQUIUQBAmwefAUEcxJ0/nYIAGMZdBL/UcAFAJHYcfsQwAkArx5wBV7GtQsAtSNxcvqDD9C6Jmv+e1X5CwRnMHHAwWRDOw0EbzZxdf7AYuwKBBw7a2vAWP/IANXvaMH/wf/ABMD6LWsFAJhDZoEMBGJFbcFXwz/0DwQFRWlt/3H9PsPELf4MANlLaaj+wBHAwfv2CQAbUGaS/sN8DwDgnWLELpZB/Pv8Q80A5bFhwMJSwATFWmC9ZwgA52ZeBcLFhRQA7G1gwgVdxi7//v0hTHXCALCmXcPA/v/8xgDCpl/CCgDrd5ZUxYT8EgD0d1OCwVAu//79/f/+mQsEJoVQQ8A4/jgFBBmEV1sJAKBMU8Qs+vpOBQBTVwnFL8kOAPmSV6fALS3//zgDACxf/frbATafDP7+BGzHTw4A+aBTVQX9Ry38PgUAVaXfwIjUAf+nVlYDxai1ivoIACy1EDhpddQAALZXwP7CALBpWyT7wAMA9LweEQIATr2cwc8BBxFW/kr//kLJACkTFsDBwFuSBgoF181cSjNCB8UuysdrXRAA49mnRjT5wf9XEgEIGV77jv8+Pv//VcYBDDph/wgAQ+7VW4jdATX0Gn50BHTE1AAJ+WY2C9U3Bcd/c4QKELvgesSSN/8MEQUIrv5/7S4HEDsOFwbAxK4REQcVccC3/jP7wFkGEQYeqP/FnxERAipwwvvA+y3+wvzBwELAEFDkDXMPEQA3qMBQLks/OAMQvfl9xNQRv0J3/v/PEFefEofDQsAJ1fVXuTJGwgQQXZkMa9sR7GJwO1MGJwHA3XF6wP7BwRB8owjF/QQQiL4A+BM='; //añadido para pruebas ES 11.06.2021
                            ////añadido 16.06.2021 <<INICIO

                            localStorage.setItem("ls_online", "<<OFFLINE");
                            console.log('offline');
                            $('#11').html('<label id="_lbl_Off" style="color:red;  text-align:center;" >OFFLINE</label> ');
                            var TotalRegOffline = 0;
                            if (localStorage.getItem("ls_TRegMarca")) {
                                var TotalRegOffline = parseInt(localStorage.getItem("ls_TRegMarca"));
                            }

                            //aqui almacenar en Local Storage
                            let MarcaNew = {
                                x_huella: strHuella
                                , x_sSerie: strSN
                                , x_idLocal: idLocal //dato adicional
                                , x_FechaHora: fechahora //dato adicional
                            }
                            //generar Clave
                            var j = TotalRegOffline + 1;
                            var clave = "x_marcaOffline_" + j.toString();
                            localStorage.setItem(clave, JSON.stringify(MarcaNew));

                            if (localStorage.getItem(clave)) {
                                localStorage.setItem("ls_TRegMarca", j);
                                window.localStorage.setItem('modo', 1);//le indica que debe levantar el lector dsps del mensaje
                                fn_verMensaje("La marca se registró exitosamente (inLS).", "Marcaciones", "success");
                                // $('#btn-filtrar-marcas').click();//comentado para pruebas 17.06.2021
                            }
                            //añadido 16.06.2021 >>FIN
                            //---------------------------------------------------------------------------
                        }

                    }//comentado ES 11.06.2021
                    else {//comentado ES 11.06.2021
                        zkonline.FreeSensor();//comentado ES 11.06.2021
                        fn_ReanudaMarca();//comentado ES 11.06.2021
                        //$('#altConector').html("").removeClass('alert-success').removeClass('alert-danger');//comentado ES 11.06.2021
                        $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021
                    }//comentado ES 11.06.2021
                    //<inicio comentado ES 11.06.2021>
                }

                else {
                    console.log("window[zkonline] (ELSE)");
                    window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
                    fn_verMensaje('Verificar que el Driver ZKOnline y el lector de huellas están conectados.', "Iniciar Marcaciones", "error");
                    $('#altConector').addClass('alert-danger').html('Sin Conexión').prop('title', 'El enrolador no está conectado.');
                    fn_ReanudaMarca();

                    //$('#altInstall').hide();

                }
                //console.log("I---------------------------------");
                //console.log('FPEngineVersion: ' + zkonline.FPEngineVersion);
                //console.log('SensorSN: ' + zkonline.SensorSN);
                //console.log('VerifyCount: ' + zkonline.VerifyCount);
                //console.log('RegisterTemplate: ' + zkonline.RegisterTemplate);
                //console.log('VerifyTemplate: ' + zkonline.VerifyTemplate);
                //console.log('Threshold: ' + zkonline.Threshold);
                //console.log('OneToOneThreshold: ' + zkonline.OneToOneThreshold);
                //console.log('CheckFinger: ' + zkonline.CheckFinger);
                //console.log('EnrollCount: ' + zkonline.EnrollCount);
                //console.log('DefaultRegFinger: ' + zkonline.DefaultRegFinger);
                //console.log('DefaultWindowClose: ' + zkonline.DefaultWindowClose);
                //console.log('SetVerHint: ' + zkonline.SetVerHint);
                //console.log('IsSupportDuress: ' + zkonline.IsSupportDuress);
                //console.log('LastQuality: ' + zkonline.LastQuality);
                //console.log('LowestQuality: ' + zkonline.LowestQuality);
                //console.log('FakeFunOn: ' + zkonline.FakeFunOn);
                //console.log("O=================================");
            }
            else {
                console.log("strSN.trim()!= ELSE");
                //window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
                fn_verMensaje('El enrolador no está conectado', "Iniciar Marcaciones", "error");
                ////            window.localStorage.setItem('enrollConected', false);
                $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021
                $('#altConector').addClass('alert-danger').html('Sin Conexión').prop('title', 'El enrolador no está conectado.');
                fn_ReanudaMarca();

                //$('#altInstall').hide();
            }//<fin comentado ES 11.06.2021>
        }
        else {
            console.log("InitSensor == undefined (ELSE)");
            //fn_verMensaje("No se pudo detectar el Driver, Vuelva a intentar.", "Iniciar Marcaciones", "error");
            //fn_ReanudaMarca();       
            //////var mensaje  = function () {
            //////    return true;
            //////}
            //////setTimeout(window.location.reload(), 3000);//Comentado 28.10.2021 HGM
            //window.location.reload(); //añadido 28.06.2021

            //AÑADIDO HGM 
            //Vuelve a aparecer el boton para descargal el driver
          
            var ejecutar = function () { window.location.reload(); }
            $.alert({
                title: "Iniciar Marcaciones",
                content: "No se pudo detectar el Driver, Vuelva a intentar.",
                type: 'orange',
                escapeKey: true,
                buttons: {
                    ok: {
                        text: 'Aceptar',
                        action: function () {
                            ejecutar();
                          
                        }
                    }
                }
            });

        }

        $('#btn-iniciarMarc').show();
        $('#altConector').tooltip();
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction:"enrollment.js | fn_iniciaMarca_callback"
            },
            function (response) {
                console.log("error al log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}

fn_ReanudaMarca = function () {
    try
    {
        if (window.localStorage.getItem('enrollPaused') * 1 !== 1)
            return;

        var valuesAddress = $('body').data('base') + "Play";
        ajax = $.ajax({
            type: "GET",
            dataType: "jsonp",
            url: valuesAddress,
            async: false,
            jsonpCallback: 'fn_ReanudaMarca_callback',
            error: function (xhr, status, error) {
                if (xhr.status) {
                    window.localStorage.setItem('servLocalOK', false)
                    return;
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
                , NameFunction: "enrollment.js | fn_iniciaEnroll_callback"
            },
            function (response) {
                console.log("Error js en TXt log")
                $.unblockUI();//Añadido HGM 12.11.2021
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
fn_ReanudaMarca_callback = function (data) {
    try
    {
            window.localStorage.removeItem('enrollPaused');
            window.localStorage.removeItem('modo');
            console.log(data);
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_ReanudaMarca_callback"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}

fn_iniciaEnroll = function () {
    try
    {
        //$('#altConector').html("").removeClass('alert-success').removeClass('alert-danger');
        $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021

        var valuesAddress = $('body').data('base') + "Pause";
        ////<inicio comentado 18.06.2021
        //ajax = $.ajax({
        //    type: "GET",
        //    dataType: "jsonp",
        //    url: valuesAddress,
        //    async: false,
        //    jsonpCallback: 'fn_iniciaEnroll_callback',
        //    error: function (xhr, status, error) {
        //        if (xhr.status) {
        //            window.localStorage.setItem('servLocalOK', false)
        //            return;
        //        }
        //    }
        //});
        ////<fin comentado 18.06.2021

        fn_iniciaEnroll_callback();//añadido para pruebas 11.06.2021 (borrar)

        $.unblockUI();//Añadido HGM 12.11.2021
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_iniciaEnroll"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
        $.unblockUI();//Añadido HGM 12.11.2021
    }

}

//ENROLAMIENTO
fn_iniciaEnroll_callback = function (data) {
    try
    {
            //<inicio comentado ES 11.06.2021 >
            //if (!data.Result) {
            //    fn_verMensaje(data.Message, "Registro de Huellas", 5);
            //    return;
            //}
           // <fin comentado ES 11.06.2021>

        //$("#zkonline").prop('classid', "clsid:A318A9AC-E75F-424C-9364-6B40A848FC6B"); //añadido 24.06.2021 QA_driver
       // $("#zkonline").attr('classid', 'clsid:A318A9AC-E75F-424C-9364-6B40A848FC6B');
        localStorage.clear(); //adicionado pruebas QA 24.06.2021.1844 ES


            window.localStorage.setItem('enrollPaused', 1);
            zkonline.InitSensor();
            zkonline.FPEngineVersion = "10";
            if (window["zkonline"]) {
                zkonline.SetVerHint = "Enrolar Huellas";
                zkonline.SetLanguageFile('zkonline.es')

            } else {
                window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector)
                fn_verMensaje('Compruebe para confirmar que el cliente ZKOnline y el dispositivo de huellas digitales están conectados.', "Registro de Huellas", "error");
                fn_ReanudaMarca();
                return;
            }

            if (typeof zkonline.InitSensor !== 'undefined') {
                zkonline.InitSensor();
                strSN = zkonline.SensorSN;
                if (strSN.trim() !== "") {//comentado ES 11.06.2021
                    $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021
                    $('#altConector').addClass('alert-success').html('Serial: ' + strSN).prop('title', 'Enrolador conectado y listo para usarse.');
                    if (window["zkonline"]) { //comentado ES 11.06.2021
                        window.localStorage.setItem('modo', 1); //añadido 23.06.2021 (proposito: decir que si debe levantar el lector dsps del mensaje)
                        zkonline.FPEngineVersion = "10";
                        if (zkonline.Register()) {
                            for (i = 1; i <= 10; i++) {
                                if (zkonline.GetRegFingerTemplate(i).length > 2) {
                                    lstHuellas[i - 1].strHuella = zkonline.GetRegFingerTemplate(i);
                                    lstHuellas[i - 1].lnHuella = lstHuellas[i - 1].strHuella.length;
                                }
                            }
                            zkonline.RegisterTemplate = "";//清空临时接收模板
                        }


                        var h = zkonline.CheckFinger;
                        var t = 0;
                        for (i = 0; i < 10; i++) {
                            t += (h[i] === "1") * 1;
                            if (h[i] === "0")
                                lstHuellas[i].lnHuella = 0;
                        }

                        $('#txtNumHuellas').val(t);
                        zkonline.FreeSensor();
                        fn_ReanudaMarca();
                        //$('#altConector').html("").removeClass('alert-success').removeClass('alert-danger');
                        $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021
               // <inicio comentado ES 11.06.2021>
                    } else {
                        window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector dsps del mensaje)
                        fn_verMensaje('Compruebe para confirmar que el cliente ZKOnline y el dispositivo de huellas digitales están conectados.', "Registro de Huellas", "error");
                        $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021
                        $('#altConector').addClass('alert-danger').html('Sin Conexión').prop('title', 'El enrolador no está conectado..');
                        fn_ReanudaMarca();

                        //$('#altInstall').hide();
                    }
                }
                else {
                    window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector dsps del mensaje)
                    fn_verMensaje('El enrolador no está conectado', "Registro de Huellas", "error");
                    $('#altConector').html("").removeClass('alert-success').removeClass('alert-danger').removeClass('alert-warning');//añadido 25.06.2021
                    $('#altConector').addClass('alert-danger').html('Sin Conexión').prop('title', 'El enrolador no está conectado.');
                    fn_ReanudaMarca();
                    //$('#altInstall').hide();

                }
               // <fin comentado ES 11.06.2021>
            }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_iniciaEnroll_callback"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
}
//#region LOCAL_SERVER
fn_validaServ = function () {
    try
    {
        if (window.localStorage.getItem('urilc') != null) {
            $('body').data('base', window.localStorage.getItem('urilc'));
            fn_existeSerlc();//comentado 24.06.2021
		    fn_establecerSede();		
        }
        else {
            $.post(
                '/Login/ObtenerUrlWslc',
                {},
                function (response) {
                    window.localStorage.setItem('urilc', response.message);
                    $('body').data('base', window.localStorage.getItem('urilc'));
                    fn_existeSerlc();
				    fn_establecerSede();				
                }
            ).fail(function (result) {
                localserver = false;
                //fn_descargaInst();
            });
        }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_validaServ"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
    }
}

fn_existeSerlc = function () {
    try
    {
        var valuesAddress = $('body').data('base') + "exists";
        ajax = $.ajax({
            type: "GET",
            dataType: "jsonp",
            url: valuesAddress,
            async: false,
            jsonpCallback: 'fn_existeSerlc_callback',
            error: function (xhr, status, error) {
                if (xhr.status) {

                    $('#altInstall').show('slow');
                    localserver = false;

                }
                //fn_descargaInst();
            }
        });
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_existeSerlc"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
    }
}
fn_existeSerlc_callback = function (data) {
    if (data.Result) {
        fn_configSerLc();
    }
}

fn_configSerLc = function () {
    try
    {
        window.localStorage.setItem('modo', 0); //añadido 23.06.2021 (proposito: decir que no debe levantar el lector dsps del mensaje)
        nInten++;
        if (nInten == 3) {
            fn_verMensaje("Ocurrió un error al iniciar el Registro de Huellas", "Registro de HUellas", 5);
            return;
        }

        var valuesAddress = $('body').data('base') + "Config";
        ajax = $.ajax({
            type: "GET",
            dataType: "jsonp",
            url: valuesAddress,
            async: false,
            jsonpCallback: 'fn_configSerLc_callback',
            error: function (xhr, status, error) {
                if (xhr.status) {

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
                , NameFunction: "enrollment.js | fn_configSerLc"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
    }
}
fn_configSerLc_callback = function (data) {
    try
    {
        if (!data.Result) {
            $.post(
                '/Login/ObtenerUrlWsse',
                {},
                function (response) {
                    fn_configSerSe(response);
                }
            ).fail(function (result) {
            });
        }
        else {
            window.localStorage.setItem('servLocalOK', true);
            $('#btn-iniciarMarc').show('slow');
        }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_configSerLc_callback"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
    }
}

fn_configSerSe = function (ur) {
    try {
        var valuesAddress = $('body').data('base') + "Save/" + ur;
        ajax = $.ajax({
            type: "GET",
            dataType: "jsonp",
            url: valuesAddress,
            async: false,
            jsonpCallback: 'fn_configSerSe_callback',
            error: function (xhr, status, error) {
                if (xhr.status) {
                    $('#altInstall').show('slow');
                    //$('#btn-iniciarMarc').hide();//comentado ES 11.06.2021
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
                , NameFunction: "enrollment.js | fn_configSerSe"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
    }
}
fn_configSerSe_callback = function (data) {
    try
    {
        if (!data.Result) {
            if (window.localStorage.getItem('initDown') == null)
                fn_configSerLc();
            else {
                window.localStorage.removeItem('initDown');
               // fn_descargaInst();
            }
        }
        else {
            nInten = 0;
            window.localStorage.setItem('servLocalOK', true);
            $('#btn-iniciarMarc').show('slow');
            $('#altInstall').remove();
            window.localStorage.removeItem('initDown');
            fn_establecerSede();
        }
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_configSerSe_callback"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    } 
}

fn_descargaInst = function () {
    try
    {
        $.post(
            '/Login/DescargaDriver',
            { },
            function (response) {

                if (response !== null) {

                    if (!response.exito) {//mostrar mensaje de validación y preguntar si desea continuar

                            $.confirm({
                                title: 'ZkOnline Descargado',
                                content: '¿Está seguro que desea volver a descargar el Driver ZkOnline?',
                                buttons: {
                                    si: {
                                        keys: ['s'],
                                        action: function () {
                                        //--------------------------------------------------------
                                            if (typeof zkonline.InitSensor !== 'undefined') {
                                                console.log("InitSensor !=undefined (IF)>> Existe Driver");
                                                $('#exampleModalCenterTitle').html("REINSTALAR ZKONLINE");
                                                $('#btn-down-n').show();
                                                $('#btn-sgt-n').hide();
                                                $('#modal-descarga-msg').modal({
                                                    show: true,
                                                    keyboard: false,
                                                    backdrop: 'static'
                                                });
                                            }
                                            else {
                                                console.log("InitSensor !=undefined (ELSE)>> No Existe Driver");
                                                window.localStorage.setItem('nDown', window.localStorage.getItem('nDown') * 1 + 1);
                                                window.location.href = $('#down_set').attr('href');
                                            }
                                        //--------------------------------------------------------
                                        }
                                    },
                                    no: function () {
                                    }
                                }
                            });
                    }
                    else {//No mostrar mensaje de registro exitoso pero si continuar con la descarga
                        if (typeof zkonline.InitSensor !== 'undefined') {
                            console.log("InitSensor !=undefined (IF)>> Existe Driver");
                            $('#exampleModalCenterTitle').html("REINSTALAR ZKONLINE");
                            $('#btn-down-n').show();
                            $('#btn-sgt-n').hide();
                            $('#modal-descarga-msg').modal({
                                show: true,
                                keyboard: false,
                                backdrop: 'static'
                            });
                            consultarDescargaZKOnline();
                        }
                        else {
                            console.log("InitSensor !=undefined (ELSE)>> No Existe Driver");
                            window.localStorage.setItem('nDown', window.localStorage.getItem('nDown') * 1 + 1);
                            window.location.href = $('#down_set').attr('href');
                        }



                        $('#div_down_set').hide('slow');
                    }

                    
                }
                //else {


                //}

            }
        ).fail(function (result) {
        });


        //if (window.localStorage.getItem('initDown') == null)
        //    return;

        //if ($('#down_set').length > 0) {
        //    if (window.localStorage.getItem('nDown') == null)
        //        window.localStorage.setItem('nDown', 0);

        //    if (window.localStorage.getItem('nDown') * 1 > 0) {

        //        var msj = "Va a descargar el instalador por " + (window.localStorage.getItem('nDown') * 1 + 1);
        //        switch (window.localStorage.getItem('nDown') * 1 + 1) {
        //            case 2: msj = msj + "da"; break;
        //            case 3: msj = msj + "ra"; break;
        //            default: msj = msj + "ta"; break;
        //        }
        //        msj = msj + " vez, ¿Desea continuar?";
        //        $.confirm({
        //            title: 'Descargar ZkOnline',
        //            content: msj,
        //            buttons: {
        //                si: {
        //                    keys: ['s'],
        //                    action: function () {
        //                        window.localStorage.setItem('nDown', window.localStorage.getItem('nDown') * 1 + 1);
        //                        window.location.href = $('#down_set').attr('href');
        //                        setTimeout(fn_msgCloseWindows(), 100);
        //                    }
        //                },
        //                no: function () {
        //                }
        //            }
        //        });
        //    }
        //    else {
        //        window.localStorage.setItem('nDown', window.localStorage.getItem('nDown') * 1 + 1);
        //        window.location.href = $('#down_set').attr('href');
        //        setTimeout(fn_msgCloseWindows(), 100);

        //    }
        //}


       //window.localStorage.removeItem('initDown');
    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_descargaInst"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    } 
}

//#endregion LOCAL_SERVER
fn_establecerSede = function () {
    try
    {
	    if (typeof $('body').data('base') === 'undefined')
	    {
		    return;
	    }
        var valuesAddress = $('body').data('base') + "SetSede/" + $('#idse').val();
        ajax = $.ajax({
            type: "GET",
            dataType: "jsonp",
            url: valuesAddress,
            async: false,
            jsonpCallback: 'fn_establecerSede_callback',
            error: function (xhr, status, error) {
                if (xhr.status) {
                   // $('#altInstall').show('slow'); //COMENTADO ES 11.06.2021
                    // $('#btn-iniciarMarc').hide(); //comentado ES 11.06.2021
                    $('#btn-iniciarMarc').show(); //AÑADIDO PARA PRUEBAS ES 11.06.2021 (BORRAR)

                     //$('#altInstall').remove();

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
                , NameFunction: "enrollment.js | fn_establecerSede"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }        
}
fn_establecerSede_callback = function (data) {
    console.log(data);
}

fn_noticaLc = function (tipo, message) {
    try
    {
        if (message != null) {
            message = message.replace(/\//g, "~");

            var valuesAddress = $('body').data('base') + "Notifica/" + tipo + "/" + message;
            ajax = $.ajax({
                type: "GET",
                dataType: "jsonp",
                url: valuesAddress,
                async: false,
                jsonpCallback: 'fn_noticaLc_callback',
                error: function (xhr, status, error) {
                    if (xhr.status) {
                        window.localStorage.setItem('servLocalOK', false)
                        return;
                    }
                }
            });
        } else {
            console.log("Mensaje es nulo en fn_noticaLc")
        }

    }
    catch (e)
    {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "enrollment.js | fn_noticaLc"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }

}
fn_noticaLc_callback = function (data) {
    console.log(data);
}




$('#btn-descarga').click(function () {
    try {
        //#region comentado
        /*
        if (window.localStorage.getItem('initDown') == null)
            return;

        if ($('#down_set').length > 0) {
            if (window.localStorage.getItem('nDown') == null)
                window.localStorage.setItem('nDown', 0);

            if (window.localStorage.getItem('nDown') * 1 > 0) {

                var msj = "Va a descargar el instalador por " + (window.localStorage.getItem('nDown') * 1 + 1);
                switch (window.localStorage.getItem('nDown') * 1 + 1) {
                    case 2: msj = msj + "da"; break;
                    case 3: msj = msj + "ra"; break;
                    default: msj = msj + "ta"; break;
                }
                msj = msj + " vez, ¿Desea continuar?";
                $.confirm({
                    title: 'Descargar ZkOnline',
                    content: msj,
                    buttons: {
                        si: {
                            keys: ['s'],
                            action: function () {
                                window.localStorage.setItem('nDown', window.localStorage.getItem('nDown') * 1 + 1);
                                window.location.href = $('#down_set').attr('href');
                            }
                        },
                        no: function () {
                        }
                    }
                });
            }
            else {
                window.localStorage.setItem('nDown', window.localStorage.getItem('nDown') * 1 + 1);
                window.location.href = $('#down_set').attr('href');
            }
        }
                window.localStorage.removeItem('initDown');
        $('#btn-down-n').hide();
        $('#btn-sgt-n').show();

        */
        //#endregion comentado

        window.localStorage.setItem('nDown', window.localStorage.getItem('nDown') * 1 + 1);
        window.location.href = $('#down_set').attr('href');
        //window.localStorage.removeItem('initDown');
        $('#btn-down-n').hide();
        $('#btn-sgt-n').show();
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | btn-down-"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});

$('#btn-sgt-n').on('click', function () {
    try {
        setTimeout(window.close(), 75);
    }
    catch (e) {
        $.post(
            '/Login/LogsJs',
            {
                Excep: e
                , NameFunction: "coreweb.js | btn-sgt-"
            },
            function (response) {
                console.log("Error js en TXt log")
            }
        ).fail(function (result) {
        });
        console.error();
    }
});





                //console.log("I---------------------------------");
                //console.log('FPEngineVersion: ' + zkonline.FPEngineVersion);
                //console.log('SensorSN: ' + zkonline.SensorSN);
                //console.log('VerifyCount: ' + zkonline.VerifyCount);
                //console.log('RegisterTemplate: ' + zkonline.RegisterTemplate);
                //console.log('VerifyTemplate: ' + zkonline.VerifyTemplate);
                //console.log('Threshold: ' + zkonline.Threshold);
                //console.log('OneToOneThreshold: ' + zkonline.OneToOneThreshold);
                //console.log('CheckFinger: ' + zkonline.CheckFinger);
                //console.log('EnrollCount: ' + zkonline.EnrollCount);
                //console.log('DefaultRegFinger: ' + zkonline.DefaultRegFinger);
                //console.log('DefaultWindowClose: ' + zkonline.DefaultWindowClose);
                //console.log('SetVerHint: ' + zkonline.SetVerHint);
                //console.log('IsSupportDuress: ' + zkonline.IsSupportDuress);
                //console.log('LastQuality: ' + zkonline.LastQuality);
                //console.log('LowestQuality: ' + zkonline.LowestQuality);
                //console.log('FakeFunOn: ' + zkonline.FakeFunOn);
                //console.log("O=================================");


/*
fn_validarHuellaLocal = function (huella, sn) {
	alert('fn_validarHuellaLocal');
    console.log(huella);
    console.log(sn);

    huella = huella.replace(/\//g, "~");
    huella = huella.replace(/=/g, "_");

    var valuesAddress = $('body').data('base') + "Valida/" + huella+"/"+sn;
    ajax = $.ajax({
        type: "GET",
        dataType: "jsonp",
        url: valuesAddress,
        async: false,
        jsonpCallback: 'fn_validarHuellaLocal_callback',
        error: function (xhr, status, error) {
            if (xhr.status) {
                window.localStorage.setItem('servLocalOK', false)
                return;
            }
        }
    });
}
fn_validarHuellaLocal_callback = function (data) {
    console.log(data);
    alert(data.Message);
}
*/




