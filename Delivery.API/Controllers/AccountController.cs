﻿using Delivery.API.Repository.Account;
using Delivery.Models.ModelView;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Delivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository repo;

        public AccountController(IAccountRepository repository)
        {
            repo = repository;
        }

        // Register
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            if (ModelState.IsValid)
            {
                var result = await repo.RegisterAsync();
            }
            return BadRequest();
        }
    }
}