using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;
using Voting_App.Dto;
using Voting_App.Models;
using Voting_App.Services;

namespace Voting_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController: Controller
    {
        private readonly IMapper _mapper;
        private readonly CandidateService _candidateService;

        public CandidateController(CandidateService candidateService, IMapper mapper)
        {
            _mapper = mapper;
            _candidateService = candidateService;
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200, Type = typeof(Candidate))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCandidates()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var candidates = await _candidateService.GetCandidates();
            var candMaps = _mapper.Map<List<CandidateDto>>(candidates);
            return Ok(candMaps);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCandidate([FromBody] CandidateDto createCandidate)
        {
            if(createCandidate == null)
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
    }
}
