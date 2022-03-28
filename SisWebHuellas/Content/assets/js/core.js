///**
// *
// */
//let hexToRgba = function (hex, opacity) {
//    let result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
//    let rgb = result ? {
//        r: parseInt(result[1], 16),
//        g: parseInt(result[2], 16),
//        b: parseInt(result[3], 16)
//    } : null;

//    return 'rgba(' + rgb.r + ', ' + rgb.g + ', ' + rgb.b + ', ' + opacity + ')';
//};

///**
// *
// */
//$(document).ready(function () {
//    /** Constant div card */
//    const DIV_CARD = 'div.card';

//    /** Initialize tooltips */
//    $('[data-toggle="tooltip"]').tooltip();

//    /** Initialize popovers */
//    $('[data-toggle="popover"]').popover({
//        html: true
//    });

//    /** Function for remove card */
//    $('[data-toggle="card-remove"]').on('click', function (e) {
//        let $card = $(this).closest(DIV_CARD);

//        $card.remove();

//        e.preventDefault();
//        return false;
//    });

//    /** Function for collapse card */
//    $('[data-toggle="card-collapse"]').on('click', function (e) {
//        let $card = $(this).closest(DIV_CARD);

//        $card.toggleClass('card-collapsed');

//        e.preventDefault();
//        return false;
//    });

//    /** Function for fullscreen card */
//    $('[data-toggle="card-fullscreen"]').on('click', function (e) {
//        let $card = $(this).closest(DIV_CARD);

//        $card.toggleClass('card-fullscreen').removeClass('card-collapsed');


//        e.preventDefault();
//        return false;
//    });

//    /**  */
//    if ($('[data-sparkline]').length) {
//        let generateSparkline = function ($elem, data, params) {
//            $elem.sparkline(data, {
//                type: $elem.attr('data-sparkline-type'),
//                height: '100%',
//                barColor: params.color,
//                lineColor: params.color,
//                fillColor: 'transparent',
//                spotColor: params.color,
//                spotRadius: 0,
//                lineWidth: 2,
//                highlightColor: hexToRgba(params.color, .6),
//                highlightLineColor: '#666',
//                defaultPixelsPerValue: 5
//            });
//        };


//        $('[data-sparkline]').each(function () {
//            let $chart = $(this);

//            generateSparkline($chart, JSON.parse($chart.attr('data-sparkline')), {
//                color: $chart.attr('data-sparkline-color')
//            });
//        });

//    }

//    /**  */
//    if ($('.chart-circle').length) {

//        $('.chart-circle').each(function () {
//            let $this = $(this);

//            $this.circleProgress({
//                fill: {
//                    color: tabler.colors[$this.attr('data-color')] || tabler.colors.blue
//                },
//                size: $this.height(),
//                startAngle: -Math.PI / 4 * 2,
//                emptyFill: '#F4F4F4',
//                lineCap: 'round'
//            });
//        });

//    }


//    $('#date_1').datepicker({
//        todayBtn: "linked",
//        keyboardNavigation: false,
//        todayHighlight: true,
//        forceParse: false,
//        calendarWeeks: true,
//        autoclose: true,
//        format: "dd/mm/yyyy",
//        language: 'es'
//    });

//    $('#date_2').datepicker({
//        todayBtn: "linked",
//        keyboardNavigation: false,
//        todayHighlight: true,
//        forceParse: false,
//        calendarWeeks: true,
//        autoclose: true,
//        format: "dd/mm/yyyy",
//        language: 'es'
//    });

//    //set value of input date
//    var date = new Date();
//    var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
//    var yesterday = new Date(date.getFullYear(), date.getMonth(), date.getDate() - 1);
//    $('#date_2').datepicker('setDate', today);
//    $('#date_1').datepicker('setDate', yesterday);




//    $('.datatable-pedido').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        fixedHeader: {    //mantener fijo el encabezado de la página
//            header: true,
//            headerOffset: 58,  //espaciado entre la parte superior de la pantalla
//            footer: false,
//        },
//        order: [[0, 'asc']],
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });

