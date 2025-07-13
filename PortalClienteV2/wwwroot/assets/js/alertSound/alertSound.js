// Configuración de sonidos
const sounds = {
    info: "/PortalClienteV2/assets/libs/Toasty/sounds/info/1.mp3",
    success: "/PortalClienteV2/assets/libs/Toasty/sounds/success/1.mp3",
    warning: "/PortalClienteV2/assets/libs/Toasty/sounds/warning/1.mp3",
    error: "/PortalClienteV2/assets/libs/Toasty/sounds/error/1.mp3",
};

// Función para reproducir sonidos
const playSound = (type) => {
    const sound = sounds[type];
    if (!sound) return; // Verifica que exista un sonido para el tipo especificado

    const audio = document.createElement("audio");
    audio.src = sound;
    audio.autoplay = true;

    // Eliminar el elemento audio al finalizar
    audio.addEventListener("ended", () => {
        audio.remove();
    });

    document.body.appendChild(audio); // Agregar al DOM temporalmente
};

// Función para mostrar alertas con SweetAlert y sonido
const showAlert = (type, title, text) => {
    // Reproducir el sonido antes de mostrar la alerta
    playSound(type);

    // Mostrar el mensaje usando SweetAlert
    Swal.fire({
        icon: type, // Tipo de alerta (info, success, warning, error)
        title: title,
        text: text,
    });
};

// Función para mostrar una alerta con confirmación
const showAlertWithConfirmed = (type, title, text, redirectUrl, buttonText) => {
    playSound(type); // Reproducir el sonido asociado al tipo de alerta

    Swal.fire({
        title: title,
        text: text,
        icon: type,
        allowOutsideClick: false,
        allowEscapeKey: false,
        allowEnterKey: false,
        confirmButtonText: buttonText || "Aceptar",
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = redirectUrl;
        }
    });
};

// Función para mostrar una alerta con confirmación
const showAlertWithConfirmedAndCancel = (type, title, text, redirectUrl, buttonText, cancelText, cancelUrl) => {
    playSound(type); // Reproducir el sonido asociado al tipo de alerta

    Swal.fire({
        title: title,
        text: text,
        icon: type,
        allowOutsideClick: false,
        allowEscapeKey: false,
        allowEnterKey: false,
        showCancelButton: true,
        confirmButtonText: buttonText || "Aceptar",
        cancelButtonText: cancelText || "Cancelar",
        customClass: {
            confirmButton: "btn btn-primary",
            cancelButton: "btn btn-danger"
        },
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = redirectUrl;
        } else if (result.isDenied) {
            window.location.href = cancelUrl;
        } else{
            window.location.href = cancelUrl;
        }
    });
};

const showAlertSIyNO = (type, title, text, SiUrl, SiText, NoUrl, NoText, inicioUrl, inicioText,) => {
    playSound(type); // Reproducir el sonido asociado al tipo de alerta

    Swal.fire({
        title: title,
        text: text,
        icon: type,
        allowOutsideClick: false,
        allowEscapeKey: false,
        allowEnterKey: false,
        showCancelButton: true,
        confirmButtonText: SiText || "Aceptar",
        cancelButtonText: NoText || "Cancelar",
        customClass: {
            confirmButton: "bg-primary fs-5 py-1",
            cancelButton: "bg-cancelar fs-5 py-1"
        },
    }).then((result) => {
        if (result.isConfirmed) {
            /*window.location.href = SiUrl;*/
            if ($("#btnCloseCar").length) {
                $("#btnCloseCar").click();
            }
        }
        else
        {
            Swal.fire({
                title: "Importante",
                text: "Para continuar el proceso, debe realizar el pago de la primera sesión de cada evaluación seleccionada. Posteriormente, una asesora lo contactará para definir la fecha y hora de la sesión.",
                icon: "info",
                allowOutsideClick: false,
                allowEscapeKey: false,
                allowEnterKey: false,
                showCancelButton: true,
                confirmButtonText:"<i class='fa fa-credit-card me-2'></i> Ir a pago",
                cancelButtonText: "<i class='fa fa-home me-2'></i> Ir a Inicio",
                customClass: {
                    confirmButton: "bg-warning",
                    cancelButton: "bg-primary"
                },
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = NoUrl;
                } else {
                    window.location.href = inicioUrl;
                }
            });
        }
    });
};