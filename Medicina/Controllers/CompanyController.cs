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
        public readonly UserContext _userContext;

        public CompanyController(IConfiguration config, CompanyContext companyContext, EquipmentContext equipmentContext,UserContext userContext)
        {
            _config = config;
            _companyContext = companyContext;
            _equipmentContext = equipmentContext;
            _userContext = userContext;
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

        [HttpPost("RegisterCompany/{selectedUserId}")]
        public IActionResult RegisterCompany(Company company, int selectedUserId)
        {
            // Extracting data from CompanyData

            User user = _userContext.Users.FirstOrDefault(u => u.UserID == selectedUserId);





            if (ModelState.IsValid)
            {
                if (user != null)
                {




                    _companyContext.Add(company);

                    _companyContext.SaveChanges();
                    user.CompanyId = company.Id;
                    _userContext.Users.Update(user);
                    _userContext.SaveChanges();
                    return Ok("Company registered successfully.");
                }
                else
                {
                    return BadRequest("No user with CAMPAIN_ADMIN role found.");
                }
            }
            else
            {
                return BadRequest("Invalid model state. Check your inputs.");
            }
        }
    }
}
