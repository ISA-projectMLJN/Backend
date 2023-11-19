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

    public class EquipmentCompanyController: ControllerBase


    {
        private readonly IConfiguration _config;
        public readonly CompanyContext _companyContext;
        public readonly EquipmentContext _equipmentContext;
        public readonly EquipmentCompanyContext _equipmentCompanyContext;

        public EquipmentCompanyController(IConfiguration config, CompanyContext companyContext, EquipmentContext equipmentContext, EquipmentCompanyContext equipmentCompanyContext)
        {
            _config = config;
            _companyContext = companyContext;
            _equipmentContext = equipmentContext;
            _equipmentCompanyContext = equipmentCompanyContext;
        }

        [HttpGet("GetEquipmentByCompanyId/{companyId}")]
        public ActionResult<IEnumerable<Equipment>> GetEquipmentByCompanyId(int companyId)
        {
            var equipmentCompanies = _equipmentCompanyContext.EquipmentCompanies
                .Where(ec => ec.CompanyId == companyId)
                .ToList();

            var equipmentListIds = equipmentCompanies.Select(ec => ec.EquipmentId).ToList();
            var equipmentList = new List<Equipment>();

            foreach (var equipmentId in equipmentListIds)
            {
                var equipment = _equipmentContext.Equipments.Find(equipmentId);
                if (equipment != null)
                {
                    equipmentList.Add(equipment);
                }
            }

            return Ok(equipmentList);
        }



        [HttpGet("GetCompanybyEquipmentId/{equipmentId}")]
        public ActionResult<IEnumerable<Equipment>> GetCompanybyEquipmentId(int equipmentId)
        {
            var equipmentCompanies = _equipmentCompanyContext.EquipmentCompanies
                .Where(ec => ec.CompanyId == equipmentId)
                .ToList();

            var companyListIds = equipmentCompanies.Select(ec => ec.CompanyId).ToList();
            var companyList = new List<Company>();

            foreach (var companyId in companyListIds)
            {
                var company = _companyContext.Companies.Find(equipmentId);
                if (company != null)
                {
                    companyList.Add(company);
                }
            }

            return Ok(companyList);
        }
    }
}