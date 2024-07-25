using GameNamespace;
using GuerreroNamespace;
using ToolNamespace;

namespace EntrenamientoNamespace;

enum EntrenamientoStates
{
    Menu,
    Entrenar_menu,Explorar,Dormir,Comer_semilla,
    Entrenar_ofensivo, Entrenar_defensivo, Entrenar_intensivo,
    Fin_entrenamiento
}

static class Entrenamiento{
    static EntrenamientoStates estado_actual;
    static Guerrero jugador;
    static Caja sideCaja;
    static Caja mainCaja;

    static int dias_entrenamiento;
    static decimal grado_cansancio = 0.0m;
    //Comienza el modo entrenamiento
    static public void Start(){
        jugador = Game.jugador;
        sideCaja = new Caja(1,1,16,16);
        mainCaja = new Caja(21,1,70,8);
        dias_entrenamiento = 1;
        // jugador.Salud /= 80; 
        //Printeamos la UI del personaje.
        Console.SetCursorPosition(21,10);
        Console.Write("Salud: ");
        updateVida();
        Console.SetCursorPosition(82,10);
        Console.Write($"Nivel {jugador.Entrenamiento.Nivel,3}");
        Console.SetCursorPosition(21,12);
        Text.WriteCenter("-- CARACTERISTICAS ADICIONALES--",70);
        Console.SetCursorPosition(21,14);
        Console.Write(string.Format("{0,-35}{1,35}",$"Ataque: {jugador.Entrenamiento.Ataque:D2} / {15*jugador.Entrenamiento.Nivel}",$"Defensa: {jugador.Entrenamiento.Defensa,3:D2} / {15*jugador.Entrenamiento.Nivel}"));
        Console.SetCursorPosition(21,16);
        Console.Write(string.Format("{0,-35}{1,35}",$"Agresividad: {jugador.Entrenamiento.Agresividad:D2} / {15*jugador.Entrenamiento.Nivel}",$"Velocidad de carga: {jugador.Entrenamiento.Velocidad_carga,3:D2} / {15*jugador.Entrenamiento.Nivel}"));
        // sideCaja.Escribir($"Día {dias_entrenamiento:D2} / 20",0,1);
        sideCaja.Escribir("Entrenar\n\nExplorar\n\nDormir\n\nComer",3,5);
        iniciarMaquina(EntrenamientoStates.Menu);
    }
    
    static EntrenamientoStates menuState(){
        if(dias_entrenamiento==10);//Termina todo
        sideCaja.Escribir($"Día {dias_entrenamiento:D2} / 10",0,1);
        bool actualizaMenu = true;
        int opciones = 0;
        while (true)
        {
            if(actualizaMenu){
                Text.borrarSeccion(22,2,68,5);
                switch (opciones)
                {
                    case 0:
                        sideCaja.Escribir("■\n\n \n\n \n\n ",1,5);
                        mainCaja.Escribir("[Entrenar]\n\nGanarás fuerza y resistencia a cambio de salud.");
                        break;
                    case 1:
                        sideCaja.Escribir(" \n\n■\n\n \n\n ",1,5);
                        mainCaja.Escribir("[Explorar]\n\n¡Puedes encontrar cosas interesantes si exploras el mundo!\nComo también puede ser una completa perdida de tiempo...");
                        break;
                    case 2:
                        sideCaja.Escribir(" \n\n \n\n■\n\n ",1,5);
                        mainCaja.Escribir("[Dormir]\n\nDescansar te ayudará a recuperarte más rápido...\nA costa de algo de tu entrenamiento.");
                        break;
                    case 3:
                        sideCaja.Escribir(" \n\n \n\n \n\n■",1,5);
                        mainCaja.Escribir("[Semilla del ermitaño]\n\nComerás una semilla del ermitaño que recuperará tu salud por\ncompleto.\n\nSOLO PUEDES COMER UNA SEMILLA EN TODA LA PARTIDA.");
                        break;
                }
                actualizaMenu=false;
            }
            ConsoleKey k = Console.ReadKey(true).Key;
            if(k == ConsoleKey.DownArrow){
                opciones = (opciones>=3) ? 0:opciones+1;
                actualizaMenu=true;
            }else if(k == ConsoleKey.UpArrow){
                opciones = (opciones<=0) ? 3:opciones-1;
                actualizaMenu=true;
            }else if(k== ConsoleKey.Enter) break;
        }

        Text.borrarSeccion(22,2,68,5);
        sideCaja.Escribir(" ",1,5+2*opciones);
        return (EntrenamientoStates) (opciones+1);
    }
    
