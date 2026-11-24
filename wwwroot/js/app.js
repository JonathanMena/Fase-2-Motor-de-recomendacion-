
        let estadoGlobal = null;
        let tipoSeleccionado = null;
        let categoriasSeleccionadas = [];
        function seleccionarTipo(elemento) {

    document.querySelectorAll('.tipo-card')
        .forEach(card => card.classList.remove('seleccionado'));

    elemento.classList.add('seleccionado');

    tipoSeleccionado = elemento.dataset.tipo;

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

        categoriasSeleccionadas =
            categoriasSeleccionadas.filter(c => c !== categoria);
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
function iniciarRecomendaciones() {

    const error = document.getElementById('preferencia-error');

    if (!tipoSeleccionado) {

        error.textContent =
            'Selecciona primero un tipo de contenido.';

        return;
    }


    if (categoriasSeleccionadas.length === 0) {

        error.textContent =
            'Selecciona al menos una categoría.';

        return;
    }


    const preferencias = {
        tipo: tipoSeleccionado,
        categorias: categoriasSeleccionadas
    };


    localStorage.setItem(
        'preferenciasRecomienda',
        JSON.stringify(preferencias)
    );


    document.querySelectorAll('.vista')
        .forEach(v => v.classList.remove('activa'));

    document.getElementById('vista-inicio')
        .classList.add('activa');


    document.querySelectorAll('.nav-btn')
        .forEach(b => b.classList.remove('active'));

    const botonInicio =
        document.querySelector('.nav-btn');

    if (botonInicio) {
        botonInicio.classList.add('active');
    }
}
        async function cargarEstado() {
            const res = await fetch('/api/lista');
            estadoGlobal = await res.json();
            renderizarUI();
        }

        function renderizarUI() {
            const { posicionActual, longitud, nodoAnterior, nodoActual, nodoSiguiente, listaCompleta } = estadoGlobal;

            // Actualizar visualizador de punteros (Ant <-> Actual <-> Sig)
            document.getElementById('ant-titulo').textContent = nodoAnterior ? nodoAnterior.titulo : 'NULL';
            document.getElementById('ant-cat').textContent = nodoAnterior ? `Similitud: ${nodoAnterior.puntajeSimilitud}%` : 'Cabeza (Sin predecesor)';

            document.getElementById('sig-titulo').textContent = nodoSiguiente ? nodoSiguiente.titulo : 'NULL';
            document.getElementById('sig-cat').textContent = nodoSiguiente ? `Similitud: ${nodoSiguiente.puntajeSimilitud}%` : 'Cola (Sin sucesor)';

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
                document.getElementById('act-mini-titulo').textContent = 'Lista Vacía (NULL)';
                document.getElementById('hero-titulo').textContent = 'SIN NODOS';
                document.getElementById('hero-sinopsis').textContent = 'La lista doblemente enlazada no contiene elementos.';
            }

            document.getElementById('lbl-posicion').textContent = `Método Actual(): Posición ${posicionActual}`;
            document.getElementById('lbl-longitud').textContent = `Método Longitud(): ${longitud} nodos`;
            const pct = longitud > 0 ? (posicionActual / longitud) * 100 : 0;
            document.getElementById('barra-progreso').style.width = `${pct}%`;

            // Renderizar Historial (Mockup Pág. 12)
            const contHist = document.getElementById('contenedor-historial');
            contHist.innerHTML = listaCompleta.map(item => `
                <div class="historial-item" style="${item.esActual ? 'border-color:var(--accent);' : ''}">
                    <div>
                        <div style="display:flex; align-items:center; gap:10px; margin-bottom:4px;">
                            <span style="color:var(--cyan); font-size:0.8rem; font-weight:700;">Nodo #${item.posicion}</span>
                            <span class="badge-estado">${item.dato.estado}</span>
                            ${item.esActual ? '<span class="badge-actual">Puntero Actual</span>' : ''}
                        </div>
                        <h3 style="font-size:1.2rem;">${item.dato.titulo}</h3>
                        <small style="color:var(--text-muted)">
                            ${item.dato.autorDirector} • ${item.dato.anio} • Similitud: ${item.dato.puntajeSimilitud}%
                        </small>
                    </div>
                    <div style="display:flex; gap:8px;">
                        <button class="btn btn-danger" onclick="eliminarPorPosicion(${item.posicion})">
                            Eliminar(${item.posicion})
                        </button>
                    </div>
                </div>
            `).join('');
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
        `Nodo ${estadoGlobal.posicionActual} de ${estadoGlobal.longitud}`;

    document.querySelectorAll('.vista')
        .forEach(v => v.classList.remove('activa'));

    document.getElementById('vista-detalle')
        .classList.add('activa');
}

function volverAPreferencias() {

    document.querySelectorAll('.vista')
        .forEach(v => v.classList.remove('activa'));

    document.getElementById('vista-preferencias')
        .classList.add('activa');

    document.querySelectorAll('.nav-btn')
        .forEach(b => b.classList.remove('active'));
}
function volverAlInicio() {

    document.querySelectorAll('.vista')
        .forEach(v => v.classList.remove('activa'));

    document.getElementById('vista-inicio')
        .classList.add('activa');

    document.querySelectorAll('.nav-btn')
        .forEach(b => b.classList.remove('active'));

    const botonInicio = document.querySelector('.nav-btn');

    if (botonInicio) {
        botonInicio.classList.add('active');
    }
}
        async function navegar(direccion) {
            const res = await fetch(`/api/lista/${direccion}`, { method: 'POST' });
            estadoGlobal = await res.json();
            renderizarUI();
        }

        async function marcarEstado(estado) {
            const res = await fetch(`/api/lista/estado/${encodeURIComponent(estado)}`, { method: 'POST' });
            estadoGlobal = await res.json();
            renderizarUI();
        }

        async function eliminarActual() {
            if (!estadoGlobal || estadoGlobal.posicionActual === 0) return;
            await eliminarPorPosicion(estadoGlobal.posicionActual);
        }

        async function eliminarPorPosicion(pos) {
            const res = await fetch(`/api/lista/eliminar/${pos}`, { method: 'DELETE' });
            estadoGlobal = await res.json();
            renderizarUI();
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
            const res = await fetch('/api/lista/insertar', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });
            estadoGlobal = await res.json();
            renderizarUI();
        }

        function cambiarVista(nombre, btn) {
            document.querySelectorAll('.vista').forEach(v => v.classList.remove('activa'));
            document.getElementById(`vista-${nombre}`).classList.add('activa');
            document.querySelectorAll('.nav-btn').forEach(b => b.classList.remove('active'));
            btn.classList.add('active');
        }

        cargarEstado();
