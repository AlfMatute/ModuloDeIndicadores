var datatable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "scrollY": "200px",
        "scrollCollapse": true,
        "filter": true,
        "columnDefs": [{
            "defaultContent": "-",
            "targets": "_all"
        }],
        "ajax": {
            "url": '/api/indicador',
            "type": "GET",
            "dataType": "json"
        },
        "columns": [
            { "data": "nombre_Indicador", "width": "20%" },
            { "data": "descripcion_Indicadores", "width": "20%" },
            { "data": "medida", "width": "20%" },
            { "data": "objetivo", "width": "20%" },
            { "data": "perfil", "width": "20%" },
            { "data": "ponderacion", "width": "20%" },
            { "data": "base", "width": "20%" },
            { "data": "meta", "width": "20%" },
            { "data": "departamento", "width": "20%" },
            { "data": "periodo", "width": "20%" },
            { "data": "fecha_Aproximada", "width": "20%" },
            { "data": "nombreUsuario", "width": "20%" },
            {
                "data": "id_Indicador",
                "render": function (data) {
                    return `<div class="text-center">
<a href="/Indicadores/Edit?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
Editar
</a>
&nbsp;
<a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
onclick=Delete('/api/indicador?id='+${data})>
Eliminar
</a>
</div>`;
                }, "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "No se encontraron datos",
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "paginate": {
                "first": "Primero",
                "previous": "Anterior",
                "next": "Siguiente",
                "last": "Ultimo"
            },
            "search": "Buscar",
            "info": "Mostrando _START_ hasta _END_ de _TOTAL_ elementos",
            "infoEmpty": "Mostrando _START_ hasta _END_ de _TOTAL_ elementos"
        },
        "width": "100%"
    })
}

function Delete(url) {
    swal({
        title: "Seguro que quiere eliminar?",
        text: "Una vez eliminado, este registro no podra ser recuperado.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }
    ).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function uploadFiles(inputId) {
    var input = document.getElementById(inputId);
    var files = input.files;
    var formData = new FormData();

    for (var i = 0; i != files.length; i++) {
        formData.append("files", files[i]);
    }

    $.ajax(
        {
            url: "/api/indicador",
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        }
    );
}