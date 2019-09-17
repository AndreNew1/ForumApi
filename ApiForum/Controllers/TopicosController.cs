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
        private BancoContexto Context { get; set; }

        public TopicosController(IMapper mapper,BancoContexto banco)
        {
            Mapper = mapper;
            Context = banco;
        }

        [HttpPost]
        public async Task<IActionResult> CadastroTopico([FromBody] Publicacao topico, [FromHeader] string tokenUsuario)
        {
            var Resultado = new PublicacaoCore(topico, Mapper,Context).RegistraTopico(tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpGet]
        public IActionResult BuscarTodosTopicos([FromHeader]string tokenUsuario)
        {
            var Resultado = new PublicacaoCore(Context).BuscarTodosTopicos(tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarUmTopico(string id ,[FromHeader]string tokenUsuario)
        {
            var Resultado = new PublicacaoCore(Context).BuscarUmTopico(id, tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarTopico(string id,[FromHeader] string tokenUsuario)
        {
            var Resultado = new PublicacaoCore(Context).DeletarTopico(id, tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarTopico(string id,[FromBody]Publicacao edit,[FromHeader] string tokenUsuario)
        {
            var Resultado = new PublicacaoCore(edit,Mapper,Context).EditarTopicos(id, tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }
    }
}