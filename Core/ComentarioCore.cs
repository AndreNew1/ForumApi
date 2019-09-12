using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class ComentarioCore:AbstractValidator<Comentario>
    {
        private Comentario _Comentario { get; set; }
        private IMapper Mapper { get; set; }
        private Sistema DB { get; set; }

        public ComentarioCore(IMapper mapper)
        {
            DB = Arquivo.LerArquivo();

            DB = DB ?? new Sistema();

            Mapper = mapper;
        }
        public ComentarioCore(Comentario comentario)
        {
            DB = Arquivo.LerArquivo();

            DB = DB ?? new Sistema();

            _Comentario = comentario;

            RuleFor(e => e.Mensagem)
                .NotNull()
                .Length(10, 500)
                .WithMessage("Mensagem deve ter entre 10 a 500 caracteres");

            RuleFor(e => e.PublicacaoId)
                .NotNull()
                .WithMessage("PublicacaoId não pode ser nulo")
                .Must(temp => DB.Topicos.SingleOrDefault(x => x.Id == temp) == null)
                .WithMessage("Publicacao não existe");


        }

        public Retorno Comentar(string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) && DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) != null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            var validade = Validate(_Comentario);

            if (!validade.IsValid)
                return new Retorno { Status = false, Resultado = validade.Errors.Select(c => c.ErrorMessage) };

            Arquivo.Escrita(DB);

            return new Retorno { Status = true, Resultado = new List<string> {"Comentario cadastrado com sucesso" } };
        }

        public Retorno RetornaComentario(string id,string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) && DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) != null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            try
            {
                return new Retorno { Status = true, Resultado = DB.Comentarios.Where(x => x.Id == Guid.Parse(id)) };
            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario não existe" } };
            }
        }

        public Retorno EditarComentario(string id,string tokenUsuario,ComentarioEdit comentario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) && DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) != null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            try
            {
                _Comentario = DB.Comentarios.Single(s => s.Id == Guid.Parse(id));


                return new Retorno { Status = true, Resultado = _Comentario };

            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Não existe comentario com esse id" } };
            }
            
        }
    }
}
