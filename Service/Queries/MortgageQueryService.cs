using AutoMapper;
using DAL.Repository.Interfaces;
using Domain;
using Domain.DTOs;
using Domain.Overview;
using Service.Exceptions;
using Service.Queries.Interfaces;

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

        /// <summary>
        /// Method that retrieved non-approved mortgages of yesterday (for calculation)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Mortgage>> GetActiveMortgagesOfYesterdayAsync()
        {
            return await GetMortgagesOfYesterdayByApprovalAsync(false);
        }

        /// <summary>
        /// Method that retrieves approved mortgages of yesterday (for email sending)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Mortgage>> GetApprovedMortgagesOfYesterdayAsync()
        {
            return await GetMortgagesOfYesterdayByApprovalAsync(true);
        }

        /// <summary>
        /// Private method that filters mortgages of yesterday by the approved status
        /// </summary>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        private async Task<IEnumerable<Mortgage>> GetMortgagesOfYesterdayByApprovalAsync(bool isApproved)
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            return await _mortgageQueryRepository.GetAllByConditionAsync(m => m.CreatedAt >= yesterday && m.CreatedAt < today && m.IsApproved == isApproved);
        }
    }
}
