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
    public class TopicosController : ControllerBase
    {
        private IMapper Mapper { get; set; }

        public TopicosController(IMapper mapper)
        {
            Mapper = mapper;
        }

        [HttpPost]
        public IActionResult CadastroTopico([FromBody] TopicoView topico, [FromHeader] string tokenUsuario)
        {
            var Resultado = new TopicoCore(topico, Mapper).RegistraTopico(tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpGet]
        public IActionResult BuscarTodosTopicos([FromHeader]string tokenUsuario)
        {
            var Resultado = new TopicoCore().BuscarTodosTopicos(tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpGet("{id}")]
        public IActionResult BuscarUmTopico(string id ,[FromHeader]string tokenUsuario)
        {
            var Resultado = new TopicoCore().BuscarUmTopico(id, tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarTopico(string id,[FromHeader] string tokenUsuario)
        {
            var Resultado = new TopicoCore().DeletarTopico(id, tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpPut("{id}")]
        public IActionResult EditarTopico(string id,[FromBody]TopicoView edit,[FromHeader] string tokenUsuario)
        {
            var Resultado = new TopicoCore(edit,Mapper).EditarTopicos(id, tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }
    }
}