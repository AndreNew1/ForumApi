using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Model
{
    public class Comentario : Base
    {
        public Guid PublicacaoId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ComentarioId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CitacaoId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Mensagem { get; set; }
        public float Resultado { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Comentario> Replicas { get; set; }

        public void CalculaResultado(List<Votacao> votos)
        {
            votos.ForEach(voto => Resultado += voto.Nota);

            Resultado = float.Parse((Resultado / votos.Count).ToString("F1"));
        }
    }
}
