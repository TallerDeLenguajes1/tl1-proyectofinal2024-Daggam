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
    public float velocidad_carga { get; set; }

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
        salud = information.salud_max;
    }

    public int Salud { get => salud; set => salud = value; }
    public GuerreroInfo Information {get => information; set => information = value;}
    public float Ki { get => ki; set => ki = value; }
    public GuerreroEntrenamiento Entrenamiento { get => entrenamiento; set => entrenamiento = value; }
}


public class GuerreroEntrenamiento{
    public int Nivel = 1;
    public int Salud_max = 0;
    public int Ataque = 0;
    public int Defensa = 0;
    public int Agresividad = 0;
    public int Velocidad_carga = 0;
}
//PODRIA CREAR DOS CLASES: UNA PARA BATALLA Y OTRA PARA ENTRENAMIENTO
//LA DE ENTRENAMIENTO TENDRÍA QUE MODIFICAR EL GUERRERO INFO Y ADEMÁS TENDRÍA SALUD,NIVEL, TODAS LAS CARACTERISTICAS BASES ADICIONALES. Por ej: Defensa_base + defensa_entrenamiento; ESTOS SON ASPECTOS PERMANENTES (tal vez fatiga...)
//BATALLA TENDRÁ ASPECTOS QUE SON DESCARTABLES QUE SOLAMENTE IMPORTAN A LA HORA DE LA BATALLA.