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
    public class CompanyController : ControllerBase
    {
        private readonly IConfiguration _config;
        public readonly CompanyContext _companyContext;
        public readonly EquipmentContext _equipmentContext;

        public CompanyController(IConfiguration config, CompanyContext companyContext, EquipmentContext equipmentContext)
        {
            _config = config;
            _companyContext = companyContext;
            _equipmentContext = equipmentContext;
        }

        [HttpGet("GetAllCompanies")]
        public ActionResult<IEnumerable<Company>> GetAll()
        {
            var companies = _companyContext.Companies.ToList();

            if (companies == null)
            {
                return NotFound();
            }

            return Ok(companies);
        }
        [HttpGet("GetCompanyById/{id}")]
        public ActionResult<Company> GetCompanyById(int id)
        {
            var company = _companyContext.Companies.Find(id);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }
        
        [HttpPatch("UpdateCompany")]
        public ActionResult<Company> UpdateCompany([FromBody] Company updatedCompany)
        {
            var existingCompany = _companyContext.Companies.Find(updatedCompany.Id);

            if (existingCompany == null )
            {
                return NotFound();
            }

            _companyContext.Entry(existingCompany).CurrentValues.SetValues(updatedCompany);
            _companyContext.SaveChanges(); 

            return Ok(existingCompany);
        }
    }
}