//    /**datatable-detalle */
//    $('.datatable-detalle-pedido').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        responsive: true,
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });

//    /**var tableGuia =  */
//    $('.datatable-Guia').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        order: [[0, 'asc']],
//        fixedHeader: {    //mantener fijo el encabezado de la página
//            header: true,
//            headerOffset: 58,  //espaciado entre la parte superior de la pantalla
//            footer: false,
//        },
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });

//    /*    tableGuia.column(3).visible(false);
//       tableGuia.column(4).visible(false); */


//    $('.datatable-detalle-Guia').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        responsive: true,
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });

//    $('.datatable-producto').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        fixedHeader: {
//            header: true,
//            headerOffset: 58,
//            footer: false,
//        },
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });


//    $('.datatable-movi-produ').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        /* responsive: true, */
//        order: [[0, 'desc']],
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });


//    $('#mas-menos-filtros').click(function () {
//        var data = $(this).data('active');
//        console.log(data);
//        if (data === 'mas') {
//            $('#div-filtros-pedido').show('fast');
//            $(this).data('active', 'menos');
//            $(this).html('<i class="fe fe-minus mr-2"></i>Menos filtros');
//        } else {
//            $('#div-filtros-pedido').hide('fast');
//            $(this).data('active', 'mas');
//            $(this).html('<i class="fe fe-plus mr-2"></i>Más filtros');
//        }
//    });



//});
////end document ready



////filtrar pedidos por los filtros
//$('#btn-filtrar-pedido').on('click', function () {

//    var date_Start = $('#date_1').val();
//    var date_End = $('#date_2').val();
//    var nOrder = $('#noOrder').val();
//    var state = '';
//    var salesman = '';
//    var annulled = false;

//    if (date_Start !== '' && date_End !== '') {
//        var data = $('#mas-menos-filtros').data('active');
//        if (data === 'menos') {
//            state = $('#estado-pedido option:selected').val();
//            salesman = $('#cboVendedor option:selected').val();
//            annulled = $('#chkAnnulled').is(':checked');
//        }

//        /*     console.log(date_Start);
//            console.log(date_End);
//            console.log(nOrder);
//            console.log(state);
//            console.log(salesman);
//            console.log(annulled); */


//        /*$.post(
//          '/Pedido/FiltrarPedido',
//          { fechaIni: date_Start, fechaFin: date_End, nroPedido: nOrder, estado: state, vendedor: salesman, anulados: annulled },
//          (response) => {
//            console.timeEnd('timeFilterPed');
//            if (response.trim() !== '') {
//              $('#pedido-cabecera-table').empty();
//              //console.log(response);
//              $('#pedido-cabecera-table').html(response);
//              reloadTableOrder();
//            }
    
//          }
//        ).fail(function (result) {
//          alert('ERROR ' + result.status + ' ' + result.statusText);
//        });*/

//        $.ajax({
//            url: '/Pedido/FiltrarPedido',
//            type: 'POST',
//            data: { fechaIni: date_Start, fechaFin: date_End, nroPedido: nOrder, estado: state, vendedor: salesman, anulados: annulled },
//            beforeSend: function () {
//                $.blockUI({
//                    css: {
//                        border: 'none',
//                        padding: '15px',
//                        backgroundColor: '#000',
//                        '-webkit-border-radius': '10px',
//                        '-moz-border-radius': '10px',
//                        opacity: .5,
//                        color: '#fff'
//                    },
//                    message: 'Procesando consulta...'
//                });
//            },
//            success: function (response) {
//                if (response.trim() !== '') {
//                    $('#pedido-cabecera-table').empty();
//                    //console.log(response);
//                    $('#pedido-cabecera-table').html(response);
//                    reloadTableOrder();
//                }
//            },
//            complete: function () {
//                $.unblockUI();
//            },
//            error: function (xhr, status, error) {
//                alert('ERROR ' + xhr.status + ' ' + error);
//            }
//        });

//    }

//});




