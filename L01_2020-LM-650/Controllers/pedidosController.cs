using Microsoft.AspNetCore.Mvc;
using L01_2020_LM_650.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2020_LM_650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pedidosController : Controller
    {
        private readonly restauranteContext _restauranteContext;

        public pedidosController(restauranteContext restauranteContexto) 
        {
            _restauranteContext = restauranteContexto;
        }

        //CRUD
        //Obtener listado
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<pedidos> listadoPedidos = (from e in _restauranteContext.pedidos
                                            select e).ToList();

            if (listadoPedidos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoPedidos);
        }

        //Guardar nuevo registro
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPedido([FromBody] pedidos pedido)
        {
            try
            {
                _restauranteContext.pedidos.Add(pedido);
                _restauranteContext.SaveChanges();
                return Ok(pedido);
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
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.pedidoId == id
                               select e).FirstOrDefault();
            if (pedido == null)
            {
                return NotFound();
            }

            _restauranteContext.pedidos.Attach(pedido);
            _restauranteContext.pedidos.Remove(pedido);
            _restauranteContext.SaveChanges();

            return Ok(pedido);
        }

        //Filtrado motorista
        [HttpGet]
        [Route("Filtro/{filtro}")]
        public IActionResult FiltrarMotorista(int filtro)
        {
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.motoristaId==filtro
                               select e).FirstOrDefault();
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        //Filtrado cliente
        [HttpGet]
        [Route("Filtro/{filtro}")]
        public IActionResult FiltrarCliente(int filtro)
        {
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.clienteId == filtro
                               select e).FirstOrDefault();
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }
    }
}
