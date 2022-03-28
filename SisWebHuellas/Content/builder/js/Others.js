// al cargar la página
$(document).ready(function () {
    moment.locale('es');
    init_sidebar();
    panelToolBox();
    init_SmartWizard();
    init_daterangepicker();
    init_daterangepicker_single_call();
    init_charts();
    loadTableTickets();
    _init_tooltip();
    _init_checkBox();
});

/**
 * Resize function without multiple trigger
 * 
 * Usage:
 * $(window).smartresize(function(){  
 *     // code here
 * });
 */
(function ($, sr) {
    // debouncing function from John Hann
    // http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
    var debounce = function (func, threshold, execAsap) {
        var timeout;

        return function debounced() {
            var obj = this, args = arguments;
            function delayed() {
                if (!execAsap)
                    func.apply(obj, args);
                timeout = null;
            }

            if (timeout)
                clearTimeout(timeout);
            else if (execAsap)
                func.apply(obj, args);

            timeout = setTimeout(delayed, threshold || 100);
        };
    };

    // smartresize 
    jQuery.fn[sr] = function (fn) { return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr); };

})(jQuery, 'smartresize');
/**
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

var CURRENT_URL = window.location.href.split('#')[0].split('?')[0],
    $BODY = $('body'),
    $MENU_TOGGLE = $('#menu_toggle'),
    $SIDEBAR_MENU = $('#sidebar-menu'),
    $SIDEBAR_FOOTER = $('.sidebar-footer'),
    $LEFT_COL = $('.left_col'),
    $RIGHT_COL = $('.right_col'),
    $NAV_MENU = $('.nav_menu'),
    $FOOTER = $('footer');



// Sidebar
function init_sidebar() {
    // TODO: This is some kind of easy fix, maybe we can improve this
    var setContentHeight = function () {
        // reset height
        $RIGHT_COL.css('min-height', $(window).height());

        var bodyHeight = $BODY.outerHeight(),
            footerHeight = $BODY.hasClass('footer_fixed') ? -10 : $FOOTER.height(),
            leftColHeight = $LEFT_COL.eq(1).height() + $SIDEBAR_FOOTER.height(),
            contentHeight = bodyHeight < leftColHeight ? leftColHeight : bodyHeight;

        // normalize content
        contentHeight -= $NAV_MENU.height() + footerHeight;

        $RIGHT_COL.css('min-height', contentHeight);
    };

    $SIDEBAR_MENU.find('a').on('click', function (ev) {
        console.log('clicked - sidebar_menu');
        var $li = $(this).parent();

        if ($li.is('.active')) {
            $li.removeClass('active active-sm');
            $('ul:first', $li).slideUp(function () {
                setContentHeight();
            });
        } else {
            // prevent closing menu if we are on child menu
            if (!$li.parent().is('.child_menu')) {
                $SIDEBAR_MENU.find('li').removeClass('active active-sm');
                $SIDEBAR_MENU.find('li ul').slideUp();
            } else {
                if ($BODY.is(".nav-sm")) {
                    $li.parent().find("li").removeClass("active active-sm");
                    $li.parent().find("li ul").slideUp();
                }
            }
            $li.addClass('active');

            $('ul:first', $li).slideDown(function () {
                setContentHeight();
            });
        }
    });

    // toggle small or large menu 
    $MENU_TOGGLE.on('click', function () {
        console.log('clicked - menu toggle');

        if ($BODY.hasClass('nav-md')) {
            $SIDEBAR_MENU.find('li.active ul').hide();
            $SIDEBAR_MENU.find('li.active').addClass('active-sm').removeClass('active');
        } else {
            $SIDEBAR_MENU.find('li.active-sm ul').show();
            $SIDEBAR_MENU.find('li.active-sm').addClass('active').removeClass('active-sm');
        }

        $BODY.toggleClass('nav-md nav-sm');

        setContentHeight();

        $('.dataTable').each(function () { $(this).dataTable().fnDraw(); });
    });

    // check active menu
    $SIDEBAR_MENU.find('a[href="' + CURRENT_URL + '"]').parent('li').addClass('current-page');

    $SIDEBAR_MENU.find('a').filter(function () {
        return this.href === CURRENT_URL;
    }).parent('li').addClass('current-page').parents('ul').slideDown(function () {
        setContentHeight();
    }).parent().addClass('active');

    // recompute content when resizing
    $(window).smartresize(function () {
        setContentHeight();
    });

    setContentHeight();

    // fixed sidebar
    if ($.fn.mCustomScrollbar) {
        $('.menu_fixed').mCustomScrollbar({
            autoHideScrollbar: true,
            theme: 'minimal',
            mouseWheel: { preventDefault: true }
        });
    }
};
// /Sidebar

//Panel toolbox
function panelToolBox() {
    $('.collapse-link').on('click', function () {
        var $BOX_PANEL = $(this).closest('.x_panel'),
            $ICON = $(this).find('i'),
            $BOX_CONTENT = $BOX_PANEL.find('.x_content');

        // fix for some div with hardcoded fix class
        if ($BOX_PANEL.attr('style')) {
            $BOX_CONTENT.slideToggle(200, function () {
                $BOX_PANEL.removeAttr('style');
            });
        } else {
            $BOX_CONTENT.slideToggle(200);
            $BOX_PANEL.css('height', 'auto');
        }

        $ICON.toggleClass('fa-chevron-up fa-chevron-down');
    });

    $('.close-link').click(function () {
        var $BOX_PANEL = $(this).closest('.x_panel');

        $BOX_PANEL.remove();
    });
}

/**Diseño de los gráficos estadísticos */
function init_charts() {

    //console.log('run_charts  typeof [' + typeof (Chart) + ']');

    if (typeof (Chart) === 'undefined') { return; }
    //console.log('init_charts');


    Chart.defaults.global.legend = {
        enabled: true,
    };

}

