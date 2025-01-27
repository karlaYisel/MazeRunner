<div align="center"> <img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/MazeRunner.png" width="400"> </div>
<h1 align=center> Maze Runner: </h1>

<h2 align=center> Como ejecutar el juego: </h2>
<div align="justify">
<ol>
  <li>Abrir la consola en el directorio MazeRunner\MazeRunner.WebApp y ejecute con el comando dotnet run.</li>
  <li>Aparecerá en su consola el directorio http del proyecto que puede copiar y pegar en su navegador o abrir automático con Ctrl + Click.</li>
  <li>Disfrutar del juego.</li>
</ol>
</div>

<h2 align=center> Estructura del juego: </h2>
<div align="justify">
<ul>
  <li>Pantalla de Carga: Después de una breve pantalla de carga aparecerá en el menú principal, no es requerido hacer nada aquí.</li>
  <li>Sobre el Autor: Enlace al perfil de GitHub del autor.</li>
  <li>Menú Principal: Se tendrá un menú para seleccionar entre dos opciones: Jugar, que lo dirige a la pantalla de Selección de Jugadores; y Salir, que le muestra un mensaje de confirmación para redirigirlo a la página de Google.</li>
  <li>Selección de Jugadores: Se puede seleccionar la cantidad de jugadores que intervendrá en el juego haciendo click en el número correspondiente, también se pueden escribir nombres para cada uno inclusive para los bots en caso que se active el modo Jugar con Bots, en caso de no añadir ninguno se toman sus nombres por defecto. Una vez listo se presiona el botón Continuar que dirige al Editor del Laberinto.</li>
Nota: Actualmente los modos de 1 Jugador y de Jugar con Bots se encuentran desactivados.
  <li>Editor del Laberinto: Se puede configurar parámetros del juego como dimensión del tablero (Actualmente solo admisible de nxn), Cantidad de fichas por jugador, cantidad de obstáculos, de trampas y de personajes no jugables (NPCs). Hay condiciones establecidas de mínimos y máximos en cada opción y en caso de introducir más interactuables (Obstáculos, Trampas y NPCs) de los que admite el tablero se notificara al intentar continuar. Una vez listo el botón Continuar dirigirá a la página de Lista de Fichas.</li>
  <li>Lista de Fichas: Esta página aparece por cada jugador, especificando el nombre del mismo. Se seleccionan las fichas que conformarán su equipo haciendo click en su imagen, su equipo actual aparecerá abajo a medida que lo modifiques. Si se desea quitar una ficha del equipo es suficiente hacer click en su imagen dentro del mismo. Una vez terminado el botón Continuar dirige a la Lista de Fichas del siguiente jugador o a la página del Juego en caso de ser el último jugador.</li>
Nota: En caso de que la cantidad de fichas seleccionadas no coincida con la cantidad de fichas por jugador establecida anteriormente se notificara con un mensaje y se podrá volver a modificar.
  <li>Juego: En esta pantalla intervienen 4 componentes principales:</li>
  <ul>
      <li>Menú del Jugador: Informa cual es el jugador al que le corresponde el turno y las acciones que puede hacer.</li>
      <li>Tablero: Tablero del juego, en caso de ser un laberinto grande se pueden encontrar barras de scroll. La mayor parte de la información del juego ocurre aquí.</li>
      <li>Cuadro de Mensajes: Donde aparece la leyenda de todo lo que ha sucedido durante la partida</li> 
      <li>Menú de la Ficha: Información de la ficha seleccionada, su jugador, su tipo y sus estadísticas.</li>
  </ul>
  En caso de que la vida de una ficha llegue a 0 reaparecerá en su posición original con la vida restaurada, en el caso de un NPC simplemente desaparecerá.
  
La partida se gana al llegar al centro del tablero, marcado con un castillo. Esto produce un mensaje de victoria para el jugador respectivo y al confirmar se dirige de nuevo al menú de inicio listo para una nueva partida.
  <li>Mensajes de Confirmación: Predominan dos tipos: uno donde se solicita hacer una acción la cual se puede confirmar o cancelar; y uno donde se notifica una advertencia.</li>
