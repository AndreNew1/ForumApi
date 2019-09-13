using System;

namespace Model
{
    public class Usuario:Base
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ConfirmaSenha {  get; set; }
        public string Nome { get; set; }
    }
}