////consultar detalle del pedido
//function detallePedido(nroDocu) {
//    $.post(
//        '/Pedido/DetallePedido',
//        { ndocu: nroDocu },
//        function (response) {
//            if (response.trim() !== '') {
//                $('#detalle-pedido-table').empty();
//                //console.log(response);
//                $('#detalle-pedido-table').html(response);
//                reloadTableDetailOrder();
//            }

//        }
//    ).fail(function (result) {
//        alert('ERROR ' + result.status + ' ' + result.statusText);
//    });

//}

///**CAPTURAR FILA SELECCIONADA DE LA TABLA PEDIDO */
//function capturarFilaPedido(row) {
//    var fila = row.cells;
//    var idFila = row.id;

//    $('.datatable-pedido tbody').find("*").removeClass('table-success');

//    $('#' + idFila).addClass('table-success');

//    var nroPedido = fila[1].innerText.trim();
//    /* console.log(nroPedido); */
//    $('#nroPedido-print').val(nroPedido);

//    detallePedido(nroPedido);
//}


////imprimir PEDIDO




////consultar guias por filtros
//$('#btn-search-guia').on('click', function () {
//    var date_Start = $('#date_1').val();
//    var date_End = $('#date_2').val();
//    var nroGuia = $('#noOrder').val();
//    var state = '';
//    var salesman = '';
//    var annulled = false;


//    if (date_Start !== '' && date_End !== '') {
//        var data = $('#mas-menos-filtros').data('active');
//        if (data === 'menos') {
//            state = $('#estado-guia option:selected').val();
//            salesman = $('#cboVendedor option:selected').val();
//            annulled = $('#chkAnnulled').is(':checked');
//        }

//        /*$.post(
//          '/Guia/FiltrarGuia',
//          { fechaIni: date_Start, fechaFin: date_End, nroGuia: nroGuia, estado: state, vendedor: salesman, anulados: annulled },
//          (response) => {
//            if (response.trim() !== '') {
//              $('#guia-cabecera-table').empty();
//              //console.log(response);
//              $('#guia-cabecera-table').html(response);
//              reloadTableGuia();
//            }
    
//          }
//        ).fail(function (result) {
//          alert('ERROR ' + result.status + ' ' + result.statusText);
//        });*/

//        $.ajax({
//            url: '/Guia/FiltrarGuia',
//            type: 'POST',
//            data: { fechaIni: date_Start, fechaFin: date_End, nroGuia: nroGuia, estado: state, vendedor: salesman, anulados: annulled },
//            beforeSend: function () {
//                $.blockUI({
//                    css: {
//                        border: 'none',
//                        padding: '15px',
//                        backgroundColor: '#000',
//                        '-webkit-border-radius': '10px',
//                        '-moz-border-radius': '10px',
//                        opacity: .5,
//                        color: '#fff'
//                    },
//                    message: 'Procesando consulta...'
//                });
//            },
//            success: function (response) {
//                if (response.trim() !== '') {
//                    $('#guia-cabecera-table').empty();
//                    //console.log(response);
//                    $('#guia-cabecera-table').html(response);
//                    reloadTableGuia();
//                }
//            },
//            complete: function () {
//                $.unblockUI();
//            },
//            error: function (xhr, status, error) {
//                alert('ERROR ' + xhr.status + ' ' + error);
//            }
//        });


//    }

//});



//// consultar detalle guía
//function detalleGuia(nroGuia) {

//    $.post(
//        '/Guia/DetalleGuia',
//        { nroGuia: nroGuia },
//        function (response) {
//            if (response.trim() !== '') {
//                $('#detalle-guia-table').empty();
//                //console.log(response);
//                $('#detalle-guia-table').html(response);
//                reloadTableDetailGuia();
//            }

//        }
//    ).fail(function (result) {
//        alert('ERROR ' + result.status + ' ' + result.statusText);
//    });
//}

//function reloadTableDetailOrder() {

//    $('.datatable-detalle-pedido').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        responsive: true,
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });


//}


//function reloadTableOrder() {

//    $('.datatable-pedido').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        fixedHeader: {
//            header: true,
//            headerOffset: 58,
//            footer: false,
//        },
//        order: [[0, 'asc']],
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });



