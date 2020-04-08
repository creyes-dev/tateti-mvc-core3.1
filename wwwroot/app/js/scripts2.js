function ChequearEstadoConfirmacionEmail(email) {
    // Llamar al servicio que está implementado en el middleware de comunicación
    $.get("/ChequearEstadoConfirmacionEmail?email=" + email, function (data) {
        if (data === "OK") {
            // Si la operación ha concluido satisfactoriamente entonces el browser 
            // debe dejar de llamar a esta función
            if (intervalo !== null)
                clearInterval(intervalo);
            // Finalmente redireccionar a la pantalla de envío de invitaciones
            window.location.href = "/GameInvitation?email=" + email;
        }
    });
}