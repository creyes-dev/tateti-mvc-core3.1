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
var intervalo;
function ChequearEstadoConfirmacionEmail(email) {
    // Llamar al servicio que está implementado en el middleware de comunicación
    // Con la finalidad de que el mismo maneje la confirmación del correo
    $.get("/ChequearEstadoConfirmacionEmail?email=" + email, function (data) {
        if (data === "OK") {
            // Si la operación ha concluido satisfactoriamente entonces el browser 
            // debe dejar de llamar a esta función
            if (intervalo !== null)
                clearInterval(intervalo);
            // Finalmente redireccionar a la pantalla de envío de invitaciones
            window.location.href = "/InvitacionJuego?email=" + email;
        }
    });
}

var abrirSocket = function (parametro, strAccion) {
    if (intervalo !== null) clearInterval(intervalo);

    var protocolo = location.protocol === "https:" ? "wss:" : "ws:";
    var operacion = "";
    var wsUri = "";

    if (strAccion == "Email") {
        wsUri = protocolo + "//" + window.location.host + "/ChequearEstadoConfirmacionEmail";
        operacion = "ChequearEstadoConfirmacionEmail";

        var socket = new WebSocket(wsUri);
        socket.onmessage = function (response) {
            console.log(response);
            if (strAccion == "Email" && response.data == "OK") {
                window.location.href = "/InvitacionJuego?email=" + parametro;
            }
        };

        socket.onopen = function () {
            var json = JSON.stringify({
                "Operation": operacion,
                "Parameters": parametro
            });
            socket.send(json);

            socket.onclose = function (event) { };
        };
    }
};