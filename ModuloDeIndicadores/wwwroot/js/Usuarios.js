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
            "url": '/api/AppUser',
            "type": "GET",
            "dataType": "json"
        },
        "columns": [
            { "data": "nombreUsuario", "width": "20%" },
            { "data": "email", "width": "20%" },
            { "data": "phoneNumber", "width": "20%" },
            { "data": "identificador", "width": "20%" },
            { "data": "numero", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
<a href="/Usuarios/Edit?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
Editar
</a>
&nbsp;
<a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
onclick=Delete('/api/appUser?id=${data}')>
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