using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOwnerCore.Application_Core.Email;
using ShopOwnerCore.Application_Core.Entities.Models;
using ShopOwnerCore.Application_Core.Entities.View_Model;
using ShopOwnerCore.Application_Core.Enum;
using ShopOwnerCore.Application_Core.Interface;
using ShopOwnerCore.Application_Core.Services;

namespace ShopOwnerCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private ILogger<AuthorizationController> _logger;
        private IAuthenticationManager _authenticationManager;

        public AuthorizationController( IMapper mapper, IUserService userService,ILogger<AuthorizationController> logger,IAuthenticationManager authenticationManager)
        {
            
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
            _authenticationManager = authenticationManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserViewModel model)
        {
            var User = _mapper.Map<UserViewModel, User>(model);
           

            var result = await _userService.CreateAsync(User, model.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _userService.AddUserToRolesAsync(User,model.Roles );

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationVm model)
        {
            if(! await _authenticationManager.ValidateUser(model))
            {
                _logger.LogInformation($"{nameof(Authenticate)}: Authentication Failed. Wrong username or password");
                return Unauthorized();
            }
            return Ok(new { Token = await _authenticationManager.CreateToken() });
        }


       


    }
}
