using BRD.DataModel;
using BRD.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BRD.API.Controllers
{
    [Route("api/v1/country")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countries;
        private readonly IAccountRepository _account;
        public CountriesController(ICountryRepository countries, IAccountRepository account)
        {
            _countries = countries;
            _account= account;
        }


        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("get-all")]
        public async Task<IActionResult> Get()
        {
            var lst = _countries.GetAll();
            return Ok(lst);
        }


        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("get-all-account")]
        public async Task<IActionResult> GetAccount()
        {
            var lst = _account.GetAll();
            return Ok(lst);
        }


    }
}
