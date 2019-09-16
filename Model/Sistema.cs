using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Sistema
    {
        public List<Publicacao> Topicos { get; set; } = new List<Publicacao>();
        public List<Comentarios> Comentarios { get; set; } = new List<Comentarios>();
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
