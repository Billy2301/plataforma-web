
$(document).ready(function () {
    // Configuración de validación para un formulario con la clase "validation-wizard"
    $(".validation-wizard").validate({

                ignore: "input[type=hidden], .ignore",  // Ignora los campos ocultos y aquellos con la clase ".ignore"
                errorClass: "text-danger",      // Clase CSS que se agregará a los elementos con errores de validación
                successClass: "text-success",   // Clase CSS que se agregará a los elementos cuando la validación sea exitosa
                highlight: function (element, errorClass) { // Función que se ejecuta cuando se detecta un error en un campo
                    console.log(element);      // Muestra el elemento con error en la consola
                    console.log(errorClass);    // Muestra el nombre de la clase de error en la consola
                    $(element).removeClass(errorClass);  // Remueve la clase de error del elemento
                },
                unhighlight: function (element, errorClass) {   // Función que se ejecuta cuando se corrige un error en un campo
                    console.log(element);       // Muestra el elemento corregido en la consola
                    console.log(errorClass);     // Muestra el nombre de la clase de error en la consola
                    $(element).removeClass(errorClass);   // Remueve la clase de error del elemento
                },
                errorPlacement: function (error, element) { // Función que define dónde colocar el mensaje de error para cada elemento con error
                    console.log(error);        // Muestra el mensaje de error en la consola
                    console.log(element);      // Muestra el elemento que causó el error en la consola
                    error.insertAfter(element);  // Inserta el mensaje de error inmediatamente después del elemento
                },

                rules: {

                },
                
            });

    // Evento para todos los campos de entrada (input) con la clase "form-control"
    document.querySelectorAll('input.form-control').forEach(input => {
                // Evento "blur" (cuando el usuario sale del campo)
                input.addEventListener('blur', function () {
                    this.value = this.value.trim();  // Elimina espacios en blanco al inicio y final del valor del campo
                });
            });

    const btnEnviar = document.querySelector("#formTriaje > div.actions.clearfix > ul > li:nth-child(3) > a");
    btnEnviar.setAttribute("onclick", "actionLoagin(this)");

    checkEdad();
    showColegioGrado(document.getElementById('Tab1_NivelEducacion'))
    verifyTipoDoc();
    verifyTipoDocCont();
    verifyGrado();
    actualizarTab();
    mackerLiBack();
    hidenButtonSave();
    disabledDocumento();
});

// Agrega un nuevo método de validación personalizado llamado "lettersonly"
$.validator.addMethod("lettersonly", function (value, element) {
            return /^[a-zA-Z]+$/.test(value);  // Retorna verdadero solo si el valor contiene solo letras (mayúsculas y minúsculas).
        }, "Este campo debe contener solo letras.");

// Selecciona el formulario con la clase "validation-wizard" y lo muestra
var form = $(".validation-wizard").show();
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
// Configura un formulario multistep (paso a paso) en el elemento con la clase "validation-wizard"
$(".validation-wizard").steps({

            headerTag: "h6", // Etiqueta para cada encabezado de paso
            bodyTag: "section", // Etiqueta para el contenido de cada paso
            transitionEffect: "fade", // Efecto de transición al cambiar de un paso a otro
            titleTemplate: '<span class="step">#index#</span> #title#', // Plantilla de título para cada paso, donde #index# es el número de paso y #title# el título del paso
            labels: { 
                finish: "Siguiente", // Texto del botón final para enviar el formulario
                next: "Siguiente",            // Texto del botón para avanzar al siguiente paso
                previous: "Anterior",         // Texto del botón para regresar al paso anterior
                current: ''                   // Texto para el paso actual, vacío aquí
            },
            startIndex: 0,

            // Función que se ejecuta al intentar avanzar al siguiente paso o retroceder
            onStepChanging: function (event, currentIndex, newIndex) {
                // Limpia la lista de elementos de error dentro de los pasos
                $("div.content.clearfix > div > ul").find("li").remove();
                // Lógica de validación para el paso actual
                if (currentIndex == 0) {
                    if (
                        !form.valid() || // Si el formulario no es válido
                        !isValidNroDocumento() || // Valida documento
                        !isValidNroDocumentoCont() || // Valida otro documento
                        !isValidFechaNacimiento() // Valida la fecha
                    ) {
                        //toastr.error("Por favor, corrige los errores en el formulario antes de continuar.", "Validación fallida");
                        showToastError();
                        return false; // Previene avanzar al siguiente paso
                    }
                }

                if (currentIndex == 1) {
                    if (!(
                        currentIndex > newIndex ||
                        (
                            currentIndex < newIndex &&
                            (form.find(".body:eq(" + newIndex + ") label.error").remove(),
                                form.find(".body:eq(" + newIndex + ") .error").removeClass("error")),
                            (form.validate().settings.ignore = ":disabled,:hidden"),
                            form.valid() // Valida el formulario
                        ))){
                            showToastError();
                            return false;
                        }
                }

                return (
                    currentIndex > newIndex ||
                    (
                        currentIndex < newIndex &&
                        (form.find(".body:eq(" + newIndex + ") label.error").remove(),
                            form.find(".body:eq(" + newIndex + ") .error").removeClass("error")),
                        (form.validate().settings.ignore = ":disabled,:hidden"),
                        form.valid() // Valida el formulario
                    )
                );
            },
            // Función que se ejecuta antes de enviar el formulario en el último paso
            onFinishing: function (event, currentIndex) {
                // Valida la última pestaña antes de enviar
                if (!validarTab3() || !form.valid()) {
                    showToastError();
                    return false; // Previene el envío si hay errores
                }
                return true; // Permite finalizar
            },
            // Función que se ejecuta al completar todos los pasos del formulario
            onFinished: function (event, currentIndex) {
                // Puedes activar el envío del formulario aquí, por ejemplo:
                 $('#btnGuardar').click();
            },
});
$('#Tab1_FechaNacimientoPaciente').datepicker({
            format: 'dd/mm/yyyy', // Formato de fecha deseado
            autoclose: true, // Para cerrar automáticamente el datepicker después de seleccionar una fecha
            endDate: new Date(), // Esto establece la fecha máxima seleccionable a la fecha actual
            startView: 2 // Muestra la vista de años al abrir
});
$('#Tab1_FechaNacimientoPaciente').off('change').on('change', function () {
            checkEdad();
});
$('#Tab1_NivelEducacion').off('change').on('change', function () {
    checkEdad();
});
$('#Tab1_PacienteDNI').off('change').on('change', function () {
    tienePendiente();
});

