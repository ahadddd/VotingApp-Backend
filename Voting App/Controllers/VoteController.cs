using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Voting_App.Dto;
using Voting_App.Models;
using Voting_App.Services;

namespace Voting_App.Controllers
{
    [ApiController]
    [Route("vote/[controller]")]
    public class VoteController: Controller
    {
        private readonly VoteService _voteService;
        private readonly IMapper _mapper;
        private readonly CandidateService _candidateService;
        private readonly VoterService _voterService;

        public VoteController(VoteService voteService,
            CandidateService candidateService,
            VoterService voterService,
            IMapper mapper)
        {
            _voteService = voteService;
            _mapper = mapper;
            _candidateService = candidateService;
            _voterService = voterService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Vote>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVotes()
        {
            var votes = await _voteService.GetVotes();
            if (!ModelState.IsValid || votes == null)
                return BadRequest(ModelState);
            return Ok(votes);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CastVote([FromBody] VoteDto vote)
        {
            if (vote == null)
            {
                ModelState.AddModelError("", "Vote not casted properly.");
                return BadRequest(ModelState);
            }
            else
            {
                var candidate = _mapper.Map<Candidate>(await _candidateService.GetCandidateByID(vote.Candidate));
                var voter = _mapper.Map<Voter>(await _voterService.GetVoterByID(vote.Voter));
                if (candidate == null || voter == null)
                {
                    ModelState.AddModelError("", "Either Candidate or Voter were not found.");
                    return BadRequest(ModelState);
                }
                else
                {
                    //create Vote and push it in DB
                    var createVote = new Vote();
                    createVote.Casted = true;
                    createVote.Candidate = candidate;
                    createVote.Voter = voter;
                    var voteMap = _mapper.Map<Vote>(createVote);
                    await _voteService.CreateVote(voteMap);
                }
            }
            return NoContent();
        }

    }
}
