namespace GameNamespace;

using ToolNamespace;
using GuerreroNamespace;
using BattleNamespace;
using System.Text;
using System.Text.Json;
using Microsoft.VisualBasic;
using System.Text.Json.Serialization;

enum GameStates{
    Menu,Seleccionar_personaje,Info,Quit,
    Battle,
    Game_over,
    Salir_juego
}
static class Game{
    //Definicion de variables
    static GameStates estadoActual;
    static public (ConsoleColor bg, ConsoleColor fg) consoleColor = (bg: ConsoleColor.Black, fg: ConsoleColor.DarkCyan);

    static List<GuerreroInfo> allWarriors = new List<GuerreroInfo>();

    static List<Planeta> allPlanets;
    static Guerrero jugador;

    public static async Task GameInit(int xres, int yres)
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(xres,yres);
        Console.OutputEncoding = Encoding.UTF8;
        Console.BackgroundColor = consoleColor.bg;
        Console.ForegroundColor = consoleColor.fg;
        //CARGAMOS LOS GUERREROS
        foreach (string filepath in Directory.EnumerateFiles("Personajes","*.json")){
            string contenido = File.ReadAllText(filepath);
            GuerreroInfo w = JsonSerializer.Deserialize<GuerreroInfo>(contenido);
            allWarriors.Add(w);
        }
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
        Console.WriteLine(new string(' ',(105-7)/2)+" Jugar\n");
        Console.WriteLine(new string(' ',(105-11)/2)+" Acerca de\n");
        Console.WriteLine(new string(' ',(105-7)/2)+" Salir\n");
        ConsoleKey k;
        int opcion=0;
        bool updateMenu=true;
        
        while(true){
            if(updateMenu){
                opcion%=3;
                switch(opcion){
                    case 0:
                        Console.SetCursorPosition(48,8);
                        Console.Write("■");
                        Console.SetCursorPosition(46,10);
                        Console.Write("*");
                        Console.SetCursorPosition(48,12);
                        Console.Write("*");
                        break;
                    case 1:
                        Console.SetCursorPosition(48,8);
                        Console.Write("*");
                        Console.SetCursorPosition(46,10);
                        Console.Write("■");
                        Console.SetCursorPosition(48,12);
                        Console.Write("*");
                        break;
                    case 2:
                        Console.SetCursorPosition(48,8);
                        Console.Write("*");
                        Console.SetCursorPosition(46,10);
                        Console.Write("*");
                        Console.SetCursorPosition(48,12);
                        Console.Write("■");
                        break;
                }
                updateMenu=false;
            }
            k = Console.ReadKey(true).Key;
            switch(k){
                case ConsoleKey.DownArrow:
                    opcion++;
                    updateMenu = true;
                    break;
                case ConsoleKey.UpArrow:
                    opcion = (opcion>0) ? opcion-1:2;
                    updateMenu=true;
                    break;
            }
            if(k==ConsoleKey.Enter) break;
        }

        //Dependiendo de la opcion, elegimos el próximo estado.
        //Esto funciona por como está dispuesto
        return (GameStates) (opcion+1);
    }
    
    //Estados de los menús.
    static GameStates seleccionarPersonajeState(){
        Caja cajaSeleccionadora = new Caja(2,1,101,11);
        Text.WriteCenter("Selecciona un personaje",Console.WindowWidth);
        (int x, int y) cursorPos = cajaSeleccionadora.CursorWritter;

        for (int i = 1; i <= 20; i++)
        {
            Console.SetCursorPosition(cursorPos.x,cursorPos.y);
            Console.WriteLine(allWarriors[i-1].nombre);
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
                Console.Write(allWarriors[opciones.anterior].nombre);
                
                Console.CursorLeft = cajaSeleccionadora.CursorWritter.Left + (opciones.actual/5)*26;
                Console.CursorTop = cajaSeleccionadora.CursorWritter.Top + (opciones.actual%5)*2;
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(allWarriors[opciones.actual].nombre);
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
        jugador = new Guerrero(allWarriors[opciones.actual]);
        jugador.Information.ataque=0;

        return GameStates.Battle;
    }
    static GameStates infoState(){
        return GameStates.Menu;
    }
    static GameStates quitState(){
        //26 = (105 - 52)/2
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
        //Elegimos 5 planetas de la API.
        
        Guerrero enemigo = new Guerrero(allWarriors[0]); //No los modifica de manera directa
        var proximo_estado = Battle.Start(jugador,enemigo); //El jugador es pasado como referencia
        //El jugador podría no ser pasado como referencia y utilizar Game.jugador (haciendolo publico)
        //Y solamente pasar el enemigo como nuevo parametro
        return proximo_estado;
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

        return (continuar) ? GameStates.Seleccionar_personaje:GameStates.Salir_juego;
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
                case GameStates.Game_over:
                    estadoActual = gameOverState();
                    break;
                case GameStates.Salir_juego:
                    salir=true;
                    break;
            }        

        }
    }

    //FUNCION QUE ME RETORNA UN DICCIONARIO CON 
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
}

class Planeta    
{
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

}
