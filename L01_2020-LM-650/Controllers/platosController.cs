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
        public IActionResult ModificarPlato(int id, [FromBody] platos pModificar)
        {
            platos? pActual = (from e in _restauranteContext.platos
                                     where e.platoId == id
                                     select e).FirstOrDefault();
            if (pActual == null)
            {
                return NotFound();
            }
            pActual.nombrePlato = pModificar.nombrePlato;
            pActual.precio = pModificar.precio;
            
           

            _restauranteContext.Entry(pActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(pActual);
        }

        //Eliminar registros
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult ElimiarPlato(int id)
        {
            platos? plato = (from e in _restauranteContext.platos
                               where e.platoId == id
                               select e).FirstOrDefault();
            if (plato == null)
            {
                return NotFound();
            }

            _restauranteContext.platos.Attach(plato);
            _restauranteContext.platos.Remove(plato);
            _restauranteContext.SaveChanges();

            return Ok(plato);
        }
    }
}
