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