/**Diseño de la linea de tiempo agrupado */
function init_SmartWizard() {

    if (typeof ($.fn.smartWizard) === 'undefined') { return; }
    //console.log('init_SmartWizard');

    $('#wizard').smartWizard({
        selected: -1,
        labelNext: 'Siguiente',
        labelPrevious: 'Anterior',
        labelFinish: 'Último',
        enableFinishButton: true,
        enableAllSteps: true
    });

    $('#wizard_verticle').smartWizard({
        transitionEffect: 'slide'
    });

    $('.buttonPrevious').addClass('btn btn-default');
    $('.buttonNext').addClass('btn btn-danger');
    $('.buttonFinish').hide();//addClass('btn btn-default');

};

/*Cargar Datatable.net para la tabla listado de tickets */
function loadTableTickets() {
    $('#dtb-tickets').dataTable({		/* filtro y paginación de la tabla de Tickets TCX} */
        lengthMenu: [10, 25, 50],
        order: [[0, 'desc']],
        language: {
            lengthMenu: 'Mostrar _MENU_ Items',
            info: 'Mostrar _START_ a _END_ de _TOTAL_ Items',
            infoEmpty: 'No hay Items para mostrar',
            search: 'Buscar: ',
            sSearchPlaceholder: 'Ingrese texto a buscar',
            zeroRecords: 'No se encontraron registros coincidentes',
            infoFiltered: '(Filtrado de _MAX_ totales Items)',
            paginate: {
                previous: 'Anterior',
                next: 'Siguiente'
            }
        }
    });
}


/** cargar controles de rango de fechas */
function init_daterangepicker() {

    var dateCurrent = moment().format('DD/MM/YYYY');

    $('#txt-fecha-inicio').val(dateCurrent);
    $('#txt-fecha-final').val(dateCurrent);

    if (typeof ($.fn.daterangepicker) === 'undefined') { return; }
    //console.log('init_daterangepicker');

    var cb = function (start, end, label) {
        console.log(start.toISOString(), end.toISOString(), label);
        $('#reportrange span').html(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY'));
        $('#reportHistorialRange span').html(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY'));
        var dateCurrent = moment().format('DD/MM/YYYY');
        console.log(dateCurrent);
        $('#txt-fecha-inicio').val(dateCurrent);
        $('#txt-fecha-final').val(dateCurrent);
    };

    var optionSet1 = {
        startDate: moment(), //moment().subtract(29, 'days'),
        endDate: moment(),
        minDate: '01/01/2019',
        maxDate: '12/31/2030',
        dateLimit: {
            months: 12
        },
        linkedCalendars: false,
        showDropdowns: true,
        showWeekNumbers: false,
        timePicker: false,
        timePickerIncrement: 1,
        timePicker12Hour: true,
        ranges: {
            'Hoy': [moment(), moment()],
            'Ayer': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Últimos 7 días': [moment().subtract(6, 'days'), moment()],
            'últimos 30 días': [moment().subtract(29, 'days'), moment()],
            'Este Mes': [moment().startOf('month'), moment().endOf('month')],
            'último Mes': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        opens: 'left',
        buttonClasses: ['btn btn-default'],
        applyClass: 'btn-small btn-danger',
        cancelClass: 'btn-small',
        format: 'DD/MM/YYYY',
        separator: ' to ',
        locale: {
            applyLabel: 'Consultar',
            cancelLabel: 'Cancelar',
            fromLabel: 'From',
            toLabel: 'To',
            customRangeLabel: 'Elegir Rango',
            daysOfWeek: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            firstDay: 1
        }
    };

    $('#reportrange span').html(moment().format('DD/MM/YYYY') + ' - ' + moment().format('DD/MM/YYYY'));
    $('#reportrange').daterangepicker(optionSet1, cb);
    $('#reportrange').on('show.daterangepicker', function () {
        console.log("show event fired");
    });
    $('#reportrange').on('hide.daterangepicker', function () {
        console.log("hide event fired");
    });
    $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
        console.log("Fechas Aplicadas para el reporte: " + picker.startDate.format('DD/MM/YYYY') + " to " + picker.endDate.format('DD/MM/YYYY'));
        var IdResponsable = $('#cboResponsable').val();
        var nomResponsable = $('#cboResponsable option:selected').text();

        var IdArea = $('#cboAreaReporte').val();
        var nomArea = $('#cboAreaReporte option:selected').text();

        if (IdResponsable === '')
            nomResponsable = '';

        if (IdArea === '')
            nomArea = '';

        $('#txt-fecha-inicio').val(picker.startDate.format('DD/MM/YYYY'));
        $('#txt-fecha-final').val(picker.endDate.format('DD/MM/YYYY'));

        reporteBarras(picker.startDate.format('DD/MM/YYYY'), picker.endDate.format('DD/MM/YYYY'), IdResponsable, IdArea);
        reporteDonuts(picker.startDate.format('DD/MM/YYYY'), picker.endDate.format('DD/MM/YYYY'), IdResponsable, IdArea);
        filtrosAplicadosReporte(picker.startDate.format('MMMM D, YYYY'), picker.endDate.format('MMMM D, YYYY'), nomResponsable, nomArea);
    });
    $('#reportrange').on('cancel.daterangepicker', function (ev, picker) {
        console.log("cancel event fired");
    });
    $('#options1').click(function () {
        $('#reportrange').data('daterangepicker').setOptions(optionSet1, cb);
    });
    $('#options2').click(function () {
        $('#reportrange').data('daterangepicker').setOptions(optionSet2, cb);
    });
    $('#destroy').click(function () {
        $('#reportrange').data('daterangepicker').remove();
    });


    /**Datepicket para filtrar fechas del reporte Historial Tickets */
    $('#reportHistorialRange span').html(moment().format('DD/MM/YYYY') + ' - ' + moment().format('DD/MM/YYYY'));
    $('#reportHistorialRange').daterangepicker(optionSet1, cb);
    $('#reportHistorialRange').on('show.daterangepicker', function () {
        console.log("show event fired");
    });
    $('#reportHistorialRange').on('hide.daterangepicker', function () {
        console.log("hide event fired");
    });
    $('#reportHistorialRange').on('apply.daterangepicker', function (ev, picker) {
        console.log("Fechas Aplicadas para el reporte historial: " + picker.startDate.format('DD/MM/YYYY') + " to " + picker.endDate.format('DD/MM/YYYY'));
        var start_Date = picker.startDate.format('DD/MM/YYYY');
        var end_Date = picker.endDate.format('DD/MM/YYYY');

        $('#txt-fecha-inicio').val(start_Date);
        $('#txt-fecha-final').val(end_Date);
        filtrarHistorialTickets();

    });
    $('#reportHistorialRange').on('cancel.daterangepicker', function (ev, picker) {
        console.log("cancel event fired");
    });
    $('#options1').click(function () {
        $('#reportHistorialRange').data('daterangepicker').setOptions(optionSet1, cb);
    });
    $('#options2').click(function () {
        $('#reportHistorialRange').data('daterangepicker').setOptions(optionSet2, cb);
    });
    $('#destroy').click(function () {
        $('#reportHistorialRange').data('daterangepicker').remove();
    });

}

/** Cargar controles datePicker */
function init_daterangepicker_single_call() {

    if (typeof ($.fn.daterangepicker) === 'undefined') { return; }
    //console.log('init_daterangepicker_single_call');

    $('#single_cal1').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        minYear: 1901,
        maxYear: parseInt(moment().format('YYYY'), 10),
        /*   locale: {
              format: 'DD/MM/YYYY '
          }, */
        singleClasses: "picker_4",
    }, function (start, end, label) {
        console.log(start.toISOString(), end.toISOString(), label);
    });
    $('#single_cal2').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        minYear: 1901,
        maxYear: parseInt(moment().format('YYYY'), 10),
        singleClasses: "picker_4"
    }, function (start, end, label) {
        console.log(start.toISOString(), end.toISOString(), label);
    });
    $('#single_cal3').daterangepicker({
        singleDatePicker: true,
        singleClasses: "picker_3"
    }, function (start, end, label) {
        console.log(start.toISOString(), end.toISOString(), label);
    });
    $('#single_cal4').daterangepicker({
        singleDatePicker: true,
        singleClasses: "picker_4"
    }, function (start, end, label) {
        console.log(start.toISOString(), end.toISOString(), label);
    });


}


