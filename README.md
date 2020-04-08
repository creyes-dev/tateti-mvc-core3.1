# tateti-mvc-core3

Juego del ta te ti con asp.net core 3.1

Dentro de una cuadrícula de 3 filas y 3 columnas cada jugador escoge la celda en donde va una X o un O. El ganador es el jugador que consiga mantener tres X en celdas sucesivas en orientación vertical, horizontal o en diagonal

## Requerimientos

1. Todo jugador debe estar registrado con su email que debe ser confirmado antes de poder jugar. En la cuenta del jugador se registra su nombre, apellido y su puntaje. A fines prácticos la confirmación del mail del usuario será automática, a través de un middleware de comunicación inmediatamente después de que el jugador se registra el navegador deberá enviar continuamente solicitudes de confirmación del correo del jugador, luego de confirmar el correo del jugador el navegador debe redirigirse a la pantalla de envío de invitaciones
2. Un jugador gana un punto cuando gana una partida.
3. Para que dos jugadores puedan jugar uno debe enviar una invitación a otro, cuando esto sucede el jugador que invita al otro jugador debe esperar a que éste último acepte su invitación. 
4. Implementar un tablero de posiciones para determinar el ranking de jugadores con mayor puntaje. 
3. Cada vez que un jugador recibe una invitación recibe un email conteniendo los datos del jugador que ha enviado la invitación
 
## Dependencias

- Asp.Net Mvc Core 3.1
- bootstrap 4.3.1
- jquery 3.3.1
- popper.js 1.14.7

