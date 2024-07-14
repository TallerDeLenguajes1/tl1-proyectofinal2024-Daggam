namespace GuerreroNamespace;

class Guerrero{
    int planetaID;
    string nombre;
    int salud;
    int maxSalud;
    int ataque;
    int defensa;
    float ki;
    int agresividad;
    int velocidadCarga;
    bool sobreCarga;

    List<Tecnicas> tecnicas;
}

class Tecnicas{
    string nombre;
    int cantidadKiNecesaria;
    int ataque;
}