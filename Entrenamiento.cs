using System.Text;
using GameNamespace;
using GuerreroNamespace;
using ToolNamespace;

namespace EntrenamientoNamespace;

enum EntrenamientoStates
{
    Menu,
    Entrenar_menu,Explorar,Dormir,Comer_semilla,
    Entrenar_ofensivo, Entrenar_defensivo, Entrenar_intensivo,
    Fin_entrenamiento, Salir_entrenamiento
}

static class Entrenamiento{
    enum SeleccionadorUpdate
    {
        Ataque,Defensa,Agresividad,Velocidad_carga
    }
    static EntrenamientoStates estado_actual;
    static Guerrero jugador;
    static Caja sideCaja;
    static Caja mainCaja;

    static int dias_max = 0;
    static int dias_entrenamiento = 0;
    static bool comioSemilla = false;

    static public void Reset(){
        dias_max = 0;
        dias_entrenamiento = 0;
        comioSemilla = false;
    }
    //Comienza el modo entrenamiento
    static public void Start(){
        jugador = Game.jugador;
        sideCaja = new Caja(1,1,16,16);
        mainCaja = new Caja(21,1,70,8);
        dias_max+=10;
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
        string str = "Entrenar\n\nExplorar \n\nDormir   \n\n";
        str+= (comioSemilla) ? new string(' ',6):"Comer ";
        sideCaja.Escribir(str,3,5);
        iniciarMaquina(EntrenamientoStates.Menu);
    }
    
    static EntrenamientoStates menuState(){
        sideCaja.Escribir($"Día {dias_entrenamiento:D2} / {dias_max}",0,1);
        bool actualizaMenu = true;
        int opciones = 0;
        string[] textosMain = {"[Entrenar]\n\nGanarás fuerza y resistencia a cambio de salud.",
                               "[Explorar]\n\n¡Puedes encontrar cosas interesantes si exploras el mundo!\nComo también puede ser una completa perdida de tiempo...",
                               "[Dormir]\n\nDescansar te ayudará a recuperarte más rápido...\nA costa de algo de tu entrenamiento.",
                               "[Semilla del ermitaño]\n\nComerás una semilla del ermitaño que recuperará tu salud por\ncompleto."};
        int opcLim = (comioSemilla) ? 2:3;
        while (true)
        {
            if(actualizaMenu){
                Text.borrarSeccion(22,2,68,5);
                StringBuilder str = new StringBuilder(" \n\n \n\n \n\n ") ;
                str[opciones*3] = '■';
                sideCaja.Escribir(str.ToString(),1,5);
                mainCaja.Escribir(textosMain[opciones]);
                if(jugador.Salud <= jugador.getSaludMax()*0.2 && (opciones==0 || opciones==1)){
                    mainCaja.Escribir($"[ NO LO PUEDES REALIZAR. TU SALUD ES MENOR AL 20% ]",0,5);
                }
                actualizaMenu=false;
            }
            ConsoleKey k = Console.ReadKey(true).Key;
            if(k == ConsoleKey.DownArrow){
                opciones = (opciones>=opcLim) ? 0:opciones+1;
                actualizaMenu=true;
            }else if(k == ConsoleKey.UpArrow){
                opciones = (opciones<=0) ? opcLim:opciones-1;
                actualizaMenu=true;
            }else if(k== ConsoleKey.Enter && !(jugador.Salud <= jugador.getSaludMax()*0.2 && (opciones==0 || opciones==1))) break;
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
        string str = "Entrenar\n\nExplorar \n\nDormir   \n\n";
        str+= (comioSemilla) ? new string(' ',6):"Comer ";
        sideCaja.Escribir(str,3,5);

        if(opciones!=3) {
            sideCaja.Escribir(" ",1,5+2*opciones);
            return (EntrenamientoStates) (opciones+5);
        }else{
            return EntrenamientoStates.Menu;
        }
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
                vida_quitada = (int) Math.Ceiling(jugador.getSaludMax() * 0.1);
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
                vida_quitada = (int) Math.Ceiling(jugador.getSaludMax() * 0.2);
                break;
        }
        updateCaracteristica(nuevo_ataque, SeleccionadorUpdate.Ataque);
        updateCaracteristica(nueva_defensa,SeleccionadorUpdate.Defensa);
            
        int salud_gastada = jugador.Salud - vida_quitada;
        int rate = 250;
        while(true){
            jugador.Salud = Math.Max(salud_gastada,jugador.Salud-rate);
            rate+=50;
            updateVida();
            if(jugador.Salud == salud_gastada){
                break;
            }
            Thread.Sleep(50);
        }
        mainCaja.EscribirAnim($"[ {nuevo_ataque:+#;-#;0} ATAQUE ] [ {nueva_defensa:+#;-#;0} DEFENSA ] [ -{((float) vida_quitada/jugador.getSaludMax())*100} % SALUD ]",0,3);
        Thread.Sleep(500);
        return EntrenamientoStates.Menu;
    }
    
