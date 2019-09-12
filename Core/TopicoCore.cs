using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class TopicoCore:AbstractValidator<Topico>
    {
        private Topico _Topico { get; set; }
        private IMapper Mapper { get; set; }
        private Sistema DB { get; set; }

        public TopicoCore()
        {
            DB = Arquivo.LerArquivo();

            DB = DB ?? new Sistema();
        }
        public TopicoCore(TopicoView topico,IMapper mapper)
        {
            DB = Arquivo.LerArquivo();

            DB = DB ?? new Sistema();

            Mapper = mapper;

            _Topico = Mapper.Map<TopicoView,Topico>(topico);

            RuleFor(e => e.Tipo)
                .NotNull()
                .WithMessage("O tipo deve ser informado")
                .Must(temp => temp == "duvida" || temp == "tutorial")
                .WithMessage("O tipo Deve ser duvida ou tutorial");


            RuleFor(e => e.Texto)
               .NotNull()
               .MinimumLength(50)
               .WithMessage("O texto deve conter no minimo 50 caracteres");

            RuleFor(e => e.Titulo)
                .NotNull()
                .Length(8, 250)
                .WithMessage("Titulo deve ter entre 8 a 250 caracteres");
            
            
        }

        public Retorno RegistraTopico(string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) && DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) != null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            var Validade = Validate(_Topico);

            if (!Validade.IsValid) return new Retorno { Status = false, Resultado = Validade.Errors.Select(c => c.ErrorMessage) };

            _Topico.Usuario = Mapper.Map<UsuarioFake>(DB.Usuarios.Single(temp => temp.Id == usuario));

            if (_Topico.Tipo.ToLower() == "duvida")
                    _Topico.Status = "aberto";
            
            DB.Topicos.Add(_Topico);

            Arquivo.Escrita(DB);
            return new Retorno { Status = true, Resultado = _Topico };
        }

        public Retorno BuscarTodosTopicos(string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) && DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) != null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };


            var Topicos = DB.Topicos;

            return Topicos.Count != 0 ? new Retorno { Status = true, Resultado = Topicos } : new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhum topico" } };
        }

        public Retorno EditarTopicos(string id, string tokenUsuario)
        {
            try
            {
                if (!Guid.TryParse(tokenUsuario, out Guid usuario) && DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) != null)
                    return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

                var Topico = DB.Topicos.Single(temp => temp.Id == Guid.Parse(id));


                return new Retorno { Status = true, Resultado = _Topico };
            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Topico não existe" } };
            }
        }

        public Retorno DeletarTopico(string id,string tokenUsuario)
        {
            try
            {
                if (!Guid.TryParse(tokenUsuario, out Guid usuario) && DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) != null)
                    return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

                _Topico = DB.Topicos.Single(temp => temp.Id == Guid.Parse(id));

                DB.Topicos.Remove(_Topico);

                Arquivo.Escrita(DB);

                return new Retorno { Status = true, Resultado = new List<string> { "Topico deletado" } };

            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhum topico com esse id" } };
            }

        }
    }
}