    static EntrenamientoStates entrenarMenuState(){
        sideCaja.Escribir("Ent. Of.\n\nEnt. Def.\n\nEnt. Int.\n\nVolver",3,5);
        bool actualizaMenu = true;
        int opciones = 0;
        while (true)
        {
            if(actualizaMenu){
                Text.borrarSeccion(22,2,68,5);
                switch (opciones)
                {
                    case 0:
                        sideCaja.Escribir("■\n\n \n\n \n\n ",1,5);
                        mainCaja.Escribir("[Entrenamiento Ofensivo]\n\nEs un entrenamiento basado en el ataque.");
                        break;
                    case 1:
                        sideCaja.Escribir(" \n\n■\n\n \n\n ",1,5);
                        mainCaja.Escribir("[Entrenamiento Defensivo]\n\nEs un entrenamiento basado en la defensa.");
                        break;
                    case 2:
                        sideCaja.Escribir(" \n\n \n\n■\n\n ",1,5);
                        mainCaja.Escribir("[Entrenamiento Intensivo]\n\nEntrena tu cuerpo al límite. Consume mucha salud.");
                        break;
                    case 3:
                        sideCaja.Escribir(" \n\n \n\n \n\n■",1,5);
                        mainCaja.Escribir("[Volver]\n\nRegresa al menú de selección.");
                        break;
                }
                actualizaMenu=false;
            }
            ConsoleKey k = Console.ReadKey(true).Key;
            if(k == ConsoleKey.DownArrow){
                opciones = (opciones>=3) ? 0:opciones+1;
                actualizaMenu=true;
            }else if(k == ConsoleKey.UpArrow){
                opciones = (opciones<=0) ? 3:opciones-1;
                actualizaMenu=true;
            }else if(k== ConsoleKey.Enter) break;
        }
        Text.borrarSeccion(22,2,68,5);
        sideCaja.Escribir("Entrenar\n\nExplorar \n\nDormir   \n\nComer ",3,5);
        if(opciones!=3) {
            sideCaja.Escribir(" ",1,5+2*opciones);
            return (EntrenamientoStates) (opciones+5);
        }else{
            return EntrenamientoStates.Menu;
        }
    }
    
    static EntrenamientoStates dormirState(){
        mainCaja.EscribirAnim("¡Has descansado por un día y recuperaste salud!");
        float vida_factor = (float) jugador.Salud/jugador.Information.salud_max;
        int salud_ganada = (int)Math.Ceiling((vida_factor*jugador.Salud));
        int salud_total = Math.Min(jugador.Salud + salud_ganada,jugador.Information.salud_max);
        while(true){
            jugador.Salud = Math.Min(salud_total,jugador.Salud+250);
            updateVida();
            if(jugador.Salud == salud_total){
                break;
            }
            Thread.Sleep(50);
        }
        mainCaja.EscribirAnim($"[ +{((float)salud_ganada/jugador.Information.salud_max)*100:N0} % SALUD ]",0,1);
        Thread.Sleep(500);
        mainCaja.EscribirAnim("Tu cuerpo se debilita ligeramente...",0,2);

        jugador.Entrenamiento.Ataque = Math.Max(0,jugador.Entrenamiento.Ataque - 1);
        Console.SetCursorPosition(21,14);
        Console.Write(string.Format("{0,-35}",$"Ataque: {jugador.Entrenamiento.Ataque:D2} / {15*jugador.Entrenamiento.Nivel}"));
        Thread.Sleep(50);

        jugador.Entrenamiento.Defensa = Math.Max(0,jugador.Entrenamiento.Defensa - 1);
        Console.SetCursorPosition(56,14);
        Console.Write(string.Format("{0,35}",$"Defensa: {jugador.Entrenamiento.Defensa,3:D2} / {15*jugador.Entrenamiento.Nivel}"));
        Thread.Sleep(50);

        mainCaja.EscribirAnim($"[ -1 ATAQUE ] [ -1 DEFENSA ]",0,3);
        Thread.Sleep(500);
        dias_entrenamiento++;
        return EntrenamientoStates.Menu;
    }

