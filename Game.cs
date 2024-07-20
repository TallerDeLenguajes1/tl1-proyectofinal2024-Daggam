namespace GameNamespace;

using System.Text;
using ToolNamespace;
using GuerreroNamespace;
using System.Text.Json;

static class Game{
    enum GameStates{
        Menu,Seleccionar_personaje,Info,Quit,
        Battle
    }
    //Definicion de variables
    static GameStates estadoActual;
    static public (ConsoleColor bg, ConsoleColor fg) consoleColor = (bg: ConsoleColor.Black, fg: ConsoleColor.DarkCyan);

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

    int barraSaludWidth = 25;
    enum BattleStates{
        Init,Turno_jugador,Turno_enemigo, Golpear, Tecnicas, Cargar_ki,
        Enemigo_derrotado,Jugador_derrotado,
        Salir_batalla
    }

    public Battle(Guerrero jugador, Guerrero enemigo){
        //Por ahora los defino nomás.
        this.jugador = jugador;
        this.enemigo = enemigo;
        
        iniciarMaquina(BattleStates.Init);
    }
    
    // ESTADOS DEL JUGADOR
    BattleStates initState(){
        //SE INICIALIZA LA UI | Barra: ▓ Foreground ; ▒ Background
        caja_batalla = new Caja(17,1,70,9);
        //VIDA
        updateVida(jugador);
        updateVida(enemigo);

        //KI
        updateKi(jugador);
        updateKi(enemigo);

        Console.SetCursorPosition(3,14);
        Text.WriteCenter("(Vos)",barraSaludWidth);
        Console.CursorLeft = Console.WindowWidth-barraSaludWidth-3;
        Text.WriteCenter("(CPU)",barraSaludWidth);

        Console.SetCursorPosition(3,15);
        Text.WriteCenter(jugador.Information.nombre,barraSaludWidth);
        Console.CursorLeft = Console.WindowWidth-barraSaludWidth-3;
        Text.WriteCenter(enemigo.Information.nombre,barraSaludWidth);

        return BattleStates.Turno_jugador;
    }

    BattleStates turnoJugadorState(){
        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+3,caja_batalla.CursorWritter.Top+1);
        Console.Write("Golpear"+new string(' ',18)+"Técnicas" + new string(' ',18) + "Cargar ki");
        int opciones = 0;
        bool updateOpciones = true;
        while (true)
        {
            if(updateOpciones){
                switch(opciones){
                    case 0:
                        Console.CursorLeft = caja_batalla.CursorWritter.Left+1;
                        Console.Write("■");
                        Console.CursorLeft += 24;
                        Console.Write(" ");
                        Console.CursorLeft += 25;
                        Console.Write(" ");
                        break;
                    case 1:
                        Console.CursorLeft = caja_batalla.CursorWritter.Left+1;
                        Console.Write(" ");
                        Console.CursorLeft += 24;
                        Console.Write("■");
                        Console.CursorLeft += 25;
                        Console.Write(" ");
                        break;
                    case 2:
                        Console.CursorLeft = caja_batalla.CursorWritter.Left+1;
                        Console.Write(" ");
                        Console.CursorLeft += 24;
                        Console.Write(" ");
                        Console.CursorLeft += 25;
                        Console.Write("■");
                        break;
                }
                updateOpciones=false;
            }
            ConsoleKey k = Console.ReadKey(true).Key;
            if(k == ConsoleKey.LeftArrow){
                opciones = (opciones <= 0) ? 2:opciones-1;
                updateOpciones=true;
            }else if(k== ConsoleKey.RightArrow){
                opciones = (opciones >= 2) ? 0:opciones+1;
                updateOpciones=true;
            }else if(k == ConsoleKey.Enter){
                break;
            }
        }
        
