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
        public async Task<IActionResult> CadastroTopico([FromBody] TopicoView topico,[FromHeader] string tokenUsuario)
        {
            var Resultado = new TopicoCore(topico,Mapper).RegistraTopico(tokenUsuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        [HttpGet]
        public  async Task<IActionResult> BuscarTodosTopicos()
        {
            return default;
        }

       
    }
}