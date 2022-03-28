/**
 *
 */
let hexToRgba = function (hex, opacity) {
  let result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
  let rgb = result ? {
    r: parseInt(result[1], 16),
    g: parseInt(result[2], 16),
    b: parseInt(result[3], 16)
  } : null;

  return 'rgba(' + rgb.r + ', ' + rgb.g + ', ' + rgb.b + ', ' + opacity + ')';
};

/**
 *
 */
$(document).ready(function () {
  /** Constant div card */
  const DIV_CARD = 'div.card';

  /** Initialize tooltips */
  $('[data-toggle="tooltip"]').tooltip();

  /** Initialize popovers */
  $('[data-toggle="popover"]').popover({
    html: true
  });

  /** Function for remove card */
  $('[data-toggle="card-remove"]').on('click', function (e) {
    let $card = $(this).closest(DIV_CARD);

    $card.remove();

    e.preventDefault();
    return false;
  });

  /** Function for collapse card */
  $('[data-toggle="card-collapse"]').on('click', function (e) {
    let $card = $(this).closest(DIV_CARD);

    $card.toggleClass('card-collapsed');

    e.preventDefault();
    return false;
  });

  /** Function for fullscreen card */
  $('[data-toggle="card-fullscreen"]').on('click', function (e) {
    let $card = $(this).closest(DIV_CARD);

    $card.toggleClass('card-fullscreen').removeClass('card-collapsed');

    e.preventDefault();
    return false;
  });

  /**  */
  if ($('[data-sparkline]').length) {
    let generateSparkline = function ($elem, data, params) {
      $elem.sparkline(data, {
        type: $elem.attr('data-sparkline-type'),
        height: '100%',
        barColor: params.color,
        lineColor: params.color,
        fillColor: 'transparent',
        spotColor: params.color,
        spotRadius: 0,
        lineWidth: 2,
        highlightColor: hexToRgba(params.color, .6),
        highlightLineColor: '#666',
        defaultPixelsPerValue: 5
      });
    };

    require(['sparkline'], function () {
      $('[data-sparkline]').each(function () {
        let $chart = $(this);

        generateSparkline($chart, JSON.parse($chart.attr('data-sparkline')), {
          color: $chart.attr('data-sparkline-color')
        });
      });
    });
  }

  /**  */
  if ($('.chart-circle').length) {
    require(['circle-progress'], function () {
      $('.chart-circle').each(function () {
        let $this = $(this);

        $this.circleProgress({
          fill: {
            color: tabler.colors[$this.attr('data-color')] || tabler.colors.blue
          },
          size: $this.height(),
          startAngle: -Math.PI / 4 * 2,
          emptyFill: '#F4F4F4',
          lineCap: 'round'
        });
      });
    });
  }

  require(['datepicker'], function () {
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

    //set value of input date
    var date = new Date();
    var today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    $('#date_1, #date_2').datepicker('setDate', today);

  });

  require(['jquery.dataTables', 'jquery'], function (datatable, $) {
    $('.datatable-pedido').DataTable({
      lengthMenu: [5, 10, 25, 50],
      order: [[2, 'desc'], [1, 'desc']],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });

    /**datatable-detalle */
    $('.datatable-detalle-pedido').DataTable({
      lengthMenu: [5, 10, 25, 50],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });


    var tableGuia = $('.datatable-Guia').DataTable({
      lengthMenu: [5, 10, 25, 50],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });

    /*    tableGuia.column(3).visible(false);
       tableGuia.column(4).visible(false); */


    $('.datatable-detalle-Guia').DataTable({
      lengthMenu: [5, 10, 25, 50],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });

    $('.datatable-producto').DataTable({
      lengthMenu: [5, 10, 25, 50],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });


    $('.datatable-movi-produ').DataTable({
      lengthMenu: [5, 10, 25, 50],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });


  });


  $('#mas-menos-filtros').click(function () {
    var data = $(this).data('active');
    console.log(data);
    if (data === 'mas') {
      $('#div-filtros-pedido').show('fast');
      $(this).data('active', 'menos');
      $(this).html('<i class="fe fe-minus mr-2"></i>Menos filtros');
    } else {
      $('#div-filtros-pedido').hide('fast');
      $(this).data('active', 'mas');
      $(this).html('<i class="fe fe-plus mr-2"></i>Más filtros');
    }
  });



});
//end document ready



