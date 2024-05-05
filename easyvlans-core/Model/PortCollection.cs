namespace easyvlans.Model
{
    public class PortCollection : List<IPortOrPortCollection>, IPortOrPortCollection
    {

        public string Title { get; init; }
        public bool IsDefault { get; init; }
        public PortCollection Parent { get; init; }
        public int Level { get; init; }

        public (List<Port>, PortCollectionStructure) GetAllPortsAndStructure()
        {
            List<int> maxSubCollectionCount = new();
            List<Port> ports = _getAllPortsAndStructure(out int depth, out int maxVisibleSize, maxSubCollectionCount);
            return (ports, new PortCollectionStructure(depth, maxVisibleSize, maxSubCollectionCount));
        }

        // level = 0 is called from root
        // level = 1 is when called for pages
        // level = 2 is when called for sub-pages
        // etc.
        private List<Port> _getAllPortsAndStructure(out int depth, out int maxVisibleSize, List<int> maxSubCollectionCount, int level = 0)
        {
            List<Port> ports = new();
            // calculating depth:
            int maxChildDepth = -1;
            // calculatin maxVisibleSize:
            int childMaxMaxVisibleSize = 0;
            maxVisibleSize = 0;
            // calculating maxSubCollectionCount:
            int subCollectionCount = 0;
            //
            foreach (IPortOrPortCollection portOrPortCollection in this)
            {
                if (portOrPortCollection is Port port)
                {
                    ports.Add(port);
                    maxVisibleSize++;
                }
                else if (portOrPortCollection is PortCollection portCollection)
                {
                    if (maxSubCollectionCount.Count <= level)
                        maxSubCollectionCount.Add(0);
                    ports.AddRange(portCollection._getAllPortsAndStructure(out int childDepth, out int childMaxVisibleSize, maxSubCollectionCount, level + 1));
                    if (childDepth > maxChildDepth)
                        maxChildDepth = childDepth;
                    if (childMaxVisibleSize > childMaxMaxVisibleSize)
                        childMaxMaxVisibleSize = childMaxVisibleSize;
                    subCollectionCount++;
                }
            }
            if ((maxSubCollectionCount.Count > level) && (maxSubCollectionCount[level] < subCollectionCount))
                maxSubCollectionCount[level] = subCollectionCount;
            depth = maxChildDepth + 1;
            maxVisibleSize += childMaxMaxVisibleSize;
            return ports;
        }

    }
}
