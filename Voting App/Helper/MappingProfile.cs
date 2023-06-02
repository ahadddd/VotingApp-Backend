using AutoMapper;
using Voting_App.Dto;
using Voting_App.Models;

namespace Voting_App.Helper
{
    public class MappingProfile: Profile
    {
        public MappingProfile() {

            CreateMap<Candidate, CandidateDto>();
            CreateMap<CandidateDto, Candidate>();
            CreateMap<Voter, VoterDto>();
            CreateMap<VoterDto, Voter>();
            
            CreateMap<Vote, VoteDto>();
            CreateMap<VoteDto, Vote>();
        }
    }
}