//filtrar pedidos por lod filtros
function FiltrarPedidos() {
  var date_Start = $('#date_1').val();
  var date_End = $('#date_2').val();
  var nOrder = $('#noOrder').val();
  var state = '';
  var salesman = '';
  var annulled = false;
  console.log('null');
  if (date_Start !== '' && date_End !== '') {
    var data = $('#mas-menos-filtros').data('active');
    if (data === 'menos') {
      state = $('#estado-pedido option:selected').val();
      salesman = $('#cboVendedor option:selected').val();
      annulled = $('#chkAnnulled').is(':checked');
    }

    /*    console.log(date_Start);
       console.log(date_End);
       console.log(nOrder);
       console.log(state);
       console.log(salesman);
       console.log(annulled); */

    $.post(
      '/Pedido/FiltrarPedido',
      { fechaIni: date_Start, fechaFin: date_End, nroPedido: nOrder, estado: state, vendedor: salesman, anulados: annulled },
      function (response)  {
        if (response.trim() !== '') {
          $('#pedido-cabecera-table').empty();
          //console.log(response);
          $('#pedido-cabecera-table').html(response);
          reloadTableOrder();
          //$(".dataTables_length label select").css('appearance', 'none');//Añadido HGM 09.10.2021
        }

      }
    ).fail(function (result) {
      alert('ERROR ' + result.status + ' ' + result.statusText);
    });
  }

}

//consultar detalle del pedido
function detallePedido(nroDocu) {

  $.post(
    '/Pedido/DetallePedido',
    { ndocu: nroDocu },
    function (response) {
      if (response.trim() !== '') {
        $('#detalle-pedido-table').empty();
        //console.log(response);
        $('#detalle-pedido-table').html(response);
        reloadTableDetailOrder();
      }

    }
  ).fail(function (result) {
    alert('ERROR ' + result.status + ' ' + result.statusText);
  });
}


//imprimir PEDIDO
function imprimirPedido(nroPed) {
  var comentario = 'Impresión de prueba';
  console.log(comentario);
  $.post(
    '/Pedido/imprimePedido',
    { ndocu: nroPed, coment: comentario },
    function (response) {
      if (response.trim() !== '') {
        alert(response);
      }

    }
  ).fail(function (xhr, textStatus, errorThrown) {
    var errorMessage = xhr.status + ': ' + xhr.statusText
    alert(errorMessage);
  });
}


//consultar guias por filtros
function filtrarGuiasCabecera() {
  var date_Start = $('#date_1').val();
  var date_End = $('#date_2').val();
  var nroGuia = $('#noOrder').val();
  var state = '';
  var salesman = '';
  var annulled = false;
  console.log('null');
  if (date_Start !== '' && date_End !== '') {
    var data = $('#mas-menos-filtros').data('active');
    if (data === 'menos') {
      state = $('#estado-guia option:selected').val();
      salesman = $('#cboVendedor option:selected').val();
      annulled = $('#chkAnnulled').is(':checked');
    }

    $.post(
      '/Guia/FiltrarGuia',
      { fechaIni: date_Start, fechaFin: date_End, nroGuia: nroGuia, estado: state, vendedor: salesman, anulados: annulled },
      function (response) {
        if (response.trim() !== '') {
          $('#guia-cabecera-table').empty();
          //console.log(response);
          $('#guia-cabecera-table').html(response);
          reloadTableGuia();
        }

      }
    ).fail(function (result) {
      alert('ERROR ' + result.status + ' ' + result.statusText);
    });
  }

}


// consultar detalle guía
function detalleGuia(nroGuia) {

  $.post(
    '/Guia/DetalleGuia',
    { nroGuia: nroGuia },
    function (response) {
      if (response.trim() !== '') {
        $('#detalle-guia-table').empty();
        //console.log(response);
        $('#detalle-guia-table').html(response);
        reloadTableDetailGuia();
      }

    }
  ).fail(function (result) {
    alert('ERROR ' + result.status + ' ' + result.statusText);
  });
}

function reloadTableDetailOrder() {
  require(['jquery.dataTables', 'jquery'], function (datatable, $) {
    $('.datatable-detalle-pedido').DataTable({
      lengthMenu: [5, 10, 25, 50],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });

  });
}


function reloadTableOrder() {
  require(['jquery.dataTables', 'jquery'], function (datatable, $) {
    $('.datatable-pedido').DataTable({
      lengthMenu: [5, 10, 25, 50],
      order: [[2, 'desc'], [1, 'desc']],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });
  });


}

function reloadTableGuia() {
  require(['jquery.dataTables', 'jquery'], function (datatable, $) {
    $('.datatable-Guia').DataTable({
      lengthMenu: [5, 10, 25, 50],
      order: [[2, 'desc'], [1, 'desc']],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });

    /*    otableGuia.column(3).visible(false);
       otableGuia.column(4).visible(false); */

  });



}

function reloadTableDetailGuia() {
  require(['jquery.dataTables', 'jquery'], function (datatable, $) {
    $('.datatable-detalle-Guia').DataTable({
      lengthMenu: [5, 10, 25, 50],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });
  });


}

