using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.DTO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Static;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.DTO.UserDTO;
using Microsoft.AspNetCore.Cors;

namespace System.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("EnableCORS")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        AbstractUnitOfWork _abstractUnitOfWork;
        public AuthController(AbstractUnitOfWork abstractUnitOfWork)
        => _abstractUnitOfWork = abstractUnitOfWork;

        [HttpPost]
        public IActionResult Login([FromBody] UserLoginDTO userLoginDto)
        {
            try
            {
                var user = _abstractUnitOfWork.Repository<User>()
                   .GetIQueryable(c => c.Username == userLoginDto.Username && c.IsEnabled == true)
                   .Include(u => u.UserGroup)
                       .ThenInclude(g => g.Group)
                           .ThenInclude(rg => rg.GroupPrivilage)
                               .ThenInclude(r => r.Privilage)
                   .FirstOrDefault();
                if (user == null)
                {
                    var message = Messages.NotFound;
                    message.ActionName = "Login";
                    message.ControllerName = "Auth";
                    message.Message = "المستخدم غير موجود";
                    return NotFound(message);
                }

                if (!MD5Hash.VerifyMd5Hash(userLoginDto.Password, user.Password))
                    return BadRequest();

                //security key
                string securityKey = "this_is_our_supper_long_security_key_for_token_validation_project_2018_09_07$smesk.in";
                //symmetric security key
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

                //signing credentials
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

                //add claims
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Email, user.Username));
                foreach (var Role in user.UserGroup.SelectMany(c => c.Group.GroupPrivilage.Select(r => r.Privilage)))
                {
                    claims.Add(new Claim(ClaimTypes.Role, Role.Name));
                }
                //create token
                var token = new JwtSecurityToken(
                        issuer: "smesk.in",
                        audience: "readers",
                        expires: DateTime.Now.AddMinutes(7 * 60),
                        signingCredentials: signingCredentials
                        , claims: claims
                    );
                AuthUserDTO auth = new AuthUserDTO()
                {
                    Username = user.Name,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Roles = user.UserGroup.SelectMany(c => c.Group.GroupPrivilage.Select(p => p.Privilage.Name).Distinct()).ToList(),
                    MainPage = "weee"
                };
                return Ok(auth);
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}