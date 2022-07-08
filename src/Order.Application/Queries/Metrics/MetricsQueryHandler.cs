using Mapster;
using MediatR;
using Order.Domain.ReadModels;
using Order.Domain.Repositories;

namespace Order.Application.Queries.Metrics;

public class MetricsQueryHandler : IRequestHandler<MetricsQuery,MetricsQueryResult>
{
    private readonly IMongoRepository _mongoRepository;

    public MetricsQueryHandler(IMongoRepository mongoRepository)
    {
        _mongoRepository = mongoRepository;
    }
    
    public async Task<MetricsQueryResult> Handle(MetricsQuery request, CancellationToken cancellationToken)
    {
        var metrics = await _mongoRepository.GetAsync<OrderMetricsReadModel>(x => true);

        return metrics.Adapt<MetricsQueryResult>();
    }
}