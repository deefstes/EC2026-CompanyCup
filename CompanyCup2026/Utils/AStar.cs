namespace CompanyCup2026.Utils
{
    public static class AStar
    {
        public static IReadOnlyList<TNode>? FindPath<TNode>(
            IGraph<TNode> graph,
            TNode start,
            TNode goal)
            where TNode : notnull
        {
            var openNodes = new PriorityQueue<TNode, double>();

            var closedNodes = new HashSet<TNode>();

            var previousNode = new Dictionary<TNode, TNode>();

            var costFromStart = new Dictionary<TNode, double>
            {
                [start] = 0
            };

            openNodes.Enqueue(
                start,
                graph.GetCost(start, goal));

            while (openNodes.Count > 0)
            {
                var currentNode = openNodes.Dequeue();

                // Ignore stale queue entries
                if (!closedNodes.Add(currentNode))
                {
                    continue;
                }

                if (EqualityComparer<TNode>.Default.Equals(currentNode, goal))
                {
                    return ReconstructPath(
                        previousNode,
                        currentNode);
                }

                foreach (var neighbour in graph.GetNeighbours(currentNode))
                {
                    if (closedNodes.Contains(neighbour))
                    {
                        continue;
                    }

                    var costToReachNeighbour =
                        costFromStart[currentNode] +
                        graph.GetCost(currentNode, neighbour);

                    var knownCostToReachNeighbour =
                        costFromStart.GetValueOrDefault(
                            neighbour,
                            double.PositiveInfinity);

                    if (costToReachNeighbour >= knownCostToReachNeighbour)
                    {
                        continue;
                    }

                    previousNode[neighbour] = currentNode;

                    costFromStart[neighbour] =
                        costToReachNeighbour;

                    var estimatedTotalPathCost =
                        costToReachNeighbour +
                        graph.GetCost(neighbour, goal);

                    openNodes.Enqueue(
                        neighbour,
                        estimatedTotalPathCost);
                }
            }

            return null;
        }

        private static IReadOnlyList<TNode> ReconstructPath<TNode>(
            Dictionary<TNode, TNode> previousNode,
            TNode goal)
            where TNode : notnull
        {
            var path = new List<TNode>();

            var currentNode = goal;

            path.Add(currentNode);

            while (previousNode.TryGetValue(
                       currentNode,
                       out var parentNode))
            {
                currentNode = parentNode;
                path.Add(currentNode);
            }

            path.Reverse();

            return path;
        }
    }
}
