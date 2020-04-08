var intervalo;
function ConfirmacionEmail(email) {
    // continuamente llamar a la función definida en scripts2.js
    intervalo = setInterval(() => {
        ChequearEstadoConfirmacionEmail(email);
    }, 5000);
}