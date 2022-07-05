using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal interface IPersistChangesMethod
    {
        string Name { get; }
        Task Do(Switch @switch);
    }
}
