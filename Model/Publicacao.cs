using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Publicacao:Base
    {   
        public string Titulo { get; set; }

        public string Texto { get; set; }

        public string Tipo { get; set; }

        public Usuario Usuario { get; set; }

        [ForeignKey("Usuarios")]
        public Guid? UsuarioId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? MediaDeVotos { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Comentarios> Comentarios { get; set; }
        
    }
}
