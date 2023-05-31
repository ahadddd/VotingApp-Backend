using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Voting_App.Dto;
using Voting_App.Models;
using Voting_App.Services;

namespace Voting_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController: Controller
    {
        private readonly IMapper _mapper;
        private readonly CityService _cityService;

        public CityController(CityService cityCollection, IMapper mapper)
        {
            _mapper = mapper;
            _cityService = cityCollection;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<City>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _cityService.GetCities();
            if (cities == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(cities);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(City))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCity(string name)
        {
            if (name == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var city = await _cityService.GetCity(name);
            if (city == null)
                return NotFound();
            return Ok(city);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateCity([FromBody] CityDto createCity)
        {
            if (createCity == null)
                return BadRequest(ModelState);
            else
            {
                var name = createCity.Name == null ? "" : createCity.Name;
                var city = await _cityService.GetCity(name);
                if (city != null)
                {
                    ModelState.AddModelError("", "City already exists.");
                    return BadRequest(ModelState);
                }
                else
                {
                    var cityMap = _mapper.Map<City>(createCity);
                    await _cityService.CreateCity(cityMap);
                }
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok();
        }

        //[HttpPut("{name}")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //public async Task<IActionResult> UpdateCity(string name, [FromBody] CityDto updateCity)
        //{
        //    if (updateCity == null)
        //        return BadRequest(ModelState);
            
        //    var city = await _cityService.GetCity(name);
        //    if(city == null)
        //    {
        //        ModelState.AddModelError("", "City does not exist");
        //        return BadRequest(ModelState);
        //    }
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    var cityMap = _mapper.Map<City>(updateCity);
        //    cityMap.Id = city.Id;
        //    await _cityService.UpdateCity(cityMap);
        //    return NoContent();
        //}

        [HttpDelete("{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCity(string name)
        {
            if (name == null)
                return BadRequest(ModelState);
            var city = await _cityService.GetCity(name);
            if (city == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _cityService.DeleteCity(name);
            return NoContent();
        }

    }
}
