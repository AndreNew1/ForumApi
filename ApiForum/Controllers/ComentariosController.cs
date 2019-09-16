using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private IMapper Mapper { get; set; }
        private BancoContexto Context { get; set; }
        public ComentariosController(IMapper mapper,BancoContexto banco)
        {
            Mapper = mapper;
            Context = banco;
        }

        [HttpPost]
        public async Task<IActionResult> Comentar([FromBody] Comentarios comentario, [FromHeader]string tokenUsuario)
        {
            var Resultado = new ComentarioCore(comentario,Context).Comentar(tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarComentario([FromBody] Comentarios comentario,[FromHeader]string tokenUsuario,string id)
        {
            var Resultado = new ComentarioCore(Mapper,Context).EditarComentario(id, tokenUsuario, comentario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpPost("votar")]
        public async Task<IActionResult> VotarComentario([FromBody] ViewVoto voto, [FromHeader]string tokenUsuario)
        {
            return default;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscaUmComentario(string id,[FromHeader]string tokenUsuario)
        {
            var Resultado = new ComentarioCore(Mapper,Context).RetornaComentario(id, tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(string id,[FromHeader]string tokenUsuario)
        {
            var Resultado = new ComentarioCore(Mapper,Context).DeletarComentario(id, tokenUsuario);

            return Resultado.Status ? Accepted(Resultado) : (IActionResult)BadRequest(Resultado);
        }
    }
}