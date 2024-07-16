using System.Text.Json.Serialization;

namespace GuerreroNamespace;

// class Guerrero{
//     [JsonPropertyName("planeta_id")]
//     public int planetaID{get;set;}

//     [JsonPropertyName("nombre")]
//     public string nombre {get;set;}
    
//     public int salud {get;set;}
//     [JsonPropertyName("salud")]
//     public int maxSalud {get;set;}
//     [JsonPropertyName("ataque")]
//     public int ataque {get;set;}
//     [JsonPropertyName("defensa")]
//     public int defensa {get;set;}
//     public float ki {get;set;}
//     [JsonPropertyName("agresividad")]
//     public int agresividad {get;set;}
//     [JsonPropertyName("velocidadCarga")]
//     public int velocidadCarga {get;set;}
//     public bool sobreCarga {get;set;}

//     [JsonPropertyName("tecnicas")]
//     public List<Tecnicas> tecnicas {get;set;}
// }

// class Tecnicas{
//     [JsonPropertyName("nombre")]
//     public string nombre {get;set;}
//     [JsonPropertyName("cantidadKiNecesaria")]
//     public int cantidadKiNecesaria {get;set;}
//     [JsonPropertyName("ataque")]
//     public int ataque {get;set;}
// }

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
    public GuerreroInfo information;
    int salud;
    float ki;
    bool sobreCarga;

    public Guerrero(GuerreroInfo wbase){
        information = wbase;
        salud = information.salud_max;
    }
}