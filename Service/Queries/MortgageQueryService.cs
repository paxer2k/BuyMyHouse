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

            if (mortgage == null)
                throw new BadRequestException($"The mortgage with id {id} does not exist!");

            if (mortgage.ExpiresAt < DateTime.Now)
                throw new ForbiddenException("The time for viewing this mortgage application has expired!");

            return _mapper.Map<MortgageResponseDTO>(mortgage);
        }

        /// <summary>
        /// Method for retrieving mortgages based on their status
        /// </summary>
        /// <param name="mortgageStatus"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Mortgage>> GetMortgagesByStatusAsync(MortgageStatus mortgageStatus)
        {
            return await _mortgageQueryRepository.GetAllByConditionAsync(m => m.MortgageStatus == mortgageStatus);
        }

        /// <summary>
        /// Method for getting all the mortgages which are either Approved or Declined and for those that email has not been sent yet.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Mortgage>> GetProcessedMortgages()
        {
            return await _mortgageQueryRepository.GetAllByConditionAsync(m => (m.MortgageStatus == MortgageStatus.Declined || m.MortgageStatus == MortgageStatus.Approved) && !m.IsEmailSent);
        }
    }
}
