using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Comentarios : Base
    {
        [ForeignKey("Publicacaos")]
        public Guid? PublicacaoId { get; set; }
        [ForeignKey("Comentarios")]
        public Guid? ComentariosId { get; set; }
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
