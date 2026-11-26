const USUARIO_ID = 1;

let estadoGlobal = null;
let tipoSeleccionado = null;
let categoriasSeleccionadas = [];

const categoriasPorTipo = {
    'Películas': [
        'Acción',
        'Comedia',
        'Drama',
        'Terror',
        'Fantasía',
        'Ciencia Ficción',
        'Romance',
        'Aventura'
    ],
    'Música': [
        'Pop',
        'Rock',
        'Electrónica',
        'Hip-Hop',
        'Clásica',
        'Jazz',
        'Indie',
        'Soundtrack'
    ],
    'Libros': [
        'Ciencia Ficción',
        'Fantasía',
        'Misterio',
        'Romance',
        'Terror',
        'Aventura',
        'Historia',
        'No Ficción'
    ]
};

function mostrarAvisoServidor(mensaje) {
    let aviso = document.getElementById('aviso-servidor');

    if (!aviso) {
        aviso = document.createElement('div');
        aviso.id = 'aviso-servidor';
        aviso.style.cssText = `
            background: rgba(239, 68, 68, 0.15);
            border: 1px solid #ef4444;
            color: #fecaca;
            padding: 12px 18px;
            border-radius: 10px;
            margin: 16px auto;
            max-width: 1200px;
            text-align: center;
            font-size: 0.9rem;
            font-weight: 600;
        `;
        const container = document.querySelector('.container');
        if (container) {
            container.prepend(aviso);
        }
    }

    aviso.textContent = mensaje;
    aviso.style.display = 'block';
}

function ocultarAvisoServidor() {
    const aviso = document.getElementById('aviso-servidor');
    if (aviso) {
        aviso.style.display = 'none';
    }
}

function actualizarNombreEncabezado(nombreUsuario) {
    const spanCuenta = document.querySelector('.user-pill span');
    if (spanCuenta && nombreUsuario) {
        spanCuenta.textContent = nombreUsuario;
    }
}

function renderizarCategorias(tipo) {
    const contenedor = document.getElementById('categorias-grid');
    categoriasSeleccionadas = [];

    const categorias = categoriasPorTipo[tipo] || [];

    if (categorias.length === 0) {
        contenedor.innerHTML = `
            <span style="color:var(--text-muted);">
                Primero selecciona un tipo de contenido.
            </span>
        `;
        return;
    }

    contenedor.innerHTML = categorias.map(categoria => `
        <button
            class="categoria-chip"
            data-categoria="${categoria}"
            onclick="toggleCategoria(this)">
            ${categoria}
        </button>
    `).join('');
}

function seleccionarTipo(elemento) {
    document.querySelectorAll('.tipo-card').forEach(card =>
        card.classList.remove('seleccionado')
    );

    elemento.classList.add('seleccionado');
    tipoSeleccionado = elemento.dataset.tipo;

    renderizarCategorias(tipoSeleccionado);
    actualizarResumenPreferencias();
}

function toggleCategoria(elemento) {
    const categoria = elemento.dataset.categoria;
    elemento.classList.toggle('seleccionado');

    if (elemento.classList.contains('seleccionado')) {
        if (!categoriasSeleccionadas.includes(categoria)) {
            categoriasSeleccionadas.push(categoria);
        }
    } else {
        categoriasSeleccionadas = categoriasSeleccionadas.filter(c => c !== categoria);
    }

    actualizarResumenPreferencias();
}

function actualizarResumenPreferencias() {
    document.getElementById('resumen-tipo').textContent =
        tipoSeleccionado || 'Ninguno';

    document.getElementById('resumen-categorias').textContent =
        categoriasSeleccionadas.length > 0
            ? categoriasSeleccionadas.join(', ')
            : 'Ninguna';

    document.getElementById('preferencia-error').textContent = '';
}

