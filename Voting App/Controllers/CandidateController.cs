using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    }
}
