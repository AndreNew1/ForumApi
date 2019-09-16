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
        private BancoContexto Context { get; set; }
        public UsuariosController(IMapper mapper,BancoContexto banco)
        {
            Mapper = mapper;
            Context = banco;
        }

        [HttpPost]
        public async Task<IActionResult> CadastroUsuario([FromBody] Usuario usuario)
        {
             var Resultado = new UsuarioCore(usuario,Context).CadastroUsuario();

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

        // PUT api/values/5
        [HttpPost("autenticar")]
        public async Task<IActionResult> AutenticarUsuario([FromBody] LoginView usuario)
        {
            var Resultado = new UsuarioCore(Mapper,Context).Login(usuario);

            return Resultado.Status ? Ok(Resultado) : (IActionResult)BadRequest(Resultado);
        }

    }
}