</ul>
</div>

<h2 align=center> Guía del Juego: </h2>

<h3 align=center> Personajes: </h3>
<div align="justify">
  En el juego se tienen actualmente 5 personajes disponibles para elegir, todos con una habilidad especial y un tiempo de recuperación de la misma de entre 1 y 3 turnos (Las estadísticas expresadas en un rango suelen ser escogidas dentro del mismo de forma aleatoria):
  <ul>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/Hero.png" width="50"> Héroe:</li>
    <ul>
      <li>Habilidad Especial: Ataque normal pero más potente, se activa sobre oponentes.</li>
      <li>Vida: 60-100</li>
      <li>Defensa: 5-8</li>
      <li>Fuerza: 5-8</li>
      <li>Destreza: 5-8</li>
      <li>Velocidad: 2-5</li>
      <li>Ataque: Largo</li>
    </ul>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/Thief.png" width="50"> Ladrón:</li>
    <ul>
      <li>Habilidad Especial: Activada sobre sí mismo puede ver todas las trampas en su rango de movimiento, sobre una casilla en caso de tener trampa puede cambiar su estado entre activada-desactivada.</li>
      <li>Vida: 40-60</li>
      <li>Defensa: 5-6</li>
      <li>Fuerza: 5-6</li>
      <li>Destreza: 5-10</li>
      <li>Velocidad: 2-5</li>
      <li>Ataque: Corto</li>
    </ul>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/Healer.png" width="50"> Sanador:</li>
    <ul>
      <li>Habilidad Especial: Cura a aliados en su rango de ataque, incluyéndose.</li>
      <li>Vida: 60-80</li>
      <li>Defensa: 5-6</li>
      <li>Fuerza: 5-6</li>
      <li>Destreza: 5-8</li>
      <li>Velocidad: 2-4</li>
      <li>Ataque: Corto</li>
    </ul>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/Paladin.png" width="50"> Paladín:</li>
    <ul>
      <li>Habilidad Especial: Aumenta su defensa durante cierta cantidad de turnos.</li>
      <li>Vida: 70-100</li>
      <li>Defensa: 5-8</li>
      <li>Fuerza: 5-8</li>
      <li>Destreza: 5-6</li>
      <li>Velocidad: 2-3</li>
      <li>Ataque: Largo</li>
    </ul>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/Archer.png" width="50"> Arquero:</li>
    <ul>
      <li>Habilidad Especial: Usa su ataque normal pero amplia su rango de ataque a 10 bloques.</li>
      <li>Vida: 60-100</li>
      <li>Defensa: 5-7</li>
      <li>Fuerza: 5-7</li>
      <li>Destreza: 5-8</li>
      <li>Velocidad: 2-5</li>
      <li>Ataque: Distancia</li>
    </ul>
  </ul>
</div>

<h3 align=center> Estados: </h3>
<div align="justify">
  Algunas trampas le pueden conferir estados a los personajes y NPCs, estos tienen un tiempo de duración y un efecto que se produce durante ese tiempo:
  <ul>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/fire.png" width="50"> Quemado: Conferido por trampas de fuego, se expresa en pasos, activándose y reduciéndose por cada cambio de casilla del personaje. Produce cierta cantidad de daño entre 0-5.</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/ice.png" width="50"> Congelado: Conferido por trampas de hielo, se expresa en turnos, mientras esté activado el personaje no puede hacer nada en su turno.</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/poison.png" width="50"> Envenenado: Conferido por trampas de veneno, se expresa en turnos, cada turno activado se le reduce la vida al personaje en 8.</li>
  </ul>
  Nota: Se considera un turno a que todos los jugadores en el juego pasen su turno y los NPCs terminen de jugar, empezando cada uno con el Jugador 1 y terminando con los NPCs.
</div>

<h3 align=center> Guía de Colores: </h3>
<div align="justify">
  Actualmente se usa una guía de colores para visualizar de que jugador es cada ficha del tablero:
  <ul>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/blue.png" width="50"> Azul: Jugador 1</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/red.png" width="50"> Rojo: Jugador 2</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/green.png" width="50"> Verde: Jugador 3</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/yellow.png" width="50"> Amarillo: Jugador 4</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/tokens/gray.png" width="50"> Gris: NPCs</li>
  </ul>