    static EntrenamientoStates explorarState(){
        mainCaja.EscribirAnim("Explorando...");
        Thread.Sleep(1000);
        Random rnd = new Random();
        float buenometro = 0.1f;//rnd.NextSingle();
        
        if(buenometro < 0.2 && comioSemilla){
            mainCaja.EscribirAnim("¡Te encontraste con el Maestro Karin!",0,2);
            Thread.Sleep(1500);
            Text.borrarSeccion(22,2,68,3);

            mainCaja.EscribirAnim("¡Ah, joven guerrero!");
            Thread.Sleep(500);
            mainCaja.EscribirAnim("Te he observado mientras explorabas por estos parajes...",0,2);
            Thread.Sleep(1000);
            mainCaja.EscribirAnim("Eres fuerte y tienes un gran potencial.",0,3);
            Thread.Sleep(800);
            mainCaja.EscribirAnim("Por lo que he decidido compartir contigo algo muy valioso...",0,5);
            Thread.Sleep(1000);
            comioSemilla=false;
            sideCaja.Escribir("Comer",3,11);
            Text.borrarSeccion(22,2,68,5);
            mainCaja.EscribirAnim("¡Recibiste una semilla del ermitaño!");
            Thread.Sleep(500);

        }else{
            mainCaja.EscribirAnim("No encontraste nada...",0,2);
            Thread.Sleep(500);
        }

        return EntrenamientoStates.Menu;
    }

    static EntrenamientoStates dormirState(){
        mainCaja.EscribirAnim("¡Has descansado por un día y recuperaste salud!");
        float vida_factor = Math.Max(0.1f,(float) jugador.Salud/(jugador.getSaludMax()*2));
        int salud_ganada = (int)Math.Ceiling((vida_factor*jugador.getSaludMax()));
        int salud_total = Math.Min(jugador.Salud + salud_ganada,jugador.getSaludMax());
        while(true){
            jugador.Salud = Math.Min(salud_total,jugador.Salud+250);
            updateVida();
            if(jugador.Salud == salud_total){
                break;
            }
            Thread.Sleep(50);
        }
        mainCaja.EscribirAnim($"[ +{((float)salud_ganada/jugador.getSaludMax())*100:N0} % SALUD ]",0,1);
        Thread.Sleep(500);
        mainCaja.EscribirAnim("Tu cuerpo se debilita ligeramente...",0,2);

        updateCaracteristica(-1,SeleccionadorUpdate.Ataque);
        updateCaracteristica(-1,SeleccionadorUpdate.Defensa);

        mainCaja.EscribirAnim($"[ -1 ATAQUE ] [ -1 DEFENSA ]",0,3);
        Thread.Sleep(500);
        return EntrenamientoStates.Menu;
    }
    
    static EntrenamientoStates comerState(){
        sideCaja.Escribir(new string(' ',6),3,11);
        mainCaja.EscribirAnim("¡Gracias a la semilla del ermitaño recuperas toda la salud!");
        int rate = 250;
        while(true){
            jugador.Salud = Math.Min(jugador.getSaludMax(),jugador.Salud+rate);
            rate+=50;
            updateVida();
            if(jugador.Salud == jugador.getSaludMax()){
                break;
            }
            Thread.Sleep(50);
        }
        mainCaja.EscribirAnim("[ +100 % SALUD]",0,2);
        Thread.Sleep(500);
        comioSemilla=true;
        return EntrenamientoStates.Menu;
    }
    
    static EntrenamientoStates finEntrenamiento(){
        Text.borrarSeccion(22,2,68,5);
        mainCaja.EscribirAnim("Tus 10 días de entrenamiento han terminado.");
        Thread.Sleep(1000);
        mainCaja.EscribirAnim("¡Ha llegado el día de la batalla!",0,1);
        Thread.Sleep(500);
        return EntrenamientoStates.Salir_entrenamiento;
    }
    
