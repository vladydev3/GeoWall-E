# GeoWall-E

GeoWall-E es una aplicación de escritorio que permite diseñar programáticamente un conjunto de pasos para obtener y dibujar figuras geométricas. Con GeoWall-E, puedes crear desde simples líneas y círculos hasta complejos polígonos. GeoWall-E te ofrece una interfaz gráfica intuitiva y fácil de usar, donde puedes escribir tu código, ejecutarlo y ver el resultado en tiempo real.

## Características

GeoWall-E utiliza el lenguaje de programación G#, que te permite definir constantes, funciones, condicionales y operaciones matemáticas.
GeoWall-E te permite dibujar figuras geométricas básicas como puntos, líneas, rectángulos, círculos, triángulos y cuadriláteros, así como otras figuras geométricas más avanzadas.
GeoWall-E te permite personalizar el color de las figuras, así como guardar y cargar tus proyectos.

## Uso

Para usar GeoWall-E, sigue estos pasos:

Ejecute el archivo _GeoWall-E.exe_ ubicado en la ruta {carpeta raiz del proyecto}\GeoWall-E\bin\Debug\net7.0-windows.

Escribe tu código en el editor de texto de la izquierda, siguiendo la sintaxis y los comandos del lenguaje de GeoWall-E. Puedes consultar una pequeña guía del lenguaje pulsando el botón Commands en el menú principal.
Presiona el botón Compile para compilar y si no hay errores, Run para ejecutar tu código y ver el resultado en el lienzo de la derecha.
Si quieres guardar o cargar tu proyecto, puedes usar las opciones del menú de la aplicación.

## Ejemplo

Aquí tienes un pequeño ejemplo de código G#

```
point p1;
point p2;
measure m;

draw circle(p1,m);

color blue;

draw ray(p1,p2);

```
