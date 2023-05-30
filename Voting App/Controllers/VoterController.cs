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
    public class VoterController: Controller
    {
        private readonly IMapper _mapper;
        private readonly VoterService _voterCollection;

        public VoterController(VoterService voterCollection, IMapper mapper)
        {
            _mapper = mapper;
            _voterCollection = voterCollection;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<Voter>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVoters()
        {
            var voters = _mapper.Map<List<Voter>>(await _voterCollection.GetVoters());
            if (voters == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(voters);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(Voter))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVoter(string name)
        {
            if (name == null)
                return BadRequest(ModelState);
            var voter = _mapper.Map<VoterDto>(await _voterCollection.GetVoter(name));
            if (voter == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(voter);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateDriver([FromBody] VoterDto createVoter)
        {
            if (createVoter == null)
                return BadRequest(ModelState);
            else
            {

                var name = createVoter.Name == null ? "" : createVoter.Name;
                var voter = await _voterCollection.GetVoter(name);
                if (voter != null)
                {
                    ModelState.AddModelError("", "Voter already exists.");
                    return BadRequest(ModelState);
                }
                else
                {
                    var cityMap = _mapper.Map<City>(createVoter.City);
                    var voterMap = _mapper.Map<Voter>(createVoter);
                    voterMap.City = cityMap;
                    await _voterCollection.CreateVoter(voterMap);
                }
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok();
        }

        [HttpPut("{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateVoter(string name, [FromBody] VoterDto updateVoter)
        {
            if (updateVoter == null)
                return BadRequest(ModelState);
            if (name != updateVoter.Name)
                return BadRequest(ModelState);
            var voter = await _voterCollection.GetVoter(name);
            if(voter == null)
            {
                ModelState.AddModelError("", "Voter does not exist");
                return NotFound(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var voterMap = _mapper.Map<Voter>(updateVoter);
            voterMap.Id = voter.Id;
            await _voterCollection.UpdateVoter(voterMap);
            return NoContent();
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteVoter(string name)
        {
            if (name == null)
                return BadRequest(ModelState);
            var voter = await _voterCollection.GetVoter(name);
            if(voter == null)
            {
                ModelState.AddModelError("", "Voter does not exist.");
                return NotFound(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _voterCollection.DeleteVoter(name);
            return NoContent();
        }





    }
}
