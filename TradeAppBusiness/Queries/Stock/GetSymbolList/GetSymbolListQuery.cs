using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TradeAppApplication.Responses.Stock;
using TradeAppSharedKernel.Application;

namespace TradeAppApplication.Queries.Stock.GetSymbolList
{
    public class GetSymbolListQuery : IQuery<List<GetSymbolListResponse>>
    {
    }

    public class GetSymbolListQueryHandler : IQueryHandler<GetSymbolListQuery, List<GetSymbolListResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public GetSymbolListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<GetSymbolListResponse>> Handle(GetSymbolListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Symbols.ProjectTo<GetSymbolListResponse>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}
