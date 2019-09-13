using AutoMapper;
using Core.Util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class VotacaoCore:AbstractValidator<Votacao>
    {
        private Votacao _Voto { get; set; }
        private Sistema Db { get; set; }
        private IMapper Mapper { get; set; }
        private readonly List<float> Notas = new List<float> { 0, 0.5f, 1, 1.5f, 2, 2.5f, 3, 3.5f, 4, 4.5f, 5 };
        public VotacaoCore()
        {
            Db = Arquivo.LerArquivo();

            Db = Db ?? new Sistema();
        }

        public VotacaoCore(ViewVoto voto,IMapper mapper)
        {
            Db = Arquivo.LerArquivo();

            Db = Db ?? new Sistema();

            Mapper = mapper;

            _Voto = Mapper.Map<Votacao>(voto);

            RuleFor(e => e.IdUsuario)
                .NotNull()
                .Must(temp => Db.Usuarios.SingleOrDefault(e => e.Id == temp) != null)
                .WithMessage("Usuario não existe");

            RuleFor(e => e.Nota)
                .Must(temp => Notas.Contains(temp))
                .WithMessage("Nota Invalida");
            
            RuleFor(e => e.VotacaoRelacionada)
                .NotNull()
                .Must(temp => Db.Topicos.SingleOrDefault(e => e.Id == temp) != null || Db.Comentarios.SingleOrDefault(e => e.Id == temp) != null)
                .WithMessage("Topico ou Comentario Não existe");
        }

        public Retorno Votar(string tokenUsuario)
        {
            if (!Guid.TryParse(tokenUsuario, out Guid usuario) || Db.Usuarios.SingleOrDefault(temp => temp.Id == usuario) == null)
                return new Retorno { Status = false, Resultado = new List<string> { "Acesso negado" } };

            _Voto.IdUsuario = usuario;

            var Validade = Validate(_Voto);

            if (!Validade.IsValid) return new Retorno { Status = false, Resultado = Validade.Errors.Select(c => c.ErrorMessage) };

            if (Db.Votos.SingleOrDefault(temp => temp.IdUsuario == _Voto.IdUsuario && temp.VotacaoRelacionada == _Voto.VotacaoRelacionada) != null)
                return new Retorno { Status = false, Resultado = new List<string> { " Usuario ja votou " } };

            Db.Votos.Add(_Voto);

            Arquivo.Escrita(Db);

            return new Retorno { Status = true, Resultado = new List<string> { "Voto realizado com sucesso" } };
        }

        public Retorno BuscaTodosOsVotos(string id)
        {
            try
            {
                var Votos = Db.Votos.Where(s => s.VotacaoRelacionada == Guid.Parse(id));

                return Votos.Count() != 0 ? new Retorno { Status = true, Resultado = Votos } : new Retorno { Status = false, Resultado = new List<string> { "Não existe nenhum voto para o Id informado" } };
            }
            catch (FormatException)
            {
                return new Retorno { Status = false, Resultado = new List<string> { "Id passado para consulta, não existe" } };
            }
            
        }

        
    }
}
