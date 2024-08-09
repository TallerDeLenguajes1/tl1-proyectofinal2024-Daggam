<div align="center">
    <h1>DRAGON SIM</h1>
</div>
<p align = "center" width="100%">
  <img width="70%" src="https://github.com/user-attachments/assets/939b025d-59e5-4209-9190-b9ed32d59090">
</p>
<h3 align="center"> Un videojuego basado en Dragon Ball Budokai Tenkaichi 3</h3>
<br/>


## Sobre el proyecto

Es un videojuego basado en *Dragon Ball Budokai Tenkaichi 3*, especificamente del modo de juego *Dragon Sim*.

El juego está seccionado de la siguiente forma:
- **Menú**: Contiene 3 opciones.
  - **Nueva Partida**: Comienza una nueva partida.
  - **Cargar Partida**: Carga una partida de guardado automatico.
  - **Información**: Muestra una pantalla de información del juego.
- **Modo Entrenamiento**: 10 días de entrenamiento donde ganas/pierdes puntos de salud o caracteristicas generales.
- **Modo Batalla**: Enfrentamiento directo contra 1 personaje de los 20 que están en el juego.

<p align = "center" width="100%">
  <img width="70%" src="https://github.com/user-attachments/assets/e0804364-b7b7-461e-b0e5-59cf566add33">
</p>
<p align="center">Esquema que representa el flujo del juego</p>

## Galería
<br/>
<p align = "center" width="100%">
  <img width="70%" src="https://github.com/user-attachments/assets/f2269a50-7fb2-4806-aa82-59080833a1f7">
</p>
<p align="center">Pantalla de información</p>
<br/>

|                                               |                                               |
|-----------------------------------------------|-----------------------------------------------|
| <img width="100%" src="https://github.com/user-attachments/assets/cc0d7681-9869-4d99-9e15-a3b231df55cf"> | <img width="100%" src="https://github.com/user-attachments/assets/089e1673-3d6e-40ae-9520-c47e7f670ae5"> |
<p align="center">Comparativa entre la inspiración y el original</p>
<br/>
<br/>
<p align = "center" width="100%">
  <img width="70%" src="https://github.com/user-attachments/assets/dae259e4-5d85-49fb-99f1-c8eb653324b3">
</p>
<p align="center">Evento de exploración</p>
<br/>
<br/>
<p align = "center" width="100%">
  <img width="70%" src="https://github.com/user-attachments/assets/d394f1ab-4dbf-4595-b0b7-3e1f6bc1f7cc">
</p>
<p align="center">Modo Batalla</p>
<br/>
<br/>
<p align = "center" width="100%">
  <img width="70%" src="https://github.com/user-attachments/assets/8a481a96-03eb-4211-b7de-aca379d4ee18">
</p>
<p align="center">Evento de batalla</p>
<br/>
<br/>
<p align = "center" width="100%">
  <img width="70%" src="https://github.com/user-attachments/assets/1e1812d1-affc-47a6-ab94-415177f6b8c2">
</p>
<p align="center">Game over</p>
<br/>

## Como jugar

Necesitarás descargar el proyecto, o en su defecto, clonar el repositorio con `git clone`.

Además, debes tener descargado el .Net8.0 SDK para compilar el proyecto y correrlo. Si no lo tienes y no sabes como descargarlo, tienes el siguiente [video](https://www.youtube.com/watch?v=oY8C4UninuM).

Una completado estos pasos, debes abrir la consola y direccionarte a la ruta donde tienes descargado el proyecto y poner el siguiente comando:

```sh
dotnet run
```
Esto compilará y correrá el juego.

## Aspectos técnicos

Hago uso de lo siguientes conceptos:
- Una "Maquina de estado finito" para un mejor manejo de los eventos.
- Utilización de una API que me regresa planetas del universo de Dragon Ball (Los utilizo en el Modo Batalla). [Link](https://web.dragonball-api.com/)
- Distintos usos de las librerias que ofrece el SDK .NET 8.0:
  - [Console](https://learn.microsoft.com/es-es/dotnet/api/system.console?view=net-8.0): Un uso particular que le doy además de imprimir en pantalla, es el uso del cursor para evitar imprimir todo en pantalla.
  - [Text.Json](https://learn.microsoft.com/es-es/dotnet/api/system.text.json?view=net-8.0): Me ayuda a cargar los archivos de las partidas, personajes, serializar y deserealizar los archivos JSON enviados por la API.
  - [System.Media](https://learn.microsoft.com/es-es/dotnet/api/system.media?view=net-8.0): Utilizo especificamente el SoundPlayer, que me permite reproducir audio en consola.
- Creación de clases que me ayudan a simplificar un poco el código. (Están en el archivo GameTools.cs)
- En el modo entrenamiento, los eventos de explorar están dados por la probabilidad. Si se repite un evento, aumenta un contador de ese evento y las interacciones son más interesantes/más rápidas.




## Créditos
**Creador**: Benjamin Villa

**Asignatura**: Taller de lenguajes I

**Lenguaje de programación**: C#

**SDK**: .NET 8.0.101

## Controles

**Flechas de dirección**: Movimiento.

**ENTER**: Confirmar

**Z**: Tecla de acción 1

**X**: Tecla de acción 2

**C**: Tecla de acción 3
