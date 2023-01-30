
var offset = 1;
var limit = 5;
var respuestaTotal = [];
var informacion = new Object();
var paginaActual = 0;
var numeroPaginas;
var idSeleccionado;
var moduloSeleccionado;
var esPantallaClase = false;

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
    Prestamo: 'Prestamo',
    PrestamoLA: 'PrestamoLA'
}
var Info = {
    Estudiante: {
        Titulos: ["Nombres", "Apellidos", "Matrícula", "Email", "Dirección", "Carrera", "Módulo"],
        Campos: ["nombres", "apellidos", "matricula", "email", "direccion", "carrera", "modulo"]
    },
    Asistencia: {
        Titulos: ["Nombres", "Apellidos", "Matrícula", "Fecha", "Estado", "Carrera", "Módulo"],
        Campos: ["nombres", "apellidos", "matricula", "fecha", "estadoAsistencia", "carrera", "modulo"]
    },
    Carrera: {
        Titulos: ["Nombre", "Módulo"],
        Campos: ["nombre", "modulo"]
    },
    Modulo: {
        Titulos: ["Nombre", "Descripción", "Periodo Académico", "Días de Clase"],
        Campos: ["nombre", "descripcion", "periodoAcademico", "diasClase"]
    },
    Inventario: {
        Titulos: ["Nombre", "Descripción", "Cantidad Disponible", "Cantidad Total"],
        Campos: ["nombre", "descripcion", "cantidadDisponible", "cantidadTotal"]
    },
    Item: {
        Titulos: ["Código RFID", "Estado", "Inventario"],
        Campos: ["rfid", "estadoItem", "inventario"]
    },
    Prestamo: {
        Titulos: ["Item", "Inventario", "Fecha Prestado", "Fecha Devuelto", "Módulo", "Estado"],
        Campos: ["item", "inventario", "fechaPrestado", "fechaDevuelto", "modulo", "estadoDevolucion"]
    },
    PrestamoLA: {
        Titulos: ["Item", "Inventario", "Fecha Prestado", "Fecha Devuelto", "Nombres", "Apellidos", "Matrícula", "Estado"],
        Campos: ["item", "inventario", "fechaPrestado", "fechaDevuelto", "nombres", "apellidos", "matricula", "estadoDevolucion"]
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
    },
    Carrera: {
        id: null,
        nombre: null,
        modulo: null
    },
    Modulo: {
        id: null,
        nombre: null,
        descripcion: null,
        periodoAcademico: null,
        diasClase: null
    },
    Inventario: {
        id: null,
        nombre: null,
        descripcion: null,
        cantidadDisponible: null,
        cantidadTotal: null
    },
    Item: {
        id: null,
        rfid: null,
        estadoItem: null,
        inventario: null
    },
    Prestamo: {
        id: null,
        item: null,
        inventario: null,
        fechaPrestado: null,
        fechaDevuelto: null,
        estadoDevolucion: null,
        modulo: null
    },
    PrestamoLA: {
        id: null,
        item: null,
        inventario: null,
        fechaPrestado: null,
        fechaDevuelto: null,
        estadoDevolucion: null,
        nombres: null,
        apellidos: null,
        matricula: null
    }
}
var CamposQuery = {
    Estudiante: ["nombres", "apellidos", "matricula", "email", "direccion", "carrera", "modulo"],
    Asistencia: ["nombres", "apellidos", "matricula", "direccion", "carrera", "modulo"],
    Carrera: ["nombre", "modulo"],
    Modulo: ["nombre", "periodoAcademico"],
    Inventario: ["nombre"],
    Item: ["rfid", "inventario"],
    Prestamo: ["modulo", "matricula"]
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
    },
    Asistencia: {
        nombres: null,
        apellidos: null,
        matricula: null,
        direccion: null,
        carrera: null,
        modulo: null
    },
    Carrera: {
        nombre: null,
        modulo: null
    },
    Modulo: {
        nombre: null,
        periodoAcademico: null
    },
    Inventario: {
        nombre: null
    },
    Item: {
        rfid: null,
        inventario: null
    },
    Prestamo: {
        modulo: null,
        matricula: null
    }
}
var CamposRequest = {
    Estudiante: ["Nombres", "Apellidos", "Matricula", "Email", "Direccion", "Carrera", "Modulo"],
    Asistencia: ["Matricula", "Fecha", "EsAsistencia", "Modulo"],
    Carrera: ["Nombre", "Modulo"],
    Modulo: ["Nombre", "Descripcion", "PeriodoAcademico", "DiasClase"],
    Inventario: ["Nombre", "Descripcion", "CantidadDisponible", "CantidadTotal"],
    Item: ["Rfid", "Inventario", "EstaDisponible"],
    Prestamo: ["Rfid", "Modulo", "Matricula"]
}
var RequestEditar = {
    Estudiante: {
        Id: null,
        Nombres: null,
        Apellidos: null,
        Matricula: null,
        Email: null,
        Direccion: null,
        Carrera: null,
        Modulo: null
    },
    Carrera: {
        Id: null,
        Nombre: null,
        Modulo: null
    },
    Modulo: {
        Id: null,
        Nombre: null,
        Descripcion: null,
        PeriodoAcademico: null,
        DiasClase: null
    },
    Inventario: {
        Id: null,
        Nombre: null,
        Descripcion: null,
        CantidadDisponible: null,
        CantidadTotal: null
    },
    Item: {
        Id: null,
        Rfid: null,
        EstaDisponible: null,
        Inventario: null
    }
}
var RequestRegistrar = {
    Estudiante: {
        Nombres: null,
        Apellidos: null,
        Matricula: null,
        Email: null,
        Direccion: null,
        Carrera: null,
        Modulo: null
    },
    Asistencia: {
        Matricula: null,
        Fecha: null,
        EsAsistencia: null,
        Modulo: null
    },
    Carrera: {
        Nombre: null,
        Modulo: null
    },
    Modulo: {
        Nombre: null,
        Descripcion: null,
        PeriodoAcademico: null,
        DiasClase: null
    },
    Inventario: {
        Nombre: null,
        Descripcion: null,
        CantidadTotal: null
    },
    Item: {
        Rfid: null,
        Inventario: null
    },
    Prestamo: {
        Rfid: null,
        Modulo: null,
        Matricula: null
    }
}

