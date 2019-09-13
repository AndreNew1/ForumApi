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
        public ComentariosController(IMapper mapper)
        {
            Mapper = mapper;
        }

        [HttpPost]
        public IActionResult Comentar([FromBody] Comentario comentario, [FromHeader]string tokenUsuario)
        {
            var Resultado = new ComentarioCore(comentario).Comentar(tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpPut("{id}")]
        public IActionResult EditarComentario([FromBody] ComentarioEdit comentario,[FromHeader]string tokenUsuario,string id)
        {
            var Resultado = new ComentarioCore(Mapper).EditarComentario(id, tokenUsuario, comentario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpPost("votar")]
        public IActionResult VotarComentario([FromBody] ViewVoto voto, [FromHeader]string tokenUsuario)
        {
            var Resultado = new VotacaoCore(voto, Mapper).Votar(tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpGet("{id}")]
        public IActionResult BuscaUmComentario(string id,[FromHeader]string tokenUsuario)
        {
            var Resultado = new ComentarioCore(Mapper).RetornaComentario(id, tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(string id,[FromHeader]string tokenUsuario)
        {
            var Resultado = new ComentarioCore(Mapper).DeletarComentario(id, tokenUsuario);

            return Resultado.Status ? Accepted(Resultado) : (IActionResult)BadRequest(Resultado);
        }
    }
}