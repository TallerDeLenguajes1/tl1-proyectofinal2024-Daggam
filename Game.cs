namespace GameNamespace;

using ToolNamespace;
using GuerreroNamespace;
using BattleNamespace;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EntrenamientoNamespace;

enum GameStates{
    Menu,Nueva_partida,Cargar_partida,Info,Quit,
    Seleccionar_personaje,Battle,Entrenamiento,
    Game_over,Final,
    Salir_juego
}
static class Game{
    //Definicion de variables
    static GameStates estadoActual;
    static public (ConsoleColor bg, ConsoleColor fg) consoleColor = (bg: ConsoleColor.Black, fg: ConsoleColor.DarkCyan);

    //Variable relacionadas a los guerreros.
    static List<string> allWarriorsPaths = new List<string>();
    
    //Variables relacionadas al jugador y la partida
    public static SaveStates partida_actual;
    public static Guerrero jugador;
    static Dictionary<char,List<int>> guerrerosPorDerrotar;

    //Variables relacionadas a los planetas
    static List<Planeta> allPlanets;
    public static List<Planeta> AllPlanets { get => allPlanets;}

    public static async Task GameInit(int xres, int yres)
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(xres,yres);
        Console.OutputEncoding = Encoding.UTF8;
        Console.BackgroundColor = consoleColor.bg;
        Console.ForegroundColor = consoleColor.fg;
        //CARGAMOS LOS GUERREROS
        allWarriorsPaths = Directory.EnumerateFiles("Personajes","*.json").ToList();
        // foreach (string filepath in ){
        //     string contenido = File.ReadAllText(filepath);
        //     GuerreroInfo w = JsonSerializer.Deserialize<GuerreroInfo>(contenido);
        //     allWarriors.Add(w);
        // }
        //Esperamos hasta que carguen los planetas
        Console.SetCursorPosition(0,8);
        Text.WriteCenter("CARGANDO API... ESPERE UNOS SEGUNDOS...",xres);
        allPlanets = await getPlanetsAsync();
        if(allPlanets!=null){
            //Meter una introducción para dar a conocer los controles.
            iniciarMaquina(GameStates.Menu);
        }else{
            Console.Clear();
            Console.Write("LA INFORMACIÓN NO HA SIDO CARGADA CORRECTAMENTE.\n[ENTER PARA SALIR]");
            while(Console.ReadKey(true).Key!=ConsoleKey.Enter);
        }
    }

    static GameStates menuState(){
        const string titulo = @"
██████╗ ██████╗  █████╗  ██████╗  ██████╗ ███╗   ██╗    ███████╗██╗███╗   ███╗
██╔══██╗██╔══██╗██╔══██╗██╔════╝ ██╔═══██╗████╗  ██║    ██╔════╝██║████╗ ████║
██║  ██║██████╔╝███████║██║  ███╗██║   ██║██╔██╗ ██║    ███████╗██║██╔████╔██║
██║  ██║██╔══██╗██╔══██║██║   ██║██║   ██║██║╚██╗██║    ╚════██║██║██║╚██╔╝██║
██████╔╝██║  ██║██║  ██║╚██████╔╝╚██████╔╝██║ ╚████║    ███████║██║██║ ╚═╝ ██║
╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝    ╚══════╝╚═╝╚═╝     ╚═╝
        ";
        Console.WriteLine(titulo);
        Console.MoveBufferArea(0,0,79,14,13,0);
        Caja cajaEmergente = new Caja(41,8,22,9);
        cajaEmergente.Escribir("NUEVA PARTIDA\n\nCARGAR PARTIDA\n\nINFORMACIÓN",2,1);
        int opciones = 0;
        bool updateOpciones = true;
        
        while (true)
        {
            if(updateOpciones){
                StringBuilder str = new StringBuilder(" \n\n \n\n ") ;
                str[opciones*3] = '■';
                cajaEmergente.Escribir(str.ToString(),0,1);
                updateOpciones = false;
            }

            ConsoleKey k = Console.ReadKey(true).Key;
            if(k==ConsoleKey.DownArrow){
                opciones = (opciones<2) ? opciones+1:0;
                updateOpciones =true;
            }else if(k == ConsoleKey.UpArrow){
                opciones = (opciones>0) ? opciones - 1:2;
                updateOpciones =true;
            }else if(k == ConsoleKey.Enter) break;
        }
        //Dependiendo de la opcion, elegimos el próximo estado.
        //Esto funciona por como está dispuesto
        return (GameStates) (opciones+1);
    }
    
    //Estados de los menús.
    static GameStates seleccionarPartida(){
        //CARGA DE PARTIDA
        List<SaveStates> partidas = obtenerPartidas();
        List<String> partidasLabel = new List<string>(3);
        Caja cajaEmergente = new Caja(27,3,50,11);
        string msj = (estadoActual==GameStates.Nueva_partida) ? "Selecciona una ranura para guardar tu progreso:":"Selecciona la partida a jugar:";
        GameStates proximo_estado = (estadoActual==GameStates.Nueva_partida) ? GameStates.Seleccionar_personaje:GameStates.Entrenamiento;
        cajaEmergente.Escribir(msj,(cajaEmergente.Width-2-msj.Length)/2,1);
        Console.SetCursorPosition(0,16);
        Text.WriteCenter("[PRESIONA X PARA REGRESAR AL MENÚ]",105);
        //Iniciamos las partidas cargadas donde corresponden
        for (int i = 0; i < partidas.Count; i++)
        {
            if(partidas[i].Jugador == null){
                partidasLabel.Add(String.Format("{0,27}{1,19}","[VACÍO]"," "));
            }else{
                string formatJugadores = string.Format("[{0,-20}|{1,-5}]",
                                                        partidas[i].Jugador.Information.nombre,
                                                        $"LV: {partidas[i].Jugador.Entrenamiento.Nivel}");
                partidasLabel.Add(String.Format("{0,-17}  {1,-27}",
                partidas[i].Time.ToString("(dd/MM/yy, HH:mm)"),
                formatJugadores));

            }
            cajaEmergente.Escribir(partidasLabel[i],0,(2*i+3));
        }
        (int actual,int anterior) opciones = (0,0);
        bool actualizarSeleccion = true;
        while (true){
            if(actualizarSeleccion){
                cajaEmergente.Escribir(partidasLabel[opciones.anterior],0,(2*opciones.anterior+3));
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                cajaEmergente.Escribir(partidasLabel[opciones.actual],0,(2*opciones.actual+3));
                Console.BackgroundColor = consoleColor.bg;
                Console.ForegroundColor = consoleColor.fg;
                actualizarSeleccion=false;
            }
            ConsoleKey k = Console.ReadKey(true).Key;
            if(k == ConsoleKey.DownArrow){
                opciones.anterior = opciones.actual;
                opciones.actual = (opciones.actual < 2) ? opciones.actual+1:0;
                actualizarSeleccion=true;
            }else if(k==ConsoleKey.UpArrow){
                opciones.anterior = opciones.actual;
                opciones.actual = (opciones.actual > 0) ? opciones.actual-1:2;
                actualizarSeleccion=true;
            }else if(k== ConsoleKey.X){
                proximo_estado = GameStates.Menu;
                break;
            }else if(k==ConsoleKey.Enter){
                break;
            }
        };

        partida_actual = partidas[opciones.actual];
        if(proximo_estado==GameStates.Entrenamiento){
            if(partida_actual.Jugador == null){
                return GameStates.Cargar_partida;
            }else{
                jugador = partida_actual.Jugador;
                guerrerosPorDerrotar = partida_actual.EnemigosPorDerrotar.ToDictionary();
                Entrenamiento.Reset();
            }
        }else{
            guerrerosPorDerrotar = new Dictionary<char, List<int>>{
                                        {'F',new List<int> {12,8}},
                                        {'M',new List<int> {6,7,3,4,9,14,15,16}},
                                        {'D',new List<int> {0,1,2,5,10,11,13,17,18,19}},
                                    };
        }
        return proximo_estado;
    }
    static GameStates seleccionarPersonajeState(){
        Caja cajaSeleccionadora = new Caja(2,1,101,11);
        Text.WriteCenter("Selecciona un personaje",Console.WindowWidth);
        (int x, int y) cursorPos = cajaSeleccionadora.CursorWritter;
        List<string> nombreGuerreros = new List<string>();

        foreach (string path in allWarriorsPaths)
        {
            nombreGuerreros.Add(getGuerreroInfo(path).nombre);
        }
        for (int i = 1; i <= 20; i++)
        {
            Console.SetCursorPosition(cursorPos.x,cursorPos.y);
            Console.WriteLine(nombreGuerreros[i-1]);
            if(i%5==0){
                cursorPos.x +=26;
                cursorPos.y = cajaSeleccionadora.CursorWritter.Top;
            }else{
                cursorPos.y+=2;   
            }
        }

        (int anterior,int actual) opciones= (0,0);
        bool updateSelectores = true;
        while (true)
        {
            if(updateSelectores){
                Console.CursorLeft = cajaSeleccionadora.CursorWritter.Left + (opciones.anterior/5)*26;
                Console.CursorTop = cajaSeleccionadora.CursorWritter.Top + (opciones.anterior%5)*2;
                Console.Write(nombreGuerreros[opciones.anterior]);
                
                Console.CursorLeft = cajaSeleccionadora.CursorWritter.Left + (opciones.actual/5)*26;
                Console.CursorTop = cajaSeleccionadora.CursorWritter.Top + (opciones.actual%5)*2;
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(nombreGuerreros[opciones.actual]);
                Console.BackgroundColor = consoleColor.bg;
                Console.ForegroundColor = consoleColor.fg;
                updateSelectores=false;
            }

            ConsoleKey k = Console.ReadKey(true).Key;
            opciones.anterior = opciones.actual;
            switch(k){
                case ConsoleKey.RightArrow:
                    opciones.actual = (opciones.actual > 14) ? opciones.actual-15:opciones.actual+5;
                    break;
                case ConsoleKey.LeftArrow:
                    opciones.actual = (opciones.actual<5) ? opciones.actual+15:opciones.actual-5;
                    break;
                case ConsoleKey.DownArrow:
                    opciones.actual = (opciones.actual%5 == 4) ? opciones.actual-4:opciones.actual+1;
                    break;
                case ConsoleKey.UpArrow:
                    opciones.actual = (opciones.actual%5 == 0) ? opciones.actual+4:opciones.actual-1;;
                    break;

            }
            if(k==ConsoleKey.Enter){
                break;
            }
            updateSelectores=true;
        }
        //Podría agregar un estado para que confirme su personaje
        //Busqueda de personaje.
        jugador = new Guerrero(getGuerreroInfo(allWarriorsPaths[opciones.actual]));
        //ELIMINO DE LA LISTA DE GUERREROS POR DERROTAR AL PERSONAJE SELECCIONADO
        foreach(char dificultad in guerrerosPorDerrotar.Keys){
            if(guerrerosPorDerrotar[dificultad].Remove(opciones.actual)) break;
        }
        return GameStates.Entrenamiento;
    }
    static GameStates infoState(){
        Caja cajaEmergente = new Caja(22,3,60,10);
        int carteles = 0;
        bool actualizarCarteles = true;
        Console.SetCursorPosition(0,0);
        Console.SetCursorPosition(0,15);
        Console.Write(string.Format("  <{0,-49}{0,50}>  ","-----------"));
        Console.SetCursorPosition(0,15);
        Text.WriteCenter("[PRESIONA ENTER PARA IR AL MENÚ PRINCIPAL]",105);
        Console.SetCursorPosition(2,16);
        Text.WriteCenter("ANTERIOR",12);
        Console.SetCursorPosition(91,16);
        Text.WriteCenter("SIGUIENTE",12);
        string[,] textos = {
                                {
                                    "CONTROLES",
                                    "[Flechas de dirección]: Movimiento\n\n[ENTER]: Confirmar\n\n[Z]: Tecla de acción 1\n[X]: Tecla de acción 2\n[C]: Tecla de acción 3"
                                },
                                {
                                    "DESCRIPCIÓN",
                                    "Basado en el modo de juego Dragon Sim del videojuego\nFighter Arena: Dragon Ball Budokai Tenkaichi 3.\n\nEs un minijuego RPG que consta de entrenar uno de los\nveinte guerreros disponibles durante 10 días.\n\nFinalizado el entrenamiento, pasas a una etapa de\ncombate donde te enfrentaras a los guerreros restantes."
                                },
                                {
                                    "CRÉDITOS",
                                    "Creador: Benjamin Villa\n\nAsignatura: Taller de lenguajes I\n\nLenguaje de programación: C#\n\nSDK: .NET 8.0.101"
                                }
                            };
        while (true)
        {
            if(actualizarCarteles){       
                Text.borrarSeccion(0,1,105,0);
                Console.SetCursorPosition(0,1);
                Text.WriteCenter(textos[carteles,0],105);
                Text.borrarSeccion(23,4,58,7);
                cajaEmergente.Escribir(textos[carteles,1]);
                actualizarCarteles=false;
            }
            ConsoleKey k = Console.ReadKey(true).Key;
            if(k == ConsoleKey.RightArrow){
                carteles = (carteles < 2) ? carteles+1:0; 
                actualizarCarteles=true;
            }else if(k==ConsoleKey.LeftArrow){
                carteles = (carteles > 0) ? carteles-1:2; 
                actualizarCarteles=true;
            }else if(k== ConsoleKey.Enter) break;
        }
        return GameStates.Menu;
    }
    static GameStates quitState(){
        Caja cajaEmergente = new Caja(26,4,52,6);
        string msj = "¿Estás seguro que quieres salir?";
        Console.SetCursorPosition(cajaEmergente.X,cajaEmergente.Y+1);
        Text.WriteCenter(msj,cajaEmergente.Width);
        Console.SetCursorPosition(cajaEmergente.X+1,cajaEmergente.Y + cajaEmergente.Height-2);
        Text.WriteCenter("Sí",cajaEmergente.Width/2);
        Console.CursorLeft = cajaEmergente.X+cajaEmergente.Width/2;
        Text.WriteCenter("No",cajaEmergente.Width/2);

        bool updateOpciones=true;
        bool salir=true;
        (int x1,int x2) cursorPositions = (x1: cajaEmergente.X + cajaEmergente.Width/4 - 2,
                                           x2: cajaEmergente.X + 3*cajaEmergente.Width/4 - 3);
        while (true)
        {
            if(updateOpciones){
                if(salir){
                    Console.CursorLeft = cursorPositions.x1;
                    Console.Write("■");
                    Console.CursorLeft = cursorPositions.x2;
                    Console.Write("*");
                }else{
                    Console.CursorLeft = cursorPositions.x1;
                    Console.Write("*");
                    Console.CursorLeft = cursorPositions.x2;
                    Console.Write("■");
                }
                updateOpciones=false;
            }

            ConsoleKey k = Console.ReadKey(true).Key;
            if(k == ConsoleKey.LeftArrow){
                salir=true;
                updateOpciones=true;
            }else if(k == ConsoleKey.RightArrow){
                salir=false;
                updateOpciones=true;
            }else if(k == ConsoleKey.Enter) break;
        }

        return (salir) ? GameStates.Salir_juego: GameStates.Menu;
    
    }
    
    //Estado de batalla
    static GameStates battleState(){
        Random rnd = new Random();
        int enemigoID = 0;
        char dificultad = ' ';
        foreach(char d in guerrerosPorDerrotar.Keys){
            if(guerrerosPorDerrotar[d].Count!=0){
                enemigoID = guerrerosPorDerrotar[d][rnd.Next(guerrerosPorDerrotar[d].Count)];
                dificultad = d;
                break;
            }
        }
        Guerrero enemigo = new Guerrero(getGuerreroInfo(allWarriorsPaths[enemigoID])); //No los modifica de manera directa
        var proximo_estado = Battle.Start(enemigo); //El jugador es pasado como referencia
        //El jugador podría no ser pasado como referencia y utilizar Game.jugador (haciendolo publico)
        //Y solamente pasar el enemigo como nuevo parametro
        if(proximo_estado != GameStates.Game_over){
            guerrerosPorDerrotar[dificultad].Remove(enemigoID);
            bool termino = (guerrerosPorDerrotar['F'].Count==0 && guerrerosPorDerrotar['M'].Count==0 && guerrerosPorDerrotar['D'].Count==0);
            if(termino){
                proximo_estado = GameStates.Final;
            }else{
                guardarPartida();
            }
        }

        return proximo_estado;
    }
    
    //Estado de entrenamiento
    static GameStates entrenamientoState(){
        Entrenamiento.Start();
        return GameStates.Battle;
    }

    //Estado de gameover.
    static GameStates gameOverState(){
        Thread.Sleep(1000);
        Console.SetCursorPosition(0,8);
        Text.WriteCenter("Perdiste.",105);
        Thread.Sleep(1000);
        Console.SetCursorPosition(0,9);
        Text.WriteCenter("¿Deseas continuar?",105);
        Thread.Sleep(500);
        
        // Entrenamiento.Reset();
        bool continuar = true;
        bool updateOpciones = true;
        while (true)
        {
            if(updateOpciones){
                Console.SetCursorPosition(0,12);
                if(continuar){
                    Text.WriteCenter(String.Format("{0,-20}{1}","■ Sí","  No"),105);
                }else{
                    Text.WriteCenter(String.Format("{0,-20}{1}","  Sí","■ No"),105);
                }
                updateOpciones=false;
            }

            ConsoleKey k = Console.ReadKey(true).Key;
            if(k==ConsoleKey.RightArrow){
                updateOpciones=true;
                continuar = false;
            }else if(k==ConsoleKey.LeftArrow){
                updateOpciones=true;
                continuar=true;
            }else if(k==ConsoleKey.Enter) break;
        }

        return (continuar) ? GameStates.Menu:GameStates.Salir_juego;
    }
    
    //Estado Final.
    static GameStates finalState(){
        string [] dialogos_finales = {"Eres un guerrero formidable... ","Lograste acabar con una serie de enemigos poderosos.","¡Me alegra saber que el destino de la tierra está en buenas manos!","Contigo aquí... ","¡reinará la paz en la tierra durante muchos años!","¡Muchas gracias!"};
        
        
        Thread.Sleep(1500);
        int n = (105-dialogos_finales[0].Length)/2;
        Console.SetCursorPosition(n,6);
        Text.Start(dialogos_finales[0]);
        Thread.Sleep(1500);

        n=(105-dialogos_finales[1].Length)/2;
        Console.SetCursorPosition(n,7);
        Text.Start(dialogos_finales[1]);
        Thread.Sleep(2000);
        
        n=(105-dialogos_finales[2].Length)/2;
        Console.SetCursorPosition(n,9);
        Text.Start(dialogos_finales[2]);
        Thread.Sleep(2000);

        n=(105-(dialogos_finales[3].Length+dialogos_finales[4].Length))/2;
        Console.SetCursorPosition(n,10);
        Text.Start(dialogos_finales[3]);
        Thread.Sleep(1000);
        Text.Start(dialogos_finales[4]);
        Thread.Sleep(2000);

        n=(105-dialogos_finales[5].Length)/2;
        Console.SetCursorPosition(n,12);
        Text.Start(dialogos_finales[5]);

        while (Console.ReadKey(true).Key != ConsoleKey.Enter);
        return GameStates.Salir_juego;
    }
    static void iniciarMaquina(GameStates nuevoEstado){
        bool salir=false;
        estadoActual = nuevoEstado;
        while(!salir){
            Console.Clear();
            switch(estadoActual){
                case GameStates.Menu:
                    estadoActual = menuState();
                    break;
                case GameStates.Nueva_partida:
                case GameStates.Cargar_partida:
                    estadoActual = seleccionarPartida();
                    break;
                case GameStates.Seleccionar_personaje:
                    estadoActual = seleccionarPersonajeState();
                    break;
                case GameStates.Info:
                    estadoActual = infoState();
                    break;
                case GameStates.Quit:
                    estadoActual = quitState();
                    break;
                case GameStates.Battle:
                    estadoActual = battleState();
                    break;
                case GameStates.Entrenamiento:
                    estadoActual = entrenamientoState();
                    break;
                case GameStates.Game_over:
                    estadoActual = gameOverState();
                    break;
                case GameStates.Final:
                    estadoActual = finalState();
                    break;
                case GameStates.Salir_juego:
                    salir=true;
                    break;
            }        

        }
    }

    //Funcion que me retorna una lista de planetas para consumir una API Web.
    static async Task<List<Planeta>> getPlanetsAsync(){
        var url = "https://dragonball-api.com/api/planets?page=1&limit=5"; //Obtenemos los primeros 5 planetas.
        try{
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string contenido = await response.Content.ReadAsStringAsync();
            List<Planeta> planetas;
            using(JsonDocument jdoc = JsonDocument.Parse(contenido)){
                JsonElement items = jdoc.RootElement.GetProperty("items");
                planetas = JsonSerializer.Deserialize<List<Planeta>>(items);
            }
            return planetas;
        }catch(HttpRequestException e)
        {
            return null;
        }
    }

    //FUNCIONES MISCELÁNEAS.
    static GuerreroInfo getGuerreroInfo(string wPath){
        string contenido = File.ReadAllText(wPath);
        return JsonSerializer.Deserialize<GuerreroInfo>(contenido);
    }

    public static void guardarPartida(){
        var partidas = obtenerPartidas();
        partida_actual.Time = DateTime.Now;
        partida_actual.Jugador = jugador; //Podría ser mejor
        partida_actual.ComisteSemilla = Entrenamiento.comioSemilla;
        partida_actual.DiasEntrenamiento = Entrenamiento.dias_max;
        partida_actual.Interacciones = Entrenamiento.interacciones;
        partida_actual.EnemigosPorDerrotar = guerrerosPorDerrotar.ToDictionary();
        //PONER LOS ENEMIGOS QUE VAS DERROTANDO
        partidas[partida_actual.Id] = partida_actual;
        File.WriteAllText("Savestates.json",JsonSerializer.Serialize(partidas,new JsonSerializerOptions { WriteIndented = true }),Encoding.Unicode);
    }

    static List<SaveStates> obtenerPartidas(){
        var nombreArchivo = "Savestates.json";
        if(!File.Exists(nombreArchivo)){
            List<SaveStates> partidas = new List<SaveStates>(3);
            // SaveStates p = new SaveStates();
            for (int i = 0; i < 3; i++)
            {
                SaveStates p = new SaveStates{ Id = i };
                partidas.Add(p);
            }
            File.WriteAllText(nombreArchivo,JsonSerializer.Serialize(partidas,new JsonSerializerOptions { WriteIndented = true }),Encoding.Unicode);
            return partidas;
        }else{
            string jsonExterno = File.ReadAllText(nombreArchivo);
            return JsonSerializer.Deserialize<List<SaveStates>>(jsonExterno);
        }
    }
}

class Planeta    
{
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

}

class SaveStates{
    [JsonPropertyName("id")]
    public int Id{get;set;}
    [JsonPropertyName("time")]
    public DateTime Time {get; set;}
    [JsonPropertyName("comio_semilla")]
    public bool ComisteSemilla {get;set;}= false;
    [JsonPropertyName("dias_entrenamiento")]
    public int DiasEntrenamiento{get;set;} = 0;
    [JsonPropertyName("interacciones")]
    public int[] Interacciones {get;set;} = {0,0,0,0,0,0,0};
    [JsonPropertyName("enemigos_derrotados")]
    public Dictionary<char,List<int>> EnemigosPorDerrotar{get;set;}
    [JsonPropertyName("jugador")]
    public Guerrero Jugador {get; set;}
}