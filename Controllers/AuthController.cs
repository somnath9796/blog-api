using CodePulse.API.Models.DTO;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO request)
        {
            try
            {
            var checkUserExists =  await _userManager.FindByEmailAsync(request.Email);

                if (checkUserExists != null)
                {
                    //check password

                   var chkpassword = await _userManager.CheckPasswordAsync(checkUserExists, request.Password);

                    if(chkpassword)
                    {
                        //Get Roles
                        var userRole = await _userManager.GetRolesAsync(checkUserExists);

                        // Create Token Response
                     var token =  _tokenRepository.CreateJWTToken(checkUserExists, userRole.ToList());

                        var response = new LoginResponseDTO
                        {
                            Email = request.Email,
                            Roles = userRole.ToList(),
                            Token = token
                        };

                        return Ok(response);
                    }
                   
                }
                ModelState.AddModelError("", "Email Or Password Incorrect");

                return ValidationProblem(ModelState);
            }
            catch (Exception ex) 
            { 
            return BadRequest(ex);
            }
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = request.Email?.Trim(),
                    Email = request.Email?.Trim()
                };
                var identityresult = await _userManager.CreateAsync(user,request.Password);

                //Create User
                if (identityresult.Succeeded)
                {
                    //Add Role to user ie reader role

                    identityresult = await _userManager.AddToRoleAsync(user, "Reader");

                    if (identityresult.Succeeded)
                    {
                        return Ok();
                    }

                }
                else
                {
                    if (identityresult.Errors.Any())
                    {
                        foreach (var error in identityresult.Errors)
                        {
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                return ValidationProblem(ModelState);
            }
            catch (Exception ex) 
            {
            return BadRequest(ex.Message);
            }
        }

    }
}