/**Init tooltip */
function _init_tooltip() {
    $('[data-toggle="tooltip"]').tooltip({
        container: 'body'
    });
}

/**barra de progreso de la tabla lista de tickets */
if ($(".progress .progress-bar")[0]) {
    $('.progress .progress-bar').progressbar();
}

/**Checkbox design para el reporte historial de tickets */
function _init_checkBox() {
    if ($("input.flat")[0]) {
        $(document).ready(function () {
            $('input.flat').iCheck({
                checkboxClass: 'icheckbox_flat-red',
                radioClass: 'iradio_flat-red'
            });
        });
    }
}

$('#detalle_prioridad_IdPrio').on('change', function () {
    var id = $('#detalle_prioridad_IdPrio option:selected').val();

    var icon = ['fa fa-long-arrow-down', 'fa fa-arrows-h', 'fa fa-long-arrow-up'];
    var colorIcon = ['#00FF00', '#0070C0', '#FF0000'];

    if (id !== '') {
        $('#icon-prioridad').html('<i class="' + icon[id - 1] + '" style="color:' + colorIcon[id - 1] + '"></i>');
    } else {
        $('#icon-prioridad').empty();
    }
});


/**disable button after submit new ticket */
$("#form-new-ticket").submit(function () {
    $this = $(this);

    /**valores de validacion */
    var contacto = $('#contacto').val();
    var empresa = $('#empresa').val();
    var motivo = $('#motivo_IdMotivo option:selected').val();
    var prioridad = $('#detalle_prioridad_IdPrio option:selected').val();
    var encargado = $('#detalle_encargado_IdEnc option:selected').val();
    var estado = $('#detalle_estado_IdEstado option:selected').val();
    var detalle = $('#detalle_descripcion').val();

    /** prevent double posting */
    if ($this.data().isSubmitted) {
        console.log('el formulario ya fue enviado');
        return false;
    }

    if (contacto !== '' && empresa !== '' && motivo !== '' && prioridad !== '' && encargado !== '' && estado !== '' && detalle !== '') {
        $this.data().isSubmitted = true;
        $('#btn-new-ticket').text('Procesando');
        console.log('enviado');

        return true;
    }

});

/* buscar ticket por fecha o nro tickets */
$('#btn-buscar-tck').click(function () {
    console.log('FiltrarTicket');
    buscarTicket();
});

function buscarTicket() {
    var NroTck = $('#txt-Nro').val();
    var dateStart = $('#single_cal1').val();
    var dateEnd = $('#single_cal2').val();
    var state = 0;
    var area = 0;

    if (NroTck === '')
        NroTck = 0;

    $.ajax({
        url: '/Ticket/FiltrarTicketLista',
        type: 'POST',
        data: { fechaInicio: dateStart, fechaFin: dateEnd, nroTck: NroTck, intEstado: state, intArea: area },
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
                message: 'Procesando..'
            });
        },
        success: function (response) {
            if (response.trim() === '') {
                window.location.replace("/Login/Login");
            } else {
                if (response.trim() !== '<p>empty</p>') {
                    $('#content-dataTable').empty();
                    $('#content-dataTable').html(response);

                    if ($(".progress .progress-bar")[0]) {
                        $('.progress .progress-bar').progressbar();
                    }
                    loadTableTickets();
                    _init_tooltip();
                } else {
                    var sinDatos = '<div class="alert alert-danger dialog-content"> No se encontraron registros para los filtros aplicados</div>';
                    $('#content-dataTable').html(sinDatos);
                }
            }
        },
        complete: function () {
            $.unblockUI();
        },
        error: function (xhr, status, error) {
            alert('ERROR ' + xhr.status + ' ' + error);
        }
    });

}


