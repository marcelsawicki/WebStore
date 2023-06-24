﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReshopApp.Models;
using ReshopApp.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReshopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("login")]
        public async Task<bool> Login([FromBody] UserCredentials userCredentials)
        {
            try
            {
                var isValid = _userService.CheckPasswordSignIn(userCredentials.Login, userCredentials.Password);

                if (!isValid)
                    return false;


      

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                };

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, "testUserId")
                };

                var userIdentity = new ClaimsIdentity(claims, "webuser");
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    authProperties);
                return isValid;
            }
            catch (Exception ex)
            { 
            var message = ex.Message;
                return false;
            }
        }

        [HttpGet]
        [Route("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
