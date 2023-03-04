using L01_2020_LM_650.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020_LM_650.Controllers
{
    public class motoristasController : Controller
    {
        private readonly restauranteContext _restauranteContext;

        public motoristasController(restauranteContext restauranteContexto)
        {
            _restauranteContext = restauranteContexto;
        }
        //CRUD
        //Obtener listado
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<motoristas> listadoMotoristas = (from e in _restauranteContext.motoristas
                                                  select e).ToList();

            if (listadoMotoristas.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoMotoristas);
        }

        //Guardar nuevo registro
        [HttpPost]
        [Route("Add")]
        public IActionResult AgregarMotorista([FromBody] motoristas motorista)
        {
            try
            {
                _restauranteContext.motoristas.Add(motorista);
                _restauranteContext.SaveChanges();
                return Ok(motorista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Modificar registros
        [HttpPut]
        [Route("Modificar/{id}")]
        public IActionResult ModificarMotorista(int id, [FromBody] motoristas mModificar)
        {
            motoristas? mActual = (from e in _restauranteContext.motoristas
                               where e.motoristaId == id
                               select e).FirstOrDefault();
            if (mActual == null)
            {
                return NotFound();
            }
            mActual.nombreMotorista = mModificar.nombreMotorista;



            _restauranteContext.Entry(mActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(mActual);
        }

        //Eliminar registros
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult ElimiarMotorista(int id)
        {
            motoristas? motorista = (from e in _restauranteContext.motoristas
                             where e.motoristaId == id
                             select e).FirstOrDefault();
            if (motorista == null)
            {
                return NotFound();
            }

            _restauranteContext.motoristas.Attach(motorista);
            _restauranteContext.motoristas.Remove(motorista);
            _restauranteContext.SaveChanges();

            return Ok(motorista);
        }

        //Filtado precios
        [HttpGet]
        [Route("Filtro/{nombre}")]
        public IActionResult FiltrarPrecios(string nombre)
        {
            motoristas? motorista = (from e in _restauranteContext.motoristas
                             where e.nombreMotorista.Contains(nombre)
                             select e).FirstOrDefault();
            if (motorista == null)
            {
                return NotFound();
            }
            return Ok(motorista);
        }
    }
}
