

var Oper;
$('body').on("keydown", function (e) {
    console.log('entró keydown');

        if (e.ctrlKey && e.which === 88) { ///CTRL + X  >> 83:s

            $('#exampleModalCenter').modal('toggle');
            //$('#myModal').modal('show');
            //$('#myModal').modal('hide');
            $('#mensaje-valor-respuesta').html('');
            $('#mensaje-alert').html('');
            $('#txt-serial').show();    //MTkyWDE2OFgwMDFYMTAzWEY4REEwQzA1NzM1RDQ2NA==
            $('#txt-serial').val('');
            $('#btn-registrar').show();
            $('#btn-cancelar').show();
            $('#btn-ok').hide();
            console.log('mensaje en duro');
            ActivarWCF(2);
            Oper = 2;
            e.preventDefault();
            $('#txt-serial').focus();
        }

 });


//Boton Aceptar/Continuar ()
$('#btn-registrar').click(function () {


        if ($('#txt-serial').val() == "") {

            $('#mensaje-alert').html('');
            $('#mensaje-alert').append('<div class="alert alert-info " role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"> <span aria-hidden="true">×</span> </button>  <label style="text-align:center;">Ingrese Serial</label>  </div>');
            $('#txt-serial').focus();
            return;
        }

        else {
            var inputValue = $('#txt-serial').val();
            console.log(inputValue);
            console.log(Oper);
            try{
                $.post(
                '/Login/RegistrarServerWCF',
                { llave: inputValue, Oper: Oper },
                function (response) {

                //(reponse) => {
                    console.log('respuesta del response');
                    if (response.type == "success") {

                        if (Oper == 2) {
                            $('#mensaje-valor-respuesta').html(response.message);
                            $('#mensaje-alert').html('');
                            $('#mensaje-alert').append('<div class="alert alert-info " role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"> <span aria-hidden="true">×</span> </button>  <label >Licencia Registrada</label>  </div>');
                            $('#txt-serial').val('');
                            $('#txt-serial').hide();
                            $('#btn-registrar').hide();
                            $('#btn-cancelar').hide();
                            $('#btn-ok').show();
                            $('#mensaje-alerta').html('');

                        } else {
                            $('#mensaje-valor-respuesta').html(response.message);
                            $('#mensaje-alert').html('');
                            $('#txt-serial').val('');
                            $('#mensaje-alert').append('<div class="alert alert-info " role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"> <span aria-hidden="true">×</span> </button>  <label >Token Generado</label>  </div>');
                        }

                    } 
                    else {
                        if (Oper == 2) {
                            $('#txt-serial').val('');
                            $('#txt-serial').focus();
                            $('#mensaje-alert').html('');
                            $('#mensaje-alert').append('<div class="alert alert-info " role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"> <span aria-hidden="true">×</span> </button>  <label >NO SE PUDO REGISTRAR LICENCIA</label>  </div>');
                        } else {
                            $('#txt-serial').val('');
                            $('#txt-serial').focus();
                            $('#mensaje-alert').html('');
                            $('#mensaje-alert').append('<div class="alert alert-info " role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"> <span aria-hidden="true">×</span> </button>  <label >NO SE PUDO GENERAR EL TOKEN</label>  </div>');
                        }

                    }
                }
            ).fail(function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            });
            }
            catch(e){
                console.error("Error en consola LoginLicencia.js");
            }


        }

    });



    function ActivarWCF(Oper) {

        if (Oper == 2) {

            $('#exampleModalLongTitle').html('Registrar Licencia'); 

        } else if (Oper == 3) {
            $('#exampleModalLongTitle').html('Generar Token de Prueba'); 
     
        } else {
            $('#exampleModalLongTitle').html('Generar Token');  
        }

        

    }



