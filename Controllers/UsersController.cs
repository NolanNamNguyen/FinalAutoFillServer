using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using FinalAutoFillServer.Interfaces;
using FinalAutoFillServer.Helpers;
using FinalAutoFillServer.Entities;
using FinalAutoFillServer.DTO;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AutoMapper;

namespace FinalAutoFillServer.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserRepository userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            //map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                //create user
                _userService.Create(user, model.Password, Request.Headers["origin"]);
                return Ok(new { message = "Registration successful, please check your email for verification instructions" });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("hola")]
        public IActionResult Hello()
        {
            return Ok("hola wtf man seriously?");
        }

        [Authorize(Roles = Role.Customer)]
        [HttpPost("verify_account")]
        public IActionResult VerifyAccount([FromBody] CheckAppUser inUser)
        {
            User user = _userService.GetById(inUser.UserId);
            if (!user.CurrentToken.Equals(inUser.Token))
            {
                return Forbid();
            }
            return Ok(inUser);
        }

        //[Authorize(Roles = Role.Customer)]
        //[HttpPost("get_excel_data")]
        //public IActionResult ReadExcelFIle([FromForm] ExcelFileModle excelModel)
        //{
        //    var supportedTypes = new[] { ".xlsx", ".xls" };
        //    string fileExt = Path.GetExtension(excelModel.ExcelFile.FileName).ToLower();
        //    if (!supportedTypes.Contains(fileExt))
        //        throw new AppException("File Extension Is InValid - Only Upload jpg/png/bmp/gif/jpeg/webp File");
        //    string fileName = excelModel.UserName.Trim() + "excel" + fileExt;
        //    string filePath = Path.Combine("wwwroot\\ExcelFiles", fileName);
        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        excelModel.ExcelFile.CopyTo(fileStream);
        //    }
        //    string excelFilPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
        //    List<AmazonFormModel> formList = _excelService.GetExcelDataList(excelFilPath);
        //    return Ok(formList);
        //}

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.UserName, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            _userService.UpdateToken(user, tokenString);
            if (user.Admin != null)
                return Ok(new
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Name = user.Name,
                    Phone = user.Phone,
                    Email = user.Email,
                    Role = user.Role,
                    AdminId = user.Admin.AdminId,
                    Created = user.Created,
                    Updated = user.Updated,
                    Token = tokenString
                });
            return Ok(new
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Name = user.Name,
                Phone = user.Phone,
                Email = user.Email,
                Role = user.Role,
                Created = user.Created,
                Updated = user.Updated,
                Token = tokenString
            });
        }
    }
}