/**Enter key pressed */
function autoComplete(e) {
  //See notes about 'which' and 'key'
  if (e.keyCode === 13) {
    console.log('Key enter pressed');
    var ndocu = $('#noOrder').val();
    var longitud = ndocu.length;
    if (longitud <= 12) {
      var diferencia = 8 - longitud;

      if (diferencia >= 0) {
        var complete = '001-';
        for (let index = 0; index < diferencia; index++) {
          complete += '0';
        }
        $('#noOrder').val(complete + ndocu);
      }

    }


    return false;
  }
}



/**Funciones de CONSULTA DE STOCK */

$('#cboFamilia').on('change', function () {
  //console.log(this.value);
  var codFam = $(this).val();
  cargarCBOsubLinea(codFam);
});

$('#cboSubLinea').on('change', function () {
  var codSubFam = $(this).val();
  cargarCBOGrupo(codSubFam);
});

//cargar combos dinámico (SUBLINEA)
function cargarCBOsubLinea(codFam) {

  $.post(
    '/Stock/cargarSubLinea',
    { codFamilia: codFam },
    function (response) {
      if (response.length > 0) {
        $('#cboSubLinea').empty();
        for (let index = 0; index < response.length; index++) {
          if (index === 0) {
            $('#cboSubLinea').append('<option value="">Seleccione...</option>');
          }

          $('#cboSubLinea').append('<option value="' + response[index].codsub + '">' + response[index].nomsub + '</option>');
        }

      } else {
        $('#cboSubLinea').empty();
        $('#cboSubLinea').append('<option value="">Seleccione...</option>');
      }
      //console.log(response);
    }
  ).fail(function (result) {
    alert('ERROR ' + result.status + ' ' + result.statusText);
  });
}

//cargar combo GRUPO
function cargarCBOGrupo(codSubFam) {

  $.post(
    '/Stock/cargarGrupo',
    { codSubFam: codSubFam },
    function (response) {
      if (response.length > 0) {
        $('#cboGrupo').empty();
        for (let index = 0; index < response.length; index++) {
          if (index === 0) {
            $('#cboGrupo').append('<option value="">Seleccione...</option>');
          }

          $('#cboGrupo').append('<option value="' + response[index].nomgru + '">' + response[index].nomgru + '</option>');
        }

      } else {
        $('#cboGrupo').empty();
        $('#cboGrupo').append('<option value="">Seleccione...</option>');
      }
      //console.log(response);
    }
  ).fail(function (result) {
    alert('ERROR ' + result.status + ' ' + result.statusText);
  });
}




/**FILTRAR PRODUCTOS POR FILTROS */

function reloadProductTable() {
  require(['jquery.dataTables', 'jquery'], function (datatable, $) {
    $('.datatable-producto').DataTable({
      lengthMenu: [5, 10, 25, 50],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });

  });
}

function reloadMoviProdTable() {
  require(['jquery.dataTables', 'jquery'], function (datatable, $) {
    $('.datatable-movi-produ').DataTable({
      lengthMenu: [5, 10, 25, 50],
      language: {
        lengthMenu: 'Mostrar _MENU_ Items',
        info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
        infoEmpty: 'No hay Items para mostrar',
        search: 'Buscar: ',
        sSearchPlaceholder: 'Criterio de busqueda',
        zeroRecords: 'No se encontraron registros coincidentes',
        infoFiltered: '(Filtrado de _MAX_ totales Items)',
        paginate: {
          previous: 'Anterior',
          next: 'Siguiente'
        }
      }
    });

  });
}


function filtrarProducto() {
  var almacen = $('#cboAlmacen option:selected').val();
  //var linea = $('#cboFamilia option:selected').val();
  var subLinea = $('#cboSubLinea option:selected').val();
  subLinea = subLinea.replace('-', '');
  var grupo = $('#cboGrupo option:selected').val();
  var estado = '';
  var descProdu = '';
  var codProdu = '';
  if (almacen !== '' && subLinea !== '') {
    var data = $('#mas-menos-filtros').data('active');
    if (data === 'menos') {
      estado = $('#estado-produ option:selected').val();
      descProdu = $('#desc-produ').val();
      codProdu = $('#cod-produ').val();
    }

    $.post(
      '/Stock/filtrarProducto',
      { codSubFam: subLinea, desGrupo: grupo, codAlmac: almacen, estado: estado, codProdu: codProdu, descProdu: descProdu },
      function (response) {
        if (response.trim() !== '') {
          $('#producto-cabecera-table').empty();
          $('#producto-cabecera-table').html(response);
          reloadProductTable();
        }

      }
    ).fail(function (result) {
      alert('ERROR ' + result.status + ' ' + result.statusText);
    });
  }

}