</div>

<h3 align=center> NPCs: </h3>
<div align="justify">
  Los NPCs no pueden activar la condición de victoria, pero pueden afectar a las fichas atacándolas o con su movimiento (Solo se permiten 2 fichas por casillas, por lo que si hay 2 NPCs en una de ellas los jugadores no podrán pasar). Existen 3 tipos de NPCs, lo que todos se visualizan con el mismo icono.
  <ul>
    <li>Pasivos: No ataca, solo se mueve de forma aleatoria por el laberinto.</li>
    <li>Neutrales: Solo ataca a las fichas que lo han atacado antes si se encuentran en su rango de movimiento, de lo contrario solo se mueven de forma aleatoria.</li>
    <li>Agresivos: Ataca a cualquier ficha en su rango de movimiento, de lo contrario solo se mueven de forma aleatoria.</li>
  </ul>
</div>

<h3 align=center> Obstáculos: </h3>
<div align="justify">
  Los obstáculos se encuentran para retrasar el movimiento del personaje por el tablero. Existen 3 tipos de obstáculos en el juego:
  <ul>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/cells/water.png" width="50"> Obstáculos Permanentes: Es un obstáculo que se encuentra desde el inicio del juego, para pasar por él se necesita gastar 2 de velocidad en lugar de 1 como suelen gastar las casillas sin obstáculos.</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/cells/water.png" width="50"><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/cells/lotus.png" width="50"><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/cells/woods.png" width="50"> Obstáculos Temporales: Son obstáculos con un intervalo de tiempo por el cual turnas en ser visibles o no. Cuando son visibles pueden consumir de 2 a 4 de velocidad respectivamente al orden de las imágenes.</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/cells/TemporalWall.png" width="50"> Paredes Temporales: Impiden que los personajes pasen por esa casilla mientras esté activa, tienen un intervalo de tiempo por el cual turnas en ser visibles o no.</li>
  </ul>
</div>

<h3 align=center> Trampas: </h3>
<div align="justify">
  Las trampas se encuentran en el tablero desde el inicio del juego, pero no son visibles a no ser que la ficha del Ladrón use su habilidad para verlas (Las trampas desactivadas se ven con una X sobre ellas). La mayoría de las trampas se activan al pasar sobre ellas, pero las trampas que confieren estados tienen cierta probabilidad de solo activarse al mover el jugador a esa casilla en específico. Siempre existe una posibilidad de que la trampa no se active en dependencia de la velocidad de la ficha. Actualmente hay 5 tipos de trampas:
  <ul>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/cells/SpikeTrap.png" width="50"> Trampa de Espinas: Reduce la vida actual de la ficha.</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/cells/PrisonTrap.png" width="50"> Trampa Prisión: Impide que la ficha continue su movimiento.</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/cells/FireTrap.png" width="50"> Trampa de Hielo: Reduce la vida actual de la ficha y le confiere el estado de Congelado en un valor aleatorio entre 1-5.</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/cells/IceTrap.png" width="50"> Trampa de Espinas: Reduce la vida actual de la ficha.</li>
    <li><img src="https://github.com/karlaYisel/MazeRunner/blob/main/MazeRunner.WebApp/wwwroot/img/cells/PoisonTrap.png" width="50"> Trampa de Veneno: Reduce la vida actual de la ficha y le confiere el estado de Envenenado en un valor aleatorio entre 1-5.</li>
  </ul>
</div>