//}

//function reloadTableGuia() {

//    $('.datatable-Guia').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        order: [[0, 'asc']],
//        fixedHeader: {
//            header: true,
//            headerOffset: 58,
//            footer: false,
//        },
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });

//    /*    otableGuia.column(3).visible(false);
//       otableGuia.column(4).visible(false); */


//}

//function reloadTableDetailGuia() {

//    $('.datatable-detalle-Guia').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        responsive: true,
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });


//}

///**Enter key pressed */
//function autoComplete(e) {
//    //See notes about 'which' and 'key'
//    if (e.keyCode == 13) {
//        console.log('Key enter pressed');
//        var ndocu = $('#noOrder').val();
//        var longitud = ndocu.length;
//        if (longitud <= 12) {
//            var diferencia = 8 - longitud;

//            if (diferencia >= 0) {
//                var complete = '001-';
//                for (let index = 0; index < diferencia; index++) {
//                    complete += '0';
//                }
//                $('#noOrder').val(complete + ndocu);
//            }

//        }


//        return false;
//    }
//}



///**Funciones de CONSULTA DE STOCK */

//$('#cboFamilia').on('change', function () {
//    //console.log(this.value);
//    var codFam = $(this).val();
//    cargarCBOsubLinea(codFam);
//});

//$('#cboSubLinea').on('change', function () {
//    var codSubFam = $(this).val();
//    cargarCBOGrupo(codSubFam);
//});

////cargar combos dinámico (SUBLINEA)
//function cargarCBOsubLinea(codFam) {

//    $.post(
//        '/Stock/cargarSubLinea',
//        { codFamilia: codFam },
//        function (response) {
//            /*   console.log(response); */
//            if (response.tipo === 'success') {

//                if (response.datos.length > 0) {

//                    $('#cboSubLinea').empty();
//                    for (let index = 0; index < response.datos.length; index++) {
//                        if (index === 0) {
//                            $('#cboSubLinea').append('<option value="">Seleccione...</option>');
//                        }

//                        $('#cboSubLinea').append('<option value="' + response.datos[index].codsub + '">' + response.datos[index].nomsub + '</option>');
//                    }

//                } else {
//                    $('#cboSubLinea').empty();
//                    $('#cboSubLinea').append('<option value="">Seleccione...</option>');
//                }

//            } else {
//                $('#cboSubLinea').empty();
//                $('#cboSubLinea').append('<option value="">Seleccione...</option>');

//                if (response.tipo === 'error')
//                    $.ambiance({
//                        message: 'Su sesión a expirado, por favor inicie nuevamente',
//                        title: "Sesión Expirada",
//                        type: response.tipo,
//                        timeout: 4,
//                        width: 500
//                    });
//            }


//        }
//    ).fail(function (result) {
//        alert('ERROR ' + result.status + ' ' + result.statusText);
//    });
//}

////cargar combo GRUPO
//function cargarCBOGrupo(codSubFam) {

//    $.post(
//        '/Stock/cargarGrupo',
//        { codSubFam: codSubFam },
//        function (response) {
//            /*  console.log(response); */

//            if (response.tipo === 'success') {
//                if (response.datos.length > 0) {
//                    $('#cboGrupo').empty();
//                    for (let index = 0; index < response.datos.length; index++) {
//                        if (index === 0) {
//                            $('#cboGrupo').append('<option value="">Seleccione...</option>');
//                        }

//                        $('#cboGrupo').append('<option value="' + response.datos[index].nomgru + '">' + response.datos[index].nomgru + '</option>');
//                    }

//                } else {
//                    $('#cboGrupo').empty();
//                    $('#cboGrupo').append('<option value="">Seleccione...</option>');
//                }
//            }
//            else {
//                $('#cboGrupo').empty();
//                $('#cboGrupo').append('<option value="">Seleccione...</option>');

//                if (response.tipo === 'error')
//                    $.ambiance({
//                        message: 'Su sesión a expirado, por favor inicie nuevamente',
//                        title: "Sesión Expirada",
//                        type: response.tipo,
//                        timeout: 4,
//                        width: 500
//                    });
//            }