        Console.CursorLeft = caja_batalla.CursorWritter.Left;
        Console.Write(new string(' ',caja_batalla.Width-3));
        return (BattleStates) opciones+3;
    }

    BattleStates golpearState(){
        
        Random rand = new Random();

        caja_batalla.Escribir("Decides atacar al enemigo...\n¡Atacas con una ráfaga de golpes! [Presiona repetidamente Z y X]");
        Console.SetCursorPosition(28,12);
        Text.WriteCenter("GOLPES DADOS:",49);
        Console.SetCursorPosition(28,14);
        Text.WriteCenter("DAÑO GENERADO:",49);

        int golpesDados = 0;
        int damageUI = 0;
        bool interrumpir=false;
        int numeroInterrupcion = rand.Next(enemigo.Information.agresividad*100,enemigo.Information.agresividad*250);
        Timer timerInterrumpir = new Timer( _ => interrumpir=true,null,numeroInterrupcion,Timeout.Infinite);
        
        bool apretarZ=true;
        bool actualizarUI=true;

        while(!interrumpir){
            if(Console.KeyAvailable){
                ConsoleKey k = Console.ReadKey(true).Key;
                if((k == ConsoleKey.Z && apretarZ) || (k== ConsoleKey.X && !apretarZ)){
                    golpesDados++;
                    damageUI += jugador.Information.ataque - enemigo.Information.defensa;
                    enemigo.Salud = Math.Max(enemigo.Salud - (jugador.Information.ataque - enemigo.Information.defensa),0);
                    //ACTUALIZAMOS UI
                    actualizarUI=true;
                    apretarZ=!apretarZ;
                }
            }

            if(actualizarUI){
                Console.SetCursorPosition(28,13);
                Text.WriteCenter(golpesDados.ToString(),49);
                Console.SetCursorPosition(28,15);
                Text.WriteCenter(damageUI.ToString(),49);
                updateVida(enemigo);
                actualizarUI=false;
             }
        }
        timerInterrumpir.Dispose();
        string[] textos = {"¡El enemigo logra escapar!","¡El enemigo te mando a volar!",
                            "El enemigo no soporto los golpes...","El ki del enemigo se desvanece..."};
        if(enemigo.Salud > 0){
            caja_batalla.Escribir(textos[rand.Next(0,2)],0,4);
            return BattleStates.Turno_enemigo;
        }else{
            caja_batalla.Escribir(textos[rand.Next(2,4)],0,4);
            return BattleStates.Enemigo_derrotado;
        }
    }

    BattleStates tecnicasState(){
        int opciones = 0;
        bool updateOpciones = true;
        // Console.CursorLeft = caja_batalla.CursorWritter.Left;
        Console.SetCursorPosition(caja_batalla.CursorWritter.Left,caja_batalla.CursorWritter.Top);
        Console.Write(string.Format("{0,-35}{1,30}","Técnicas","Cantidad de ki"));
        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+3,caja_batalla.CursorWritter.Top+2);
        for (int i = 0; i <= 2; i++)
        {
            Console.Write("{0,-56}{1}",jugador.Information.tecnicas[i].nombre,jugador.Information.tecnicas[i].cantidad_ki_necesaria);
            Console.SetCursorPosition(caja_batalla.CursorWritter.Left+3,Console.CursorTop+2);
        }
        Console.SetCursorPosition(28,12);
        Text.WriteCenter("PRESIONA [X] PARA REGRESAR",49);
        while (true)
        {
            if(updateOpciones){
                switch(opciones){
                    case 0:
                        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+1,caja_batalla.CursorWritter.Top+2);
                        Console.Write("■");
                        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");
                        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");
                    break;
                    case 1:
                        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+1,caja_batalla.CursorWritter.Top+2);
                        Console.Write(" ");
                        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write("■");
                        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");
                    break;
                    case 2:
                        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+1,caja_batalla.CursorWritter.Top+2);
                        Console.Write(" ");
                        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");
                        Console.SetCursorPosition(caja_batalla.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write("■");
                    break;
                }
                updateOpciones=false;
            }

            ConsoleKey k = Console.ReadKey(true).Key;
            if(k == ConsoleKey.DownArrow){
                opciones = (opciones<2) ? opciones+1:0; 
                updateOpciones=true;
            }else if(k==ConsoleKey.UpArrow){
                opciones = (opciones>0) ? opciones-1:2;
                updateOpciones=true;
            }else if(k==ConsoleKey.Enter){
                 if(jugador.Ki >= jugador.Information.tecnicas[opciones].cantidad_ki_necesaria){
                    break;
                 }else{
                    Console.SetCursorPosition(28,13);
                    Text.WriteCenter("NO TIENES SUFICIENTE KI PARA USAR ESTA TECNICA",49);
                 }
            }else if(k==ConsoleKey.X){
                opciones=-1;
                break;
            }
        }
        //LIMPIO TODO ANTES DE SALIR
        Text.borrarSeccion(caja_batalla.CursorWritter.Left,caja_batalla.CursorWritter.Top,70-3,9-3);
        Text.borrarSeccion(28,12,49,4);
        if(opciones!=-1){
            jugador.Ki -= jugador.Information.tecnicas[opciones].cantidad_ki_necesaria;
            updateKi(jugador);
            caja_batalla.Escribir($"¡Has utilizado la técnica {jugador.Information.tecnicas[opciones].nombre}!");
            enemigo.Salud = Math.Max(enemigo.Salud - jugador.Information.tecnicas[opciones].ataque,0);
            updateVida(enemigo);
            Console.SetCursorPosition(28,12);
            Text.WriteCenter("DAÑO GENERADO:",49);
            int damageAnim = 0;
            while(damageAnim != jugador.Information.tecnicas[opciones].ataque){
                damageAnim = Math.Min(damageAnim+100,jugador.Information.tecnicas[opciones].ataque);
                Console.SetCursorPosition(28,13);
                Text.WriteCenter(damageAnim.ToString(),49);
                Thread.Sleep(50);
            }
            if(enemigo.Salud>0){
                return BattleStates.Turno_enemigo;
            }else{
                Random rnd = new Random();
                string[] textos = {"El enemigo no pudo contener tu ataque...","El ki del enemigo se desvanece..."};
                caja_batalla.Escribir(textos[rnd.Next(textos.Length)],0,4);
                return BattleStates.Enemigo_derrotado;
            }
        }else{
            return BattleStates.Turno_jugador;
        }
    }

    BattleStates cargarKiState(){
        caja_batalla.Escribir("Tratas de concentrarte...\nManten [C] para cargar ki.");
        
        Random rnd = new Random();
        int dueTimeInterrumpir = rnd.Next(15000/enemigo.Information.agresividad,30000/enemigo.Information.agresividad);
        bool interrumpir=false;
        bool actualizarNumero=true;
        Timer timerInterrupcion = new Timer( _=>interrumpir=true,null,dueTimeInterrumpir,Timeout.Infinite);
        
        Console.SetCursorPosition(28,12);
        Text.WriteCenter("CANTIDAD DE KI:",49);
        while(!interrumpir){
            if(Console.KeyAvailable){
                ConsoleKey k = Console.ReadKey(true).Key;
                if(k == ConsoleKey.C){
                    jugador.Ki = Math.Min(jugador.Ki + jugador.Information.velocidad_carga*0.00035f,5);
                    updateKi(jugador);
                    actualizarNumero=true;
                }
            }
            if(actualizarNumero){
                Console.SetCursorPosition(28,13);
                Text.WriteCenter(jugador.Ki.ToString("N2"),49);
                actualizarNumero=false;
            }
            if(jugador.Ki == 5) break;
            
        }
        timerInterrupcion.Dispose();
        if(interrumpir){
            string[] textos = {"¡El enemigo te mando a volar!","¡El enemigo lanza una ráfaga de aire!","¡El enemigo interrumpe tu carga!"}; 
            caja_batalla.Escribir(textos[rnd.Next(textos.Length)]+"\n\n",0,4);
        }else{
            caja_batalla.Escribir("¡Alcanzaste tu máximo poder!\n\n",0,4);
        }
        return BattleStates.Turno_enemigo;
    }

    //ESTADOS DEL ENEMIGO
    BattleStates turnoEnemigoState(){
        caja_batalla.Escribir("Ahora es el turno del enemigo.");
        Thread.Sleep(1000);
        Random rnd = new Random();
        int acciones = rnd.Next(2); // Atacar, (Ki_para_tecnica) ? tecnica:cargar_ki.
        updateKi(enemigo);
        //Usar una tecnica
        if(acciones==1){
            List<Tecnica> tecnicasDisponibles = new List<Tecnica>();
            foreach(var tecnica in enemigo.Information.tecnicas){
                if(enemigo.Ki >= tecnica.cantidad_ki_necesaria){
                    tecnicasDisponibles.Add(tecnica);
                }
            }

            if(tecnicasDisponibles.Count != 0){
                var tecnicaElegida = tecnicasDisponibles[rnd.Next(tecnicasDisponibles.Count)];
                enemigo.Ki -= tecnicaElegida.cantidad_ki_necesaria;
                updateKi(enemigo) ;
                caja_batalla.Escribir($"¡El enemigo utiliza la tecnica {tecnicaElegida.nombre}!",0,1);
                float totalTecnicaDamage = (float) tecnicaElegida.ataque/100;
                float tecnicaDamage = 0; 
                Console.SetCursorPosition(28,12);
                Text.WriteCenter("DAÑO RECIBIDO:",49);
                for (int i = 0; i < 100; i++)
                {
                    tecnicaDamage+=totalTecnicaDamage;
                    Console.SetCursorPosition(28,13);
                    Text.WriteCenter(tecnicaDamage.ToString("0"),49);
                    Thread.Sleep(25);
                }
                jugador.Salud = Math.Max(jugador.Salud-tecnicaElegida.ataque,0);
                updateVida(jugador);
            }else{ //CARGAR KI
                caja_batalla.Escribir("El enemigo decide cargar su ki...\n\nPuedes interrumpirlo cuando aparezca la leyenda [Z] [X] ó [C].",0,2);
                Thread.Sleep(1000);
                Console.SetCursorPosition(28,12);
                Text.WriteCenter("CANTIDAD DE KI:",49);

                ConsoleKey[] teclasDisponibles = {ConsoleKey.Z,ConsoleKey.X,ConsoleKey.C};
                ConsoleKey randomKey = teclasDisponibles[0];
                string consoleText = "";
                bool actualizarTecla=false;
                bool puedeInterrumpir=false;
                
                //Reloj
                System.Timers.Timer reloj = new System.Timers.Timer(rnd.Next(jugador.Information.agresividad*50,jugador.Information.agresividad*100));
                reloj.Elapsed += (sender,e) =>{
                    System.Timers.Timer t = (System.Timers.Timer) sender;
                    puedeInterrumpir=!puedeInterrumpir;
                    actualizarTecla=true;

                    if(puedeInterrumpir){
                        t.Interval = 400; //respuesta humana
                    }else{
                        t.Interval = rnd.Next(5000/jugador.Information.agresividad,10000/jugador.Information.agresividad);
                    }
                };
                reloj.AutoReset = true;
                reloj.Enabled=true;

                while (true)
                {
                    enemigo.Ki = Math.Min(enemigo.Ki + enemigo.Information.velocidad_carga*0.0000035f,5);
                    updateKi(enemigo);
                    Console.SetCursorPosition(28,13);
                    Text.WriteCenter(enemigo.Ki.ToString("N2"),49);
                    if(enemigo.Ki == 5)
                    {
                        consoleText = "No pudiste interrumpir la carga.";
                        break;
                    }
                    if (actualizarTecla)
                    {
                        Console.SetCursorPosition(28,14);
                        if(puedeInterrumpir){
                            randomKey = teclasDisponibles[rnd.Next(teclasDisponibles.Length)];
                            Text.WriteCenter($"[{randomKey}]",49);
                        }else{
                            Text.WriteCenter(new string(' ',3),49);
                        }
                        actualizarTecla=false;
                    }
                    if(puedeInterrumpir && Console.KeyAvailable){
                        var k = Console.ReadKey(true).Key;
                        if(k == randomKey){
                            consoleText = "¡Interrumpes la carga del enemigo con una ráfaga de aire!";
                            break;
                        }
                    }else if(Console.KeyAvailable){
                            Console.ReadKey(true);
                    }
                }
                //LIBERAMOS EL RELOJ.
                reloj.Dispose();
                //Borramos y mostramos el mensaje
                Text.borrarSeccion(caja_batalla.CursorWritter.Left,caja_batalla.CursorWritter.Top,70-3,9-3);
                Console.SetCursorPosition(28,14);
                Text.WriteCenter(new string(' ',3),49);
                caja_batalla.Escribir(consoleText);
            }
        }else 
        if(acciones == 0){ //ESTADO ATAQUE
            //Luego podría agregar una interrupción por parte del jugador.
            int cantidadGolpes = rnd.Next(20,30);
            caja_batalla.Escribir("¡El enemigo va atacar con una ráfaga de golpes!",0,1);
            int totalDamage = 0;
            Console.SetCursorPosition(28,12);
            Text.WriteCenter("DAÑO RECIBIDO:",49);
            for (int i = 0; i < cantidadGolpes; i++)
            {
                totalDamage+=enemigo.Information.ataque-jugador.Information.defensa;
                jugador.Salud = Math.Max(0,jugador.Salud-(enemigo.Information.ataque-jugador.Information.defensa));
                updateVida(jugador);
                Console.SetCursorPosition(28,13);
                Text.WriteCenter(totalDamage.ToString(),49);
                Thread.Sleep(50);
            }
        }
        Thread.Sleep(1500);
        string[] textos = {"Puedes contraatacar...","Tienes tiempo para actuar...",
                            "Tu cuerpo comienza a desvanecerse...","Ya no puedes seguir resistiendo..."};
        if(jugador.Salud > 0){
            caja_batalla.Escribir(textos[rnd.Next(0,2)]+"\n\n",0,4);
            return BattleStates.Turno_jugador;
        }else{
            caja_batalla.Escribir(textos[rnd.Next(2,4)]+"\n\n",0,4);
            return BattleStates.Jugador_derrotado;
        }
    }

    //ESTADOS DE VICTORIA Y DERROTA.
    bool salirBatalla(){
        string texto = "ESCAPAS DE LA BATALLA...";
        if(estado_actual == BattleStates.Enemigo_derrotado){
            texto = "¡GANASTE EL COMBATE!";
        }else if(estado_actual==BattleStates.Jugador_derrotado){
            texto = "PERDISTE EL COMBATE...";
        }

        caja_batalla.Escribir(texto,(caja_batalla.Width - texto.Length)/2 -3,3);
        while(Console.ReadKey(true).Key != ConsoleKey.Enter);
        Console.SetCursorPosition(0,0);
        for (int i = 0; i < 18; i++)
        {
            Console.Write(new string(' ',104)+"\n");
            Thread.Sleep(20);
        }
        return (estado_actual==BattleStates.Jugador_derrotado);
    }
    void iniciarMaquina(BattleStates estadoInicial){
        //SE INICIALIZA LA UI | Barra: ▓ Foreground ; ▒ Background
        BattleStates estado_previo;
        estado_actual = estadoInicial;
        bool gameOver;
        bool salir=false;
        while(!salir){
            estado_previo = estado_actual;
            //CAMBIO DE ESTADO
            switch (estado_actual){
            case BattleStates.Init:
                estado_actual = initState();
                break;
            case BattleStates.Turno_jugador:
                estado_actual = turnoJugadorState();
                break;
            case BattleStates.Golpear:
                estado_actual = golpearState();
                break;
            case BattleStates.Tecnicas:
                estado_actual = tecnicasState();
                break;
            case BattleStates.Cargar_ki:
                estado_actual = cargarKiState();
                break;
            case BattleStates.Turno_enemigo:
                estado_actual = turnoEnemigoState();
                break;
            case BattleStates.Enemigo_derrotado:
            case BattleStates.Jugador_derrotado:
                gameOver = salirBatalla();
                salir=true;
                break;
            }

            //Funciones de salida
            switch(estado_actual){
                case BattleStates.Turno_enemigo:case BattleStates.Turno_jugador:case BattleStates.Enemigo_derrotado:case BattleStates.Jugador_derrotado:
                    if(estado_previo != BattleStates.Turno_enemigo && (estado_actual == BattleStates.Turno_jugador) || salir ) break;
                    Thread.Sleep(1000);
                    while(Console.KeyAvailable) Console.ReadKey(true); //Limpio el buffer.
                    Console.SetCursorPosition(caja_batalla.CursorWritter.Left,caja_batalla.CursorWritter.Top+6);
                    Console.Write("Presiona [ENTER] para continuar.");
                    while(Console.ReadKey(true).Key != ConsoleKey.Enter);
                    Text.borrarSeccion(caja_batalla.CursorWritter.Left,caja_batalla.CursorWritter.Top,70-3,9-3);
                    Text.borrarSeccion(28,12,49,4);
                break;
            }
        }
    }
    
    //FUNCIONES DE ACTUALIZACIÓN DE UI.
    void updateVida(Guerrero w){
        float wBarra = (float) w.Salud/w.Information.salud_max * barraSaludWidth; 
        if(w.Equals(jugador)){
            Console.SetCursorPosition(3,12);
        }else if(w.Equals(enemigo)){
            Console.SetCursorPosition(Console.WindowWidth - barraSaludWidth - 3,12);
        }
        Console.Write(new string('▓',(int) Math.Ceiling(wBarra)) + new string('▒',barraSaludWidth - (int) Math.Ceiling(wBarra)));
    }
    void updateKi(Guerrero w){
        if(w.Equals(jugador)){
            Console.SetCursorPosition(3,13);
        }else if(w.Equals(enemigo)){
            Console.SetCursorPosition(Console.WindowWidth - barraSaludWidth - 3,13);
        }
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        for (int i = 0; i < (int) w.Ki; i++)
        {
            Console.Write("■");
            Console.CursorLeft +=1;
        }
        Console.ForegroundColor = Game.consoleColor.fg;

        for (int i = 0; i < (5-w.Ki); i++)
        {
            Console.Write("■");
            Console.CursorLeft +=1;
        }
    }

}