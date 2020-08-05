var dataTable;

$(document).ready(function () {
    cargarDatatable();
});


function cargarDatatable() {
    dataTable = $("#tblArticulos").DataTable({
        "ajax": {
            "url": "/Admin/Articulos/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "nombre", "width": "45%" },
            { "data": "categoria.nombre", "width": "15%" },
            { "data": "fechaCreacion", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Articulos/Edit/${data}" class="btn btn-success text-white" style="cursor: pointer; width=100px;"> 
                                    <i class="fas fa-edit"></i> Editar
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Articulos/Delete/${data}") class="btn btn-danger text-white" style="cursor: pointer; width=100px;"> 
                                    <i class="fas fa-trash-alt"></i> Borrar
                                </a>
                            </div>
                            `;
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "No hay registros"
        },
        "width": "100%"
    });
}


//SwitAlert
function Delete(url) {
    swal({
        title: "Esta seguro de que desea borrar el registro?",
        text: "Este contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirButtonText: "Si, borrar!",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                } else {
                    toastr.error(data.message);
                }
            }
        });
    });
}