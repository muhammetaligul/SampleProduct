using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleProduct.ORM.Models.DB;
using SampleProduct.Service;

namespace SampleProduct.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepository<User> userRepository;
        public LoginController(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }





    }
}