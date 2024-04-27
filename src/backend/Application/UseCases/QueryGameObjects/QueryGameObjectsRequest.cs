using Domain.GameObjects;
using Mediator;

namespace Application.UseCases.QueryGameObjects;

public record QueryGameObjectsRequest(string QueryKey) : IRequest<IEnumerable<IGameObject>>;

public class QueryGameObjectsRequestHandler : IRequestHandler<QueryGameObjectsRequest, IEnumerable<IGameObject>>
{
    public ValueTask<IEnumerable<IGameObject>> Handle(QueryGameObjectsRequest request, CancellationToken cancellationToken)
    {
        var queryKey = request.QueryKey;
        var queriedGameObjects = GameObjects.All.Where(go => go.QueryKey == queryKey).ToArray();
        return ValueTask.FromResult((IEnumerable<IGameObject>)queriedGameObjects);
    }
}
