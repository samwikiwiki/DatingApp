using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){
            
            if (await UserExist(registerDto.Username)) return BadRequest("Username is taken");
            
            /*
            INICIALIZA UNA INSTANCIA DE LA CLASE HASH QUE GENERARÁ UNA LLAVE ALEATORIA

            ESTA LLAVE ALEATORIA, SERÁ GUARDADA COMO PASSWORD SALT

            COMPUTEDHASH HARÁ UN CALCULO MEDIANTE ESTA LLAVE ALEATORIA PARA CIFRAR LA CONTRASEÑA A HASH

            SI NO GUARDARAMOS ESTA LLAVE, NUNCA PODRÍAMOS CALCULAR LA CONTRASEÑA ORIGINAL A HASH PARA COMPARARLA EN EL LOGIN
            CON LA CONTRASEÑA ALMACENADA EN LA BASE DE DATOS EN EL LOGIN
            */
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                //encoding para convertir el string en byte
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            //tracea la entidad dada e insertara los datos a la base de datos cuando se llame a .SaveChanges
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        //    api/account/login 
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){

            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName==loginDto.Username.ToLower());

            if (user==null) return Unauthorized("Invalid username");

            //inicializamos la instancia HMACSHA512 con la llave del passowrd salt
            using var hmac = new HMACSHA512(user.PasswordSalt);

            //codificamos a hash con la llave que usamos en el registro para compararla posteriormente
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i=0; i<computedHash.Length; i++){

                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExist(string username){

            return await _context.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }
    }
}