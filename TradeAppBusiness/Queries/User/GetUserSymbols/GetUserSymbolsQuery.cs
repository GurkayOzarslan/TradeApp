using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TradeAppApplication.Responses.User;
using TradeAppSharedKernel.Application;
using TradeAppSharedKernel.Infrastructure.Helpers.TokenInfo;

namespace TradeAppApplication.Queries.User.GetUserSymbolList
{
    public class GetUserSymbolsQuery : IQuery<List<UserSymbolsResponse>>
    {
    }

    public class GetUserSymbolsQueryHandler : IQueryHandler<GetUserSymbolsQuery, List<UserSymbolsResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly ITokenInfoHandler _tokenInfoHandler;
        private readonly IMapper _mapper;

        public GetUserSymbolsQueryHandler(IAppDbContext context, ITokenInfoHandler tokenInfoHandler, IMapper mapper)
        {
            _context = context;
            _tokenInfoHandler = tokenInfoHandler;
            _mapper = mapper;
        }
        public async Task<List<UserSymbolsResponse>> Handle(GetUserSymbolsQuery request, CancellationToken cancellationToken)
        {
            var tokenInfo = _tokenInfoHandler.TokenInfo();
            var userId = tokenInfo.NameIdentifier;
            var symbols = await _context.UserSymbols
                        .Where(us => us.UserId == userId)
                        .ProjectTo<UserSymbolsResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
            return symbols;
        }
    }
}
