using System.Threading.Tasks;

namespace easyvlans.Model
{
    internal interface IPersistChangesMethod : IMethod<IPersistChangesMethod>
    {
        Task Do();
    }
}
