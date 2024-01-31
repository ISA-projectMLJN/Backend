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
    public class CompanyRateController : ControllerBase
    {
        private readonly IConfiguration _config;
        public readonly CompanyRateContext _companyrateContext;
        public readonly CompanyContext _companyContext;
        public readonly ReservationContext _reservationContext;
        public readonly EquipmentContext _equipmentContext;

        public CompanyRateController(IConfiguration config, CompanyRateContext companyrateContext, CompanyContext companyContext)
        {
            _config = config;
            _companyrateContext = companyrateContext;
            _companyContext = companyContext;
        }

        [HttpGet("GetCompanyRateById/{id}")]
        public ActionResult<Company> GetCompanyRateById(int id)
        {
            var rateList = _companyrateContext.CompanyRates.Where(e => e.CompanyId == id).ToList();


            if (rateList == null)
            {
                return NotFound();
            }

            return Ok(rateList);
        }

        [HttpPost("RateCompany/{selectedCompanyId}")]
        public IActionResult Rate(int selectedCompanyId, CompanyRate rate)
        {
            // Proceed with rating
            Company company = _companyContext.Companies.FirstOrDefault(u => u.Id == selectedCompanyId);
            if (company == null)
            {
                return NotFound("Company not found.");
            }

            // Save the rating
            _companyrateContext.Add(rate);
            _companyrateContext.SaveChanges();

            return Ok("Rating added successfully");
        }


    }
}
