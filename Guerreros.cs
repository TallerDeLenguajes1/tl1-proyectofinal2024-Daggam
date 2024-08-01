using System.Text.Json.Serialization;

namespace GuerreroNamespace;
public class GuerreroInfo
{
    [JsonPropertyName("planeta_id")]
    public int planeta_id { get; set; }

    [JsonPropertyName("nombre")]
    public string nombre { get; set; }

    [JsonPropertyName("salud_max")]
    public int salud_max { get; set; }

    [JsonPropertyName("ataque")]
    public int ataque { get; set; }

    [JsonPropertyName("defensa")]
    public int defensa { get; set; }

    [JsonPropertyName("agresividad")]
    public int agresividad { get; set; }

    [JsonPropertyName("velocidad_carga")]
    public int velocidad_carga { get; set; }

    [JsonPropertyName("tecnicas")]
    public List<Tecnica> tecnicas { get; set; }
}

public class Tecnica
{
    [JsonPropertyName("nombre")]
    public string nombre { get; set; }

    [JsonPropertyName("cantidad_ki_necesaria")]
    public int cantidad_ki_necesaria { get; set; }

    [JsonPropertyName("ataque")]
    public int ataque { get; set; }
}

public class Guerrero{
    // GuerreroInfo information;
    // GuerreroEntrenamiento entrenamiento;
    // int salud;
    // float ki;
    [JsonPropertyName("salud")]
    public int Salud { get; set;}
    [JsonPropertyName("information")]

    public GuerreroInfo Information { get; set;}
    [JsonPropertyName("ki")]
    public float Ki { get; set;}
    [JsonPropertyName("entrenamiento")]
    public GuerreroEntrenamiento Entrenamiento { get; set;}

    [JsonConstructor]
    public Guerrero(){

    }
    public Guerrero(GuerreroInfo wbase){
        Entrenamiento = new GuerreroEntrenamiento();
        Information = wbase;
        Salud = getSaludMax();
    }

    public int getSaludMax(){
        return Information.salud_max + Entrenamiento.Salud_max;
    }
    public int getAtaque(){
        return Information.ataque + Entrenamiento.Ataque;
    }
    public int getDefensa(){
        return Information.defensa + Entrenamiento.Defensa;
    }
    public int getAgresividad(){
        return Information.agresividad + Entrenamiento.Agresividad;
    }
    public int getVelocidadCarga(){
        return Information.velocidad_carga + Entrenamiento.Velocidad_carga;
    }
    public int getTecnicaAtaque(int i){
        return Information.tecnicas[i].ataque + 1000*Entrenamiento.Nivel;
    }

}


public class GuerreroEntrenamiento{
    [JsonPropertyName("nivel")]
    public int Nivel {get;set;}= 1;
    [JsonPropertyName("salud_max")]
    public int Salud_max {get;set;}= 0;
    [JsonPropertyName("ataque")]
    public int Ataque {get;set;}= 0;
    [JsonPropertyName("defensa")]
    public int Defensa {get;set;}= 0;
    [JsonPropertyName("agresividad")]
    public int Agresividad {get;set;}= 0; //acotar 10 por nivel
    [JsonPropertyName("velocidad_carga")]
    public int Velocidad_carga {get;set;}= 0;
}
//PODRIA CREAR DOS CLASES: UNA PARA BATALLA Y OTRA PARA ENTRENAMIENTO
//LA DE ENTRENAMIENTO TENDRÍA QUE MODIFICAR EL GUERRERO INFO Y ADEMÁS TENDRÍA SALUD,NIVEL, TODAS LAS CARACTERISTICAS BASES ADICIONALES. Por ej: Defensa_base + defensa_entrenamiento; ESTOS SON ASPECTOS PERMANENTES (tal vez fatiga...)
//BATALLA TENDRÁ ASPECTOS QUE SON DESCARTABLES QUE SOLAMENTE IMPORTAN A LA HORA DE LA BATALLA.