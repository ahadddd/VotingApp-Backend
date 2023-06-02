using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using ThirdParty.Json.LitJson;
using Voting_App.Dto;
using Voting_App.Models;
using Voting_App.Services;
using Newtonsoft.Json;

namespace Voting_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController : Controller
    {
        private readonly IMapper _mapper;
        private readonly CandidateService _candidateService;
        private readonly VoterService _voterService;
        private readonly VoteService _voteService;

        public CandidateController(CandidateService candidateService, VoteService voteService, IMapper mapper, VoterService voterService)
        {
            _mapper = mapper;
            _candidateService = candidateService;
            _voterService = voterService;
            _voteService = voteService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Candidate>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCandidates()
        {
            var candidates = _mapper.Map<IEnumerable<Candidate>>(await _candidateService.GetCandidates());
            if (candidates == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(candidates);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(Candidate))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCandidate(string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var candidate = await _candidateService.GetCandidate(name);
            if (candidate == null)
                return NotFound();
            else
            {
                var candMap = _mapper.Map<Candidate>(candidate);
                return Ok(candMap);
            }
        }

        [HttpGet("id/{ID}")]
        [ProducesResponseType(200, Type = typeof(Candidate))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCandidateByID(string ID)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(ID == null)
                return BadRequest(ModelState);
            var candidate = await _candidateService.GetCandidateByID(ID);
            if (candidate == null)
                return NotFound();
            return Ok(candidate);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCandidate([FromBody] CandidateDto createCandidate)
        {
            if (createCandidate == null)
            {   
                return BadRequest(ModelState);
            }
            else if(createCandidate != null)
            {
                
                var candidate = await _candidateService.GetCandidate(createCandidate.Name);
                if (candidate != null)
                {
                    ModelState.AddModelError("", "Candidate already exists.");
                    return BadRequest(ModelState);
                }
                
                else
                {
                    if (createCandidate.City == null)
                        return BadRequest(ModelState);

                    var candidateMap = _mapper.Map<Candidate>(createCandidate);
                    if(createCandidate.City != null)
                    {
                        candidateMap.City = (string)createCandidate.City;
                        await _candidateService.CreateCandidate(candidateMap);
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPut("{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCandidate(string name, [FromBody] CandidateDto updateCandidate)
        {
            if (updateCandidate == null)
                return BadRequest(ModelState);
            if (updateCandidate.Name != name)
                return BadRequest(ModelState);
            var candidate = await _candidateService.GetCandidate(name);
            if (candidate == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var candidateMap = _mapper.Map<Candidate>(updateCandidate);
            candidateMap.Id = candidate.Id;
            await _candidateService.UpdateCandidate(candidateMap);
            return NoContent();
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCandidate(string name)
        {
            if (name == null)
                return BadRequest(ModelState);
            var candidate = await _candidateService.GetCandidate(name);
            if (candidate == null)
                return NotFound();
            await _candidateService.DeleteCandidate(name);
            return Ok();
        }


        [HttpPut("candidate/castVote")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CastVote([FromBody] VoteDto vote)
        {
            if (vote != null)
            {

                if(vote.Candidate != null)
                {
                    var candidate = await _candidateService.GetCandidateByID(vote.Candidate);
                    if (candidate == null)
                    {
                        ModelState.AddModelError("", "Candidate/Voter was not passed in vote.");
                        return BadRequest(ModelState);
                    }
                    else
                    {
                        candidate.Votes.Add(vote);
                        await _candidateService.UpdateCandidate(candidate);
                    }
                }
            }
            return NoContent();
        }
    }
}
