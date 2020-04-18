var intervalo;
function ConfirmacionEmail(email) {

    if (window.WebSocket) {
        alert("Websocket se encuentra habilitado");
        abrirSocket(email, "Email");
    }
    else {
        alert("Websocket no se encuentra habilitado");

        // continuamente llamar a la función definida en scripts2.js
        intervalo = setInterval(() => {
            ChequearEstadoConfirmacionEmail(email);
        }, 5000);
    }
}

function ConfirmacionInvitacionJuego(id) {
    if (window.WebSocket) {
        alert("websocket está habilitado");
        abrirSocket(id, "InvitacionJuego");
    }
    else {
        alert("websocket no está habilitado");
        // Debido a que websocket no está habilitado entonces 
        // Continuamente llamar a la función definida en scripts2.js
        intervalo = setInterval(() => {
            ChequearEstadoConfirmacionInvitacionJuego(id);
        }, 5000);
    }
}