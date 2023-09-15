using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using battleship_server.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace battleship_server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthenticationController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response = await _authRepository.Register(
                new User { Username = request.Username }, request.Password
            );

            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }
    }
}