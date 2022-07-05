using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal interface IPersistChangesMethod : IMethod
    {
        Task Do(Switch @switch);
    }
}
