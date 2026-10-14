# 🎬 Motor de Recomendación de Contenido

**Asignatura:** Programación con Estructuras de Datos (`PED941 G01T`)  
**Proyecto:** Fase 2 — Implementación del Motor de Recomendación de Contenido con Estructuras de Datos Dinámicas

---

## 👥 Integrantes del Equipo

| Nombre Completo | Carnet |
| :--- | :--- |
| **Calvin Jonathan Mena Dimas** | `MD232400` |
| **Oscar Alberto López Martínez** | `LM230296` |
| **Diego Alberto Sandoval Rivera** | `SR200029` |
| **Fernando Alexis Alvarado Calderón** | `AC251546` |

---

## 📌 Descripción del Proyecto

**Recomienda+** es un motor de recomendación de contenido multimedia diseñado para solucionar la sobrecarga de información mediante el uso de **Estructuras de Datos Dinámicas Lineales** programadas desde cero en **C#** con clases propias autorreferenciadas.

El núcleo del sistema utiliza una **Lista Doblemente Enlazada (`ListaDoble`)** donde cada unidad base (`Nodo`) está compuesta por tres partes fundamentales:
1. **`Anterior` (Puntero al nodo predecesor):** Permite retroceder en el historial de recomendaciones en tiempo constante $O(1)$.
2. **`Dato` (Objeto `Recomendacion`):** Almacena los atributos del contenido (ID, Título, Tipo, Categoría, Director/Autor, Año, Sinopsis, Porcentaje de Similitud y Estado).
3. **`Siguiente` (Puntero al nodo sucesor):** Permite avanzar linealmente hacia la siguiente recomendación o identificar el final de la lista (`NULL`).

```text
   Cabeza (primerNodo)                                      Cola (ultimoNodo)
           |                                                        |
           v                                                        v
NULL <-- [Ant | DATO | Sig] <==> [Ant | DATO | Sig] <==> [Ant | DATO | Sig] --> NULL
```

---

## 🛠️ Requisitos Previos (Programas Necesarios)

Para descargar, compilar y ejecutar este proyecto en cualquier computadora, asegúrate de tener instalados los siguientes programas:

1. **.NET SDK (Versión 8.0, 9.0 o 10.0):**
   * Descárgalo oficialmente desde: [https://aka.ms/dotnet/download](https://aka.ms/dotnet/download)
   * También puedes instalarlo desde PowerShell en Windows con el comando:
     ```powershell
     winget install Microsoft.DotNet.SDK.8
     ```
   * Para verificar que está correctamente instalado, abre una terminal y ejecuta:
     ```powershell
     dotnet --version
     ```
2. **Git (Opcional, para clonar el repositorio):**
   * Descárgalo desde: [https://git-scm.com/](https://git-scm.com/)
3. **Editor de Código (Opcional):**
   * [Visual Studio Code](https://code.visualstudio.com/) (con la extensión *C# Dev Kit*) o Visual Studio Community.
4. **Navegador Web Moderno:**
   * Google Chrome, Microsoft Edge, Brave o Mozilla Firefox para visualizar la interfaz gráfica interactiva.

---

## 🚀 Instrucciones y Comandos para Ejecutar el Proyecto

Sigue estos pasos en tu terminal (**PowerShell**, **CMD** o la terminal integrada de **VS Code**):

### 1. Clonar el repositorio
```powershell
git clone https://github.com/JonathanMena/Fase-2-Motor-de-recomendacion-.git
```

### 2. Entrar a la carpeta del proyecto
```powershell
cd Fase-2-Motor-de-recomendacion-
```

### 3. Restaurar dependencias y compilar el proyecto
```powershell
dotnet build
```

### 4. Iniciar el servidor y el motor de recomendación
```powershell
dotnet run
```

### 5. Abrir la aplicación en el navegador
Una vez que la consola indique que el servidor está activo (`Now listening on: http://localhost:5150`), abre tu navegador web e ingresa a la siguiente dirección:

👉 **[http://localhost:5150](http://localhost:5150)**

*(Para detener la ejecución del servidor en cualquier momento, presiona `Ctrl + C` en la terminal).*

---

## ⚙️ Operaciones Implementadas en la Estructura `ListaDoble`

El motor respeta los algoritmos fundamentales de manipulación de punteros protegiendo `primerNodo` (Cabeza) y `ultimoNodo` (Cola), utilizando apuntadores temporales (`punteroAuxiliar` y `punteroActual`) para sus operaciones:

* **`Inicio()`:** Sitúa el puntero actual directamente en la cabeza (`primerNodo`) de la lista.
* **`Actual()`:** Devuelve el índice de la posición actual dentro de la lista.
* **`Siguiente()`:** Avanza el puntero actual al nodo sucesor (`punteroActual.Siguiente`).
* **`Anterior()`:** Retrocede el puntero actual al nodo predecesor (`punteroActual.Anterior`).
* **`Obtener(posición)`:** Recorre la lista con un puntero auxiliar temporal y devuelve el objeto `Recomendacion` ubicado en la posición indicada.
* **`Añadir(elemento)`:** Inserta un nuevo nodo directamente al final de la lista (`ultimoNodo` / Cola) en $O(1)$.
* **`Insertar(elemento, posición)`:** Inserta una recomendación prioritaria en cualquier posición específica ($1 \dots N$), reajustando los punteros `Anterior` y `Siguiente`.
* **`Eliminar(posición)`:** Suprime el nodo de la posición indicada (cuando el usuario descarta un contenido), reenlazando su nodo predecesor con su sucesor en memoria dinámica.
* **`Longitud()`:** Devuelve el total de nodos activos en la estructura.

---

## 📂 Estructura de Archivos del Repositorio

```text
📦 Fase-2-Motor-de-recomendacion-
 ┣ 📂 wwwroot/
 ┃ ┗ 📜 index.html               # Interfaz gráfica (Mockups interactivos de Recomienda+)
 ┣ 📜 Recomendacion.cs           # Clase del objeto de datos que almacena cada Nodo
 ┣ 📜 Nodo.cs                    # Clase autorreferenciada (Anterior, Dato, Siguiente)
 ┣ 📜 ListaDoble.cs              # Estructura de datos dinámica y métodos de punteros
 ┣ 📜 BaseDeDatos.cs             # Persistencia automática en archivo JSON hacia ListaDoble
 ┣ 📜 Program.cs                 # Servidor web ASP.NET Core y endpoints de la API
 ┣ 📜 bd_recomienda.json         # Catálogo persistente de contenidos y estados
 ┣ 📜 MotorRecomendacion.csproj  # Configuración del proyecto .NET
 ┗ 📜 README.md                  # Documentación y guía de ejecución
```

---

## 📚 Referencias Bibliográficas

* Cairó, O., & Guarati, S. (2006). Listas. En *Estructuras de datos* (pp. 141–176). México D. F., México: McGraw-Hill.
* Dirección de Educación a Distancia [UDB Virtual]. (s.f.). *Unidad 1: Estructuras de datos dinámicas lineales. Programación con estructuras de datos (PED941 G01T)*. San Salvador, El Salvador: Universidad Don Bosco.
* Joyanes, A. (2008). Estructuras dinámicas lineales de datos (pilas, colas y listas enlazadas). En *Fundamentos de programación. Algoritmos, estructura de datos y objetos* (pp. 429–477). Madrid, España: McGraw-Hill.
