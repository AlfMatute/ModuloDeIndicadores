var datatable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "scrollY": "200px",
        "scrollCollapse": true,
        "columnDefs": [{
            "defaultContent": "-",
            "targets": "_all"
        }],
        "ajax": {
            "url": "/api/Nota",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [
            { "data": "nombre_Indicador", "width": "20%" },
            { "data": "descripcion_Indicadores", "width": "20%" },
            { "data": "nombreUsuario", "width": "20%" },
            { "data": "departamento", "width": "20%" },
            { "data": "descripcion", "width": "20%" },
            { "data": "logro", "width": "20%" },
            { "data": "puntos", "width": "20%" },
            { "data": "mes", "width": "20%" },
            { "data": "año", "width": "20%" },
            {
                "data": "id_Nota",
                "render": function (data) {
                    return `<div class="text-center">
<a href="/Notas/Edit?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
Editar
</a>
&nbsp;
<a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
onclick=Delete('/api/nota?id='+${data})>
Eliminar
</a>
&nbsp;
<a class='btn btn-info text-white' style='cursor:pointer; width:70px;'
onclick=Descargar('/api/notaadjunto?id='+${data})>
Adjunto
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

function Descargar(url) {
    $.ajax({
        type: "GET",
        url: url,
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (blob, status, xhr) {
            // check for a filename
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                window.navigator.msSaveBlob(blob, filename);
            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename) {
                    // use HTML5 a[download] attribute to specify filename
                    var a = document.createElement("a");
                    // safari doesn't support this yet
                    if (typeof a.download === 'undefined') {
                        window.location.href = downloadUrl;
                    } else {
                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {
                    window.location.href = downloadUrl;
                }

                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
            }
        }
    });
}