/*filtrar Tickets por estado */
$('#cboEstado').on('change', function () {
    var NroTck = 0;
    var dateStart = $('#single_cal1').val();
    var dateEnd = $('#single_cal2').val();
    var state = $('#cboEstado').val();
    var stateText = $('#cboEstado option:selected').text();
    var area = $('#cboArea').val();
    var areaText = $('#cboArea option:selected').text();

    /*  console.log(state);
     console.log(area); */
    $.ajax({
        url: '/Ticket/FiltrarTicketLista',
        type: 'POST',
        data: { fechaInicio: dateStart, fechaFin: dateEnd, nroTck: NroTck, intEstado: state, intArea: area },
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
                message: 'Procesando..'
            });
        },
        success: function (response) {
            if (response.trim() === '') {
                window.location.replace("/Login/Login");
            } else {
                if (response.trim() !== '<p>empty</p>') {
                    $('#content-dataTable').empty();

                    $('#content-dataTable').html(response);

                    if ($(".progress .progress-bar")[0]) {
                        $('.progress .progress-bar').progressbar();
                    }
                    loadTableTickets();
                    _init_tooltip();
                } else {
                    var validarArea = '';
                    if (area !== '')
                        validarArea = ' y el área <strong>' + areaText + '</strong>';
                    var sinDatos = '<div class="alert alert-danger dialog-content"> No se encontraron registros con el estado <strong>' + stateText + '</strong> ' + validarArea + '</div>';
                    $('#content-dataTable').html(sinDatos);
                }
            }
        },
        complete: function () {
            $.unblockUI();
        },
        error: function (xhr, status, error)  {
            alert('ERROR ' + xhr.status + ' ' + error);
        }
    });

});


/**filtrar ticket por Área */
$('#cboArea').on('change', function () {
    var NroTck = 0;
    var dateStart = $('#single_cal1').val();
    var dateEnd = $('#single_cal2').val();
    var state = $('#cboEstado').val();
    var stateText = $('#cboEstado option:selected').text();
    var area = $('#cboArea').val();
    var areaText = $('#cboArea option:selected').text();

    /*     console.log(area);
        console.log(state); */
    $.ajax({
        url: '/Ticket/FiltrarTicketLista',
        type: 'POST',
        data: { fechaInicio: dateStart, fechaFin: dateEnd, nroTck: NroTck, intEstado: state, intArea: area },
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
                message: 'Procesando..'
            });
        },
        success: function (response) {
            if (response.trim() === '') {
                window.location.replace("/Login/Login");
            } else {
                if (response.trim() !== '<p>empty</p>') {
                    $('#content-dataTable').empty();
                    //console.log(response);
                    $('#content-dataTable').html(response);

                    if ($(".progress .progress-bar")[0]) {
                        $('.progress .progress-bar').progressbar();
                    }
                    loadTableTickets();
                    _init_tooltip();
                } else {
                    var validarEstado = '';
                    if (state !== '')
                        validarEstado = ' y el estado <strong>' + stateText + '</strong>';
                    var sinDatos = '<div class="alert alert-danger dialog-content"> No se encontraron registros para el área <strong>' + areaText + '</strong> ' + validarEstado + '</div>';
                    $('#content-dataTable').html(sinDatos);
                }
            }
        },
        complete: function () {
            $.unblockUI();
        },
        error: function (xhr, status, error)  {
            alert('ERROR ' + xhr.status + ' ' + error);
        }
    });

});


/* confirmar eliminación de ticket */
function confirElimina(id) {
    $('#id-del-Codigo').val(id);
    var mensaje = '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
        '<span aria-hidden="true">&times;</span></button>';
    $('.dialog-content').empty();
    $('.dialog-content').html('<p>Está seguro de eliminar al ticket Nro <strong>' + id + '</strong> de forma permanente?</p>');
}

/* eliminar ticket */
$('#btn-elimina-tck').on('click', function () {
    var Nro = $('#id-del-Codigo').val();
    var motivo = $('#txt-motivo-elimina').val();

    if (motivo === '') {
        $.ambiance({
            message: 'Ingrese Motivo por el cual desea eliminar el ticket.',
            title: "Eliminar Ticket",
            type: "error"
        });
        return;
    }

    $.ajax({
        url: '/Ticket/CambiarEstadoTicket',
        type: 'POST',
        data: { id: Nro, observa: motivo },
        success: function (response) {
            console.log(response);
            if (response.success !== null) {
                buscarTicket();
                $.ambiance({
                    message: 'El ticket fue eliminado correctamente',
                    title: "Eliminar Ticket",
                    type: "success"
                });
                $('#fm-modal-delete').modal('hide');
            } else { /* No se pudo eliminar el registro */
                $.ambiance({
                    message: response.error,
                    title: "Eliminar Ticket",
                    type: "error"
                });
                $('#fm-modal-delete').modal('hide');
            }
        },
        complete: function () {
            $('#txt-motivo-elimina').val('');
        },
        error: function(xhr, status, error)  {
            alert('ERROR ' + xhr.status + ' ' + error);
        }
    });
});

function eliminarTicket() {

}