//movimiento de los productos
function movimientoProdu(codProdu) {
  var onlySales = '';
  $.post(
    '/Stock/movimientoProducto',
    { codProdu: codProdu, chk: onlySales },
    function (response) {
      if (response.trim() !== '') {
        $('#movimiento-produ-table').empty();
        $('#movimiento-produ-table').html(response);
        $('#chk-solo-ventas').data('cod-produ', codProdu);
        reloadMoviProdTable();
      }

    }
  ).fail(function (result) {
    alert('ERROR ' + result.status + ' ' + result.statusText);
  });
}

$('#chk-solo-ventas').on('change', function () {
  var chkVentas = $(this).is(':checked');
  var codProdu = $('#chk-solo-ventas').data('cod-produ');
  var onlySales = '02';

  if (chkVentas) {
    onlySales = '01';
    console.log('checked');
  }
  else {
    console.log('unchecked');
  }

  $.post(
    '/Stock/movimientoProducto',
    { codProdu: codProdu, chk: onlySales },
    function (response) {
      if (response.trim() !== '') {
        $('#movimiento-produ-table').empty();
        $('#movimiento-produ-table').html(response);
        reloadMoviProdTable();
      }

    }
  ).fail(function (result) {
    alert('ERROR ' + result.status + ' ' + result.statusText);
  });

});




/*Inicio Dialog functions */
function pregutarImpresion() {


  $('#idModalPregunta').modal('show');

  /*  require(['dialog', 'jquery'], function () {
     swal("Here's a message!");
     swal({
       title: "Impresión de Ticktet / Etiqueta",
       text: "¿Qué desea imprimir?",
       type: "warning",
       showCancelButton: true,
       confirmButtonText: "Imprimir Ticket",
       cancelButtonText: "Imprimir Etiqueta",
     }).then(function (isConfirm) {
       if (isConfirm) {
         swal("Deleted!", "Your imaginary file has been deleted.", "success");
       } else {
         swal("Cancelled", "Your imaginary file is safe :)", "error");
       }
     });
 
   }); */


}

function showRow(row) {
  var x = row.cells;
  var id = row.id;


  $('.datatable-Guia tbody').find("*").removeClass('table-danger');

  $('#' + id).addClass('table-danger');

  var nroGuia = x[0].innerText.trim();
  var cliente = x[2].innerText.trim();
  var ruccli = x[3].innerText.trim();
  var direccion = x[4].innerText.trim();
  var empresa = x[8].innerText.trim();

  console.log('Nro Guia: ' + nroGuia.trim());
  console.log('Nombre Cliente: ' + cliente.trim());
  console.log('Ruc cliente: ' + ruccli.trim());
  console.log('Dirección: ' + direccion.trim());
  console.log('Empresa: ' + empresa.trim());
  console.log('');

  $('#nroGuia-print').val(nroGuia);
  $('#nom-cli-print').val(cliente);
  $('#ruccli-print').val(ruccli);
  $('#direccion-print').val(direccion);
  $('#empresa-print').val(empresa);

  detalleGuia(nroGuia);

}


/* Fin dialog functions */


/* Imprimir Etiqueta */
function printEtiqueta() {

  var Cantidad = $('#canti-print').val().trim();
  var NoGuia = $('#nroGuia-print').val().trim();
  var NomCliente = $('#nom-cli-print').val().trim();
  var RucCliente = $('#ruccli-print').val().trim();
  var DirecCliente = $('#direccion-print').val().trim();
  var Empresa = $('#empresa-print').val().trim();

  if (Cantidad === '') {
    swal("Here's a message!");
    alert('Ingrese cantidad de Bultos para la Impresión');
    return false;
  }
  else {
    if (NoGuia === '' && NomCliente === '' && RucCliente === '' && DirecCliente === '') {
      require(['pnotify'], function () {
        new PNotify({
          title: 'Oh No!',
          text: 'Something terrible happened.',
          type: 'error',
          styling: 'bootstrap3'
        });
      });

      alert('Vuelva a seleccionar el registro que desea imprimir');
      return false;
    }
  }

  var Guia = {
    nroGuia: NoGuia,
    rzCliente: NomCliente,
    ruccli: RucCliente,
    dirent: DirecCliente,
    transp: Empresa
  };

  console.log(Guia);

  $.post(
    '/Guia/imprimirEtiqueta',
    { guia: Guia, cant: Cantidad },
    function (response) {
      if (response.trim() !== '') {
        alert(response);
      }

    }
  ).fail(function (result) {
    alert('ERROR ' + result.status + ' ' + result.statusText);
  });

}

/* Imprimir Etiqueta */