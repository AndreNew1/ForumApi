using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class ComentarioCore:AbstractValidator<Comentarios>
    {
        private Comentarios _Comentario { get; set; }
        private IMapper Mapper { get; set; }
        private BancoContexto DB { get; set; }

        public ComentarioCore(IMapper mapper,BancoContexto banco)
        {
            DB = banco;
            Mapper = mapper;
        }
        public ComentarioCore(Comentarios comentario,BancoContexto banco)
        {
            DB = banco;

            _Comentario = comentario;

            RuleFor(e => e.Mensagem)
                .NotNull()
                .Length(10, 500)
                .WithMessage("Mensagem deve ter entre 10 a 500 caracteres");

            RuleFor(e => e.PublicacaoId)
                .NotNull()
                .WithMessage("PublicacaoId não pode ser nulo")
                .Must(temp => DB.Publicacaos.SingleOrDefault(x => x.Id == temp) != null)
                .WithMessage("Publicacao não existe");


        }

        public Retorno Comentar(string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            var validade = Validate(_Comentario);

            if (!validade.IsValid)
                return new Retorno { Status = false, Resultado = validade.Errors.Select(c => c.ErrorMessage) };


            _Comentario.UsuarioId = usuario;

            DB.Comentarios.Add(_Comentario);

            DB.SaveChanges();

            return new Retorno { Status = true, Resultado = new List<string> {"Comentario cadastrado com sucesso" } };
        }

        public Retorno RetornaComentario(string id,string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            try
            {
                var Resposta = DB.Comentarios.SingleOrDefault(x => x.Id == Guid.Parse(id));
                Resposta.Replicas = DB.Comentarios.Where(x => x.ComentariosId == Resposta.Id).ToList();

                return new Retorno { Status = true, Resultado = Resposta };
            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Comentario não existe" } };
            }
        }

        public Retorno EditarComentario(string id,string tokenUsuario,Comentarios comentario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            try
            {
                _Comentario = DB.Comentarios.Single(s => s.Id == Guid.Parse(id)&&s.UsuarioId==usuario);

                if (_Comentario == null)
                    return new Retorno { Status = false, Resultado = new List<string> { "Comentario não existe" } };

                if (comentario.CitacaoId != null)
                {
                    if (DB.Comentarios.SingleOrDefault(temp => temp.Id == _Comentario.ComentariosId) == null)
                        return new Retorno { Status = false, Resultado = new List<string> { "Citação não existe" } };
                }
                if (comentario.Mensagem != null)
                {
                    if (comentario.Mensagem.Length < 10&& comentario.Mensagem.Length>500)
                        return new Retorno { Status = false, Resultado = new List<string> { "Mensagem  deve ter entre 10 a 500 caracteres" } };

                    _Comentario.Mensagem = comentario.Mensagem;
                }

                return new Retorno { Status = true, Resultado = new List<string> { " Comentario editado com sucesso " } };

            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { " Não existe comentario com esse id " } };
            }
            
        }

        public Retorno DeletarComentario(string id ,string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            try
            {
                _Comentario = DB.Comentarios.Single(s => s.Id == Guid.Parse(id));

                DB.Comentarios.Remove(_Comentario);


                return new Retorno { Status = true, Resultado = new List<string> { "Comentario apagado" } };

            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Não existe comentario com esse id" } };
            }
        }
    }
}
