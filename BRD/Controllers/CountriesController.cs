using BRD.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BRD.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countries;
        public CountriesController(ICountryRepository countries)
        {
            _countries = countries;
        }

        
        [HttpGet]
        public IActionResult Get()
        {
            var lst = _countries.GetAll();
            return Ok(lst);
        }

       
    }
}
