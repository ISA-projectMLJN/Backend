using Medicina.Context;
using Medicina.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Medicina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class EquipmentController : ControllerBase
    {
        private readonly IConfiguration _config;
        public readonly EquipmentContext _equipmentContext;
        public readonly CompanyContext _companyContext;

        public EquipmentController(IConfiguration config, EquipmentContext equipmentContext, CompanyContext companyContext)
        {
            _config = config;
            _equipmentContext = equipmentContext;
            _companyContext = companyContext;
        }      

        [HttpGet("GetAllEquipments")]
        public ActionResult<IEnumerable<Equipment>> GetAll()
        {
            var equipments = _equipmentContext.Equipment.ToList();

            if (equipments == null)
            {
                return NotFound();
            }

            return Ok(equipments);
        }
        [HttpGet("GetCompanyEquipmentById/{id}")]
        public ActionResult<Company> GetCompanyEquipmentById(int id)
        {
            var equipmentList = _equipmentContext.Equipment.Where(e => e.CompanyId == id).ToList();




            if (equipmentList == null)
            {
                return NotFound();
            }

            return Ok(equipmentList);
        }
    }
}
