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
    GuerreroInfo information;
    GuerreroEntrenamiento entrenamiento;
    int salud;
    float ki;

    public Guerrero(GuerreroInfo wbase){
        entrenamiento = new GuerreroEntrenamiento();
        information = wbase;
        salud = getSaludMax();
    }

    public int Salud { get => salud; set => salud = value; }
    public GuerreroInfo Information {get => information; set => information = value;}
    public float Ki { get => ki; set => ki = value; }
    public GuerreroEntrenamiento Entrenamiento { get => entrenamiento; set => entrenamiento = value; }

    public int getSaludMax(){
        return information.salud_max + entrenamiento.Salud_max;
    }
    public int getAtaque(){
        return information.ataque + entrenamiento.Ataque;
    }
    public int getDefensa(){
        return information.defensa + entrenamiento.Defensa;
    }
    public int getAgresividad(){
        return information.agresividad + entrenamiento.Agresividad;
    }
    public int getVelocidadCarga(){
        return information.velocidad_carga + entrenamiento.Velocidad_carga;
    }
    public int getTecnicaAtaque(int i){
        return information.tecnicas[i].ataque + 1000*entrenamiento.Nivel;
    }

}


public class GuerreroEntrenamiento{
    public int Nivel = 1;
    public int Salud_max = 0;
    public int Ataque = 0;
    public int Defensa = 0;
    public int Agresividad = 0; //acotar 10 por nivel
    public int Velocidad_carga = 0;
}
//PODRIA CREAR DOS CLASES: UNA PARA BATALLA Y OTRA PARA ENTRENAMIENTO
//LA DE ENTRENAMIENTO TENDRÍA QUE MODIFICAR EL GUERRERO INFO Y ADEMÁS TENDRÍA SALUD,NIVEL, TODAS LAS CARACTERISTICAS BASES ADICIONALES. Por ej: Defensa_base + defensa_entrenamiento; ESTOS SON ASPECTOS PERMANENTES (tal vez fatiga...)
//BATALLA TENDRÁ ASPECTOS QUE SON DESCARTABLES QUE SOLAMENTE IMPORTAN A LA HORA DE LA BATALLA.