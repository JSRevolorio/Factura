
$(document).on('submit', '#formBuscarCliente', function (e) {
    e.preventDefault();

    var noNIT = $("#noNiT").val();

    if (noNIT != '') {

        $.ajax({
            type: 'POST',
            url: '/Venta/FindCliente/',
            data: { nit: noNIT},

            success: function (data) {

                if (data.Estado == true) {

                    $('#idCliente').val(data.Resultado.id);
                    $('#NombreCliente').val(data.Resultado.nombre);
                    $('#DireccionCliente').val(data.Resultado.direccion);

                       
                    $('#ErrorMensaje').removeClass('alert alert-danger');
                    $('#ErrorMensaje').text('');
                }
                else {
                    $('#CrearCliente').append(`<a href="/Cliente/Create" class="btn btn-success">Crear</a>`);
                    $('#ErrorMensaje').addClass('alert alert-danger');
                    $('#ErrorMensaje').text(data.Mensaje);
                }

            },
            error: function (xhr, status) {
            }
        });
    }
    else {
        $('#CrearCliente').append(`<a href="/Cliente/Create" class="btn btn-success">Crear</a>`);
        $('#ErrorMensaje').addClass('alert alert-danger');
        $('#ErrorMensaje').text("Ingrese un numero de nit");
    }
});

$(document).on('submit', '#formDetalleVenta', function (e) {
    e.preventDefault();

    $.validator.unobtrusive.parse("#formCreateVenta");
    $.validator.unobtrusive.parse("#formDetalleVenta");
    var venta =  $('#idVenta').val();

    if (venta !== '') {

        var detalle = $('#formDetalleVenta').serialize();

        $.ajax({
            type: 'POST',
            url: '/Venta/Add/',
            data: detalle,

            success: function (data) {

                if (data.Estado == true) {
                    var html = tabla(data);

                    $('#tablaBody').append(html);

                    totalSuma();
                }
                else {
                    $('#CrearCliente').append(`<a href="/Cliente/Create" class="btn btn-success">Crear</a>`);
                    $('#ErrorMensaje').addClass('alert alert-danger');
                    $('#ErrorMensaje').text(data.Mensaje);
                }

            },
            error: function (xhr, status) {
            }
        });

    }
    else
    {
        var datos = $("#formCreateVenta").serialize() + '&numeroFactura=' + $('#numeroFactura').val() + '&serie=' + $('#Serie').val() + '&numeroDTE=' + $('#numeroDTE').val() + '&fecha=' + $('#fecha').val();

        $.ajax({
            type: 'POST',
            url: '/Venta/Create/',
            data: datos,

            success: function (data) {

                if (data.Estado == true) {
                    $('#idVenta').val(data.Resultado)

                    var detalle = $('#formDetalleVenta').serialize();

                    $.ajax({
                        type: 'POST',
                        url: '/Venta/Add/',
                        data: detalle,

                        success: function (data) {

                            if (data.Estado == true) {
                                var html = tabla(data);

                                $('#tablaBody').append(html);

                                totalSuma();
                            }
                            else {
                                $('#CrearCliente').append(`<a href="/Cliente/Create" class="btn btn-success">Crear</a>`);
                                $('#ErrorMensaje').addClass('alert alert-danger');
                                $('#ErrorMensaje').text(data.Mensaje);
                            }

                        },
                        error: function (xhr, status) {
                        }
                    });

                    $('#ErrorMensaje').removeClass('alert alert-danger');
                    $('#ErrorMensaje').text('');
                }
                else {
                    $('#CrearCliente').append(`<a href="/Cliente/Create" class="btn btn-success">Crear</a>`);
                    $('#ErrorMensaje').addClass('alert alert-danger');
                    $('#ErrorMensaje').text(data.Mensaje);
                }

            },
            error: function (xhr, status) {
            }
        });


    }
});

//{ numeroFactura = noFactura, serie = serie, numeroDTE = dte, idCliente = idCliente, fecha = fecha, precioTotal= precioTotal }


function tabla(data) {
    var html = `<tr id="Fila${data.Resultado.idDetalle}">"
                 <td> ${data.Resultado.id} </td>
                 <td> ${data.Resultado.nombre}  </td> 
                 <td> ${data.Resultado.cantida} </td>
                 <td> Q.${data.Resultado.precio} </td>
                 <td class="dato"> ${data.Resultado.subTotal} </td>
                 <td>
                   <button type="button" value="${data.Resultado.idDetalle}" class="btn btn-outline-danger" id="EliminaFila">Eliminar</button>
                 </td> </tr >`;
    return html;
};

function totalSuma() {
    $('#TotalCompra').val(0);

    var sum = 0;
    $('.dato').each(function () {
        sum += parseFloat($(this).text());
    });

    $('#TotalCompra').val(sum);
}

$(document).ready(function () { 
    $('#tablaBody').on('click', '#EliminaFila', function () {
        var id = $('#EliminaFila').val();

        $.ajax({
            type: 'POST',
            url: '/Venta/Delete/' + id,
            success: function (data) {

                if (data.Estado == true) {
                    var fila = 'Fila' + id;

                    $('#' + fila).remove();

                    totalSuma();
                }
                else {
                    $('#ErrorMensaje').addClass('alert alert-danger');
                    $('#ErrorMensaje').text(data.Mensaje);
                }
            },
            error: function (xhr, status) {
                $('#mensaje').addClass('alert alert-danger');
            }
        });
    });
});