function corouselImage(images) {

    var image = images.split(';');
    var tags = '';

    for (let index = 0; index < image.length; index++) {
        if (index === 0) {
            tags = '<div class="easyzoom easyzoom--overlay easyzoom--with-thumbnails">' +
                '<a href="../../images/' + image[index] + '">' +
                '<img src="../../images/' + image[index] + '" alt="" width="100%" height="400" class="img-responsive" />' +
                '</a>' +
                '</div>' +
                '<ul class="thumbnails">' +
                '<li>' +
                '<a href="../../images/' + image[index] + '" data-standard="../../images/' + image[index] + '">' +
                '<img src="../../images/' + image[index] + '" alt="" />' +
                '</a>' +
                '</li>';
        } else {
            tags += '<li>' +
                '<a href="../../images/' + image[index] + '" data-standard="../../images/' + image[index] + '">' +
                '<img src="../../images/' + image[index] + '" alt="" />' +
                '</a>' +
                '</li>';
        }
    }
    tags += '</ul>';


    /*  $('.carousel-inner').html(tags); */
    $('#galeria-id').html(tags);
    var $easyzoom = $('.easyzoom').easyZoom();

    // Setup thumbnails example
    var api1 = $easyzoom.filter('.easyzoom--with-thumbnails').data('easyZoom');

    $('.thumbnails').on('click', 'a', function (e) {
        var $this = $(this);

        e.preventDefault();

        // Use EasyZoom's `swap` method
        api1.swap($this.data('standard'), $this.attr('href'));
    });

}

function cargarPdf(rutaPDF) {

    var pdf = rutaPDF.split(';');
    var tags = '';
    var buttonsPDF = '';
    for (let index = 0; index < pdf.length; index++) {

        if (index === 0) {
            tags = '<embed src="../../PDF/' + pdf[index] + '#zoom=50" type="application/pdf" width="100%" height="600"></embed>';
            /* if(pdf.length>1){ */
            buttonsPDF =/* ' <div class="btn-group">'+ */
                '<button class="btn btn-default" onclick="cambiarPDF(\'' + pdf[index] + '\')" type="button">' + (index + 1) + '</button>';
            /* } */

        } else {
            buttonsPDF += '<button class="btn btn-default" onclick="cambiarPDF(\'' + pdf[index] + '\')" type="button">' + (index + 1) + '</button>';
        }

    }
    /*  buttonsPDF+='</div>'; */

    $('#pdf-carga').html(tags);
    $('#grupo-pdf').html(buttonsPDF);
}

function cambiarPDF(PDF) {
    $('#pdf-carga embed').remove();
    $('#pdf-carga').html('<embed src="../../PDF/' + PDF + '#zoom=50" type="application/pdf" width="100%" height="600"></embed>');
}

function maxSizeIMG_PDF() {
    console.log('maxSizeIMG_PDF')
    var fileImage = $('#real-input');
    var maxSizeImg = fileImage.data('max-size');
    var sizeImg = 0;

    //pdf
    var filePDF = $('#input-pdf');
    var maxSizePDF = filePDF.data('max-size');
    var sizePDF = 0;

    if (fileImage.get(0).files.length) {
        for (let index = 0; index < fileImage.get(0).files.length; index++) {
            sizeImg += fileImage.get(0).files[index].size;        // in bytes 
        }

        if (sizeImg > maxSizeImg) {
            $('.file-info').next('p').remove();
            $('.file-info').after('<p class="text-danger">El tamaño de los archivos no debe superar los 3 MB</p>');
            return false;
        } else {
            $('.file-info').next('p').remove();
        }
    }

    if (filePDF.get(0).files.length) {
        for (let index = 0; index < filePDF.get(0).files.length; index++) {
            sizePDF += filePDF.get(0).files[index].size;        // in bytes 
        }
        if (sizePDF > maxSizePDF) {
            $('.txt-files-selected').next('p').remove();
            $('.txt-files-selected').after('<p class="text-danger">El tamaño de los archivos no debe superar los 3 MB</p>');
            return false;
        } else {
            $('.txt-files-selected').next('p').remove();
        }
    }

}

//perfil 

function onFileSelected(event) {
    var selectedFile = event.target.files[0];
    var reader = new FileReader();

    var imgtag = document.getElementById("imgUser");
    imgtag.title = selectedFile.name;

    reader.onload = function (event) {
        imgtag.src = event.target.result;
    };

    reader.readAsDataURL(selectedFile);
}

//validar tamaño foto de perfil del usuario MAX 100 KB
function maxSizeImagePefil() {
    var fotoPerfil = $('#id-load-file');
    var size = fotoPerfil.get(0).files[0].size;
    console.log('tamaño foto ' + size);
    if (size > 100000) {
        $('#alert-max-size-image').html('<p class="text-danger">Tamaño máximo 100 KB</p>');
        return false;
    }
}

function ActualizarPerfil() {
    var IdUsu = $('input[name=IdUsu]').val().trim();
    var Nombres = $('input[name=Nombres]').val().trim();
    var ApePaterno = $('input[name=ApePaterno]').val().trim();
    var ApeMaterno = $('input[name=ApeMaterno]').val().trim();
    var Usuario = $('input[name=Usuario]').val().trim();
    var clave = $('input[name=clave]').val().trim();
    var IdRol = $('#rol_IdRol option:selected').val().trim();
    var IdEmp = $('#empresa_IdEmp option:selected').val().trim();

    var UsuarioEN = {
        IdUsu: IdUsu,
        Nombres: Nombres,
        ApePaterno: ApePaterno,
        ApeMaterno: ApeMaterno,
        Usuario: Usuario,
        clave: clave,
        rol: {
            IdRol: IdRol
        },
        empresa: {
            IdEmp: IdEmp
        }
    };
    console.log(UsuarioEN);

    $.ajax({
        url: '/Usuario/Perfil',
        type: 'POST',
        data: UsuarioEN,
        success: function (response) {
            console.log(response);
            if (response.success !== null) {

                $.ambiance({
                    message: response.success,
                    title: "Perfil",
                    type: "success"
                });
            } else {
                $.ambiance({
                    message: response.error,
                    title: "Perfil",
                    type: "error"
                });
            }
        }
    });

}

