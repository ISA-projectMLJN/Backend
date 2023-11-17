﻿using Medicina.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Medicina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        public readonly UserContext _userContext;
        public UserController(IConfiguration config, UserContext userContext)
        {
            _config = config;
            _userContext = userContext;
        }

        [HttpPost("CreateUser")]
        public IActionResult Create(User user)
        {
            user.MemberSince = DateTime.Now;
            _userContext.Add(user);
            _userContext.SaveChanges();
            return Ok("Succes from Create Method");
        }

    }
}


//https://localhost:44356/
