using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext context;
        public UsersController(DataContext context)
        {
            this.context = context;

        }

        //    api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> getUsers(){

            var users = await this.context.Users.ToListAsync();

            return users;
        }

        //    api/users/1 
        [HttpGet("{id}")]

        //también es posible sin ActionResult y IEnumerable
        public async Task<List <AppUser>> getUsers (int id){

            var users = await this.context.Users.Where(x=> x.Id==id).ToListAsync();

            return users;
        }
    }
}