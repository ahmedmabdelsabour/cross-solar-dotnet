using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossSolar.Controllers
{
    [Route("api/panel")]
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        private readonly IPanelRepository _panelRepository;

        private readonly IOneHourElectricityRepository _oneHourElectricityRepository;

        public AnalyticsController(IAnalyticsRepository analyticsRepository, IPanelRepository panelRepository, IOneHourElectricityRepository oneHourElectricityRepository)
        {
            _analyticsRepository = analyticsRepository;
            _panelRepository = panelRepository;
            _oneHourElectricityRepository = oneHourElectricityRepository;
        }

        // GET panel/XXXX1111YYYY2222/analytics
        [HttpGet("{banelId}/[controller]")]
        public async Task<IActionResult> Get([FromRoute] string panelId)
        {
            var panel = await _panelRepository.Query()
                .FirstOrDefaultAsync(x => x.Serial.Equals(panelId, StringComparison.CurrentCultureIgnoreCase));

            if (panel == null) return NotFound();

            var analytics = await _analyticsRepository.Query()
                .Where(x => x.PanelId.Equals(panelId, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();

            var result = new OneHourElectricityListModel
            {
                OneHourElectricitys = analytics.Select(c => new OneHourElectricityModel
                {
                    Id = c.Id,
                    KiloWatt = c.KiloWatt,
                    DateTime = c.DateTime
                })
            };

            return Ok(result);
        }

        // GET panel/XXXX1111YYYY2222/analytics/day
        [HttpGet("{panelId}/[controller]/day")]
        public async Task<IActionResult> DayResults([FromRoute] string panelId)
        {
            List < OneDayElectricityModel > lstOneDay= new List<OneDayElectricityModel>();
           
            var query =(from oneHoureElec in _oneHourElectricityRepository.Query()
                         where oneHoureElec.PanelId == panelId
                         group oneHoureElec by new { oneHoureElec.PanelId, oneHoureElec.DateTime.Date } into g
                         select new
                         {
                             panelId = g.Key.PanelId,
                             DateT = g.Key.Date,
                             SUM = g.Sum(oh => oh.KiloWatt),
                             Minimum = g.Min(m => m.KiloWatt),
                             Maximum = g.Max(ma => ma.KiloWatt),
                             Average = g.Average(av => av.KiloWatt),
                         }).ToList();
            if (query!=null)
            {
                 
                foreach (var item in query)
                {
                    lstOneDay.Add(new OneDayElectricityModel
                    {
                        panelId = item.panelId,
                        DateTime = item.DateT,
                        Sum = item.SUM,
                        Minimum = item.Minimum,
                        Maximum = item.Maximum,
                        Average = item.Average
                    });
                }
            }


            var result = lstOneDay;

            return Ok(result);
        }

        // POST panel/XXXX1111YYYY2222/analytics
        [HttpPost("{panelId}/[controller]")]
        public async Task<IActionResult> Post([FromRoute] string panelId, [FromBody] OneHourElectricityModel value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var oneHourElectricityContent = new OneHourElectricity
            {
                PanelId = panelId,
                KiloWatt = value.KiloWatt,
                DateTime = DateTime.UtcNow
            };

            await _analyticsRepository.InsertAsync(oneHourElectricityContent);

            var result = new OneHourElectricityModel
            {
                Id = oneHourElectricityContent.Id,
                KiloWatt = oneHourElectricityContent.KiloWatt,
                DateTime = oneHourElectricityContent.DateTime
            };

            return Created($"panel/{panelId}/analytics/{result.Id}", result);
        }
    }
}