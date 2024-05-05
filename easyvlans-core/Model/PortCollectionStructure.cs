namespace easyvlans.Model
{
    // depth = 0 means that there are no pages
    // depth = 1 means that there are pages
    // depth = 2 means that there are pages and sub-pages
    // etc.
    //
    // length of maxPageCount should equal to depth
    //
    public record PortCollectionStructure(int Depth, int MaxVisibleSize, List<int> MaxSubCollectionCount);
}
