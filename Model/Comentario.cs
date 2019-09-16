using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Comentarios : Base
    {
        [ForeignKey("Publicacao")]
        public Guid? PublicacaoId { get; set; }
        [NotMapped]
        public Comentarios Comentario { get; set; }
        [ForeignKey("Comentarios")]
        public Guid? ComentarioId { get; set; }
        public Comentarios Citacao { get; set; }
        [ForeignKey("Comentarios")]
        public Guid? CitacaoId { get; set; }
        public Usuario Usuario { get; set; }
        [ForeignKey("Usuarios")]
        public Guid? UsuarioId { get; set; }
        public string Mensagem { get; set; }
        public float? Nota { get; set; }
        public ICollection<Comentarios> Replicas { get; set; }

    }
}