/**filtrar Reporte Diario -- */
/**---------------------------------------------------- */
var mybarChart = null;
function reportesDiarios() {
    reporteBarras('', '', 0, 0);
    reporteDonuts('', '', 0, 0);
    filtrosAplicadosReporte('', '', '', '');

}

function reporteBarras(fechaInicio, fechaFin, IdResponsable, IDArea) {
    $('#report-barras-null').empty();
    var coordenadaX = new Array();
    var coordenadaY = new Array();

    var arrayEstado = new Array();
    var arrayCantSuport = new Array();
    var arrayEncargado = new Array();

    if (mybarChart != null) {
        mybarChart.destroy();
    }

    function colorArray(array) {
        var colors = new Array(array.length);

        for (let i = 0; i < array.length; i++) {
            switch (array[i][0]) {
                case 1:
                    colors[i] = '#FD9644';//'#CD201F';
                    break;
                case 2:
                    colors[i] = '#f1c40f';//'#FFA54A';
                    break;
                case 3:
                    colors[i] = '#67CC00';
                    break;
                case 4:
                    colors[i] = '#39A7F0';
                    break;
            }
        }
        return colors;
    }

    $.ajax({
        url: '/Reporte/ReporteAvanceBarras',
        type: 'POST',
        data: jQuery.param({ dateStart: fechaInicio, dateEnd: fechaFin, idResp: IdResponsable, idarea: IDArea }),
        success: function (response) {

            if (response.length !== 0) {
                var cantTickets = 0;
                for (let i = 0; i < response.length; i++) {
                    coordenadaX.push('Tck ' + response[i].Nro);
                    coordenadaY.push(response[i].detalle.progreso);

                    var estado = [response[i].detalle.estado.IdEstado, response[i].detalle.estado.DesEstado];
                    arrayEstado.push(estado);
                    arrayCantSuport.push(response[i].cantSoporte);
                    arrayEncargado.push(response[i].detalle.encargado.Nombre);
                    cantTickets++;
                }
                /* console.log(coordenadaX);
                console.log(coordenadaY);
                console.log(arrayEstado);
                console.log(arrayCantSuport); 
                console.log(arrayEncargado);*/
                $('#id-cant-sum-total').html(cantTickets);

                if (cantTickets > 40) {
                    $('#container-bar-chart').removeClass('col-md-6 col-sm-6');
                    $('#container-bar-chart').addClass('col-md-12 col-sm-12');
                } else {
                    $('#container-bar-chart').removeClass('col-md-12 col-sm-12');
                    $('#container-bar-chart').addClass('col-md-6 col-sm-6');
                }

                var ctx = $('#report-barras');
                var backColors = colorArray(arrayEstado);
                //https://www.chartjs.org/docs/latest/configuration/tooltip.html#tooltip-callbacks
                //https://stackoverflow.com/questions/44427411/how-to-append-more-data-in-tooltip-of-graph-in-chart-js
                mybarChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: coordenadaX,
                        datasets: [{
                            label: '% Avance',
                            backgroundColor: backColors,
                            //borderColor:'#7bd235',
                            //borderWidth: 1,
                            data: coordenadaY
                        }]
                    },

                    options: {
                        maintainAspectRatio: false,//ajustar su altura 
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        },
                        tooltips: {
                            callbacks: {
                                afterBody: function (tooltipItem, data) {
                                    //console.log(data);
                                    var label = new Array();
                                    var i = tooltipItem[0].index;
                                    var estado = 'Estado: ' + arrayEstado[i][1];
                                    var cantSoporte = 'Cant. Soporte: ' + arrayCantSuport[i];
                                    var encargado = 'Resp: ' + arrayEncargado[i];
                                    label.push(estado, cantSoporte, '', encargado);
                                    return label;
                                }
                            }
                        },
                    }
                });

            } else {
                $('#id-cant-sum-total').html('0');
                $('#report-barras-null').html('<p><code>No se encontraron registros para el reporte Barras</code><p>');
            }

        }
    });

    /*  $.ajax({
         url: '/Reporte/ReporteProgresoBarras',
         type: 'POST',
         data: jQuery.param({ dateStart: fechaInicio, dateEnd: fechaFin, idResp: IdResponsable }),
         success: function (response) {
 
             if (response.length !== 0) {
                 var cantTickets = 0;
                 for (let i = 0; i < response.length; i++) {
                     //coordenadaX.push(response[i].detalle.estado.DesEstado + ' /Tck ' + response[i].Nro);
                     coordenadaX.push('Tck ' + response[i].Nro);
                     coordenadaY.push(response[i].detalle.progreso);
                     cantTickets++;
                 }
                 //console.log(coordenadaX);
                 //console.log(coordenadaY);
                 console.log('cantidad tickets ' + cantTickets);
                 if (cantTickets > 40) {
                     $('#container-bar-chart').removeClass('col-md-6 col-sm-6');
                     $('#container-bar-chart').addClass('col-md-12 col-sm-12');
                 } else {
                     $('#container-bar-chart').removeClass('col-md-12 col-sm-12');
                     $('#container-bar-chart').addClass('col-md-6 col-sm-6');
                 }
 
                 var ctx = $('#report-barras');
 
                 mybarChart = new Chart(ctx, {
                     type: 'bar',
                     data: {
                         labels: coordenadaX,
                         datasets: [{
                             label: '% Avance',
                             backgroundColor: '#7bd235',
                                //borderColor:'#7bd235',
                                //borderWidth: 1,
                             data: coordenadaY
                         }]
                     },
 
                     options: {
                         maintainAspectRatio: false,//ajustar su altura 
                         scales: {
                             yAxes: [{
                                 ticks: {
                                     beginAtZero: true
                                 }
                             }]
                         }
                     }
                 });
                 console.log(mybarChart);
 
 
             } else {
                 $('#report-barras-null').html('<p><code>No se encontraron registros para el reporte Barras</code><p>');
             }
 
         }
     }); */
}