const gradosPorNivel = {
    INICIAL: ["1 año", "2 años", "3 años", "4 años", "5 años"],
    PRIMARIA: ["1° grado", "2° grado", "3° grado", "4° grado", "5° grado", "6° grado"],
    SECUNDARIA: ["1° grado", "2° grado", "3° grado", "4° grado", "5° grado", "Bachillerato Internacional (IB)"],
    SUPERIOR: ["Ningún grado disponible para este nivel"],
    OTRO: ["Sin grado específico"],
};


const inputMotivo = document.getElementById('Tab2_MotivoConsulta');
const contador = document.getElementById('contador');
const maxLength = 1000
inputMotivo.addEventListener('input', function () {
    const caracteresRestantes = maxLength - inputMotivo.value.length;
    contador.textContent = caracteresRestantes;
});
// Inicializamos el contador al cargar la página
contador.textContent = maxLength - inputMotivo.value.length;

function checkEdad() {
            // Calcula la edad a partir de la fecha ingresada en 'txtFechaNac'
            let edad = getEdad("Tab1_FechaNacimientoPaciente");
            var checkboxSi = document.getElementById('Tab1_DependienteSi');
            var checkboxNo = document.getElementById('Tab1_DependienteNo');
            
            // Verifica si la edad es menor de 18 años
            document.getElementById('Tab1_Edad').value = edad;
    
            if (edad < 18 && edad != null) {
                $('.isDependiente').addClass('d-sm-flex');
                $('.isDependiente').removeClass('d-none');
                checkboxSi.checked = true;
                checkboxNo.checked = false;
                
                ifDependienteValidate();
            } else {
                $('.isDependiente').addClass('d-none');
                $('.isDependiente').removeClass('d-sm-flex');
                checkboxSi.checked = false;
                checkboxNo.checked = true;
                
                ifNotDependienteValidate()
            }

    //configureLabelLenguaje(edad);
    //configureLabelAprendizaje(edad);
            
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function configureLabelLenguaje(edad) {
    var labelLenguaje = document.getElementById('lbLenguaje0');
    var smallLenguaje = document.getElementById('smlLenguaje0');
    var evalSelectDes = document.getElementById('desEval_Lenguaje'); 

    var lbLenguaje = "";
    var smlLenguaje = smallLenguaje.innerText;
    var esdLenguaje = "";
    if (edad < 5 && edad != null)
    {
        lbLenguaje = "Menor de 5 años";
        smlLenguaje = smlLenguaje.replace("5 Sesiones", "4 Sesiones");
        esdLenguaje = "Menor de 5 años";
    }
    else
    {
        lbLenguaje = "De 5 años a más";
        smlLenguaje = smlLenguaje.replace("4 Sesiones", "5 Sesiones");
        esdLenguaje = "De 5 años a más";
    }

    if (evalSelectDes) {
        evalSelectDes.value = esdLenguaje;
    }

    if (smallLenguaje) {
        smallLenguaje.innerHTML = smlLenguaje;
    }

    if (labelLenguaje) {
        labelLenguaje.innerHTML = lbLenguaje;
    }
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function configureLabelAprendizaje(edad) {

    var nivel = document.getElementById('Tab1_NivelEducacion').value;
    var labelAprendizaje = document.getElementById('lbAprendizaje0');
    var smallAprendizaje = document.getElementById('smlAprendizaje0');
    var evalSelectDes = document.getElementById('desEval_Aprendizaje');

    var lbAprendizaje = "Adultos";
    var smlAprendizaje = smallAprendizaje.innerText;
    var esdAprendizaje = "";

    if (edad < 18 && edad != null) {
        if (nivel == "INICIAL") {
            lbAprendizaje = "Inicial (3, 4 y 5 años)";
            smlAprendizaje = smlAprendizaje.replace("5 Sesiones", "4 Sesiones");
            esdAprendizaje = "Inicial (3, 4 y 5 años)";
        } else if (nivel == "PRIMARIA") {
            lbAprendizaje = "Primaria (1° a 6° grado)";
            smlAprendizaje = smlAprendizaje.replace("4 Sesiones", "5 Sesiones");
            esdAprendizaje = "Primaria (1° a 6° grado)";
        } else if (nivel == "SECUNDARIA") {
            lbAprendizaje = "Secundaria (1° a 5° grado)";
            smlAprendizaje = smlAprendizaje.replace("4 Sesiones", "5 Sesiones");
            esdAprendizaje = "Secundaria (1° a 5° grado)";
        } else if (nivel == "SUPERIOR") {
            lbAprendizaje = "Universitarios";
            smlAprendizaje = smlAprendizaje.replace("4 Sesiones", "5 Sesiones");
            esdAprendizaje = "Universitarios";
        } else if (nivel == "OTROS") {
            lbAprendizaje = "Adultos";
            smlAprendizaje = smlAprendizaje.replace("4 Sesiones", "5 Sesiones");
            esdAprendizaje = "Adultos";
        }
    }
    else {
        lbAprendizaje = "Adultos";
        smlAprendizaje = smlAprendizaje.replace("4 Sesiones", "5 Sesiones");
        esdAprendizaje = "Adultos";
    }
    if (evalSelectDes) {
        evalSelectDes.value = esdAprendizaje;
    }

    if (smallAprendizaje) {
        smallAprendizaje.innerHTML = smlAprendizaje;
    }

    if (labelAprendizaje) {
        labelAprendizaje.innerHTML = lbAprendizaje;
    }
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function getEdad(id) {
            var edad;
            const inputFechaNac = document.getElementById(id).value;
            if (inputFechaNac.length <= 0) { return null }
            const partesFecha = inputFechaNac.split('/');
            const fechaReformateada = partesFecha[1] + '/' + partesFecha[0] + '/' + partesFecha[2];
            const fechaNacimiento = new Date(fechaReformateada);

            if (isNaN(fechaNacimiento.getTime())) {
                // Si la fecha es inválida, asigna 0 a la variable edad
                edad = 0;
            }
            else {
                const fechaActual = new Date();
                fechaActual.setHours(0, 0, 0, 0);
                // Calcula la diferencia en milisegundos entre la fecha actual y la fecha de nacimiento
                const diff = fechaActual - fechaNacimiento;
                // Convierte la diferencia a años dividiendo entre el número de milisegundos en un año y redondeando
                edad = Math.floor(diff / (1000 * 60 * 60 * 24 * 365.25));
            }
            return edad;
        }
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function isCheckedDependiente() {
            var checkbox = document.getElementById('Tab1_DependienteSi');
            // Verificar si el checkbox está marcado
            if (checkbox.checked) {
                $('.isDependiente').addClass('d-sm-flex');
                $('.isDependiente').removeClass('d-none');
                ifDependienteValidate();
            } else {
                $('.isDependiente').addClass('d-none');
                $('.isDependiente').removeClass('d-sm-flex');
                ifNotDependienteValidate()
            }
        }
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function ifDependienteValidate() {

            document.getElementById("Tab1_ApoderadoNombre").required = true;
            document.getElementById("Tab1_ApoderadoNombre").setCustomValidity("Este campo 'Nombre del apoderado' es Obligatorio");

            document.getElementById("Tab1_ContactoTipoDoc").required = true;
            document.getElementById("Tab1_ContactoTipoDoc").setCustomValidity("Este campo 'Tipo documento del apoderaod' es Obligatorio");

            document.getElementById("Tab1_ContactoNroDoc").required = true;
            document.getElementById("Tab1_ContactoNroDoc").setCustomValidity("Este campo 'Nro documento del apoderado' es Obligatorio");

            document.getElementById("Tab1_NivelEducacion").required = true;
            document.getElementById("Tab1_NivelEducacion").setCustomValidity("Este campo 'Nivel de educación' es Obligatorio");

            const radios = document.getElementsByName("Tab1.Parentesco");
            for (const radio of radios) {
                radio.required = true;
                radio.setCustomValidity("Debe marcar almenos un parentesco");
            }

        }
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function ifNotDependienteValidate() {
            document.getElementById("Tab1_ApoderadoNombre").required = false;
            document.getElementById("Tab1_ContactoTipoDoc").required = false;
            document.getElementById("Tab1_ContactoNroDoc").required = false;
            document.getElementById("Tab1_NivelEducacion").required = false;
            const radios = document.getElementsByName("Tab1.Parentesco");
            for (const radio of radios) {
                radio.required = false;
            }
        }
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function showColegioGrado(imput) {
            const nivel = imput.value;
            const inputGrado = document.getElementById('Tab1_PacienteGrado');
            const inputColegio = document.getElementById('Tab1_Colegio');

            //Mostrar u ocultar el campo grado segun la edad
            if (nivel == "SUPERIOR" || nivel == "OTROS" || nivel == "") {
                $(".hideColegioGrado").addClass("d-none")
                inputGrado.removeAttribute("required");
                inputColegio.removeAttribute("required");
            } else {
                $(".hideColegioGrado").removeClass("d-none")
                inputGrado.setAttribute("required", "required");
                inputGrado.setAttribute("title", "El campo grado es obligatorio");
                inputColegio.setAttribute("required", "required");
                inputColegio.setAttribute("title", "El campo centro educativo es obligatorio");
            }

            const grados = gradosPorNivel[nivel] || [];

            // Limpiar opciones de grado
            $("#Tab1_PacienteGrado").empty().append('<option value="" disabled selected >Seleccione grado</option>');

            // Añadir nuevas opciones
            grados.forEach(function (grado) {
                $("#Tab1_PacienteGrado").append(new Option(grado, grado));
            });

            //si existe mensaje de error: eliminar
            if (document.getElementById('Tab1_PacienteGrado-error')) {
                document.getElementById('Tab1_PacienteGrado-error').style.display = "none"
            }
        }
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function verifyTipoDoc() {
            const tipoDoc = document.getElementById("Tab1_TipoDocumento").value
            const nroDocumento = document.getElementById("Tab1_PacienteDNI");
            console.log(tipoDoc);
            if (tipoDoc == "DNI") {
                nroDocumento.setAttribute("maxlength", "8");
                nroDocumento.setAttribute("minlength", "8");
                nroDocumento.setAttribute("pattern", "\\d{8}");
                nroDocumento.setAttribute("title", "EL DNI debe contener exactamente 8 dígitos");
            }
            else if (tipoDoc == "CARNET_EXT") {
                nroDocumento.setAttribute("maxlength", "9");
                nroDocumento.setAttribute("minlength", "9");
                nroDocumento.setAttribute("pattern", "^[a-zA-Z0-9]{9}$");
                nroDocumento.setAttribute("title", "El Carnet de extranjeria debe contener exactamente 9 dígitos");
            }
            else if (tipoDoc == "PASAPORTE") {
                nroDocumento.setAttribute("maxlength", "12");
                nroDocumento.setAttribute("minlength", "12");
                nroDocumento.setAttribute("pattern", "^[a-zA-Z0-9]{12}$");
                nroDocumento.setAttribute("title", "El pasaporte debe contener exactamente 12 dígitos");
            }
            else {
                $('#Tab1_PacienteDNI').val('')
                nroDocumento.removeAttribute("minlength");
                nroDocumento.removeAttribute("maxlength");
                nroDocumento.removeAttribute("pattern");
                nroDocumento.removeAttribute("title");
            }
        }
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function verifyTipoDocCont() {
            const tipoDoc = document.getElementById("Tab1_ContactoTipoDoc").value
            const nroDocumento = document.getElementById("Tab1_ContactoNroDoc");
            console.log(tipoDoc);
            if (tipoDoc == "DNI") {
                nroDocumento.setAttribute("maxlength", "8");
                nroDocumento.setAttribute("minlength", "8");
                nroDocumento.setAttribute("pattern", "\\d{8}");
                nroDocumento.setAttribute("title", "El DNI debe contener exactamente 8 dígitos");
            }
            else if (tipoDoc == "CARNET_EXT") {
                nroDocumento.setAttribute("maxlength", "9");
                nroDocumento.setAttribute("minlength", "9");
                nroDocumento.setAttribute("pattern", "^[a-zA-Z0-9]{12}$");
                nroDocumento.setAttribute("title", "El carnet de extranjeria debe contener exactamente 9 dígitos");
            }
            else if (tipoDoc == "PASAPORTE") {
                nroDocumento.setAttribute("maxlength", "12");
                nroDocumento.setAttribute("minlength", "12");
                nroDocumento.setAttribute("pattern", "^[a-zA-Z0-9]{12}$");
                nroDocumento.setAttribute("title", "El pasaporte debe contener exactamente 12 dígitos");
            }
            else {
                $('#Tab1_ContactoNroDoc').val('')
                nroDocumento.removeAttribute("minlength");
                nroDocumento.removeAttribute("maxlength");
                nroDocumento.removeAttribute("pattern");
                nroDocumento.removeAttribute("title");
            }
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function isValidNroDocumento() {
    const documento = document.getElementById("Tab1_PacienteDNI");
    const pattern = new RegExp(documento.getAttribute("pattern"));
    var flag = false
    if (!pattern.test(documento.value)) {
        documento.setCustomValidity("Invalid");
        flag = false
    } else {
        documento.setCustomValidity("");
        flag = true
    }
    return flag;
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function isValidNroDocumentoCont() {
    var checkbox = document.getElementById('Tab1_DependienteSi');
    const documento = document.getElementById("Tab1_ContactoNroDoc");
    const pattern = new RegExp(documento.getAttribute("pattern"));

    var flag = false
    if (checkbox.checked) {
        if (!pattern.test(documento.value)) {
            documento.setCustomValidity("Invalid");
            flag = false
        } else {
            documento.setCustomValidity("");
            flag = true
        }
    } else {
        flag = true
    }
    return flag;
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function isValidFechaNacimiento() {
    const fecha = document.getElementById("Tab1_FechaNacimientoPaciente");
    const pattern = new RegExp(fecha.getAttribute("pattern"));
    var flag = false
    if (!pattern.test(fecha.value)) {
        fecha.setCustomValidity("Invalid");
        flag = false
    } else {
        fecha.setCustomValidity("");
        flag = true
    }
    return flag;
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function especialidadSelect(id, variable, idEsp) {
    setTimeout(function () {
        var checkbox = document.getElementById(id);
        var checked = checkbox.checked;
        if (checked) {
            
            $('#containerEspecialidades').addClass('d-none')
            if (!idEsp) {
                loadOrientacion(variable);
            } else {
                loadEvaluacionesByEsp(idEsp);
            }
        } else {
            /*clearCheckRadio(div)*/
            $('#containerEspecialidades').removeClass('d-none')
            $("#containerEvaluaciones").empty();
            /*$('#' + div).addClass('d-none')*/
        }
        checkCompare(id);
    }, 50)
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function unmarkEvaluacion(cbx, div) {

    var a = document.getElementById("delete_" + div);
    a.firstElementChild.classList.add("d-none");
    actionLoagin(a);
    setTimeout(function () {
        $("#" + cbx).click();
        especialidadSelect(cbx, div);
        /*clearCheckRadio(div)*/
        a.firstElementChild.classList.remove("d-none");
        hideButtonBack();
        hidenButtonSave();
    }, 400);

}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function validarTab3() {
    if (doAction == "Revisar") {
        return true;
    }

    const form = document.getElementById('formTriaje');
    const checkboxes = form.querySelectorAll('input[type="checkbox"].btn-check');
    var numControles = checkboxes.length;

    var numCheched = 0
    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            numCheched = numCheched + 1
        }
    });
    // console.log(evaluaciones);
    if (numCheched > 0) {
        /*return isRadioChecked();*/
        return isRadioChecked();
    } else {
        showErrorString('Seleccione una especialidad  para continuar');
        return false
    }
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function showErrorString(message) {
    var ul = document.querySelector("#formTriaje > div.content.clearfix > div > ul");
    ul.innerHTML = "";
    // Crea un nuevo elemento li
    var nuevoElementoLi = document.createElement("li");
    nuevoElementoLi.textContent = message;
    // Agrega el nuevo elemento li al final de la lista ul
    ul.appendChild(nuevoElementoLi);
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function isRadioChecked() {
    const divContainer = document.getElementById("containerEvaluaciones");
    if (!divContainer)
    {
        showErrorString("No se encontraron evaluaciones");
        return false;
    }
    const checks = divContainer.querySelectorAll('input[id*="rd"]');
    for (const check of checks) {
        if (check.checked) {
            return true;
        }
    }
    
    showErrorString("Marque una opción para Continuar.");
    return false;
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function clearCheckRadio(eval) {
    const form_ = document.getElementById('formTriaje');
    const radios = form_.querySelectorAll('input[id*="rd' + eval + '"][type="radio"]');
    radios.forEach(function (radio) {
        if (radio.checked) {
            radio.checked = false;
        }
    });
    console.log("Clear-change: " + eval);
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function checkCompare(id) {
    const form1 = document.getElementById('formTriaje');
    const checkboxes = form1.querySelectorAll('input[type="checkbox"].btn-check');
    var numCheched = 0
    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            numCheched = numCheched + 1
        }
    });

    if (numCheched > 0) {
        if (id == "cbx_Orientacion") {
            disabledEval();
        } else if (id == "cbx_Neurologia") {
            disabledEval();
        } else {
            enabledEval();
        }
    } else {
        enabledEvalAll();
    }
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function enabledEval() {
    const form2 = document.getElementById('formTriaje');
    const labels = form2.querySelectorAll('label[id*="box"]');
    labels.forEach(function (label) {
        label.classList.remove("disable-box")
        if (label.id == "box_Orientacion") {
            label.classList.add("disable-box")
        }
        if (label.id == "box_Neurologia") {
            label.classList.add("disable-box")
        }
    });
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function disabledEval() {
    const form3 = document.getElementById('formTriaje');
    const labels = form3.querySelectorAll('label[id*="box"]');
    labels.forEach(function (label) {
        label.classList.add("disable-box")
        if (label.id == "box_Orientacion") {
            label.classList.remove("disable-box")
        }
        if (label.id == "box_Neurologia") {
            label.classList.remove("disable-box")
        }
    });
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function enabledEvalAll() {
    const form4 = document.getElementById('formTriaje');
    const labels = form4.querySelectorAll('label[id*="box"]');
    labels.forEach(function (label) {
        label.classList.remove("disable-box")
    });
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function showToastError2() {
    toastr.options = {
        "closeButton": true,          // Agregar un botón para cerrar el toast
        "debug": false,
        "newestOnTop": true,         // Los nuevos toasts aparecerán debajo de los anteriores
        "progressBar": true,          // Mostrar barra de progreso
        "positionClass": "toast-top-right", // Toasts apilados en la esquina superior derecha
        "preventDuplicates": true,    // Previene toasts duplicados
        "onclick": null,
        "showDuration": "300",        // Duración de la animación de aparición
        "hideDuration": "1000",       // Duración de la animación de desaparición
        "timeOut": "5000",            // Duración de la visibilidad del toast
        "extendedTimeOut": "1000",    // Tiempo extra al pasar el mouse
        "showEasing": "swing",        // Efecto de aparición
        "hideEasing": "linear",       // Efecto de desaparición
        "showMethod": "fadeIn",       // Método de aparición
        "hideMethod": "fadeOut"      // Método de desaparición
    };
    var validationSummary = $(".validation-summary-errors, .text-danger");
    if (validationSummary.length > 0 && validationSummary.is(":visible")) {

        // Recorre y muestra los mensajes de error
        validationSummary.find("ul li").each(function () {
            var errorMessage = $(this).text();
            if (errorMessage) {
                toastr.error(errorMessage);
            }
        });
    }

    return true;
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function showToastError() {
    var validationSummary = $(".validation-summary-errors, .text-danger, .d-none");
    if (validationSummary.length > 0 && validationSummary.is(":visible")) {

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
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function loadEvaluacionesByEsp(idEsp){
    $.ajax({
        url: urlEvaluciones, // URL de la acción en el controlador
        type: 'GET', // Método de la solicitud
        data: { id: idEsp }, // Parámetro que deseas enviar
        success: function (response) {
            
            // Ejemplo: Actualizar el DOM con los datos recibidos
            renderEvaluaciones(response)
        },
        error: function (xhr, status, error) {
            // Manejo de errores
            console.error('Error en la solicitud AJAX:', status, error);
            window.navigation.reload();
        }
    });
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function renderEvaluaciones(response) {
    // Contenedor principal donde se añadirán los elementos
    const container = $("#containerEvaluaciones");
    var load = `<div class="text-center mt-5">
                        <div class="spinner-border" style="width: 2.5rem; height: 2.5rem" role="status">
                            <span class="visually-hidden">Loading...</span>
                        </div>
                        <h4 class="text-muted mt-2">Cargando...</h4>
                    </div>`;
    container.html(load);
    setTimeout(() => {
        container.empty(); // Limpia cualquier contenido previo en el contenedor

        if (response.length === 0) {
        container.append("<p>No hay evaluaciones disponibles.</p>");
        return;
        }
        response.sort((a, b) => a.evaluacionPublicaNroOrden - b.evaluacionPublicaNroOrden);
        console.table(response);
        let titulo = response[0].especialidadPublica;
        let variable = response[0].variable;
        let desEspecialidad = response[0].descripcionEspecialidad;

        const divRow = $("<div>").attr("id", variable);
        const header = $("<h4>").addClass("text-primary d-flex");
        const title = $("<b>").text(titulo);
        const deleteButton = $("<a>")
            .addClass("ms-auto my-auto d-flex btn btn-sm text-white")
            .attr({
            "data-bs-toggle": "tooltip",
            "data-bs-placement": "top",
            "data-bs-title": "Quitar Evaluación",
            "href": "javascript:void(0)",
            "onclick": `unmarkEvaluacion('cbx_${variable}','${variable}')`,
            "id": `delete_${variable}`
        })
            .html('<i class="ti ti-arrow-back-up fs-1 me-1"></i> Anterior');

        header.append(title).append(deleteButton);

        const divAlert = $("<div>")
            .addClass("p-3 bg-light rounded border-start border-2 border-muted fs-5")
            .text(desEspecialidad);

        divRow.append(header); 
        divRow.append(divAlert); 

        const divPolitica = $("<div>").addClass("col-12 text-md-start");
        const pPolitica = $("<p>").addClass("text-primary");
        const iPolitica = $("<i>").addClass("fa fa-warning me-2");
        const aPolitica = $("<i>").attr({
            "data-bs-toggle": "modal",
            "href": "javascript:void(0)",
            "data-bs-target":"#modal-politicas",
        });
        const bPolitica = $("<b>").addClass("text-warning").attr({
            "style": "cursor:pointer",
        }).text("Ver política de privacidad.");

        pPolitica.append(iPolitica).text('Al hacer clic en "Siguiente" está aceptando la política de privacidad del CPAL.')
        aPolitica.append(bPolitica);
        pPolitica.append(aPolitica);
        divPolitica.append("<hr>");
        divPolitica.append(pPolitica);
        // Contenedor de opciones de evaluación
        const colDiv = $("<div>").addClass("row");
        const col1Div = $("<div>").addClass("col-md-6 mt-4");
        const col2Div = $("<div>").addClass("col-md-6 mt-4").attr({
        "id": "containerSessiones"
        });

        showButtonBack(variable);

        response.forEach((evaluacion, index) => {

            var style = evaluacion.evaluacionPublica == "Evaluación del nivel de audición" ? "border-radius: 20% !important;" : "";

            if (evaluacion.evaluacionId != null && evaluacion.evaluacionPublicaIdpadre == null) {
                const optionDiv = $("<div>").addClass("text-start fs-4 ms-3");
                const formCheck = $("<div>").addClass("form-check form-check-inline");
                // Crear las opciones dinámicamente
                const inputRadio = $("<input>")
                    .addClass("form-check-input warning check-outline outline-warning")
                    .attr({
                        type: "radio",
                        name: `Tab3.EvaluacionSeleccionada`,
                        id: `rd${variable}_${index}`,
                        value: evaluacion.evaluacionPublicaId,
                        onchange: `evaluacionSelect('rd${variable}_${index}','${evaluacion.evaluacionId}','${evaluacion.evaluacionPublica}','${evaluacion.evaluacionPublicaId}')`,
                        style: style,
                    });
                const labelRadio = $("<label>")
                    .addClass("form-check-label")
                    .attr("for", `rd${variable}_${index}`)
                    .html(`<b>${evaluacion.evaluacionPublica}</b>`);

                // Ensamblar la opción
                formCheck.append(inputRadio).append(labelRadio);
                optionDiv.append(formCheck);
                col1Div.append(optionDiv); // Agregar las opciones al contenedor

            }
            else if (evaluacion.evaluacionId != null && evaluacion.evaluacionPublicaIdpadre != null) {
                var innerDiv = $("<div>").addClass("ps-5 d-none " + `child-${evaluacion.evaluacionPublicaIdpadre}`);
                /*innerDiv.attr({ id: `divHijo_${evaluacion.evaluacionPublicaIdpadre}` });*/
                const optionDiv = $("<div>").addClass("text-start fs-4 ms-3");
                const formCheck = $("<div>").addClass("form-check form-check-inline");
                /*const InnerDiv = $("<div>").addClass("text-start fs-4 ms-3");*/
                // Crear las opciones dinámicamente
                const inputRadio = $("<input>")
                    .addClass("form-check-input warning check-outline outline-warning " + `child-${evaluacion.evaluacionPublicaIdpadre}`)
                    .attr({
                        type: "radio",
                        name: `Tab3.EvaluacionSeleccionada`,
                        id: `rd${variable}_${index}`,
                        value: evaluacion.evaluacionPublicaId,
                        onchange: `evaluacionSelect('rd${variable}_${index}','${evaluacion.evaluacionId}','${evaluacion.evaluacionPublica}','${evaluacion.evaluacionPublicaId}')`,
                        style: style,
                    });
                const labelRadio = $("<label>")
                    .addClass("form-check-label")
                    .attr("for", `rd${variable}_${index}`)
                    .html(`<b>${evaluacion.evaluacionPublica}</b>`);

                // Ensamblar la opción
                formCheck.append(inputRadio).append(labelRadio);
                optionDiv.append(formCheck);
                innerDiv.append(optionDiv);
                col1Div.append(innerDiv); // Agregar las opciones al contenedor

            }
            else if (evaluacion.evaluacionId == null && evaluacion.evaluacionPublicaIdpadre == null)
            {
                const optionDiv = $("<div>").addClass("text-start fs-4 ms-3").attr({ id: `divPadre_${evaluacion.evaluacionPublicaId}` });;
                const formCheck = $("<div>").addClass("form-check form-check-inline");
                const inputCheck = $("<input>")
                    .addClass("form-check-input primary check-outline outline-primary")
                    .attr({
                        type: "checkbox",
                        id: `cb${variable}_${index}`,
                        onchange: `showHidenDivAugiologia('cb${variable}_${index}','child-${evaluacion.evaluacionPublicaId}')`,
                        style: style,
                    });
                const labelCheck = $("<label>")
                    .addClass("form-check-label")
                    .attr("for", `cb${variable}_${index}`)
                    .html(`<b>${evaluacion.evaluacionPublica}</b>`);

                // Ensamblar la opción
                formCheck.append(inputCheck).append(labelCheck);
                optionDiv.append(formCheck);
                col1Div.append(optionDiv); // Agregar las opciones al contenedor
            }
        });

        
        // Agregar el contenedor al bloque principal
        colDiv.append(col1Div); 
        colDiv.append(col2Div); 
        divRow.append(colDiv); 

        // Añadir el bloque completo al contenedor principal
        container.append(divRow);
/*        loadScreening();*/
        setTimeout(() => {
        container.append(divPolitica);
        }, 100);
        showButtonSave();
    }, 1000); // 1000 ms = 1 segundo
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function showHidenDivAugiologia(inputId,divHijoId) {
    var checkbox = document.getElementById(inputId);
    const divContent = document.getElementById('audiologia');
    const container = $("#containerSessiones");
    // Verificar si el checkbox está marcado
    if (checkbox.checked) {
        $('.' + divHijoId).removeClass('d-none');
    //    uncheckAudiologia(checkbox);
    } else {
        $('.' + divHijoId).addClass('d-none');
    }
    
    const radios = divContent.querySelectorAll('input[type="radio"].' + divHijoId);
    var numCheched = 0
    radios.forEach(function (radio) {
        if (radio.checked) {
            numCheched = numCheched + 1
            radio.checked = false
        }
    });
    if (numCheched > 0) {
        container.empty();
    }
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function loadOrientacion(variable) {
    const container = $("#containerEvaluaciones");
    var load = `<div class="text-center mt-5">
                        <div class="spinner-border" style="width: 2.5rem; height: 2.5rem" role="status">
                            <span class="visually-hidden">Loading...</span>
                        </div>
                        <h4 class="text-muted mt-2">Cargando...</h4>
                    </div>`;
    container.html(load);
    setTimeout(() => {
        container.empty();

        const desOrientacion = "¡Queremos escucharlo! Un asesor se comunicará con usted, elija el medio de contacto.";
        const response = [
            { "id": 1, "nombre": "Por WhatsApp", "titulo": "Título 1", "icono": "ti-brand-whatsapp" },
            { "id": 2, "nombre": "Por llamada telefónica", "titulo": "Título 2", "icono": "ti-phone-call" },
            { "id": 3, "nombre": "Por correo electrónico", "titulo": "Título 3", "icono": "ti-mail" }
        ]

        const divRow = $("<div>").attr("id", variable);
        const header = $("<h4>").addClass("text-primary d-flex");
        const title = $("<b>").text("DESEA OTRA OPCIÓN / SOLICITAR ORIENTACIÓN");
        const deleteButton = $("<a>")
            .addClass("ms-auto my-auto d-flex btn btn-sm  text-white")
            .attr({
                "data-bs-toggle": "tooltip",
                "data-bs-placement": "top",
                "data-bs-title": "Quitar Evaluación",
                "href": "javascript:void(0)",
                "onclick": `unmarkEvaluacion('cbx_${variable}','${variable}')`,
                "id": `delete_${variable}`
            })
            .html('<i class="ti ti-arrow-back-up fs-5 me-1"></i> Regresar');

        header.append(title).append(deleteButton);

        const divAlert = $("<div>")
            .addClass("p-3 bg-light rounded border-start border-2 border-primary fs-5")
            .text(desOrientacion);

        divRow.append(header);
        divRow.append(divAlert);

        showButtonBack(variable)

        // Contenedor de opciones de evaluación
        const colDiv = $("<div>").addClass("row");
        const col1Div = $("<div>").addClass("col-md-6 mt-4");
        const col2Div = $("<div>").addClass("col-md-6 mt-4").attr({
            "id": "containerSessiones"
        }).html(`<h5></h5>`);

        response.forEach((evaluacion, index) => {
            const optionDiv = $("<div>").addClass("text-start fs-4 ms-3");
            const formCheck = $("<div>").addClass("form-check form-check-inline");
            const inputRadio = $("<input>")
                .addClass("form-check-input warning check-outline outline-warning")
                .attr({
                    type: "radio",
                    name: "Tab3.OrientacionSeleccionada",
                    id: `rdOrientacion_${index}`,
                    value: evaluacion.id
                });
            const labelRadio = $("<label>")
                .addClass("form-check-label")
                .attr("for", `rdOrientacion_${index}`)
                .html(`<b>${evaluacion.nombre}</b><i class='ti ${evaluacion.icono} fs-7 ms-3'></i>`);

            // Ensamblar cada opción
            formCheck.append(inputRadio).append(labelRadio);
            optionDiv.append(formCheck);
            col1Div.append(optionDiv);

        })


        // Agregar el contenedor al bloque principal
        colDiv.append(col1Div);
        colDiv.append(col2Div);
        divRow.append(colDiv);

        // Añadir el bloque completo al contenedor principal
        container.append(divRow);
        showButtonSave();
    }, 1000); // 1000 ms = 1 segundo




}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function evaluacionSelect(id,idEval,desc,publicId) {
    var radio = document.getElementById(id);
    var checked = radio.checked;
    if (checked) {
        if (!idEval) {
            return
        } else {
            if (neuropsicOver7years(id, publicId)) {
                loadSesionesByEval(idEval, desc, publicId);
            }
        }
    } else {
        $("#containerSesiones").empty();
    }
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function loadSesionesByEval(idEval, desc, publicId) {
    $.ajax({
        url: urlSesioness, // URL de la acción en el controlador
        type: 'GET', // Método de la solicitud
        data: { id: idEval, idPublic: publicId }, // Parámetro que deseas enviar
        success: function (response) {
            // Actualizar el DOM con los datos recibidos
            console.table(response);
            renderSesiones(response,desc);
        },
        error: function (xhr, status, error) {
            // Manejo de errores
            console.error('Error en la solicitud AJAX:', status, error);
            window.navigation.reload();
        }
    });
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function renderSesiones(response,desc) {
    const container = $("#containerSessiones");
    var load = `<div class="text-center mt-5">
                        <div class="spinner-border" style="width: 2.5rem; height: 2.5rem" role="status">
                            <span class="visually-hidden">Loading...</span>
                        </div>
                        <h4 class="text-muted mt-2">Cargando...</h4>
                    </div>`;
    container.html(load);
    setTimeout(() => {
        container.empty(); // Limpia cualquier contenido previo en el contenedor
        if (response.length === 0) {
            container.append("<p>No hay sesiones disponibles.</p>");
            return;
        }

        let titulo = desc;
        let descripcion = response[0].descripcion;

        // Agregar el título
        const title = $("<h5>").html(`<b>${titulo}</b>`);;

        container.append(title);

        if (descripcion) {
            // Agregar la alerta
            const alertDiv = $("<div>")
                .addClass("mb-3 p-2 bg-light rounded border-end border-2 border-muted")
                .html(`<span>${descripcion}</span>`);
            container.append(alertDiv);
        }

        // Crear la tabla
        const table = $("<table>")
            .addClass("table border text-nowrap mb-0 align-middle");

        // Crear el encabezado de la tabla
        const thead = $("<thead>").html(`
        <tr>
            <th class="p-2">Sesión</th>
            <th class="p-2">Modalidad</th>
            <th class="p-2 d-none">Usuario</th>
            <th class="p-2 d-none">Duración (min)</th>
            <th class="p-2">Tarifa</th>
        </tr>
    `);
        table.append(thead);

        // Crear el cuerpo de la tabla
        const tbody = $("<tbody>");

        response.forEach((sesion, index) => {
            var modalidad = sesion.tipoCitaId == 1 ? "Presencial" : "Virtual";
            var citaPara = sesion.citaPara == 1 ? "Niño" : sesion.citaPara == 2 ? "Padre" : "Adulto";
            var duracion = Number(sesion.duracion) * 60;

            const row = $("<tr>");
            row.append($("<td>").addClass("px-2 py-1 text-muted").text(sesion.nombre));
            row.append($("<td>").addClass("px-2 py-1 text-muted").text(modalidad));
            row.append($("<td>").addClass("px-2 py-1 text-muted d-none").text(citaPara));
            row.append($("<td>").addClass("px-2 py-1 text-muted d-none").text(duracion));
            row.append($("<td>").addClass("px-2 py-1 text-muted").text("S/ " + sesion.precio));
            tbody.append(row); // Agregar la fila al cuerpo de la tabla

        })
        // Ensamblar la tabla
        table.append(tbody);

        // Agregar la tabla al div principal
        container.append(table);

    }, 1000); // 1000 ms = 1 segundo
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function loadScreening() {
    var idScreeming = 106;
    var orden = 5;
    $.ajax({
        url: urlSesioness, // URL de la acción en el controlador
        type: 'GET', // Método de la solicitud
        data: { id: idScreeming, orden: orden }, // Parámetro que deseas enviar
        success: function (response) {
            // Actualizar el DOM con los datos recibidos
            console.table(response);
            renderScreening(response);
        },
        error: function (xhr, status, error) {
            // Manejo de errores
            console.error('Error en la solicitud AJAX:', status, error);
        }
    });
}

function renderScreening(response) {

    if (response.length === 0) {
        return;
    }
    let descripcion = response[0].descripcion;

    const container = $("#containerEvaluaciones");
    const divRow = $("<div>").attr("id", "screening");
    const colDiv = $("<div>").addClass("row");
    const col1Div = $("<div>").addClass("col-md-6 mt-4");
    const col2Div = $("<div>").addClass("col-md-6 mt-4 d-none").attr({
        "id": "screeningSessiones"
    });

    const optionDiv = $("<div>").addClass("text-start fs-4 ms-3");
    const formCheck = $("<div>").addClass("form-check form-check-inline");
    const hiddenInput = $("<input>")
        .attr({
            type: "hidden",
            name: "Tab3.SolicitaScreening",
            value: "false" // Valor enviado si el checkbox no se selecciona
        });
    const inputCheck = $("<input>")
        .addClass("form-check-input primary check-outline outline-primary")
        .attr({
            type: "checkbox",
            id: `cbScreening`,
            name: "Tab3.SolicitaScreening",
            value: "true", // Valor enviado si el checkbox está marcado
            onchange: "showHideScreening()"
        });
    const labelCheck = $("<label>")
        .addClass("form-check-label")
        .attr("for", `cbScreening`)
        .html(`<b>Screening auditivo</b><br><small class='fs-3'>Todo niño menor de 7 años debe pasar un screening auditivo.</small>`);

    // Ensamblar la opción
    formCheck.append(inputCheck, hiddenInput).append(labelCheck);
    optionDiv.append(formCheck);
    col1Div.append(optionDiv); // Agregar las opciones al contenedor


    const title = $("<h5>").html(`<b>Screening auditivo</b>`);
    col2Div.append(title);
    const alertDiv = $("<div>")
        .addClass("mb-3 p-2 bg-light rounded border-start border-2 border-muted")
        .html(`<span>${descripcion}</span>`);
    col2Div.append(alertDiv);
    const table = $("<table>")
        .addClass("table border text-nowrap mb-0 align-middle");

    // Crear el encabezado de la tabla
    const thead = $("<thead>").html(`
        <tr>
            <th class="p-2">Sesión</th>
            <th class="p-2">Modalidad</th>
            <th class="p-2 d-none">Usuario</th>
            <th class="p-2 d-none">Duración (min)</th>
            <th class="p-2">Tarifa</th>
        </tr>
    `);
    table.append(thead);

    // Crear el cuerpo de la tabla
    const tbody = $("<tbody>");
    response.forEach((sesion, index) => {
        var modalidad = sesion.tipoCitaId == 1 ? "Presencial" : "Virtual";
        var citaPara = sesion.citaPara == 1 ? "Niño" : sesion.citaPara == 2 ? "Padre" : "Adulto";
        var duracion = 15; //Number(sesion.duracion) * 60;

        const row = $("<tr>");
        row.append($("<td>").addClass("px-2 py-1 text-muted").text(sesion.nombre));
        row.append($("<td>").addClass("px-2 py-1 text-muted").text(modalidad));
        row.append($("<td>").addClass("px-2 py-1 text-muted d-none").text(citaPara));
        row.append($("<td>").addClass("px-2 py-1 text-muted d-none").text(duracion));
        row.append($("<td>").addClass("px-2 py-1 text-muted").text("S/ " + sesion.precio));
        tbody.append(row); // Agregar la fila al cuerpo de la tabla

    })
    table.append(tbody);

    // Agregar la tabla al div principal
    col2Div.append(table);

    // Agregar el contenedor al bloque principal
    colDiv.append(col1Div);
    colDiv.append(col2Div);
    divRow.append(colDiv);

    const line = $("<hr>");
    // Añadir el bloque completo al contenedor principal
    container.append(line);
    container.append(divRow);
}
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
function uncheckAudiologia(Checkbox) {
    const divContainer = document.getElementById("audiologia");
    const radios = divContainer.querySelectorAll('input[type="radio"][id*="rdaudiologia"]');
    const checks = divContainer.querySelectorAll('input[type="checkbox"][id*="cbaudiologia"]');
    for (const radio of radios) {
        radio.checked = false;
    }
    for (const check of checks) {
        if (check !== Checkbox) { // Evita desmarcar el radio actual
            check.checked = false;
        }
    }
}

function showHideScreening() {
    const checkBox = document.getElementById('cbScreening');
    const cheked = checkBox.checked
    if (cheked) {
        $('#screeningSessiones').removeClass('d-none');
    } else {
        $('#screeningSessiones').addClass('d-none');
    }
}

function verifyGrado() {
    const input = document.getElementById('Tab1_PacienteGrado');
    if (typeof grado !== 'undefined' && grado !== null && grado !== '') {
        input.value = grado;
    }
}

function actualizarTab() {
    var btnSteep = document.querySelector("#formTriaje > div.actions.clearfix > ul > li:nth-child(2) > a")
    if (doAction == "Agregar") {
        document.getElementById("divOrientacion").classList.add('d-none');
        btnSteep.click();
        btnSteep.click();
    }
    if (doAction == "Revisar") {
        document.getElementById("divOrientacion").classList.add('d-none');
    }
}

function mackerLiBack() {
    var ul = $("#formTriaje > div.actions.clearfix > ul"); // Selección con jQuery
    const li = $("<li>").attr({
        "aria-hidden": "false",
        "id": "btnAtras",
    })

    if (ul) {
        ul.prepend(li);
    }
}
function showButtonBack(variable) {
    document.querySelector("#formTriaje > div.actions.clearfix > ul > li:nth-child(2) > a").style.display = "none";

    var li = $("#btnAtras");
    const deleteButton = $("<a>")
        .attr({
            "href": "javascript:void(0)",
            "onclick": `unmarkEvaluacion('cbx_${variable}','${variable}')`,
            "id": `delete_${variable}`
        })
        .css({ "background-color":"rgba(42, 53, 71, 0.6)"})
        .html('Anterior');
    li.append(deleteButton)
}
function hideButtonBack(variable) {
    document.querySelector("#formTriaje > div.actions.clearfix > ul > li:nth-child(2) > a").style.display = "";
    var li = $("#btnAtras");
    li.empty();
}

function DeleteEvaluacionPagoCita(pagoCita, evaluacion) {

    Swal.fire({
        title: "",
        text: "¿Esta seguro de Eliminar la evaluación?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Eliminar",
        cancelButtonText: "Cancelar",
        customClass: {
            confirmButton: "bg-eliminar",
            cancelButton: "bg-cancelar"
        },
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
            url: urlDeleteEvalPagocita, // URL de la acción en el controlador
            type: 'POST', // Método de la solicitud
            data: { pagoCitaId: pagoCita, evaluacionId: evaluacion }, // Parámetros a enviar
            success: function (response) {
                // Manejo del éxito
                /*                location.reload(); // Si quieres recargar toda la página*/

                const currentUrl = new URL(window.location.href);
                currentUrl.searchParams.set("doAction", "Revisar");
                window.location.href = currentUrl.toString();
            },
            error: function (xhr, status, error) {
                // Manejo de errores
                if (xhr.responseJSON && xhr.responseJSON.error) {
                    showAlertWithConfirmed(
                        "error",
                        "",
                        xhr.responseJSON.error,
                        '',
                        'cerrar'
                    );
                } else {
                    showAlertWithConfirmed(
                        "error",
                        "",
                        "Ocurrió un error al intentar eliminar la evaluación.",
                        '',
                        'cerrar'
                    );
                }

                location.reload();
            }
        });       
        }
    });




}

function hidenButtonSave() {
    document.querySelector("#formTriaje > div.actions.clearfix > ul > li:nth-child(4) > a").style.display = "none";
    if (doAction == "Revisar") {
        document.querySelector("#formTriaje > div.actions.clearfix > ul > li:nth-child(4) > a").style.display = "";
        document.querySelector("#formTriaje > div.actions.clearfix > ul > li:nth-child(4) > a").textContent = "Siguiente";
    }
}
function showButtonSave() {
    document.querySelector("#formTriaje > div.actions.clearfix > ul > li:nth-child(4) > a").style.display = "";
}

function showListEspecialidades() {
    document.getElementById("containerAddEspecialidad").classList.add("d-none")
    document.getElementById("containerEspecialidades").classList.remove("d-none")
}

function tienePendiente() {
    const dni = document.getElementById("Tab1_PacienteDNI").value;
    $.ajax({
        url: urlPendiente, // URL de la acción en el controlador
        type: 'GET', // Método de la solicitud
        data: { dni: dni }, // Parámetro que deseas enviar
        success: function (response) {
            if (response === true) {
                showAlert("info", "¡Importante!", "Ya existe una reserva pendiente para esta persona");
                document.getElementById("Tab1_PacienteDNI").value = ""
            }
        },
        error: function (xhr, status, error) {
            // Manejo de errores
            console.error('Error en la solicitud AJAX:', status, error);
        }
    });
}

function disabledDocumento() {
    const tipoDoc = document.getElementById("Tab1_TipoDocumento");
    const nroDocumento = document.getElementById("Tab1_PacienteDNI");
    const tipoDocC = document.getElementById("Tab1_ContactoTipoDoc");
    const nroDocumentoC = document.getElementById("Tab1_ContactoNroDoc");

    if (nroDocumento.value.trim().length > 8) {
        tipoDoc.classList.add("disable-box");
        nroDocumento.classList.add("disable-box");
    }
    //if (nroDocumentoC.value.trim().length > 0) {
    //    tipoDocC.classList.add("disable-box");
    //    nroDocumentoC.classList.add("disable-box");
    //}
}

function neuropsicOver7years(id, publicId) {
    var edad = document.getElementById('Tab1_Edad').value;
    const neuro = document.getElementById(id);
    if (edad < 7 && publicId == 23) {
        const container = $("#containerSessiones");
        container.empty();
        neuro.checked = false;
        showAlert("info", "¡Lo sentimos!", "La evaluación de neurología sólo está disponible para niños a partir de 7 años.");
        return false
    }
    return true
}