    static EntrenamientoStates entrenarState(){
        Random rnd = new Random();
        string[] textos = {"Te has sobreentrenado.","No has obtenido resultados.", "Tu entrenamiento sirvio de algo.","¡Has despertado parte de tu poder oculto!"};
        float efectividad = rnd.NextSingle();
        int nuevo_ataque = 0, nueva_defensa = 0,vida_quitada = 0;
        mainCaja.EscribirAnim("Entrenando...");
        Thread.Sleep(1000);
        switch(estado_actual){
            case EntrenamientoStates.Entrenar_ofensivo:
            case EntrenamientoStates.Entrenar_defensivo:
                int principal = 0;
                if(efectividad < 0.1 ){
                    mainCaja.EscribirAnim(textos[3],0,2);
                    principal = rnd.Next(10,16);
                }else if( efectividad < 0.4){
                    mainCaja.EscribirAnim(textos[1],0,2);
                }else{
                    mainCaja.EscribirAnim(textos[2],0,2);
                    principal = rnd.Next(1,6);
                };
                vida_quitada = (int) Math.Ceiling(jugador.Information.salud_max * 0.1);
                if(estado_actual == EntrenamientoStates.Entrenar_defensivo){
                    nueva_defensa = principal;
                    nuevo_ataque--;
                }else{
                    nuevo_ataque = principal;
                    nueva_defensa--;
                }
                break;
            case EntrenamientoStates.Entrenar_intensivo:
                if(efectividad<0.5){
                    mainCaja.EscribirAnim(textos[0],0,2);
                    nueva_defensa = -rnd.Next(5,11);
                    nuevo_ataque = -rnd.Next(5,11);
                }else{
                    mainCaja.EscribirAnim(textos[3],0,2);
                    nueva_defensa = rnd.Next(5,11);
                    nuevo_ataque = rnd.Next(5,11);
                }
                vida_quitada = (int) Math.Ceiling(jugador.Information.salud_max * 0.2);
                break;
        }
        for (int i = Math.Abs(nuevo_ataque); i > 0; i--)
        {
            jugador.Entrenamiento.Ataque = Math.Min(Math.Max(0,jugador.Entrenamiento.Ataque + Math.Sign(nuevo_ataque)), 15* jugador.Entrenamiento.Nivel);
            Console.SetCursorPosition(21,14);
            Console.Write(string.Format("{0,-35}",$"Ataque: {jugador.Entrenamiento.Ataque:D2} / {15*jugador.Entrenamiento.Nivel}"));
            Thread.Sleep(50);
        }
        for (int i = Math.Abs(nueva_defensa); i > 0; i--)
        {
            jugador.Entrenamiento.Defensa = Math.Min(Math.Max(0,jugador.Entrenamiento.Defensa + Math.Sign(nueva_defensa)), 15* jugador.Entrenamiento.Nivel);
            Console.SetCursorPosition(56,14);
            Console.Write(string.Format("{0,35}",$"Defensa: {jugador.Entrenamiento.Defensa,3:D2} / {15*jugador.Entrenamiento.Nivel}"));
            Thread.Sleep(50);
        }
            
        int salud_gastada = jugador.Salud - vida_quitada;
        while(true){
            jugador.Salud = Math.Max(salud_gastada,jugador.Salud-250);
            updateVida();
            if(jugador.Salud == salud_gastada){
                break;
            }
            Thread.Sleep(50);
        }
        mainCaja.EscribirAnim($"[ {nuevo_ataque:+#;-#;0} ATAQUE ] [ {nueva_defensa:+#;-#;0} DEFENSA ]",0,3);
        Thread.Sleep(500);
        dias_entrenamiento++;
        return EntrenamientoStates.Menu;
    }
    static void iniciarMaquina(EntrenamientoStates nuevo_estado){
        EntrenamientoStates estado_previo;
        estado_actual = nuevo_estado;
        bool salir = false;
        while(!salir){
            estado_previo = estado_actual;
            switch(estado_actual){
                case EntrenamientoStates.Menu:
                    estado_actual = menuState();
                    break;
                case EntrenamientoStates.Entrenar_menu:
                    estado_actual = entrenarMenuState();
                    break;
                case EntrenamientoStates.Entrenar_ofensivo:
                case EntrenamientoStates.Entrenar_defensivo:
                case EntrenamientoStates.Entrenar_intensivo:
                    estado_actual = entrenarState();
                    break;
                case EntrenamientoStates.Dormir:
                    estado_actual = dormirState();
                    break;
            }
            if(estado_actual == EntrenamientoStates.Menu && (estado_previo != EntrenamientoStates.Entrenar_menu)){
                mainCaja.Escribir("Presiona [ENTER] para continuar.",0,5);
                while(Console.ReadKey(true).Key != ConsoleKey.Enter);
            }
        }
    }
    
    static void updateVida(){
        float cocienteSalud = (float) jugador.Salud/jugador.Information.salud_max * 25;
        string barra = new string('▓',(int) Math.Ceiling(cocienteSalud)) + new string('▒',25 - (int) Math.Ceiling(cocienteSalud));
        Console.SetCursorPosition(28,10);
        string line = string.Format("{0}  {1,6:N0} / {2:N0}",barra,jugador.Salud,jugador.Information.salud_max);
        Console.Write(line);
        
    }

    static void updateCaracteristicas(GuerreroEntrenamiento wc){

    }
}