async function iniciarRecomendaciones() {
    const error = document.getElementById('preferencia-error');

    if (!tipoSeleccionado) {
        error.textContent = 'Selecciona primero un tipo de contenido.';
        return;
    }

    if (categoriasSeleccionadas.length === 0) {
        error.textContent = 'Selecciona al menos una categoría.';
        return;
    }

    const preferencias = {
        tipoPreferido: tipoSeleccionado,
        categoriasPreferidas: categoriasSeleccionadas
    };

    try {
        ocultarAvisoServidor();
        error.textContent = 'Generando recomendaciones...';

        const respuesta = await fetch(`/api/recomendaciones/calcular/${USUARIO_ID}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(preferencias)
        });

        if (!respuesta.ok) {
            const mensaje = await respuesta.text();
            console.error(mensaje);
            throw new Error('HTTP_ERROR');
        }

        estadoGlobal = await respuesta.json();

        if (!estadoGlobal || estadoGlobal.longitud === 0) {
            error.textContent =
                'Sin resultados: no encontramos contenido para estas preferencias. Prueba eligiendo otras categorías.';
            return;
        }

        renderizarUI();

        document.querySelectorAll('.vista').forEach(v => v.classList.remove('activa'));
        document.getElementById('vista-inicio').classList.add('activa');

        document.querySelectorAll('.nav-btn').forEach(b => b.classList.remove('active'));
        const botonInicio = document.querySelector('.nav-btn');
        if (botonInicio) {
            botonInicio.classList.add('active');
        }

        error.textContent = '';
    } catch (err) {
        console.error('Error generando recomendaciones:', err);
        const msg = err.message === 'HTTP_ERROR'
            ? 'No fue posible generar las recomendaciones. Intenta nuevamente.'
            : 'No se pudo conectar con el servidor. Verifica que esté encendido e intenta de nuevo.';
        error.textContent = msg;
        mostrarAvisoServidor(msg);
    }
}

async function cargarEstado() {
    try {
        ocultarAvisoServidor();
        const res = await fetch('/api/lista');
        if (!res.ok) throw new Error('HTTP_ERROR');
        estadoGlobal = await res.json();
        renderizarUI();
    } catch (err) {
        console.error('Error cargando estado:', err);
        mostrarAvisoServidor(
            'No se pudo conectar con el servidor. Verifica que el backend esté en ejecución.'
        );
    }
}

function renderizarUI() {
    if (!estadoGlobal) return;

    const { posicionActual, longitud, nodoAnterior, nodoActual, nodoSiguiente } = estadoGlobal;

    document.getElementById('ant-titulo').textContent = nodoAnterior ? nodoAnterior.titulo : 'NULL';
    document.getElementById('ant-cat').textContent = nodoAnterior
        ? `Similitud: ${nodoAnterior.puntajeSimilitud}%`
        : 'Cabeza (Sin predecesor)';

    document.getElementById('sig-titulo').textContent = nodoSiguiente ? nodoSiguiente.titulo : 'NULL';
    document.getElementById('sig-cat').textContent = nodoSiguiente
        ? `Similitud: ${nodoSiguiente.puntajeSimilitud}%`
        : 'Cola (Sin sucesor)';

    if (nodoActual) {
        document.getElementById('act-mini-titulo').textContent = nodoActual.titulo;
        document.getElementById('act-mini-sim').textContent = `★ Similitud: ${nodoActual.puntajeSimilitud}%`;

        document.getElementById('hero-categoria').textContent = `${nodoActual.tipoContenido} • ${nodoActual.categoria}`;
        document.getElementById('hero-titulo').textContent = nodoActual.titulo.toUpperCase();
        document.getElementById('hero-meta').textContent = `${nodoActual.autorDirector} • ${nodoActual.anio}`;
        document.getElementById('hero-sinopsis').textContent = nodoActual.sinopsis;
        document.getElementById('hero-similitud').textContent = `${nodoActual.puntajeSimilitud}%`;
        document.getElementById('hero-estado').textContent = `Estado: ${nodoActual.estado}`;
    } else {
        document.getElementById('act-mini-titulo').textContent = 'Sin resultados';
        document.getElementById('act-mini-sim').textContent = '';

        document.getElementById('hero-categoria').textContent = 'SIN RESULTADOS';
        document.getElementById('hero-titulo').textContent = 'NO HAY RECOMENDACIONES';
        document.getElementById('hero-meta').textContent = '';
        document.getElementById('hero-sinopsis').textContent =
            'No hay recomendaciones disponibles en este momento. Cambia tus preferencias para generar nuevas sugerencias.';
        document.getElementById('hero-similitud').textContent = '0%';
        document.getElementById('hero-estado').textContent = 'Estado: Sin contenido';
    }

    document.getElementById('lbl-posicion').textContent = `Posición ${posicionActual}`;
    document.getElementById('lbl-longitud').textContent = `Total: ${longitud}`;
    const pct = longitud > 0 ? (posicionActual / longitud) * 100 : 0;
    document.getElementById('barra-progreso').style.width = `${pct}%`;
}

async function cargarHistorialSql() {
    const contenedor = document.getElementById('contenedor-historial');

    contenedor.innerHTML = `
        <div class="info-card">
            Cargando historial...
        </div>
    `;

    try {
        ocultarAvisoServidor();
        const respuesta = await fetch(`/api/historial/${USUARIO_ID}`);

        if (!respuesta.ok) {
            throw new Error('HTTP_ERROR');
        }

        const historial = await respuesta.json();

        if (!historial || historial.length === 0) {
            contenedor.innerHTML = `
                <div class="info-card">
                    <h3>Tu historial está vacío</h3>
                    <p>
                        Todavía no has marcado contenidos como Me gusta, Guardado o Descartado.
                    </p>
                </div>
            `;
            return;
        }

        contenedor.innerHTML = historial.map(item => {
            let icono = '•';
            let textoEstado = item.tipoInteraccion;

            if (item.tipoInteraccion === 'MeGusta') {
                icono = '♥';
                textoEstado = 'Me gusta';
            } else if (item.tipoInteraccion === 'Guardado') {
                icono = '🔖';
                textoEstado = 'Guardado';
            } else if (item.tipoInteraccion === 'Descartado') {
                icono = '✕';
                textoEstado = 'Descartado';
            }

            const fecha = new Date(item.fecha).toLocaleString('es-SV');

            return `
                <div class="historial-item">
                    <div>
                        <div style="
                            display:flex;
                            align-items:center;
                            gap:10px;
                            margin-bottom:7px;
                            flex-wrap:wrap;
                        ">
                            <span style="
                                color:var(--cyan);
                                font-size:.75rem;
                                font-weight:700;
                            ">
                                ${item.tipoContenido}
                            </span>

                            <span class="badge-estado">
                                ${icono} ${textoEstado}
                            </span>
                        </div>

                        <h3 style="font-size:1.2rem; margin-bottom:5px;">
                            ${item.titulo}
                        </h3>

                        <div style="color:var(--text-muted); font-size:.82rem;">
                            ${item.autorDirector} • ${item.anio} • ${item.categoria}
                        </div>

                        <div style="color:var(--text-muted); font-size:.7rem; margin-top:7px;">
                            ${fecha}
                        </div>
                    </div>
                </div>
            `;
        }).join('');
    } catch (error) {
        console.error('Error cargando historial:', error);
        const esErrorServidor = error.message !== 'HTTP_ERROR';
        contenedor.innerHTML = `
            <div class="info-card">
                <h3>${esErrorServidor ? 'Servidor desconectado' : 'No se pudo cargar el historial'}</h3>
                <p>
                    ${esErrorServidor
                        ? 'No se pudo conectar con el servidor. Verifica que esté encendido e intenta nuevamente.'
                        : 'Ocurrió un problema al consultar las interacciones almacenadas.'}
                </p>
            </div>
        `;
    }
}

function abrirDetalle() {
    if (!estadoGlobal || !estadoGlobal.nodoActual) {
        return;
    }

    const nodo = estadoGlobal.nodoActual;

    document.getElementById('detalle-categoria').textContent =
        `${nodo.tipoContenido} • ${nodo.categoria}`;

    document.getElementById('detalle-titulo').textContent =
        nodo.titulo.toUpperCase();

    document.getElementById('detalle-meta').textContent =
        `${nodo.autorDirector} • ${nodo.anio}`;

    document.getElementById('detalle-sinopsis').textContent =
        nodo.sinopsis;

    document.getElementById('detalle-similitud').textContent =
        `${nodo.puntajeSimilitud}%`;

    document.getElementById('detalle-barra').style.width =
        `${nodo.puntajeSimilitud}%`;

    document.getElementById('detalle-razon-categoria').textContent =
        nodo.categoria;

    document.getElementById('detalle-razon-similitud').textContent =
        `${nodo.puntajeSimilitud}%`;

    document.getElementById('detalle-posicion').textContent =
        `Recomendación ${estadoGlobal.posicionActual} de ${estadoGlobal.longitud}`;

    document.querySelectorAll('.vista').forEach(v => v.classList.remove('activa'));
    document.getElementById('vista-detalle').classList.add('activa');
}

function volverAPreferencias() {
    document.querySelectorAll('.vista').forEach(v => v.classList.remove('activa'));
    document.getElementById('vista-preferencias').classList.add('activa');
    document.querySelectorAll('.nav-btn').forEach(b => b.classList.remove('active'));
}

function volverAlInicio() {
    document.querySelectorAll('.vista').forEach(v => v.classList.remove('activa'));
    document.getElementById('vista-inicio').classList.add('activa');
    document.querySelectorAll('.nav-btn').forEach(b => b.classList.remove('active'));

    const botonInicio = document.querySelector('.nav-btn');
    if (botonInicio) {
        botonInicio.classList.add('active');
    }
}

async function navegar(direccion) {
    try {
        ocultarAvisoServidor();
        const res = await fetch(`/api/lista/${direccion}`, { method: 'POST' });
        if (!res.ok) throw new Error('HTTP_ERROR');
        estadoGlobal = await res.json();
        renderizarUI();
    } catch (err) {
        console.error('Error navegando:', err);
        mostrarAvisoServidor(
            'No se pudo conectar con el servidor. Verifica que esté encendido.'
        );
    }
}

async function marcarEstado(estado) {
    try {
        ocultarAvisoServidor();
        const res = await fetch(`/api/lista/estado/${encodeURIComponent(estado)}`, { method: 'POST' });
        if (!res.ok) throw new Error('HTTP_ERROR');
        estadoGlobal = await res.json();
        renderizarUI();
    } catch (err) {
        console.error('Error actualizando estado:', err);
        mostrarAvisoServidor(
            'No se pudo guardar tu interacción. Verifica que el servidor esté encendido.'
        );
    }
}

async function eliminarActual() {
    if (!estadoGlobal || estadoGlobal.posicionActual === 0) return;
    await eliminarPorPosicion(estadoGlobal.posicionActual);
}

async function eliminarPorPosicion(pos) {
    try {
        ocultarAvisoServidor();
        const res = await fetch(`/api/lista/eliminar/${pos}`, { method: 'DELETE' });
        if (!res.ok) throw new Error('HTTP_ERROR');
        estadoGlobal = await res.json();
        renderizarUI();
    } catch (err) {
        console.error('Error eliminando recomendación:', err);
        mostrarAvisoServidor(
            'No se pudo descartar la recomendación. Verifica que el servidor esté encendido.'
        );
    }
}

async function insertarNodo() {
    const body = {
        titulo: document.getElementById('ins-titulo').value || 'Nuevo Contenido',
        tipoContenido: 'Películas',
        categoria: 'Ciencia Ficción',
        autorDirector: document.getElementById('ins-autor').value || 'Director Invitado',
        anio: 2026,
        sinopsis: 'Recomendación insertada dinámicamente en la Lista Doblemente Enlazada.',
        puntajeSimilitud: parseInt(document.getElementById('ins-sim').value) || 95,
        posicion: parseInt(document.getElementById('ins-pos').value) || 1
    };

    try {
        ocultarAvisoServidor();
        const res = await fetch('/api/lista/insertar', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        });
        if (!res.ok) throw new Error('HTTP_ERROR');
        estadoGlobal = await res.json();
        renderizarUI();
    } catch (err) {
        console.error('Error insertando recomendación:', err);
        mostrarAvisoServidor(
            'No se pudo insertar la recomendación. Verifica que el servidor esté encendido.'
        );
    }
}

async function cargarPerfil() {
    const nombre = document.getElementById('perfil-nombre');
    const email = document.getElementById('perfil-email');
    const contenedor = document.getElementById('perfil-afinidades');

    try {
        ocultarAvisoServidor();
        const respuesta = await fetch(`/api/perfil/${USUARIO_ID}`);

        if (!respuesta.ok) {
            throw new Error('HTTP_ERROR');
        }

        const perfil = await respuesta.json();

        actualizarNombreEncabezado(perfil.nombre);

        if (nombre) nombre.textContent = perfil.nombre;
        if (email) email.textContent = perfil.email;

        if (!contenedor) return;

        if (!perfil.afinidades || perfil.afinidades.length === 0) {
            contenedor.innerHTML = `
                <p>
                    Todavía no existen suficientes preferencias o interacciones para calcular afinidades.
                </p>
            `;
            return;
        }

        contenedor.innerHTML = perfil.afinidades.map(afinidad => `
            <div>
                <div style="
                    display:flex;
                    justify-content:space-between;
                    margin-bottom:6px;
                ">
                    <span>● ${afinidad.categoria}</span>
                    <span>${afinidad.puntaje}%</span>
                </div>

                <div class="progress-bar">
                    <div class="progress-fill" style="width:${afinidad.puntaje}%;"></div>
                </div>
            </div>
        `).join('');
    } catch (error) {
        console.error('Error cargando perfil:', error);
        const esErrorServidor = error.message !== 'HTTP_ERROR';

        if (nombre) {
            nombre.textContent = esErrorServidor
                ? 'Servidor desconectado'
                : 'No se pudo cargar el perfil';
        }
        if (email) email.textContent = '';
        if (contenedor) {
            contenedor.innerHTML = `
                <p>
                    ${esErrorServidor
                        ? 'No se pudo conectar con el servidor. Verifica que esté encendido e intenta de nuevo.'
                        : 'Ocurrió un error al consultar la información del usuario.'}
                </p>
            `;
        }
    }
}

async function cambiarVista(nombre, btn) {
    document.querySelectorAll('.vista').forEach(v => v.classList.remove('activa'));
    document.getElementById(`vista-${nombre}`).classList.add('activa');

    document.querySelectorAll('.nav-btn').forEach(b => b.classList.remove('active'));
    btn.classList.add('active');

    if (nombre === 'historial') {
        await cargarHistorialSql();
    }

    if (nombre === 'perfil') {
        await cargarPerfil();
    }
}

cargarEstado();
cargarPerfil();