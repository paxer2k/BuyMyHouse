using AutoMapper;
using DAL.Repository.Interfaces;
using Domain;
using Domain.DTOs;
using Domain.Overview;
using Service.Exceptions;
using Service.Queries.Interfaces;
using Domain.Enums;

namespace Service.Queries
{
    public class MortgageQueryService : IMortgageQueryService
    {
        private readonly IQueryRepository<Mortgage> _mortgageQueryRepository;
        private readonly IMapper _mapper;
        public MortgageQueryService(IQueryRepository<Mortgage> mortgageQueryRepository, IMapper mapper)
        {
            _mortgageQueryRepository = mortgageQueryRepository;
            _mapper = mapper;
        }

        public async Task<GenericOverview<MortgageResponseDTO>> GetAllMortgagesAsync(int startIndex, int endIndex)
        {
            var mortgages = await _mortgageQueryRepository.GetAllAsync();

            if (mortgages == null)
                throw new BadRequestException("The mortages could not be retrieved");

            var mortgageResponseDto = _mapper.Map<IEnumerable<MortgageResponseDTO>>(mortgages);

            var mortgageOverview = new GenericOverview<MortgageResponseDTO>()
            {
                Data = mortgageResponseDto.Skip(startIndex).Take(endIndex - startIndex + 1).ToList(),
                Total = mortgageResponseDto.Count(),
            };

            return mortgageOverview;
        }

        public async Task<MortgageResponseDTO> GetMortgageByIdAsync(Guid id)
        {
            var mortgage = await _mortgageQueryRepository.GetByConditionAsync(m => m.Id == id);

            if (mortgage is null)
                throw new BadRequestException($"The mortgage with id {id} does not exist!");

            if (mortgage.ExpiresAt < DateTime.Now)
                throw new ForbiddenException("The time for viewing this mortgage application has expired!");

            return _mapper.Map<MortgageResponseDTO>(mortgage);
        }

        public async Task<IEnumerable<Mortgage>> GetMortgagesByStatusAsync(ApplicationStatus applicationStatus)
        {
            return await _mortgageQueryRepository.GetAllByConditionAsync(m => m.ApplicationStatus == applicationStatus);
        }

        public async Task<IEnumerable<Mortgage>> GetFinishedMortgages()
        {
            return await _mortgageQueryRepository.GetAllByConditionAsync(m => (m.ApplicationStatus == ApplicationStatus.Declined || m.ApplicationStatus == ApplicationStatus.Approved) && !m.IsEmailSent);
        }
    }
}
