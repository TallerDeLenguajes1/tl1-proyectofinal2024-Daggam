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
    public static bool comioSemilla = false;

    public static int[] interacciones = {0,0,0,0,0,0,0};
    static public void Reset(){
        dias_max = 0;
        dias_entrenamiento = 0;
        comioSemilla = false;
        for (int i = 0; i < interacciones.Length; i++)
        {
            interacciones[i] = 0;
        }
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
        Console.SetCursorPosition(21,12);
        Text.WriteCenter("-- CARACTERISTICAS ADICIONALES--",70);
        updateNivel(jugador.Entrenamiento.Nivel);
        string str = "Entrenar\n\nExplorar \n\nDormir   \n\n";
        str+= (comioSemilla) ? new string(' ',6):"Comer ";
        sideCaja.Escribir(str,3,5);
        int cota = jugador.Entrenamiento.Nivel*10;
        if(jugador.Entrenamiento.Ataque >=  cota|| jugador.Entrenamiento.Defensa >= cota) {
            sideCaja.Escribir($"Día {dias_entrenamiento:D2} / {dias_max}",0,1);
            mainCaja.EscribirAnim("¡Aumentaste de Nivel!\n\n[+1 NIVEL]");
            updateNivelAnim(1);
            Thread.Sleep(1500);
        }
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
        mainCaja.EscribirAnim($"[ {nuevo_ataque:+#;-#;0} ATAQUE ] [ {nueva_defensa:+#;-#;0} DEFENSA ] [ -{((float) vida_quitada/jugador.getSaludMax())*100:N0} % SALUD ]",0,3);
        Thread.Sleep(500);
        return EntrenamientoStates.Menu;
    }
    
    static EntrenamientoStates explorarState(){
        mainCaja.EscribirAnim("Explorando...");
        Thread.Sleep(1000);
        Random rnd = new Random();
        double buenometro = rnd.NextSingle();
        int eventoElegido=0;
        //Modificar todo para que sea una serie de eventos
        //Aumento de vida maxima -  Dende (Aumenta vida y vida máxima)
        //Aumento de vida - chichi +50% salud
        //Aumento de velocidaddeki - Mr.Popo
        //Aumento de Agresividad - Kaiosama (Entrenamiento)
        //Aumento de Nivel, salud máxima, ataque, defensa, agresividad, velocidaddeki - Anciano kaioshin
        //      Eventos --- Escenarios más probables
        //     (6,0.3f), //NADAAA -- 1er (0,0.3)
        //     (2,0.25f), //Mr.Popo -- 2do (0.3,0.55)
        //     (3,0.20f), //Kaiosama -- 3er (0.55,0.75)
        //     (1,0.15f), //Chichi -- 4to (0.75,0.9)
        //     (0,0.05f), //Dende -- 5to (0.9,0.95)
        //     (5,0.03f), //Karin -- 6to  (0.95,0.98)
        //     (4,0.02f), //Anciano Kaioshin -- 7mo (0.98,1)
        double[] probs = {0.35,0.25,0.20,0.1,0.05,0.03,0.02};
       
        double sumaProbs=0;
        for (int i = 0; i < probs.Length; i++)
        {
            sumaProbs+=probs[i];
            if(buenometro<=sumaProbs){
                eventoElegido=i;
                break;
            }
        }
        switch(eventoElegido){
            case 0: case 5 when !comioSemilla:
                mainCaja.EscribirAnim("No encontraste nada...",0,2);
                Thread.Sleep(1000);
                Text.borrarSeccion(22,2,68,3);
                mainCaja.EscribirAnim("Te debilitaste ligeramente...");
                updateCaracteristica(-1,SeleccionadorUpdate.Ataque);
                updateCaracteristica(-1,SeleccionadorUpdate.Defensa);
                AnimBarraVida(-(int) Math.Ceiling(jugador.getSaludMax() * 0.1));
                mainCaja.EscribirAnim($"[ -1 ATAQUE ] [ -1 DEFENSA ] [ -10 % SALUD ]",0,2);
                break;
            case 1:
                {
                    mainCaja.EscribirAnim("¡Te encuentras con Mr. Popo en el Palacio de Kamisama!",0,2);
                    Thread.Sleep(1500);
                    Text.borrarSeccion(22,2,68,3);
                    if(interacciones[1] == 0){
                        mainCaja.EscribirAnim("Veo que has llegado hasta aquí, guerrero.");
                        Thread.Sleep(500);
                        mainCaja.EscribirAnim("¿Qué es lo que buscas?",0,1);
                        Thread.Sleep(1000);
                        mainCaja.EscribirAnim("Ya veo... quieres dominar tu energía interior...",0,3);
                        Thread.Sleep(800);
                        mainCaja.EscribirAnim("No será fácil. Será un entrenamiento intenso y riguroso.",0,5);
                        Thread.Sleep(1000);
                        Text.borrarSeccion(22,2,68,5);
                    }
                    float prob = rnd.NextSingle();
                    int vel_carga = 0;
                    int vida_quitada=0;
                    string msjEntr="";
                    if(prob < 0.2)
                    { 
                        vel_carga = rnd.Next(5,11);
                        msjEntr = "¡Lograste sobrepasar tus limites!";
                        vida_quitada = (int) Math.Ceiling(jugador.getSaludMax() * 0.2);
                    }else{
                        vel_carga = rnd.Next(1,6);
                        msjEntr = "El entrenamiento sirvio de algo...";
                        vida_quitada = (int) Math.Ceiling(jugador.getSaludMax() * 0.1);
                    }

                    mainCaja.EscribirAnim(msjEntr);
                    updateCaracteristica(vel_carga,SeleccionadorUpdate.Velocidad_carga);
                    AnimBarraVida(-vida_quitada);
                    mainCaja.EscribirAnim($"[ +{vel_carga} VELOCIDAD DE CARGA ] [ -{((float) vida_quitada/jugador.getSaludMax())*100:N0} % SALUD ]",0,2);
                    break;
                }
            case 2:
            {
                mainCaja.EscribirAnim("¡Alguien se quiere comunicar contigo desde el más allá!",0,2);
                Thread.Sleep(1500);
                Text.borrarSeccion(22,2,68,3);
                if(interacciones[2]==0){
                    mainCaja.EscribirAnim("¡Joven guerrero!");
                    Thread.Sleep(500);
                    mainCaja.EscribirAnim("Soy Kaiosama, el Supremo Kai del Norte.",17);
                    Thread.Sleep(800);
                    mainCaja.EscribirAnim("Sé que muy pronto te enfrentarás a un enemigo...",0,1);
                    Thread.Sleep(500);
                    mainCaja.EscribirAnim("Por lo que he decido guiarte en el control de tu ser interior.",0,3);
                    Thread.Sleep(1000);
                    Text.borrarSeccion(22,2,68,5);
                }

                mainCaja.EscribirAnim("Aceptas la ayuda.");
                Thread.Sleep(1000);
                float prob = rnd.NextSingle();
                int agresividad = 0;
                int vida_quitada=0;
                string msjEntr="";
                if(prob < 0.2)
                { 
                    agresividad = rnd.Next(5,11);
                    msjEntr = "¡La meditación fue muy efectiva!";
                    vida_quitada = (int) Math.Ceiling(jugador.getSaludMax() * 0.2);
                }else{
                    agresividad = rnd.Next(1,6);
                    msjEntr = "La meditación sirvio de algo...";
                    vida_quitada = (int) Math.Ceiling(jugador.getSaludMax() * 0.1);
                }
                
                mainCaja.EscribirAnim(msjEntr,0,2);
                updateCaracteristica(agresividad,SeleccionadorUpdate.Agresividad);
                AnimBarraVida(-vida_quitada);
                mainCaja.EscribirAnim($"[ +{agresividad} AGRESIVIDAD ] [ -{((float) vida_quitada/jugador.getSaludMax())*100:N0} % SALUD ]",0,3);
                break;
            }
            case 3:
                {
                    mainCaja.EscribirAnim("¡Te encontraste con Chi-Chi!",0,2);
                    Thread.Sleep(1500);
                    Text.borrarSeccion(22,2,68,3);
                    mainCaja.EscribirAnim("Se nota que has entrenando bastante...");
                    Thread.Sleep(800);
                    mainCaja.EscribirAnim("No te exigas demasiado.",0,2);
                    Thread.Sleep(500);
                    mainCaja.EscribirAnim("Toma, come esto.",24,2);
                    int atq_g = rnd.Next(1,5);
                    int def_g = rnd.Next(1,5);
                    updateCaracteristica(atq_g,SeleccionadorUpdate.Ataque);
                    updateCaracteristica(def_g,SeleccionadorUpdate.Defensa);
                    AnimBarraVida((int) Math.Ceiling(jugador.getSaludMax() * 0.25));
                    Thread.Sleep(500);
                    mainCaja.EscribirAnim($"[ +{atq_g} ATAQUE] [ +{def_g} DEFENSA] [ +25 % SALUD ]",0,3);                    
                    break;
                }
            case 4:
                {
                    mainCaja.EscribirAnim("¡Te encontraste con Dende!",0,2);
                    Thread.Sleep(1500);
                    Text.borrarSeccion(22,2,68,3);
                    int posy = 2;
                    if(interacciones[4]==0){
                        mainCaja.EscribirAnim("¡Hola guerrero!");
                        Thread.Sleep(500);
                        mainCaja.EscribirAnim("¿Necesitas ayuda con tu entrenamiento?",0,2);
                        Thread.Sleep(800);
                        mainCaja.EscribirAnim("Bueno podría ayudarte, pero tendrás que hacer un gran sacrificio.",0,4);
                        Thread.Sleep(1000);
                        Text.borrarSeccion(22,2,68,4);
                        mainCaja.EscribirAnim("Aún así, ¿Estas dispuesto a correr ese riesgo?");
                    }else{
                        mainCaja.EscribirAnim("¡Ey! ¡Hola de nuevo!");
                        Thread.Sleep(500);
                        mainCaja.EscribirAnim("La oferta sigue en pie.",21);
                        Thread.Sleep(500);
                        mainCaja.EscribirAnim("¿Aún estas dispuesto a correr ese riesgo?",0,2);
                        posy = 4;
                    }
                    int opciones = 0;
                    bool actualizarMenu = true;
                    while(true){
                        if(actualizarMenu){
                            StringBuilder str = new StringBuilder( new string(' ',17) + "Sí" +new string(' ',34)+ "No");
                            str[15+opciones*36] = '■';
                            mainCaja.Escribir(str.ToString(),0,posy);
                            actualizarMenu=false;
                        }
                        ConsoleKey k = Console.ReadKey().Key;
                        if(k==ConsoleKey.RightArrow){
                            opciones = (opciones<1) ? opciones+1:0;
                            actualizarMenu =true;
                        }else if(k == ConsoleKey.LeftArrow){
                            opciones = (opciones>0) ? opciones - 1:1;
                            actualizarMenu =true;
                        }else if(k == ConsoleKey.Enter) break;
                    }
                    Text.borrarSeccion(22,2,68,5);
                    if(opciones==0){
                        mainCaja.EscribirAnim("Esta bien, te ayudaré con mis poderes.");
                        int salud_actual = jugador.Salud;
                        AnimBarraVida(-(int) Math.Ceiling(jugador.getSaludMax() * 0.35));
                        int salud_maxima_ganada = salud_actual - jugador.Salud;
                        jugador.Entrenamiento.Salud_max += salud_maxima_ganada;
                        updateVida();
                        Thread.Sleep(500);
                        mainCaja.EscribirAnim($"[ -35% SALUD ] [ +{salud_maxima_ganada:N0} SALUD MÁXIMA ]",0,1);            
                        Thread.Sleep(500);
                        mainCaja.EscribirAnim("Hice un intercambio de salud. Espero que te haya sido de ayuda.",0,3);
                    }else{
                        mainCaja.EscribirAnim("Entendido... Entonces mucho no podré hacer.");
                        Thread.Sleep(800);  
                        mainCaja.EscribirAnim("¡Exitos guerrero!",0,2);
                    }
                    break;
                }
            case 5:
                mainCaja.EscribirAnim("¡Te encontraste con el Maestro Karin!",0,2);
                Thread.Sleep(1500);
                Text.borrarSeccion(22,2,68,3);
                if(interacciones[5]==0){
                    mainCaja.EscribirAnim("¡Ah, joven guerrero!");
                    Thread.Sleep(500);
                    mainCaja.EscribirAnim("Te he observado mientras explorabas por estos parajes...",0,2);
                    Thread.Sleep(1000);
                    mainCaja.EscribirAnim("Eres fuerte y tienes un gran potencial.",0,3);
                    Thread.Sleep(800);
                    mainCaja.EscribirAnim("Por lo que he decidido compartir contigo algo muy valioso...",0,5);
                    Thread.Sleep(1000);
                }
                comioSemilla=false;
                sideCaja.Escribir("Comer",3,11);
                Text.borrarSeccion(22,2,68,5);
                mainCaja.EscribirAnim("¡Recibiste una semilla del ermitaño!");
                break;
            case 6:
                {
                    mainCaja.EscribirAnim("¡Te encontraste con el Anciano Kaioshin!",0,2);
                    Thread.Sleep(1000);
                    Text.borrarSeccion(22,2,68,3);
                    if(interacciones[6]==0){
                        mainCaja.EscribirAnim("¡Ah, un guerrero errante!");
                        Thread.Sleep(500);
                        mainCaja.EscribirAnim("Veo en tus ojos la chispa del potencial no descubierto...",0,1);
                        Thread.Sleep(800);
                        mainCaja.EscribirAnim("La promesa de un poder oculto que aguarda ser desatado.",0,3);
                        Thread.Sleep(500);
                        mainCaja.EscribirAnim("¡Muy bien! Preparémonos para desatar ese poder latente.",0,5);
                        Thread.Sleep(1000);
                        Text.borrarSeccion(22,2,68,5);
                    }
                    mainCaja.EscribirAnim("¡Has despertado tu poder oculto!");
                    int atq_g = rnd.Next(5,11);
                    int def_g = rnd.Next(5,11);
                    int agr_g = rnd.Next(5,11);
                    int carga_g = rnd.Next(5,11);
                    updateNivelAnim(1);
                    updateCaracteristica(atq_g,SeleccionadorUpdate.Ataque);
                    updateCaracteristica(def_g,SeleccionadorUpdate.Defensa);
                    updateCaracteristica(agr_g,SeleccionadorUpdate.Agresividad);
                    updateCaracteristica(carga_g,SeleccionadorUpdate.Velocidad_carga);
                    mainCaja.EscribirAnim($"[ + 1 NIVEL ] [ + {atq_g} ATAQUE ] [ + {atq_g} DEFENSA ]\n[ + {agr_g} AGRESIVIDAD ] [ + {carga_g} VELOCIDAD DE CARGA ]",0,2);
                    break;
                }
        }
        interacciones[eventoElegido] = (interacciones[eventoElegido]%5==0) ? 1:interacciones[eventoElegido] + 1; //Pongo una cota para que no sobrapase de 5.
        Thread.Sleep(500);
        return EntrenamientoStates.Menu;
    }

    static EntrenamientoStates dormirState(){
        mainCaja.EscribirAnim("¡Has descansado por un día y recuperaste salud!");
        float vida_factor = Math.Max(0.1f,(float) jugador.Salud/(jugador.getSaludMax()*2));
        int salud_ganada = (int)Math.Ceiling((vida_factor*jugador.getSaludMax()));
        AnimBarraVida(salud_ganada);
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
        AnimBarraVida(jugador.getSaludMax());
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

    static void updateNivelAnim(int nivel_ganado){
        int nivel_final = jugador.Entrenamiento.Nivel + nivel_ganado;
        for (int i = jugador.Entrenamiento.Nivel+1; i <= nivel_final; i++)
        {
            updateNivel(i);
            Thread.Sleep(50);
        }
    }
    static void updateNivel(int nivel){
            jugador.Entrenamiento.Nivel = nivel;
            Console.SetCursorPosition(82,10);
            Console.Write($"Nivel {nivel,3}");
            Console.SetCursorPosition(21,14);
            Console.Write(string.Format("{0,-35}{1,35}",$"Ataque: {jugador.Entrenamiento.Ataque:D2} / {15*nivel}",$"Defensa: {jugador.Entrenamiento.Defensa,3:D2} / {15*nivel}"));
            Console.SetCursorPosition(21,16);
            Console.Write(string.Format("{0,-35}{1,35}",$"Agresividad: {jugador.Entrenamiento.Agresividad:D2} / {15*nivel}",$"Velocidad de carga: {jugador.Entrenamiento.Velocidad_carga,3:D2} / {15*nivel}"));
    }
    static void AnimBarraVida(int salud_ganada){
        int salud_final = Math.Max(1000,Math.Min(jugador.Salud+salud_ganada,jugador.getSaludMax()));
        int rate = 250;
        while(true){
            jugador.Salud = (Math.Sign(salud_ganada) == 1) ? Math.Min(salud_final,jugador.Salud+rate):Math.Max(salud_final,jugador.Salud-rate);
            rate +=50;
            updateVida();
            if(jugador.Salud == salud_final){
                break;
            }
            Thread.Sleep(50);
        }
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