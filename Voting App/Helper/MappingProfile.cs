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
            CreateMap<City, CityDto>();
            CreateMap<CityDto, City>();
            CreateMap<Voter, VoterDto>();
            CreateMap<VoterDto, Voter>();
            CreateMap<VoteDto, Voter>();
            CreateMap<Vote, VoteDto>();
        }
    }
}
