namespace CompanyCup2026.Utils
{
    public interface IGraph<TNode>
    {
        IEnumerable<TNode> GetNeighbours(TNode node);

        double GetCost(TNode from, TNode to);

        double Heuristic(TNode from, TNode goal);
    }
}