<h3 align=center> Menús del Juego: </h3>
<div align="justify">
  <ul>
    <li>Menú de la Ficha: Aparecen las estadísticas principales de la ficha seleccionada. Se puede seleccionar una ficha en dos casos: 1- Se encuentra en la opción de seleccionar una ficha de algún jugador donde puede seleccionar una de las fichas de este para realizarle alguna acción, 2- No se requiere tener una ficha seleccionada, caso en el que puede seleccionar cualquier ficha y ver sus estadísticas, pero no interactuar con ella.</li>
    <li>Menú del Jugador: Se muestra el jugador al que le corresponde jugar. Si el jugador no es un Bot ni NPC, aparece un menú inicial con los botones de Seleccionar Token y Pasar Turno, al seleccionar una ficha aparecen las acciones que se pueden hacer con ella como Mover, Atacar, Usar Habilidad y también opciones de Atrás y Pasar Turno (cada acción se puede realizar solo una vez por turno en cada ficha y la de Usar Habilidad cuando se cumpla su tiempo de enfriamiento), mientras se esté realizando una acción no habrá botón alguno si la acción es auto conclusiva o los botones de Atrás y Pasar Turno si es requerido que el propio jugador le ponga fin , en todo momento la opción Pasar Turno le cede el control al jugador siguiente.</li>
  </ul>
</div>

