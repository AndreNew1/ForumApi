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
                .Must(temp => DB.Topicos.SingleOrDefault(x => x.Id == temp) != null)
                .WithMessage("Publicacao não existe");


        }

        public Retorno Comentar(string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            var validade = Validate(_Comentario);

            if (!validade.IsValid)
                return new Retorno { Status = false, Resultado = validade.Errors.Select(c => c.ErrorMessage) };

            if (_Comentario.CitacaoId != null)
            {
                if (!Guid.TryParse(_Comentario.CitacaoId, out Guid comentarioId) || DB.Comentarios.SingleOrDefault(temp => temp.Id == comentarioId) == null)
                    return new Retorno { Status = false, Resultado = new List<string> { "Citacaoid não existe" } };
            }

            if (_Comentario.ComentarioId != null)
            {
                if (!Guid.TryParse(_Comentario.ComentarioId, out Guid comentarioId) || DB.Comentarios.SingleOrDefault(temp => temp.Id == comentarioId) == null)
                    return new Retorno { Status = false, Resultado = new List<string> { "ComentarioId não existe" } };
            }

            _Comentario.UsuarioId = usuario;

            DB.Comentarios.Add(_Comentario);

            Arquivo.Escrita(DB);

            return new Retorno { Status = true, Resultado = new List<string> {"Comentario cadastrado com sucesso" } };
        }

        public Retorno RetornaComentario(string id,string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) == null)
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
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            try
            {
                _Comentario = DB.Comentarios.Single(s => s.Id == Guid.Parse(id)&&s.UsuarioId==usuario);

                if (_Comentario == null)
                    return new Retorno { Status = false, Resultado = new List<string> { "Comentario não existe" } };

                if (comentario.CitacaoId != null)
                {
                    if (!Guid.TryParse(comentario.CitacaoId, out Guid comentarioid) && DB.Comentarios.SingleOrDefault(temp => temp.Id == comentarioid) == null)
                        return new Retorno { Status = false, Resultado = new List<string> { "Citação não existe" } };

                   _Comentario.CitacaoId = comentario.CitacaoId;
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

                if (DB.Comentarios.Where(s => Guid.Parse(s.ComentarioId) == _Comentario.Id) != null || DB.Comentarios.Where(s => Guid.Parse(s.CitacaoId) == _Comentario.Id) != null)
                    return new Retorno { Status = false, Resultado = new List<string> { "Comentario não pode ser apagado pois é citado ou ja possue uma ou mais comentarios" } };

                DB.Comentarios.Remove(_Comentario);

                Arquivo.Escrita(DB);

                return new Retorno { Status = true, Resultado = new List<string> { "Comentario apagado" } };

            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Não existe comentario com esse id" } };
            }
        }
    }
}