//        }
//    ).fail(function (result) {
//        alert('ERROR ' + result.status + ' ' + result.statusText);
//    });
//}




///**FILTRAR PRODUCTOS POR FILTROS */

//function reloadProductTable() {

//    $('.datatable-producto').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        fixedHeader: {
//            header: true,
//            headerOffset: 58,
//            footer: false,
//        },
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });


//}

//function reloadMoviProdTable() {

//    $('.datatable-movi-produ').DataTable({
//        lengthMenu: [5, 10, 25, 50],
//        /* responsive: true, */
//        order: [[0, 'desc']],
//        language: {
//            lengthMenu: 'Mostrar _MENU_ Items',
//            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
//            infoEmpty: 'No hay Items para mostrar',
//            search: 'Buscar: ',
//            sSearchPlaceholder: 'Criterio de búsqueda',
//            zeroRecords: 'No se encontraron registros coincidentes',
//            infoFiltered: '(Filtrado de _MAX_ totales Items)',
//            paginate: {
//                previous: 'Anterior',
//                next: 'Siguiente'
//            }
//        }
//    });

//}

//$('#btn-search-produ-stock').on('click', function () {
//    var almacen = $('#cboAlmacen option:selected').val();
//    //var linea = $('#cboFamilia option:selected').val();
//    var subLinea = $('#cboSubLinea option:selected').val();
//    subLinea = subLinea.replace('-', '');
//    var grupo = $('#cboGrupo option:selected').val();
//    var estado = '';
//    var descProdu = '';
//    var codProdu = '';
//    if (almacen !== '' && subLinea !== '') {
//        var data = $('#mas-menos-filtros').data('active');
//        if (data === 'menos') {
//            estado = $('#estado-produ option:selected').val();
//            descProdu = $('#desc-produ').val();
//            codProdu = $('#cod-produ').val();
//        }

//        /*$.post(
//          '/Stock/filtrarProducto',
//          { codSubFam: subLinea, desGrupo: grupo, codAlmac: almacen, estado: estado, codProdu: codProdu, descProdu: descProdu },
//          (response) => {
//            if (response.trim() !== '') {
//              $('#producto-cabecera-table').empty();
//              $('#producto-cabecera-table').html(response);
//              reloadProductTable();
//            }
    
//          }
//        ).fail(function (result) {
//          alert('ERROR ' + result.status + ' ' + result.statusText);
//        });*/

//        $.ajax({
//            url: '/Stock/filtrarProducto',
//            type: 'POST',
//            data: { codSubFam: subLinea, desGrupo: grupo, codAlmac: almacen, estado: estado, codProdu: codProdu, descProdu: descProdu },
//            beforeSend: function () {
//                $.blockUI({
//                    css: {
//                        border: 'none',
//                        padding: '15px',
//                        backgroundColor: '#000',
//                        '-webkit-border-radius': '10px',
//                        '-moz-border-radius': '10px',
//                        opacity: .5,
//                        color: '#fff'
//                    },
//                    message: 'Procesando consulta...'
//                });
//            },
//            success: function (response) {
//                if (response.trim() !== '') {
//                    $('#producto-cabecera-table').empty();
//                    $('#producto-cabecera-table').html(response);
//                    reloadProductTable();
//                }
//            },
//            complete: function () {
//                $.unblockUI();
//            },
//            error: function (xhr, status, error) {
//                alert('ERROR ' + xhr.status + ' ' + error);
//            }
//        });

//    }

//});


////movimiento de los productos


//function capturaFilaStock(row) {

//    var fila = row.cells;
//    var idFila = row.id;

//    $('.datatable-producto tbody').find("*").removeClass('table-success');

//    $('#' + idFila).addClass('table-success');

//    var codProdu = fila[8].innerText.trim();

//    var onlySales = '';
//    $.post(
//        '/Stock/movimientoProducto',
//        { codProdu: codProdu, chk: onlySales },
//        function (response) {
//            if (response.trim() !== '') {
//                $('#movimiento-produ-table').empty();
//                $('#movimiento-produ-table').html(response);
//                $('#chk-solo-ventas').data('cod-produ', codProdu);
//                reloadMoviProdTable();
//            }

