
var form = $("#formReservaTx");

var toasty = new Toasty({
    transition: "slideDownFade",
    duration: 5, // calculated automatically.
    enableSounds: true,
    progressBar: true,
    autoClose: true,
    onShow: function (type) {
        console.log("a toast " + type + " message is shown!");
    },
    onHide: function (type) {
        console.log("the toast " + type + " message is hidden!");
    }
});

function loadInformeTx(btn) {
    const nro = document.getElementById("txtNroDocumento").value.trim(); // Elimina espacios en blanco
    if (!nro || nro.length < 8) {
        toasty.error("Número de documento no valido", 2500);
        return;
    }

    document.getElementById("divInformes").classList.add("d-none")
    document.getElementById("divInformesNo").classList.add("d-none");
    document.getElementById("txtNombres").classList.add("d-none");
    document.getElementById("txtNombres").value = "";

    actionLoagin(btn)


    var load = `<div class="text-center mt-5">
                        <div class="spinner-border" style="width: 4rem; height: 4rem" role="status">
                            <span class="visually-hidden">Loading...</span>
                        </div>
                        <h4 class="text-muted mt-2">Cargando...</h4>
                    </div>`;
    document.getElementById("divLoading").innerHTML = load;
    setTimeout(() => {

        $.ajax({
            url: urlInforme, // URL de la acción en el controlador
            type: 'GET', // Método de la solicitud
            data: { nroDoc: nro }, // Parámetro que deseas enviar
            success: function (response) {
                // Actualizar el DOM con los datos recibidos
                console.log(response);
                renderInformes(response)
            },
            error: function (xhr, status, error) {
                // Manejo de errores
                console.error('Error en la solicitud AJAX:', status, error);
                window.navigation.reload();
            },
            complete: function () {
                document.getElementById("divLoading").innerHTML = "";
            }
        });

    }, 1000);  
   
}

function renderInformes(data) {
    if (data.existe === "si") {
        // Mostrar datos personales en la tabla principal
        //const tablaInformes = document.querySelector("#tablaInformes tbody");
        //tablaInformes.innerHTML = `
        //            <tr>
        //                <td>${data.hc ?? "No disponible"}</td>
        //                <td>${data.nombre}</td>
        //                <td>${data.apellidoPaterno}</td>
        //                <td>${data.apellidoMaterno}</td>
        //            </tr>
        //        `;
        document.getElementById("txtNombres").classList.remove("d-none")
        document.getElementById("txtNombres").value = `${data.nombre} ${data.apellidoPaterno} ${data.apellidoMaterno}`;
        document.getElementById("txtHc").value = `${data.hc}`;

        // Mostrar lista de informes en la segunda tabla
        const tablaListaInformes = document.querySelector("#tablaListaInformes tbody");
        tablaListaInformes.innerHTML = ""; // Limpiar antes de agregar datos

        if (data.listInforme.length > 0) {
            data.listInforme.forEach(informe => {
                let terapia = informe.terapiaAsociada && informe.terapiaAsociada.trim() !== "" ? informe.terapiaAsociada : informe.evaluacion;
                tablaListaInformes.innerHTML += `
                            <tr>
                                <td>
                                 <label class="w-100 d-md-flex">
                                    <div class="col-md-9 d-flex ">
                                      <div class="col-10">
                                        <h6 class="ms-md-5 fs-4 fw-semibold">${terapia}</h6>
                                      </div>
                                      <div class="col-2 text-end">
                                        <input type="checkbox" name="OpcionesSeleccionadas" value="${informe.historialPacienteId}" class="fs-4 form-check-input warning check-outline" />
                                      </div>
                                    </div>
                                    <div class="col-md-3"></div>
                                </label>
                                </td>
                            </tr>
                        `;
            });           
            document.getElementById("divInformes").classList.remove("d-none")
            document.getElementById("divInformesNo").classList.add("d-none");
        } else {
            document.getElementById("divInformes").classList.add("d-none");
            document.getElementById("divInformesNo").classList.remove("d-none");
            tablaListaInformes.innerHTML = `<tr><td colspan="3">No hay informes disponibles</td></tr>`;
        }
    }
    else {
        toasty.error("Historial no encontrado.", 2500);
    }
}

function showToastError() {
    var validationSummary = $(".validation-summary-errors, .d-none, .text-danger");
    if (validationSummary.length > 0) {

        // Recorre y muestra los mensajes de error
        validationSummary.find("ul li").each(function () {
            var errorMessage = $(this).text();
            if (errorMessage) {
                toasty.error(errorMessage, 2500);
            }
        });
    }
    return true;
}

function sendFormData() {
    if (form.valid() && validarChecksTx()) { 
        form.submit();  
    } else {
        showToastError(); 
    }
}

function validarChecksTx() {
    const form = document.getElementById('formReservaTx');
    const checkboxes = form.querySelectorAll('input[type="checkbox"].form-check-input');

    var numCheched = 0
    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            numCheched = numCheched + 1
        }
    });
    // console.log(evaluaciones);
    if (numCheched > 0) {
        return true
    } else {
        showErrorString('Por favor marque un tratamiento para continuar');
        return false
    }
}

function showErrorString(message) {
    var container = document.querySelector("#formReservaTx > div.text-danger.d-none.validation-summary-errors");

    if (!container) {
        container = document.querySelector("#formReservaTx > div.text-danger.d-none.validation-summary-valid");
    }
    
    if (!container) return; // Si el contenedor no existe, salir

    var ul = container.querySelector("ul");

    if (!ul) {
        ul = document.createElement("ul");
        container.appendChild(ul); // Agregar la lista UL al contenedor
    }

    ul.innerHTML = ""; // Limpiar mensajes previos

    // Crear un nuevo elemento li con el mensaje de error
    var nuevoElementoLi = document.createElement("li");
    nuevoElementoLi.textContent = message;

    // Agregar el nuevo li al ul
    ul.appendChild(nuevoElementoLi);
}
