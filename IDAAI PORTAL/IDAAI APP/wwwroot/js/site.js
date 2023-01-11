
var offset = 1;
var limit = 5;
var respuestaTotal = [];
var informacion = new Object();
var paginaActual;
var numeroPaginas;
var idSeleccionado;

var campos = 'Campos';
var nombres = 'nombres';
var apellidos = 'apellidos';
var matricula = 'matricula';
var email = 'email';
var direccion = 'direccion';
var carrera = 'carrera';
var modulo = 'modulo';

var Pantalla = {
    Estudiante: 'Estudiante',
    Asistencia: 'Asistencia',
    Carrera: 'Carrera',
    Modulo: 'Modulo',
    Inventario: 'Inventario',
    Item: 'Item',
    Prestamo: 'Prestamo'
}
var Info = {
    Estudiante: {
        Titulos: ["Nombres", "Apellidos", "Matrícula", "Email", "Dirección", "Carrera", "Módulo"],
        Campos: ["nombres", "apellidos", "matricula", "email", "direccion", "carrera", "modulo"]
    }   
}
var PantallaActual;
var Datos = {
    Estudiante: {
        id: null,
        nombres: null,
        apellidos: null,
        matricula: null,
        email: null,
        direccion: null,
        carrera: null,
        modulo: null
    }
}
var Campos = {
    Estudiante: ["nombres", "apellidos", "matricula", "email", "direccion", "carrera", "modulo"]
}
var Query = {
    Estudiante: {
        nombres: null,
        apellidos: null,
        matricula: null,
        email: null,
        direccion: null,
        carrera: null,
        modulo: null
    }
}




function prepararPantalla(pantalla) {
    PantallaActual = pantalla;
    generarModalEliminacion(pantalla);
    $(`#navBar${PantallaActual}`).addClass("active");
}

function generarModalEliminacion(pantalla) {
    $("#pantalla").append(`
        <div class="modal fade" id="modalConfirmarEliminacion" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">Confirmar eliminación</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body px-4 col-md-12 row">
                        <p>¿Está seguro de que desea eliminar el registro seleccionado?</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <button type="button" class="btn btn-primary" onclick="eliminarRegistro(${pantalla})">Confirmar</button>
                    </div>
                </div>
            </div>
        </div>
    `)
}

function mostrarCarga(estaCargando) {
    if (estaCargando == true) {
        $("#cargando").removeClass("invisible");
    } else {
        $("#cargando").addClass("invisible");
    }
}

function formatearNombres(nombres) {
    const palabras = nombres.split(" ");
    for (let i = 0; i < palabras.length; i++) {
        palabras[i] = palabras[i][0].toUpperCase() + palabras[i].substr(1).toLowerCase();
        if (i + 1 == palabras.length) {            
            return palabras.join(" ");
        }
    }
}

function obtenerQuery(){
    Campos[PantallaActual].forEach(campo => {
        Query[PantallaActual][campo] = $(`#${campo}`).val() != '' && $(`#${campo}`).val() != "0" ? $(`#${campo}`).val() : null;
    })
    return Query;
}

function crearTabla(listaDatos, info) {
    limpiarTabla();
    respuestaTotal = listaDatos;
    informacion = info;
    cargarPagina(offset, limit);
}

function cargarPagina(offset, limit) {
    var listaPaginada = [];
    paginaActual = offset;
    numeroPaginas = Math.ceil(respuestaTotal.length / limit);
    var contador = 0;
    var romperFor = false;
    respuestaTotal.forEach(registro => {
        contador++;
        if (romperFor == false) {
            if ((contador > ((limit * offset) - limit)) && (contador <= (offset * limit))) {
                listaPaginada.push(registro);
            }
            if (contador == (offset * limit) || contador == respuestaTotal.length) {
                cargarTabla(listaPaginada, numeroPaginas)
                romperFor = true;
            }
        }       
    })
}

function limpiarTabla() {
    $("#tabla").remove();
    $("#paginacion").remove();
}

