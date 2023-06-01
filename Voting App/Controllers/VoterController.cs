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
        private readonly VoterService _voterService;
        private readonly VoteService _voteService;
        private readonly CandidateService _candidateService;

        public VoterController(VoterService voterService, CandidateService candidateService, IMapper mapper, VoteService voteService)
        {
            _mapper = mapper;
            _voterService = voterService;
            _voteService = voteService;
            _candidateService = candidateService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<Voter>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVoters()
        {
            var voters = _mapper.Map<List<Voter>>(await _voterService.GetVoters());
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
            var voter = _mapper.Map<VoterDto>(await _voterService.GetVoter(name));
            if (voter == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(voter);
        }

        [HttpGet("id/{id}")]
        [ProducesResponseType(200, Type = typeof(Voter))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVoterByID(string id)
        {
            var voter = await _voterService.GetVoterByID(id);
            return Ok(voter);
        }



        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateVoter([FromBody] VoterDto createVoter)
        {
            if (createVoter == null)
                return BadRequest(ModelState);
            else
            {

                var name = createVoter.Name == null ? "" : createVoter.Name;
                var voter = await _voterService.GetVoter(name);
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
                    await _voterService.CreateVoter(voterMap);
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
            var voter = await _voterService.GetVoter(name);
            if(voter == null)
            {
                ModelState.AddModelError("", "Voter does not exist");
                return NotFound(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var voterMap = _mapper.Map<Voter>(updateVoter);
            voterMap.Id = voter.Id;
            await _voterService.UpdateVoter(voterMap);
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
            var voter = await _voterService.GetVoter(name);
            if(voter == null)
            {
                ModelState.AddModelError("", "Voter does not exist.");
                return NotFound(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _voterService.DeleteVoter(name);
            return NoContent();
        }


        [HttpPut("voter/castVote")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CastVote([FromBody] VoteDto vote)
        {
            if (vote == null)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var voter = await _voterService.GetVoterByID(vote.Voter);
                var candidate = await _candidateService.GetCandidateByID(vote.Candidate);
                var voteIDCatcher = await _voteService.GetVoteByVoterID(vote.Voter);
                if(voter == null || voteIDCatcher == null)
                {
                    ModelState.AddModelError("", "Voter/Vote could not be found to link vote with.");
                    return BadRequest(ModelState);
                }
                else
                {
                    var voteMap = new Vote();
                    voteIDCatcher.Id = voteMap.Id;
                    voteMap.Casted = true;
                    voteMap.Voter = voter;
                    voteMap.Candidate = candidate;
                    voter.VoteCasted = _mapper.Map<VoteDto>(voteMap);
                    var voterMap = _mapper.Map<Voter>(voter);
                    await _voterService.UpdateVoter(voterMap);
                }
            }
            return NoContent();
        }
    }
}
