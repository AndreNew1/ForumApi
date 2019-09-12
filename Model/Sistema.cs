using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Sistema
    {
        public List<Topico> Topicos { get; set; } = new List<Topico>();
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public List<Votacao> Votos { get; set; } = new List<Votacao>();
    }
}