function reporteDonuts(fechaInicio, fechaFin, IdResponsable, IDArea) {

    var theme = {
        color: [
            '#FD9644', '#f1c40f', '#5eba00', '#3498DB', //'#cd201f', '#fd9644', '#5eba00', '#3498DB',
            '#9B59B6', '#8abb6f', '#759c6a', '#bfd3b7'
        ],

        title: {
            itemGap: 8,
            textStyle: {
                fontWeight: 'normal',
                color: '#408829'
            }
        },

        dataRange: {
            color: ['#1f610a', '#97b58d']
        },

        toolbox: {
            color: ['#408829', '#408829', '#408829', '#408829']
        },

        tooltip: {
            backgroundColor: 'rgba(0,0,0,0.5)',
            axisPointer: {
                type: 'line',
                lineStyle: {
                    color: '#408829',
                    type: 'dashed'
                },
                crossStyle: {
                    color: '#408829'
                },
                shadowStyle: {
                    color: 'rgba(200,200,200,0.3)'
                }
            }
        },

        dataZoom: {
            dataBackgroundColor: '#eee',
            fillerColor: 'rgba(64,136,41,0.2)',
            handleColor: '#408829'
        },
        grid: {
            borderWidth: 0
        },

        categoryAxis: {
            axisLine: {
                lineStyle: {
                    color: '#408829'
                }
            },
            splitLine: {
                lineStyle: {
                    color: ['#eee']
                }
            }
        },

        valueAxis: {
            axisLine: {
                lineStyle: {
                    color: '#408829'
                }
            },
            splitArea: {
                show: true,
                areaStyle: {
                    color: ['rgba(250,250,250,0.1)', 'rgba(200,200,200,0.1)']
                }
            },
            splitLine: {
                lineStyle: {
                    color: ['#eee']
                }
            }
        },
        timeline: {
            lineStyle: {
                color: '#408829'
            },
            controlStyle: {
                normal: { color: '#408829' },
                emphasis: { color: '#408829' }
            }
        },

        k: {
            itemStyle: {
                normal: {
                    color: '#68a54a',
                    color0: '#a9cba2',
                    lineStyle: {
                        width: 1,
                        color: '#408829',
                        color0: '#86b379'
                    }
                }
            }
        },
        map: {
            itemStyle: {
                normal: {
                    areaStyle: {
                        color: '#ddd'
                    },
                    label: {
                        textStyle: {
                            color: '#c12e34'
                        }
                    }
                },
                emphasis: {
                    areaStyle: {
                        color: '#99d2dd'
                    },
                    label: {
                        textStyle: {
                            color: '#c12e34'
                        }
                    }
                }
            }
        },
        force: {
            itemStyle: {
                normal: {
                    linkStyle: {
                        strokeColor: '#408829'
                    }
                }
            }
        },
        chord: {
            padding: 4,
            itemStyle: {
                normal: {
                    lineStyle: {
                        width: 1,
                        color: 'rgba(128, 128, 128, 0.5)'
                    },
                    chordStyle: {
                        lineStyle: {
                            width: 1,
                            color: 'rgba(128, 128, 128, 0.5)'
                        }
                    }
                },
                emphasis: {
                    lineStyle: {
                        width: 1,
                        color: 'rgba(128, 128, 128, 0.5)'
                    },
                    chordStyle: {
                        lineStyle: {
                            width: 1,
                            color: 'rgba(128, 128, 128, 0.5)'
                        }
                    }
                }
            }
        },
        gauge: {
            startAngle: 225,
            endAngle: -45,
            axisLine: {
                show: true,
                lineStyle: {
                    color: [[0.2, '#86b379'], [0.8, '#68a54a'], [1, '#408829']],
                    width: 8
                }
            },
            axisTick: {
                splitNumber: 10,
                length: 12,
                lineStyle: {
                    color: 'auto'
                }
            },
            axisLabel: {
                textStyle: {
                    color: 'auto'
                }
            },
            splitLine: {
                length: 18,
                lineStyle: {
                    color: 'auto'
                }
            },
            pointer: {
                length: '90%',
                color: 'auto'
            },
            title: {
                textStyle: {
                    color: '#333'
                }
            },
            detail: {
                textStyle: {
                    color: 'auto'
                }
            }
        },
        textStyle: {
            fontFamily: 'Arial, Verdana, sans-serif'
        }
    };

    class Data {
        constructor(value, name) {
            this.value = value;
            this.name = name;
        }
    }


    $.ajax({
        url: '/Reporte/ReporteEstadoDonuts',
        type: 'POST',
        data: jQuery.param({ dateStart: fechaInicio, dateEnd: fechaFin, idResp: IdResponsable, idarea: IDArea }),
        success: function (response) {
            if (response.length !== 0) {
                var estado = new Array();
                var grafico = new Array();

                for (let i = 0; i < response.length; i++) {
                    var datosReporte = new Data(response[i].IdEstado, response[i].DesEstado);
                    estado.push(response[i].DesEstado);
                    grafico.push(datosReporte);
                    /*   grafico.push(new Data(response[i].IdEstado, response[i].DesEstado)); */
                    reporteCantXEstado(datosReporte);
                }
                /* console.log(estado);
                console.log(grafico); */
                var echartPie = echarts.init(document.getElementById('report-donut'), theme);

                echartPie.setOption({
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    legend: {
                        x: 'center',
                        y: 'bottom',
                        data: estado
                    },
                    toolbox: {
                        show: true,
                        feature: {
                            magicType: {
                                show: true,
                                type: ['pie', 'funnel'],
                                option: {
                                    funnel: {
                                        x: '25%',
                                        width: '50%',
                                        funnelAlign: 'center',
                                        max: 1548
                                    }
                                }
                            },
                            restore: {
                                show: true,
                                title: "Restaurar"
                            },
                            saveAsImage: {
                                show: true,
                                title: "Guardar Imagen"
                            }
                        }
                    },
                    calculable: true,
                    series: [{
                        name: 'Cantidad Tickets',
                        type: 'pie',
                        radius: '55%',
                        center: ['50%', '48%'],
                        data: grafico
                    }]
                });


            } else {
                reporteCantXEstado(null);
                $('#report-donut').html('<p><code>No se encontraron registros para el reporte Circular</code><p>');
            }
        }
    });
}


