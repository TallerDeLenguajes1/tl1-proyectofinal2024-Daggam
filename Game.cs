namespace GameNamespace;

using System.Text;
using ToolNamespace;
using GuerreroNamespace;

static class Game{
    enum GameStates{
        Menu,Game,Info,Quit
    }
    //Definicion de variables
    static GameStates estadoActual;


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
            k = Console.ReadKey().Key;
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

    static void gameState(){
        Battle batalla = new Battle();
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

            ConsoleKey k = Console.ReadKey().Key;
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
    public static void GameInit(int xres, int yres)
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(xres,yres);
        Console.OutputEncoding = Encoding.UTF8;
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
            case GameStates.Game:
                gameState();
                break;
            case GameStates.Info:
                infoState();
                break;
            case GameStates.Quit:
                quitState();
                break;
        }        
        estadoActual = nuevoEstado;
    }
}

class Battle{
    Guerrero jugador;
    Guerrero enemigo;

    Caja caja_Batalla;
    enum BattleStates{
        Init, Golpear, Ataques_ki, Cargar_ki, Turno_enemigo
    }


    public Battle(){
        //Por ahora los defino nomás.
        jugador = new Guerrero();
        enemigo = new Guerrero();
        cambiarEstado(BattleStates.Init);
    }
    public void initState(){
        //SE INICIALIZA LA UI | Barra: ▓ Foreground ; ▒ Background
        caja_Batalla = new Caja(17,1,70,9);
        Console.SetCursorPosition(0,17);
        Console.Write("");
        while (true)
        {
            
        }
    }
    void cambiarEstado(BattleStates nuevoEstado){
        switch(nuevoEstado){
            case BattleStates.Init:
                initState();
                break;
        }
    }
}