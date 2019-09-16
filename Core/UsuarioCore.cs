using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Core
{
    public class UsuarioCore : AbstractValidator<Usuario>
    {
        private Usuario _Usuario { get; set; }
        private BancoContexto Db { get; set; }
        private IMapper Mapper { get; set; }

        public UsuarioCore(IMapper mapper,BancoContexto banco)
        {
            Db = banco;

            Mapper = mapper;
        }
        public UsuarioCore(Usuario usuario,BancoContexto banco)
        {
            Db = banco;

            _Usuario = usuario;

            RuleFor(e => e.Nome)
                .NotNull()
                .Length(3, 99)
                .WithMessage("Nome deve ter entre 3 a 99 caracteres");

            RuleFor(e => e.Email)
                .NotNull()
                .EmailAddress()
                .WithMessage("Email Invalido")
                .Must(e => Db.Usuarios.SingleOrDefault(temp => temp.Email == e) == null)
                .WithMessage("Email Ja cadastrado");

            RuleFor(e => e.Senha)
                .NotNull()
                .Length(8, 12)
                .WithMessage("Senha deve ter entre 8 a 12 ");

            RuleFor(e => e.Senha)
                .Must(x => !Regex.IsMatch(x, @"^[a-zA-Z]+$") && !Regex.IsMatch(x, @"^[0-9]+$"))
                .WithMessage("Senha deve conter pelo menos um letra e um numero");

            RuleFor(e => e.ConfirmaSenha)
                .NotNull()
                .Must(x => x == _Usuario.Senha)
                .WithMessage("Confirmação da senha esta incorreta");

        }

        public Retorno CadastroUsuario()
        {
            var Validade = Validate(_Usuario);

            if (!Validade.IsValid)
                return new Retorno { Status = false, Resultado = Validade.Errors.Select(c => c.ErrorMessage) };

            Db.Usuarios.Add(_Usuario);

            Db.SaveChangesAsync();

            return new Retorno { Status = true, Resultado = new List<string> { "Cadastro efetuado com sucesso" } };
        }

        public Retorno Login(LoginView usuario)
        {

            try
            {
                _Usuario = Db.Usuarios.Single(temp => temp.Email == usuario.Login);

                return _Usuario.Senha == usuario.Senha ? new Retorno { Status = true, Resultado = Mapper.Map<UsuarioToken>(_Usuario) } : new Retorno { Status = false, Resultado = new List<string> { " Senha Incorreta " } };
            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Email Incorreto" } };
            }

        }
    }
}