//        }
//    ).fail(function (result) {
//        alert('ERROR ' + result.status + ' ' + result.statusText);
//    });
//}

//$('#chk-solo-ventas').on('change', function () {
//    var chkVentas = $(this).is(':checked');
//    var codProdu = $('#chk-solo-ventas').data('cod-produ');
//    var onlySales = '02';

//    if (chkVentas) {
//        onlySales = '01';
//        console.log('checked');
//    }
//    else {
//        console.log('unchecked');
//    }

//    $.post(
//        '/Stock/movimientoProducto',
//        { codProdu: codProdu, chk: onlySales },
//        function (response) {
//            if (response.trim() !== '') {
//                $('#movimiento-produ-table').empty();
//                $('#movimiento-produ-table').html(response);
//                reloadMoviProdTable();
//            }

//        }
//    ).fail(function (result) {
//        alert('ERROR ' + result.status + ' ' + result.statusText);
//    });

//});




///*Inicio Dialog functions */

//$('#btn-preguntar-impresion').on('click', function () {

//    $('#rdb-print-ticket').prop('checked', false);
//    $('#rdb-print-etiqueta').prop('checked', false);
//    $('#div-print-comentario').hide('fast');
//    $('#div-print-cantidad').hide('fast');
//    $('#coment-print').val('');
//    $('#canti-print').val('1')

//    $('#idModalPregunta').modal('show');
//});

//function capturarFilaGuia(row) {
//    var x = row.cells;
//    var id = row.id;


//    $('.datatable-Guia tbody').find("*").removeClass('table-success');

//    $('#' + id).addClass('table-success');

//    var nroGuia = x[1].innerText.trim();
//    var cliente = x[3].innerText.trim();
//    var ruccli = x[4].innerText.trim();
//    var direccion = x[5].innerText.trim();
//    var empresa = x[9].innerText.trim();

//    /*  console.log('Nro Guia: ' + nroGuia.trim());
//     console.log('Nombre Cliente: ' + cliente.trim());
//     console.log('Ruc cliente: ' + ruccli.trim());
//     console.log('Dirección: ' + direccion.trim());
//     console.log('Empresa: ' + empresa.trim());
//     console.log('');
// */
//    $('#nroGuia-print').val(nroGuia);
//    $('#nom-cli-print').val(cliente);
//    $('#ruccli-print').val(ruccli);
//    $('#direccion-print').val(direccion);
//    $('#empresa-print').val(empresa);

//    detalleGuia(nroGuia);

//}


///* Fin dialog functions */


///* Imprimir Etiqueta */
//$('#rdb-print-ticket').on('change', function () {

//    $('#div-print-cantidad').hide('fast');
//    var rdbPrintTicket = $('#rdb-print-ticket').is(':checked');

//    if (rdbPrintTicket) {
//        $('#div-print-comentario').show('fast');
//    }

//});

//$('#rdb-print-etiqueta').on('change', function () {

//    $('#div-print-comentario').hide('fast');
//    var rdbPrintEtiqueta = $('#rdb-print-etiqueta').is(':checked');

//    if (rdbPrintEtiqueta) {
//        $('#div-print-cantidad').show('fast');
//    }

//});


//$('#btn-print-guia').on('click', function () {

//    var rdbPrintEtiqueta = $('#rdb-print-etiqueta').is(':checked');
//    var rdbPrintTicket = $('#rdb-print-ticket').is(':checked');
//    //nro guía
//    var NoGuia = $('#nroGuia-print').val().trim();

//    if (rdbPrintEtiqueta) {
//        var Cantidad = $('#canti-print').val().trim();

//        var NomCliente = $('#nom-cli-print').val().trim();
//        var RucCliente = $('#ruccli-print').val().trim();
//        var DirecCliente = $('#direccion-print').val().trim();
//        var Empresa = $('#empresa-print').val().trim();

