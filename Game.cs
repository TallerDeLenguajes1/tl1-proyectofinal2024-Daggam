namespace GameNamespace;

using System.Text;
using ToolNamespace;
using GuerreroNamespace;
using System.Text.Json;
using System.Runtime.CompilerServices;

static class Game{
    enum GameStates{
        Menu,Seleccionar_personaje,Info,Quit,
        Battle
    }
    //Definicion de variables
    static GameStates estadoActual;
    static (ConsoleColor bg, ConsoleColor fg) consoleColor = (bg: ConsoleColor.Black, fg: ConsoleColor.DarkCyan);

    static List<GuerreroInfo> allWarriors = new List<GuerreroInfo>();
    static Guerrero jugador;
    static void menuState(){
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
        cambiarEstado((GameStates) (opcion+1));
    }
    
    static void battleState(){
        Guerrero enemigo = new Guerrero(allWarriors[0]);
        Battle batalla = new Battle(jugador,enemigo);
        
    }
    
    static void infoState(){

    }
    
    static void quitState(){
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
            }else if(k == ConsoleKey.Enter)
            {
                break;
            }
        }

        if(!salir){
            cambiarEstado(GameStates.Menu);
        }
    }
    
    static void seleccionarPersonajeState(){
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
        cambiarEstado(GameStates.Battle);

    }
    
    public static void GameInit(int xres, int yres)
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
        //Meter una introducción para dar a conocer los controles.
        cambiarEstado(GameStates.Menu);
        
    }

    static void cambiarEstado(GameStates nuevoEstado){
        Console.Clear();
        //Función de entrada.
        //Funcion de salida.
        switch(nuevoEstado){
            case GameStates.Menu:
                menuState();
                break;
            case GameStates.Seleccionar_personaje:
                seleccionarPersonajeState();
                break;
            case GameStates.Info:
                infoState();
                break;
            case GameStates.Quit:
                quitState();
                break;
            case GameStates.Battle:
                battleState();
                break;
        }        
        estadoActual = nuevoEstado;
    }
}

class Battle{
    Guerrero jugador;
    Guerrero enemigo;

    BattleStates estado_actual;
    Caja caja_batalla;
    enum BattleStates{
        Init,Turno_jugador, Golpear, Ataques_ki, Cargar_ki, Turno_enemigo
    }


    public Battle(Guerrero jugador, Guerrero enemigo){
        //Por ahora los defino nomás.
        this.jugador = jugador;
        this.enemigo = enemigo;
        cambiarEstado(BattleStates.Init);
    }
    public void initState(){
        //SE INICIALIZA LA UI | Barra: ▓ Foreground ; ▒ Background
        caja_batalla = new Caja(17,1,70,9);
        //VIDA
        jugador.Salud /= 4 ; 
        int barraWidth = 25;
        float jugadorBarra = (float) jugador.Salud/jugador.Information.salud_max * barraWidth; 
        float enemigoBarra = (float) jugador.Salud/jugador.Information.salud_max * barraWidth; 
        Console.SetCursorPosition(3,12);
        Console.Write(new string('▓',(int) Math.Ceiling(jugadorBarra)) + new string('▒',barraWidth - (int) Math.Ceiling(jugadorBarra) ));
        Console.CursorLeft = Console.WindowWidth-3-25;
        Console.Write(new string('▓',25));

        //KI
        Console.SetCursorPosition(3,13);
        Console.Write("■ ■ ■ ■ ■");
        Console.CursorLeft = Console.WindowWidth-3-25;
        Console.Write("■ ■ ■ ■ ■");



        Console.SetCursorPosition(3,14);
        Text.WriteCenter("(Vos)",25);
        Console.CursorLeft = Console.WindowWidth-3-25;
        Text.WriteCenter("(CPU)",25);

        Console.SetCursorPosition(3,15);
        Text.WriteCenter(jugador.Information.nombre,25);
        Console.CursorLeft = Console.WindowWidth-3-25;
        Text.WriteCenter(enemigo.Information.nombre,25);

        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+1+2,caja_batalla.CursorWritter.Top+1);
        Console.WriteLine("Golpear"+new string(' ',20-2)+"Técnicas" + new string(' ',20-2) + "Cargar ki");
        
        while (true)
        {
            
        }
    }
    void cambiarEstado(BattleStates nuevoEstado){
        estado_actual = nuevoEstado;
        switch(nuevoEstado){
            case BattleStates.Init:
                initState();
                break;
        }
    }

    void updateVida(Guerrero w){
        int barraWidth = 25;
        float wBarra = (float) w.Salud/w.Information.salud_max * barraWidth; 
        if(w.Equals(jugador)){
            Console.SetCursorPosition(3,12);
        }else if(w.Equals(enemigo)){
            Console.SetCursorPosition(Console.WindowWidth - barraWidth - 3,12);
        }
        Console.Write(new string('▓',(int) Math.Ceiling(wBarra)) + new string('▒',barraWidth - (int) Math.Ceiling(wBarra)));
    }
}