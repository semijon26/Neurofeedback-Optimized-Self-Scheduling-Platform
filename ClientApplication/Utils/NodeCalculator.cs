namespace ClientApplication.Utils;
/*
public static class NodeCalculator
{
    public static void CalculateNodePositions(List<Node> nodes)
    {
        double nodeWidth = 50; // Setzen Sie die Breite jedes Knotens auf 50 Pixel
        double nodeHeight = 50; // Setzen Sie die Höhe jedes Knotens auf 50 Pixel
        double xSpacing = 50; // Setzen Sie den horizontalen Abstand zwischen den Knoten auf 50 Pixel
        double ySpacing = 100; // Setzen Sie den vertikalen Abstand zwischen den Knoten auf 100 Pixel

        int maxDepth = GetMaxDepth(nodes); // Bestimmen Sie die maximale Tiefe des Graphen

        for (int i = 0; i < nodes.Count; i++)
        {
            Node node = nodes[i];
            int depth = GetDepth(node); // Bestimmen Sie die Tiefe des Knotens im Graphen

            double x = (depth * xSpacing) + (nodeWidth / 2); // Berechnen Sie die horizontale Position des Knotens
            double y = (maxDepth - depth) * ySpacing; // Berechnen Sie die vertikale Position des Knotens

            node.X = x;
            node.Y = y;
        }
    }

    private int GetMaxDepth(List<Node> nodes)
    {
        int maxDepth = 0;

        foreach (Node node in nodes)
        {
            int depth = GetDepth(node);
            if (depth > maxDepth)
            {
                maxDepth = depth;
            }
        }

        return maxDepth;
    }

    private int GetDepth(Node node)
    {
        if (node.AdjacentNodes.Count == 0)
        {
            return 0;
        }

        int maxDepth = 0;

        foreach (Node adjacentNode in node.AdjacentNodes)
        {
            int depth = GetDepth(adjacentNode);
            if (depth > maxDepth)
            {
                maxDepth = depth;
            }
        }

        return maxDepth + 1;
    }
}*/