//        if (Cantidad === '' | Cantidad === '0') {
//            $.ambiance({
//                message: 'Ingrese cantidad de Bultos mayor a 0  para la Impresión',
//                title: "Impresión",
//                type: "error",
//                timeout: 5,
//                width: 500
//            });

//            return false;
//        }
//        else {
//            if (NoGuia === '' && NomCliente === '' && RucCliente === '' && DirecCliente === '') {

//                $.ambiance({
//                    message: 'Vuelva a seleccionar el registro que desea imprimir',
//                    title: "Impresión",
//                    type: "error",
//                    timeout: 5,
//                    width: 500
//                });

//                return false;
//            }
//        }

//        var Guia = {
//            nroGuia: NoGuia,
//            rzCliente: NomCliente,
//            ruccli: RucCliente,
//            dirent: DirecCliente,
//            transp: Empresa
//        };

//        /*  console.log(Guia); */

//        $.post(
//            '/Guia/imprimirEtiqueta',
//            { guia: Guia, cant: Cantidad },
//            function (response) {
//                if (response !== null) {

//                    $.ambiance({
//                        message: response.message,
//                        title: "Impresión",
//                        type: response.type,
//                        timeout: 5,
//                        width: 500
//                    });
//                }
//                $('#idModalPregunta').modal('hide');
//            }
//        ).fail(function (result) {
//            alert('ERROR ' + result.status + ' ' + result.statusText);
//        });
//    } else {

//        if (rdbPrintTicket) {
//            var comentario = $('#coment-print').val().trim();
//            if (comentario !== '') {

//                $.post(
//                    '/Guia/imprimeGuia',
//                    { ndocu: NoGuia, coment: comentario },
//                    function (response) {
//                        if (response !== null) {
//                            console.log(response);

//                            $.ambiance({
//                                message: response.message,
//                                title: "Impresión",
//                                type: response.type,
//                                timeout: 4,
//                                width: 500
//                            });

//                        }
//                        $('#idModalPregunta').modal('hide');
//                    }
//                ).fail(function (xhr, textStatus, errorThrown) {
//                    var errorMessage = xhr.status + ': ' + xhr.statusText
//                    alert(errorMessage);
//                });

//            } else {
//                $.ambiance({
//                    message: 'Ingrese comentario para la impresión',
//                    title: "Impresión",
//                    type: "warning",
//                    timeout: 5,
//                    width: 500
//                });
//            }
//        } else {
//            $.ambiance({
//                message: 'Seleccione tipo de impresión',
//                title: "Impresión",
//                type: "warning",
//                timeout: 5,
//                width: 500
//            });
//        }

//    }

//});

///* Imprimir Etiqueta */


////=========================================================================================================================================
//$('#btn-print-pedido').on('click', function () {
//    var codigo = $('#txtCodigo').val();
//    if (codigo == "") {
//        $.ambiance({
//            message: 'Ingrese un código',
//            title: "Registro de Huellas",
//            type: "warning",
//            timeout: 2,
//            width: 500
//        });
//        return;
//    }
//    var nombre = $('#txtNombres').val();
//    if (nombre == "") {
//        $.ambiance({
//            message: 'Ingrese un nombre',
//            title: "Registro de Huellas",
//            type: "warning",
//            timeout: 2,
//            width: 500
//        });
//        return;
//    }
//    var numTar = $('#txtNumTar').val();
//    var codPer = $('#txtCodPerso').val();

//    var dedos = $('#hddDedos').val();
//    var huellas = $('#hddHuellas').val();

//    $.post(
//        '/Pedido/imprimePedido',
//        { ndocu: nroPed, coment: comentario },
//        function (response) {
//            if (response !== null) {
//                console.log(response);

//                $.ambiance({
//                    message: response.message,
//                    title: "Impresión",
//                    type: response.type,
//                    timeout: 4,
//                    width: 500
//                });

//            }
//            $('#modal-pedido-print').modal('hide');
//        }
//    ).fail(function (xhr, textStatus, errorThrown) {
//        var errorMessage = xhr.status + ': ' + xhr.statusText
//        alert(errorMessage);
//    });
//});