function prepararPantalla(pantalla) {
    PantallaActual = pantalla;
    generarModalEliminacion(pantalla);
}

function generarModalEliminacion() {
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
                        <button type="button" class="btn btn-primary" onclick="eliminar()">Confirmar</button>
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
    CamposQuery[PantallaActual].forEach(campo => {
        Query[PantallaActual][campo] = $(`#${campo}`).val() != '' && $(`#${campo}`).val() != "0" ? $(`#${campo}`).val() : null;
    })
    return Query;
}

function obtenerEditarRequest() {
    CamposRequest[PantallaActual].forEach(campo => {
        RequestEditar[PantallaActual][campo] = $(`#modalEditar${campo}`).val();
    })
    RequestEditar[PantallaActual].Id = idSeleccionado;
    return RequestEditar;
}

function obtenerRegistrarRequest() {
    CamposRequest[PantallaActual].forEach(campo => {
        RequestRegistrar[PantallaActual][campo] = $(`#modalRegistrar${campo}`).val();
    })
    return RequestRegistrar;
}

function crearTabla(listaDatos, info, recarga) {
    limpiarTabla();
    respuestaTotal = listaDatos;
    informacion = info;
    if (recarga == true) {
        cargarPagina(paginaActual, limit);
    } else {
        cargarPagina(offset, limit);
    }    
}

function cargarPagina(offset, limit) {
    var listaPaginada = [];
    if (offset == 0) {
        return
    }
    if (paginaActual <= 1) {
        paginaActual = 1;
    }
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
    if (listaDatos.length == 0) {
        cargarPagina(paginaActual - 1, limit)
        return;
    }
    var tabla = $(`
                <table class="table table-hover table-bordered">
                    <thead>
                        <tr class="table-dark">
                          <th scope="col">#</th>
                                ${informacion.Titulos.map(titulo => {
                                    return `<th scope="col">${titulo}</th>`
                                }).join('')}
                          <th scope="col" style="min-width: 90px; width: 90px">Acciones</th>
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
                            return `<td class="${registro[campo] ? '' : 'text-center'}">${!registro[campo] || registro[campo] == '' ? '-' : campo == 'nombres' || campo == 'apellidos' ? formatearNombres(registro[campo]) : registro[campo]
                        }</td >`
                        }).join('')}
                        ${registro.nombre != 'General' ? ` <td id='acciones${registro.nombre}'><button ${PantallaActual == 'Asistencia' || PantallaActual == 'Prestamo' || PantallaActual == 'PrestamoLA' ? `onClick="editar(${registro.id})"` : `onClick="seleccionarDatosFila(${registro.id})" data-bs-toggle="modal" data-bs-target="#modalEditar"`} id='botonEditar' class='btn-sm btn-primary' type='button' title='Editar registro'/><i class="bi bi-pencil-square"></i></button>
                        <button onClick="seleccionarIdFila(${registro.id})" data-bs-toggle="modal" data-bs-target="#modalConfirmarEliminacion" class='btn-sm btn-primary' type='button' title='Eliminar registro'/><i class="bi-trash"></i></button></td>` : `<td></td>` }
                       
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
    if (PantallaActual == Pantalla.Modulo) {
        respuestaTotal.forEach(registro => {
            if (registro.id == id) {
                var periodoAcademico = registro.periodoAcademico.split(/-| /)
                $("#modalEditarPeriodoAcademicoInicio").val(periodoAcademico[0])
                $("#modalEditarPeriodoAcademicoFin").val(periodoAcademico[1])
                $("#modalEditarPeriodo").val(periodoAcademico[2])
                $("#modalEditarNombre").val(registro.nombre)
                $("#modalEditarDescripcion").val(registro.descripcion)
                moduloSeleccionado = registro.nombre;

                if (!registro.diasClase) {
                    $("#modalEditarLunes").prop('checked', false);
                    $("#modalEditarMartes").prop('checked', false);
                    $("#modalEditarMiercoles").prop('checked', false);
                    $("#modalEditarJueves").prop('checked', false);
                    $("#modalEditarViernes").prop('checked', false);
                    $("#modalEditarSabado").prop('checked', false);
                    $("#modalEditarDomingo").prop('checked', false);
                    return
                }

                if (registro.diasClase.includes('L')) {
                    $("#modalEditarLunes").prop('checked', true);
                }
                if (registro.diasClase.includes('M')) {
                    $("#modalEditarMartes").prop('checked', true);
                }
                if (registro.diasClase.includes('X')) {
                    $("#modalEditarMiercoles").prop('checked', true);
                }
                if (registro.diasClase.includes('J')) {
                    $("#modalEditarJueves").prop('checked', true);
                }
                if (registro.diasClase.includes('V')) {
                    $("#modalEditarViernes").prop('checked', true);
                }
                if (registro.diasClase.includes('S')) {
                    $("#modalEditarSabado").prop('checked', true);
                }
                if (registro.diasClase.includes('D')) {
                    $("#modalEditarDomingo").prop('checked', true);
                }
            }
        })
        idSeleccionado = id;
        return;
    }

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

function esNumero(n) {
    if (typeof (n) === 'number' || n instanceof Number) {
        return true
    }
    return false
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