using Microsoft.AspNetCore.Mvc;
using L01_2020_LM_650.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2020_LM_650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class platosController : Controller
    {
        private readonly restauranteContext _restauranteContext;

        public platosController(restauranteContext restauranteContexto)
        {
            _restauranteContext = restauranteContexto;
        }
        //CRUD
        //Obtener listado
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<platos> listadoPlatos = (from e in _restauranteContext.platos
                                            select e).ToList();

            if (listadoPlatos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoPlatos);
        }

        //Guardar nuevo registro
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPedido([FromBody] platos plato)
        {
            try
            {
                _restauranteContext.platos.Add(plato);
                _restauranteContext.SaveChanges();
                return Ok(plato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Modificar registros
        [HttpPut]
        [Route("Modificar/{id}")]
        public IActionResult ModificarPedido(int id, [FromBody] pedidos pedidoModificar)
        {
            pedidos? pedidoActual = (from e in _restauranteContext.pedidos
                                     where e.pedidoId == id
                                     select e).FirstOrDefault();
            if (pedidoActual == null)
            {
                return NotFound();
            }
            pedidoActual.motoristaId = pedidoModificar.motoristaId;
            pedidoActual.clienteId = pedidoModificar.clienteId;
            pedidoActual.platoId = pedidoModificar.platoId;
            pedidoActual.cantidad = pedidoModificar.cantidad;
            pedidoActual.precio = pedidoModificar.precio;

            _restauranteContext.Entry(pedidoActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(pedidoActual);
        }

        //Eliminar registros
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult ElimiarPedido(int id)
        {
            pedidos? equipo = (from e in _restauranteContext.pedidos
                               where e.pedidoId == id
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }

            _restauranteContext.pedidos.Attach(equipo);
            _restauranteContext.pedidos.Remove(equipo);
            _restauranteContext.SaveChanges();

            return Ok(equipo);
        }
    }
}
