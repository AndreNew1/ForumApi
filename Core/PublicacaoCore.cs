using AutoMapper;
using Core.Util;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class PublicacaoCore:AbstractValidator<Publicacao>
    {
        private Publicacao _Topico { get; set; }
        private IMapper Mapper { get; set; }
        private BancoContexto DB { get; set; }

        public PublicacaoCore(BancoContexto banco)
        {
            DB = banco;
        }
        public PublicacaoCore(Publicacao topico,IMapper mapper,BancoContexto banco)
        {
            DB = banco;

            Mapper = mapper;

            _Topico = topico;

            RuleFor(e => e.Tipo)
                .NotNull()
                .WithMessage("O tipo deve ser informado");

            if (_Topico.Tipo != null)
            {
                RuleFor(e => e.Tipo)
                    .Must(temp => temp.ToLower() == "duvida" || temp.ToLower() == "tutorial")
                    .WithMessage("O tipo Deve ser duvida ou tutorial");
            }
           

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
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            var Validade = Validate(_Topico);

            if (!Validade.IsValid) return new Retorno { Status = false, Resultado = Validade.Errors.Select(c => c.ErrorMessage) };

            _Topico.UsuarioId = usuario;

            if (_Topico.Tipo.ToLower() == "duvida")
                _Topico.Status = "aberto";
            else
                _Topico.Status = null;
            
            DB.Publicacaos.Add(_Topico);

            
            DB.SaveChangesAsync();

            return new Retorno { Status = true, Resultado = _Topico };
        }

        public Retorno BuscarUmTopico(string id,string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            try
            {
                _Topico = DB.Publicacaos.Include(c=>c.Comentario).Single(s => s.Id == Guid.Parse(id));

                return new Retorno { Status = true, Resultado = new List<Publicacao> { _Topico } };
            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { " Id informado não existe" } };
            }

        }
        public Retorno BuscarTodosTopicos(string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario)==null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };


            var Topicos = DB.Publicacaos.Include(x=>x.Comentario).ToList();

            return Topicos.Count != 0 ? new Retorno { Status = true, Resultado = Topicos } : new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhum topico" } };
        }

        public Retorno EditarTopicos(string id, string tokenUsuario)
        {
            Guid.TryParse(tokenUsuario, out Guid usuario);

            var Usuario = DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario);

            if (Usuario == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };
            try
            {
                var Topico = DB.Publicacaos.SingleOrDefault(temp => temp.Id == Guid.Parse(id)&&temp.Usuario.Email==Usuario.Email);

                if (Topico == null)
                    return new Retorno { Status = false, Resultado = new List<string> { "Nenhum topico encontrado" } };

                if (_Topico.Titulo != null)
                {
                    if (_Topico.Titulo.Length < 8 || _Topico.Titulo.Length > 250)
                        return new Retorno { Status = false, Resultado = new List<string> { "Titulo invalido" } };

                    Topico.Titulo = _Topico.Titulo;
                }
                if (_Topico.Titulo != null)
                {
                    if (_Topico.Texto.Length < 50)
                        return new Retorno { Status = false, Resultado = new List<string> { "Texto invalido" } };

                    Topico.Texto = _Topico.Texto;
                }
                if (Topico.Tipo.ToLower() == "duvida")
                {
                    if (_Topico.Status != "fechado" || _Topico.Status != "aberto")
                        return new Retorno { Status = false, Resultado = new List<string> { "Status Invalido" } };
                }

                return new Retorno { Status = true, Resultado = new List<string> { " Topico Editado com sucesso " } };
            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Topico id não existe" } };
            }
        }

        public Retorno DeletarTopico(string id,string tokenUsuario)
        {
            Guid.TryParse(tokenUsuario, out Guid usuario);

            var Usuario = DB.Usuarios.SingleOrDefault(temp => temp.Id == usuario);

            if ( Usuario != null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            try
            {
                _Topico = DB.Publicacaos.Single(temp => temp.Id == Guid.Parse(id)&&temp.Usuario.Email==Usuario.Email);

                DB.Publicacaos.Remove(_Topico);


                return new Retorno { Status = true, Resultado = new List<string> { "Topico deletado" } };

            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhum topico com esse id" } };
            }

        }
    }
}
