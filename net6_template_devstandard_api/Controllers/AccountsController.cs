using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using net6_template_devstandard_api.Models;
using net6_template_devstandard_api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace net6_template_devstandard_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IAdService adService;
        public AccountsController(IAdService adService
            , IConfiguration configuration)
        {
            this.adService = adService;
            this.configuration = configuration;
        }
        [HttpPost("token")]
        public ActionResult GetToken(GetTokenRequest getTokenRequest)
        {
            try
            {
                var response = adService.ValidateCredentials(getTokenRequest.EmpId, getTokenRequest.Password);
                if (response)
                {
                    var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, getTokenRequest.EmpId),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtBearer:JwtKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expires = DateTime.Now.AddDays(Convert.ToDouble(configuration["JwtBearer:JwtExpireDays"]));

                    var token = new JwtSecurityToken(
                        issuer: configuration["JwtBearer:JwtIssuer"],
                        audience: configuration["JwtBearer:JwtAudience"],
                        claims: claims,
                        expires: expires,
                        signingCredentials: creds
                    );

                    return Ok(new
                    {
                        isuccess = true,
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
                else
                {
                    return StatusCode(200, new
                    {
                        issuccess = false,
                        message = "Wrong Username or Password"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    issuccess = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost("faketoken")]
        public ActionResult FakeToken()
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "11316684"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtBearer:JwtKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(Convert.ToDouble(configuration["JwtBearer:JwtExpireDays"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JwtBearer:JwtIssuer"],
                    audience: configuration["JwtBearer:JwtAudience"],
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return Ok(new
                {
                    isuccess = true,
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    issuccess = false,
                    message = ex.Message
                });
            }
        }
    }
}
