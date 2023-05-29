using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using Voting_App.Dto;
using Voting_App.Models;
using Voting_App.Services;

namespace Voting_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController : Controller
    {
        private readonly IMapper _mapper;
        private readonly CandidateService _candidateService;

        public CandidateController(CandidateService candidateService, IMapper mapper)
        {
            _mapper = mapper;
            _candidateService = candidateService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Collection<Candidate>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCandidates()
        {
            var candidates = _mapper.Map<List<Candidate>>(await _candidateService.GetCandidates());
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
                var candMap = _mapper.Map<CandidateDto>(candidate);
                return Ok(candMap);
            }
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
            else
            {
                var candidate = await _candidateService.GetCandidate(createCandidate.Name);
                if (candidate != null)
                {
                    ModelState.AddModelError("", "Candidate already exists.");
                    return BadRequest(ModelState);
                }
                else
                {
                    var candidateMap = _mapper.Map<Candidate>(createCandidate);
                    await _candidateService.CreateCandidate(candidateMap);
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok("Candidate added.");
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
            return Ok("Candidate updated.");
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
            return Ok("Candidate deleted.");
        }

    }
}
