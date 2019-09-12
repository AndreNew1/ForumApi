using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IMapper Mapper { get; set; }
        public UsuariosController(IMapper mapper)
        {
            Mapper = mapper;
        }

        [HttpPost]
        public IActionResult CadastroUsuario([FromBody] Usuario usuario)
        {
             var Resultado = new UsuarioCore(usuario).CadastroUsuario();

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        // PUT api/values/5
        [HttpPost("autenticar")]
        public IActionResult AutenticarUsuario([FromBody] LoginView usuario)
        {
            var Resultado = new UsuarioCore(Mapper).Login(usuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        // DELETE api/values/5
        [HttpDelete]
        public IActionResult DeletarUsuario([FromBody]LoginView usuario,[FromHeader] string tokenUsuario)
        {
            var Resultado = new UsuarioCore(Mapper).DeletaUsuario(tokenUsuario,usuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }
    }
}
