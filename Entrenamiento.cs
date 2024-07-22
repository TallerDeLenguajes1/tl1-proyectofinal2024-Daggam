using GameNamespace;
using GuerreroNamespace;
using ToolNamespace;

namespace EntrenamientoNamespace;

enum EntrenamientoStates
{
    Menu,
    Entrenar,Explorar,Dormir,Comer_semilla,
    Fin_entrenamiento
}

static class Entrenamiento{
    static EntrenamientoStates estado_actual;
    static Guerrero jugador;
    static Caja sideCaja;
    static Caja mainCaja;

    static int dias_entrenamiento = 1;
    //Comienza el modo entrenamiento
    static public void Start(){
        jugador = Game.jugador;
        sideCaja = new Caja(1,1,16,16);
        mainCaja = new Caja(21,1,70,8);
        // jugador.Salud /= 80; 
        //Printeamos la UI del personaje.
        Console.SetCursorPosition(21,10);
        Console.Write("Salud: ");
        updateVida();
        Console.SetCursorPosition(82,10);
        Console.Write($"Nivel {0,3}");
        Console.SetCursorPosition(21,12);
        Text.WriteCenter("-- CARACTERISTICAS ADICIONALES--",70);
        Console.SetCursorPosition(21,14);
        Console.Write(string.Format("{0,-35}{1,35}","Ataque: 000","Defensa: 000"));
        Console.SetCursorPosition(21,16);
        Console.Write(string.Format("{0,-35}{1,35}","Agresividad: 000","Velocidad de carga: 000"));
        sideCaja.Escribir($"Día {dias_entrenamiento:D2} / 20",0,1);
        
        
        iniciarMaquina(EntrenamientoStates.Menu);
    }
    
    static EntrenamientoStates menuState(){
        Console.SetCursorPosition(sideCaja.CursorWritter.Left+3,sideCaja.CursorWritter.Top+5);
        Console.Write("Entrenar");
        Console.SetCursorPosition(sideCaja.CursorWritter.Left+3,Console.CursorTop+2);
        Console.Write("Explorar");
        Console.SetCursorPosition(sideCaja.CursorWritter.Left+3,Console.CursorTop+2);
        Console.Write("Dormir");
        Console.SetCursorPosition(sideCaja.CursorWritter.Left+3,Console.CursorTop+2);
        Console.Write("Comer");

        bool actualizaMenu = true;
        int opciones = 0;
        while (true)
        {
            if(actualizaMenu){
                Text.borrarSeccion(22,2,68,5);
                switch (opciones)
                {
                    case 0:
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,sideCaja.CursorWritter.Top+5);
                        Console.Write("■");
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");                        
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");                        
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");

                        mainCaja.Escribir("Entrenando ganarás fuerza y resistencia, pero pierdes algo de\nsalud. (+ Ataque ) (+ Defensa) (- Salud)\n\nCuidado: Si te exiges demasiado, tu cuerpo se debilitará por el\nexceso de entrenamiento. (- Ataque) (- Defensa) (- Salud)");
                        break;
                    case 1:
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,sideCaja.CursorWritter.Top+5);
                        Console.Write(" ");                        
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write("■");
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");                        
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");

                        mainCaja.Escribir("¡Puedes encontrar cosas interesantes si exploras el mundo!\n\nComo también puede ser una completa perdida de tiempo...");
                        break;
                    case 2:
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,sideCaja.CursorWritter.Top+5);
                        Console.Write(" ");                        
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");                        
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write("■");
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");

                        mainCaja.Escribir("Dormir te ayudará a recuperarte más rápido...\nA costa de algo de tu entrenamiento.\n\n(- Ataque) (- Defensa) (+ Salud)");
                        break;
                    case 3:
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,sideCaja.CursorWritter.Top+5);
                        Console.Write(" ");                        
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");                        
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write(" ");
                        Console.SetCursorPosition(sideCaja.CursorWritter.Left+1,Console.CursorTop+2);
                        Console.Write("■");

                        mainCaja.Escribir("Comerás una semilla del ermitaño que recuperará tu salud por\ncompleto.\n\nSOLO PUEDES COMER UNA SEMILLA EN TODA LA PARTIDA.");

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

    }
    static void iniciarMaquina(EntrenamientoStates nuevo_estado){
        estado_actual = nuevo_estado;
        bool salir = false;
        while(!salir){
            switch(estado_actual){
                case EntrenamientoStates.Menu:
                    estado_actual = menuState();
                    break;
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


}