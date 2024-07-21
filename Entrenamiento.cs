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
    static Guerrero jugador;
    static Caja sideCaja;
    static Caja mainCaja;
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
    }
    static void updateVida(){
        float cocienteSalud = (float) jugador.Salud/jugador.Information.salud_max * 25;
        string barra = new string('▓',(int) Math.Ceiling(cocienteSalud)) + new string('▒',25 - (int) Math.Ceiling(cocienteSalud));
        Console.SetCursorPosition(28,10);
        string line = string.Format("{0}  {1,6:N0} / {2:N0}",barra,jugador.Salud,jugador.Information.salud_max);
        Console.Write(line);
        
    }
}