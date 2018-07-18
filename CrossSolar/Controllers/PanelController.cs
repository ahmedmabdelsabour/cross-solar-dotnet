using System.Threading.Tasks;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Http.Cors;
//using System.Web.Http;
//using System.Net.Http;
//using System.Net;

namespace CrossSolar.Controllers
{
    //
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    public class PanelController : ControllerBase
    {
        private readonly IPanelRepository _panelRepository;
        

        public PanelController(IPanelRepository panelRepository)
        {
            _panelRepository = panelRepository;
        }

        //[System.Web.Http.HttpGet]
        //public HttpResponseMessage Get()
        //{
        //    return null; // Request.CreateResponse(HttpStatusCode.OK, null);
        //}

        // POST api/panel
        [HttpPost]
        public async Task<IActionResult> Register([System.Web.Http.FromBody] PanelModel value)
        {
            if (!ModelState.IsValid) return (IActionResult) BadRequest(ModelState);

            var panel = new Panel
            {
                Latitude = value.Latitude,
                Longitude = value.Longitude,
                Serial = value.Serial,
                Brand = value.Brand
            };

            await _panelRepository.InsertAsync(panel);

            return (IActionResult) Created($"panel/{panel.Id}", panel);
        }
    }
}