function reporteCantXEstado(datos) {
    if (datos !== null) {
        if (datos.name === 'Abierto') {
            $('#id-cant-open').html(datos.value);
        } else if (datos.name === 'Pendiente') {
            $('#id-cant-pending').html(datos.value);
        } else if (datos.name === 'Solucionado') {
            $('#id-cant-solved').html(datos.value);
        } else {
            $('#id-cant-closed').html(datos.value);
        }
    } else {
        $('#id-cant-open').html('0');

        $('#id-cant-pending').html('0');

        $('#id-cant-solved').html('0');

        $('#id-cant-closed').html('0');
    }
}

function filtrosAplicadosReporte(desde, hasta, responsable, area) {
    if (desde === '' && hasta === '') {
        desde = moment().format('MMMM D, YYYY');
        hasta = moment().format('MMMM D, YYYY');
    }

    if (responsable !== '')
        area = ', ' + area;

    $('#date-start-label').html(desde);
    $('#date-end-label').html(hasta);
    $('#resp-label-filter').html(responsable);
    $('#area-label-filter').html(area);
}

$('#cboResponsable').on('change', function ()  {

    var date_start = $('#txt-fecha-inicio').val();
    var date_end = $('#txt-fecha-final').val();

    var IdResponsable = $('#cboResponsable').val();
    var nomResponsable = $('#cboResponsable option:selected').text();

    var IdArea = $('#cboAreaReporte').val();
    var nomArea = $('#cboAreaReporte option:selected').text();

    if (IdResponsable === '')
        nomResponsable = '';

    if (IdArea === '')
        nomArea = '';

    reporteBarras(date_start, date_end, IdResponsable, IdArea);
    reporteDonuts(date_start, date_end, IdResponsable, IdArea);
    filtrosAplicadosReporte(date_start, date_end, nomResponsable, nomArea);

});

$('#cboAreaReporte').on('change', function ()  {

    $('#cboResponsable').empty();
    /**Generar resporte */
    var date_start = $('#txt-fecha-inicio').val();
    var date_end = $('#txt-fecha-final').val();

    var IdResponsable = $('#cboResponsable').val();
    var nomResponsable = $('#cboResponsable option:selected').text();

    var IdArea = $('#cboAreaReporte').val();
    var nomArea = $('#cboAreaReporte option:selected').text();

    if (IdResponsable === '')
        nomResponsable = '';

    if (IdArea === '')
        nomArea = '';

    reporteBarras(date_start, date_end, IdResponsable, IdArea);
    reporteDonuts(date_start, date_end, IdResponsable, IdArea);
    filtrosAplicadosReporte(date_start, date_end, nomResponsable, nomArea);

    /**Traer Encargado por Area */
    var IdArea = $('#cboAreaReporte').val();
    if (IdArea === '') {
        if ($('#cboResponsable').has('option').length === 0) {
            $('#cboResponsable').append('<option value="">Filtrar por Responsable..</option>');
        }
        return;
    }


    $.post(
        '/Reporte/getEncargadoArea',
        { idarea: IdArea },
        function (response)  {
            if (response.length > 0) {
                for (let index = 0; index < response.length; index++) {
                    if (index === 0) {
                        $('#cboResponsable').append('<option value="">Filtrar por Responsable..</option>');
                    }

                    $('#cboResponsable').append('<option value="' + response[index].IdEnc + '">' + response[index].Nombre + '</option>');
                }

            } else {
                $('#cboResponsable').empty();
                $('#cboResponsable').append('<option value="">Filtrar por Responsable..</option>');
            }

        }
    ).fail(function (result) {
        alert('ERROR ' + result.status + ' ' + result.statusText);
    });

});

/**----------------------------------------------*/


/**filtrar historial de tickets */
/**----------------------------------------------*/
function filtrarHistorialTickets() {

    var fechaInicio = $('#txt-fecha-inicio').val();
    var fechaFin = $('#txt-fecha-final').val();
    var estado = $('#cboEstadoFilter').val();
    var area = $('#cboAreaFilter').val();
    var responsable = $('#cboResponsableFilter').val();
    var incluyeFechaReg = $('#chk-include-fecReg').is(':checked');

    $.ajax({
        url: '/Reporte/filtrarHistorialTicket',
        type: 'POST',
        data: { startDate: fechaInicio, endDate: fechaFin, idState: estado, idArea: area, idResp: responsable, includeFecReg: incluyeFechaReg },
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
                message: 'Procesando..'
            });
        },
        success: function (response) {
            if (response.trim() !== '') {
                $('#container-report-historial').empty();

                $('#container-report-historial').html(response);
                panelToolBox();
            }
        },
        complete: function () {
            $.unblockUI();
        },
        error: function (xhr, status, error)  {
            alert('ERROR ' + xhr.status + ' ' + error);
        }
    });
}

$('#cboAreaFilter').on('change', function () {
    filtrarHistorialTickets()
});

$('#cboEstadoFilter').on('change', function () {
    filtrarHistorialTickets()
});

$('#cboResponsableFilter').on('change', function () {
    filtrarHistorialTickets()
});

/**----------------------------------------------*/



