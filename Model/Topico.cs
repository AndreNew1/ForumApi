using Newtonsoft.Json;
using System.Collections.Generic;

namespace Model
{

    public class Topico:Base
    {
        public string Titulo { get; set; }

        public string Texto { get; set; }

        public string Tipo { get; set; }
        
        public UsuarioFake Usuario { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Comentario> Comentarios { get; set; }
        public float Resultado { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        public void CalculaResultado(List<Votacao> votos)
        {
            votos.ForEach(voto => Resultado += voto.Nota);

            Resultado =float.Parse((Resultado / votos.Count).ToString("F1"));
        }
        
    }
}