<h2 align=center> Proceso de desarrollo: </h2>
<div align="justify">
  <p>Lo primera al comenzar a hacer el juego fue crear una lógica que facilitara su posterior desarrollo, así empezó la creación de toda la carpeta MazeRunner.Core y justamente con la creación de las clases Cell y Maze (Aprovechando así las ventajas de un lenguaje de programación orientado a objetos).</p>
    <h3 align=center>Clase Cell:</h3> 
    <ul>
      <li>Representación de las casillas del laberinto, facilita la interacción con la estructura de la casilla y los objetos dentro de ella. Contiene como propiedades su posición X y Y, un valor de visitada (Usada para el algoritmo de creación del laberinto), un diccionario para sus paredes (Se usó esta estructura de datos para almacenar las 4 paredes en una sola propiedad donde se pudiera acceder a su valor), y una referencia a un objeto contenido en ella que posteriormente seria de la clase Interactive, junto a una propiedad de si está marcada para efectos visuales en la interfaz gráfica.</li>
    </ul>
	  <h3 align=center>Clase Maze:</h3>
    <ul>
      <li>Representación del laberinto, conteniendo todos los métodos necesarios para su creación y modificación. Consta con propiedades referidas a su ancho, largo y una matriz de casillas que es el tablero en sí. Con métodos para inicializar las casillas, generar el laberinto (Empleando un método conocido como BackTracking) al que posteriormente se le incluiría una condición para generar ciclos en los caminos de forma aleatoria, y otros métodos adicionales como los que dada una casilla devuelve la lista de casillas adyacentes a ella que: no hayan sido visitadas, tengan paredes; y romper la pared entre dos casillas dadas las mismas si son adyacentes. Posteriormente se agregaron métodos para quitar todos los interactuables en las casillas, regenerar las paredes, quitar todas las paredes, hacer un nuevo laberinto, obtener casillas adyacentes sin paredes y comprobar si una casilla es de este laberinto. (Algunos métodos fueron creados, pero no se emplearán durante el juego, como el caso de regenerar laberinto, esto es debido a que se crearon para facilitar algunas ideas para el juego que luego no fueron implementadas).</li>
    </ul>
	<p>Una vez con estas clases empezadas se creó una aplicación de consola para probar el funcionamiento de las mismas sin necesidad de tener hecha la interfaz gráfica, como su funcionamiento y estructura no afecta al juego en sí se omitirán explicaciones sobre la misma en el presente documento, de todas formas, en los commits se puede presenciar estos.</p>
  <p>Sin embargo, por cuestiones de mejor entendimiento de la estructura del proyecto, esto generó una necesidad de separar el programa de consola y las clases hechas en el Core en carpetas (y namespaces) distintos manteniendo un modelo de “arquitectura limpia” que se mantendría durante el resto del proyecto (O al menos se intentó mantener).</p>
	<p>Lo siguiente en desarrollar fueron la clase de Interactive y sus descendientes (Empleando herencia y polimorfismo), además de la clase Player. Empleando enums para almacenar los tipos de Interactive, Obstacle, Trap… y estados. (Esta estructura de datos fue usada para acceder a nombres de objetos o propiedades sin correr el riesgo de llamar algo inexistente por error, además de que posteriormente en la generación de los objetos de forma aleatoria serian útiles para elegir que objeto crear)</p>
	  <h3 align=center>Clase Interactive:</h3> 
    <ul>
      <li>Clase abstracta (Para que no pudiera existir una instancia solo Interactive, así se podían definir propiedades útiles que se pudieran heredar por clases distintas sin correr el riego de tener algo que no sea clasificado como objeto, trampa, …).  Esta tendría una propiedad boolena de su estado actual (Active o Inactive) y un método para cambiar la misma.</li>
    </ul>
		<h3 align=center>Clase Obstacle:</h3> 
    <ul>
      <li>Clase abstracta, inicialmente conteniendo a la clase NPC (Esto cambiaria después al descubrir mayor compatibilidad de esta con la clase Character). Posteriormente se incluye una propiedad de demora para referirse a la cantidad de velocidad requerida para pasar por él.</li>
    </ul>
		<h3 align=center>Clase TemporalWall:</h3>  
    <ul>
      <li>Representación de paredes que alternan su estado en un lapso de tiempo. Inicialmente solo con una propiedad referida a ese tiempo. Después se incorporaría un método que dado el turno actual calcula si debe ser activada o desactivada.</li>
    </ul>
		<h3 align=center>Clase Trap:</h3>  
    <ul>
      <li>Clase abstracta, inicialmente solo conteniendo a la clase SpikeTrap. Contiene una propiedad para saber si se activa al pasar o solo al pararse sobre ella, y métodos de intentar activar y activar, el segundo solo accesible dende esta clase o sus clases descendientes, para garantizar que no se active si no se cumplen las condiciones necesarias, y clasificada como override para que se pueda cambiar en las clases que heredan de esta. Posteriormente se le incluiría una propiedad de es visible para facilitar la habilidad del Ladrón, y un método para cambiar la visibilidad.</li>
    </ul>
		<h3 align=center>Clase SpikeTrap:</h3>  
    <ul>
      <li>Representación de las trampas de espinas, con una propiedad de cuanto daño hace. Posteriormente modificada su propiedad de activar para que si se cumplen las propiedades necesarias quite de visa 3 veces el numero obtenido de restarle a la cantidad de daño de la trampa la mitad de la defensa del personaje.</li>
    </ul>
		<h3 align=center>Clase Character:</h3>  
    <ul>
      <li>Clase abstracta, conteniendo las clases PlayableCharacter y NPC. Con propiedades referidas a la posición de la ficha y sus estadísticas, métodos como cambiar de posición, y ataque normal. Posteriormente se le agregarían las propiedades de está siendo apuntado, y los puntos restantes de cada efecto, junto a los métodos que los modifican.</li>
    </ul>
		<h3 align=center>Clase NPC:</h3>  
    <ul>
      <li>Inicialmente abstracta (esto se modificó posteriormente), con los tipos Passive, Neutral y Aggressive. Posteriormente se le agregaría la propiedad de tipo de NPC.</li>
    </ul>
		<h3 align=center>Clase PlayableCharacter:</h3>  
    <ul>
      <li>Clase abstracta, inicialmente con la clase Hero. Con propiedades de tipo de ataque, tiempo de enfriamiento de la habilidad y ultimo turno usando la habilidad, junto a un método para activar la habilidad. Se le agregarían propiedades de si ha atacado, se ha movido y métodos de que hacer en un nuevo turno, para renovar las propiedades anteriores, y se movió y atacó.</li>
    </ul>
		<h3 align=center>Clase Hero:</h3>  
    <ul>
      <li>Representación de la ficha Héroe. Con su método de habilidad especial definido para hacer un ataque más potente (Multiplicado por 5 en lugar de por 3 como en su ataque normal).</li>
    </ul>
		<h3 align=center>Clase Player:</h3>  
    <ul>
      <li>Representación de los jugadores. Con propiedades de nombre y lista de fichas, junto a métodos para modificar estos.</li>
    </ul>
	<p>Se puede notar que no se creó más de una representación de cada tipo de Interactive (menos de NPC), esto es debido a que se consideró que crear más sería simplemente agregar clases parecidas a las ya creadas y por eso se priorizo completar una versión básica jugable del proyecto para entonces agregar los demás tipos.</p>
	<p>Lo siguiente fue desarrollar una estructura que pudiera almacenar y modificar los datos del juego, lo cual primeramente se intento dentro de la misma clase Maze, pero por mayor entendimiento del código, y mantener una estructura limpia donde las clases no tuvieran responsabilidades más allá de las relacionadas con su propio funcionamiento, se modifico esto a un gestor del juego (GameSystem) conformado por un GameManager y otras clases relacionadas al ataque, movimiento…</p>
		<h3 align=center>Clase GameManager:</h3>  
    <ul>
      <li>Contiene la información general del juego: Jugadores Activos, Jugadores no Activos (Bots y NPC), laberinto, puntos de salida, meta y condiciones iniciales como cantidad de fichas, trampas, obstáculos, NPCs entre otros. Métodos enfocados a la inicialización del juego, gestión de eventos y otros mas generales como obtener los personajes en una casilla, la casilla inicial de un personaje, número del jugador correspondiente a un token, estabilizar la vida de un personaje y estabilizar sus efectos.</li>
    </ul>
	<p>Esta clase emplea delegados y eventos, su implementación facilitaría notificar eventos importantes a otras clases como es el caso de un cambio en el tablero para la posterior interfaz gráfica.</p>
		<h3 align=center>Clase GeneratorManager:</h3> 
    <ul>
      <li>Encargado de gestionar la generación de los objetos que intervienen en el laberinto (Interactuables, NPCs y las fichas de los jugadores) empleando un método que, dada una lista de nombres de clases, crea una instancia de una de ellas.</li>
    </ul>
		<h3 align=center>Clase MovementManager:</h3>  
    <ul>
      <li>Encargado de gestionar el movimiento de las fichas por el laberinto, con un método para hacer un movimiento aleatorio dada una ficha y otros para mover la ficha hasta la casilla dada (Este método incluye un retardo de movimiento para agregarle suavidad al mismo y controla las trampas por las que pase o si se llega a la meta), obtener camino óptimo, casillas alcanzables (Detecta casillas con Obstacle contando la cantidad de velocidad necesitada para pasar o si no se puede pasar), y cambiar las propiedades de marcada de las casillas para visualizar el movimiento en estas.</li>
    </ul>
		<h3 align=center>Clase AttackManager:</h3>  
    <ul>
      <li>Encargado de gestionar los ataques, con métodos para obtener los posibles oponentes según tipo de ataque y jugador de la ficha, así como posibles aliados (Empleado en la habilidad del Sanador), obtener las casillas a una distancia en especifica (Para el ataque del Arquero, no importando la cantidad de velocidad consumida sino solo la distancia) y marcar a las fichas a las que se está apuntando.</li>
    </ul>
		<h3 align=center>Clase NonActiveTurnManager:</h3>  
    <ul>
      <li>Encargado de realizar los turnos de las fichas que no son controladas por un jugador activo, por ahora solo gestionando a los NPCs. Consta de un método que dado un NPC decide su acción en dependencia de su tipo.</li>
    </ul>
	<p>Estas clases emplean un patrón de diseño conocido como Singleton, restringiendo la creación a una única instancia de estos objetos.</p>
	<p>Inicialmente la estructura generaba una dependencia circular entre la clase GameManager y los otros gestores que necesitaban algo de este, pero después de una refactorización del código en las mismas se mejoró la distribución de los métodos y resolvió dicho problema.</p>
	<p>Finalmente, con una lógica estructurada y funcional se comenzó a desarrollar la interfaz visual. El por qué se usó web fue en gran medida para facilitar su desarrollo al funcionar en gran variedad de dispositivos y actualización prácticamente automática, con un mejor control sobre los estilos, permitiendo una interfaz más amigable con el usuario; pero la principal razón fue simplemente aprender a trabajar con Blazor WebAssembly.</p>
		<h3 align=center>Interfaz Visual:</h3>  
    <ul>
      <li>Se crearon componentes como mensaje emergente (Mensaje y botones configurables), cuadro de artículos (Para mostrar imágenes de fichas y jugadores en los menús) y componente de casilla (Mostrar imágenes referentes a casilla, interactuables, fichas e información de la ficha referentes a la casilla asignada), al ser frecuentemente empleados en las distintas páginas. En las paginas iniciales se recopila información sobre el juego y se envían al GameManager y GeneratorManager, para luego ser usada en la página GameDisplay donde el usuario puede interactuar con los datos del juego mediante los distintos gestores. Imágenes acordes a la temática del juego y un estilo “responsive” para acomodar la disposición en dependencia del tamaño de la pantalla.</li>
    </ul>
	<p>Inicialmente se limitó este desarrollo a lo mínimo requerido para tener una versión funcional del juego, luego de completarse la lógica del Core se termina la interfaz gráfica y se modifican los estilos para una visualización más agradable.</p>
	<p>Durante este desarrollo surgió un problema en el renderizado de la página, enfocado al movimiento de las fichas, debido a que Blazor acumula llamados de renderizado hasta tener todos sus procesos libres por optimización, pero esto interfería con los efectos agregados, por lo que se tuvo que declarar asíncronos a gran parte de los métodos del Core, permitiendo cederle la prioridad al motor de renderizado cuando era requerido.</p>
	<p>Lo último en añadirse fueron las restantes clases de Obstacle, Trap y PlayableCharacter:</p>
		<h3 align=center>Clase PrisonTrap:</h3>  
    <ul>
      <li>Representante de las trampas prisión, su método de activación solo retorna true si se cumplen las condiciones, y el método de movimiento se detiene al recibir esa respuesta.</li>
    </ul>
		<h3 align=center>Clase FireTrap:</h3>  
    <ul>
      <li>Representante de las trampas de fuego, aumenta la propiedad de pasos restantes quemado y afecta la vida de la ficha. El método de movimiento detecta si ese estado está activo y quita vida por cada paso de la ficha.</li>
    </ul>
		<h3 align=center>Clase IceTrap:</h3>  
    <ul>
      <li>Representante de las trampas de hielo, aumenta la propiedad de turnos restantes congelado y afecta la vida de la ficha. El método de estabilizar efectos detecta si ese estado está activo y a la hora de realizar una acción en GameDisplay notifica que no es posible.</li>
    </ul>
		<h3 align=center>Clase PoisonTrap:</h3>  
    <ul>
      <li>Representante de las trampas de veneno, aumenta la propiedad de turnos restantes envenenado y afecta la vida de la ficha. El método de estabilizar efecto detecta si ese estado está activo y quita a la ficha.</li>
    </ul>
		<h3 align=center>Clase TemporalObstacle:</h3>  
    <ul>
      <li>Representante de los obstáculos temporales, funcionan parecido a la clase TemporalWall con la diferencia de que se puede pasar, pero consume más velocidad.</li>
    </ul>
		<h3 align=center>Clase PermanentObstacle:</h3>  
    <ul>
      <li>Representante de los obstáculos permanentes, solo disponibles los de retraso 2 para que cualquier ficha pueda pasarlos.</li>
    </ul>
		<h3 align=center>Clase Thief:</h3>  
    <ul>
      <li>Representación de la ficha Ladrón. Con su método de habilidad especial definido para activar la propiedad de visibilidad de trampas cercanas o cambiar estado de la trampa seleccionada.</li>
    </ul>
		<h3 align=center>Clase Healer:</h3>  
    <ul>
      <li>Representación de la ficha Sanador. Con su método de habilidad especial definido para aumentar la vida de aliados en su rango de ataque.</li>
    </ul>
		<h3 align=center>Clase Paladin:</h3>  
    <ul>
      <li>Representación de la ficha Paladín. Con su método de habilidad especial definido para aumentar su defensa durante el tiempo de recarga de su habilidad.</li>
    </ul>
		<h3 align=center>Clase Archer:</h3>  
    <ul>
      <li>Representación de la ficha Ladrón. Con su método de habilidad especial definido para usar su ataque normal, pero se toman sus oponentes hasta un rango de 10 casillas de distancia.</li>
    </ul>
</div>
