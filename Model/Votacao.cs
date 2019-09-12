using System;

namespace Model
{
    public class Votacao
    {
        public Guid VotacaoRelacionada { get; set; }
        public Guid IdUsuario { get; set; }
        public float Nota { get; set; }
        public DateTime DiaDoVoto { get; set; } = DateTime.Now;
    }
}