function cargarTabla(listaDatos, numeroPaginas) {
    if ($("#tabla").length > 0) {
        $("#tabla").remove();
        $("#paginacion").remove();
    }
    var tabla = $(`
                <table class="table table-hover table-bordered">
                    <thead>
                        <tr class="table-dark">
                          <th scope="col">#</th>
                                ${informacion.Titulos.map(titulo => {
                                    return `<th scope="col">${titulo}</th>`
                                }).join('')}
                          <th scope="col" style="min-width: 90px">Acciones</th>
                        </tr>
                  </thead>
                </table>`);
    tabla.attr({
        id: "tabla"
    });

    var tablaBody = $(`
                <tbody>
                </tbody>
            `);

    var contador = 0;
    listaDatos.forEach(registro => {
        contador++;
        tablaBody.append(`
                        <tr class='align-middle'>
                        <th scope="row">${contador}</th>
                        ${informacion.Campos.map(campo => {
                            return `<td class="${registro[campo] ? '' : 'text-center'}">${!registro[campo] || registro[campo] == '' ? '-' : campo == 'nombres' || campo == 'apellidos' ? formatearNombres(registro[campo]) : registro[campo]}</td >`
                        }).join('')}                       
                        <td><button ${PantallaActual == 'Asistencia' || PantallaActual == 'Prestamo' ? `onClick="editar(${registro.Id})"` : `onClick="seleccionarDatosFila(${registro.id})" data-bs-toggle="modal" data-bs-target="#modalEditar"`} id='botonEditar' class='btn-sm btn-primary' type='button' title='Editar estudiante'/><i class="bi bi-pencil-square"></i></button>
                        <button onClick="seleccionarIdFila(${registro.id})" data-bs-toggle="modal" data-bs-target="#modalConfirmarEliminacion" class='btn-sm btn-primary' type='button' title='Eliminar estudiante'/><i class="bi-trash"></i></button></td>
                    </tr>
                `);
    });

    tabla.append(tablaBody);

    var pagination = $('<nav id="paginacion"><ul class="pagination"></ul></nav>');
    for (var i = 0; i < numeroPaginas; i++) {
        var pageNum = i + 1;
        if (pageNum == 1) {
            var liPrev = $(`<li class="page-item ${paginaActual == 1 ? 'disabled' : ''}"></li>`);
            var a = $(`<a class="page-link" style="cursor: pointer;" onclick="cargarPagina(${paginaActual}-1, ${limit})"></a>`).text('Anterior');
            liPrev.append(a);
            pagination.find('ul').append(liPrev);
        }
        if (pageNum == 1 || pageNum == paginaActual || pageNum == numeroPaginas) {
            var li = $(`<li class="page-item ${pageNum == paginaActual ? 'active' : ''}"></li>`);
            var a = $(`<a class="page-link" style="cursor: pointer;" onclick="cargarPagina(${pageNum}, ${limit})"></a>`).text(pageNum);
            li.append(a);
        }
        pagination.find('ul').append(li);
        if (pageNum == numeroPaginas) {
            var liNext = $(`<li class="page-item ${paginaActual == numeroPaginas ? 'disabled' : ''}"></li>`);
            var a = $(`<a class="page-link" style="cursor: pointer;" onclick="cargarPagina(${paginaActual}+1, ${limit})"></a>`).text('Siguiente');
            liNext.append(a);
            pagination.find('ul').append(liNext);
        }
    }

    $("#contenedorTabla").append(tabla);
    $("#contenedorTabla").append(pagination);
}

function seleccionarDatosFila(id) {
    respuestaTotal.forEach(registro => {
        if (registro.id == id) {
            Info[PantallaActual][campos].forEach(campo => {
                Datos[PantallaActual][campo] = registro[campo]
            })
        }
    })

    Info[PantallaActual][campos].forEach(campo => {
        $(`#modalEditar${formatearNombres(campo)}`).val(Datos[PantallaActual][campo]);
    })

    idSeleccionado = id;
}

function seleccionarIdFila(id) {
    idSeleccionado = id;
}

function eliminarRegistro(pantalla) {
    var request = {
        Id: idSeleccionado
    }

    $.ajax({
        cache: false,
        type: "DELETE",
        url: `@Url.Action(Eliminar${pantalla}, "Operaciones")`,
        data: request,
        success: function (response) {
            if (response == null) {
                showToastError("Ha ocurrido un error");
                return;
            }
            consultar();
            $("#modalConfirmarEliminacion").modal("hide");
            showToastOk('Se eliminó el registro correctamente');
        },
        error: function (xhr) {
            showToastError(xhr.responseText);
            $("#modalConfirmarEliminacion").modal("hide");
            return false;
        },
        beforeSend: function () {
            mostrarCarga(true);
        },
        complete: function () {
            mostrarCarga(false);
        }
    });
}

function esEmail(email) {
    if (email == '' || email == null) {
        return true;
    }
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function showToastOk(mensaje){
    $.toast({
        heading: 'Success',
        text: mensaje,
        showHideTransition: 'slide',
        icon: 'success'
    })
}

function showToastError(mensaje) {
    $.toast({
        heading: 'Error',
        text: mensaje,
        showHideTransition: 'fade',
        icon: 'error'
    })
}

function showToastAlert(mensaje) {
    $.toast({
        heading: 'Warning',
        text: mensaje,
        showHideTransition: 'plain',
        icon: 'warning'
    })
}