    static void iniciarMaquina(EntrenamientoStates nuevo_estado){
        EntrenamientoStates estado_previo;
        estado_actual = nuevo_estado;
        bool salir = false;
        bool esperar = false;
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
                case EntrenamientoStates.Explorar:
                    estado_actual = explorarState();
                    break;
                case EntrenamientoStates.Dormir:
                    estado_actual = dormirState();
                    break;
                case EntrenamientoStates.Comer_semilla:
                    estado_actual = comerState();
                    break;
                case EntrenamientoStates.Fin_entrenamiento:
                    estado_actual = finEntrenamiento();
                    esperar=true;
                    break;
                case EntrenamientoStates.Salir_entrenamiento:
                    Console.SetCursorPosition(0,0);
                    for (int i = 0; i < 18; i++)
                    {
                        Console.Write(new string(' ',104)+"\n");
                        Thread.Sleep(20);
                    }
                    salir=true;
                    break;

            }
            
            if(estado_actual == EntrenamientoStates.Menu && (estado_previo != EntrenamientoStates.Entrenar_menu)){
                esperar=true;
                //Termina todo
                if(dias_entrenamiento==dias_max){
                    estado_actual = EntrenamientoStates.Fin_entrenamiento;  
                }else{
                    dias_entrenamiento++;
                }
                
            }
            if(esperar){
                mainCaja.Escribir("Presiona [ENTER] para continuar.",0,5);
                while(Console.ReadKey(true).Key != ConsoleKey.Enter);
                esperar=false;
            }
        }
    }
    
    static void updateVida(){
        float cocienteSalud = (float) jugador.Salud/jugador.getSaludMax() * 25;
        string barra = new string('▓',(int) Math.Ceiling(cocienteSalud)) + new string('▒',25 - (int) Math.Ceiling(cocienteSalud));
        Console.SetCursorPosition(28,10);
        string line = string.Format("{0}  {1,6:N0} / {2:N0}",barra,jugador.Salud,jugador.getSaludMax());
        Console.Write(line);
        
    }

    static void updateCaracteristica(int nuevo_valor,SeleccionadorUpdate seleccionador){
        string str="";
        (int left, int top) cursor = (0,0);
        int caracteristica_final = 0;
        int it=0;
        switch(seleccionador){
            case SeleccionadorUpdate.Ataque:
                caracteristica_final = Math.Min(jugador.Entrenamiento.Nivel*15,Math.Max(0,jugador.Entrenamiento.Ataque + nuevo_valor));
                str = $"Ataque: {{0:D2}} / {15*jugador.Entrenamiento.Nivel}";
                it = caracteristica_final-jugador.Entrenamiento.Ataque;
                jugador.Entrenamiento.Ataque = caracteristica_final;
                cursor = (21,14);
            break;
            case SeleccionadorUpdate.Defensa:
                caracteristica_final = Math.Min(jugador.Entrenamiento.Nivel*15,Math.Max(0,jugador.Entrenamiento.Defensa + nuevo_valor));
                str = string.Format("{0,40}",$"Defensa: {{0,3:D2}} / {15*jugador.Entrenamiento.Nivel}");
                it = caracteristica_final-jugador.Entrenamiento.Defensa;
                jugador.Entrenamiento.Defensa = caracteristica_final;
                cursor = (56,14);

            break;
            case SeleccionadorUpdate.Agresividad:
                caracteristica_final = Math.Min(jugador.Entrenamiento.Nivel*15,Math.Max(0,jugador.Entrenamiento.Agresividad + nuevo_valor));
                str = $"Agresividad: {{0:D2}} / {15*jugador.Entrenamiento.Nivel}";
                it = caracteristica_final-jugador.Entrenamiento.Agresividad;
                jugador.Entrenamiento.Agresividad = caracteristica_final;
                cursor = (21,16);
            break;
            case SeleccionadorUpdate.Velocidad_carga:
                caracteristica_final = Math.Min(jugador.Entrenamiento.Nivel*15,Math.Max(0,jugador.Entrenamiento.Velocidad_carga + nuevo_valor));
                str = string.Format("{0,40}",$"Velocidad de carga: {{0,3:D2}} / {15*jugador.Entrenamiento.Nivel}");
                it = caracteristica_final-jugador.Entrenamiento.Velocidad_carga;
                jugador.Entrenamiento.Velocidad_carga = caracteristica_final;
                cursor = (56,16);
            break;
            }


        for (int i = Math.Abs(it); i > 0; i--)
        {
            Console.SetCursorPosition(cursor.left,cursor.top);
            Console.Write(string.Format(str,caracteristica_final+Math.Sign(it)*(1-i)));
            Thread.Sleep(50);
        }
    }
}