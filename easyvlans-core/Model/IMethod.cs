namespace easyvlans.Model
{
    public interface IMethod
    {
        string Code { get; }
        string DetailedCode { get; }
        public interface IFactory<TMethodInterface>
        {
            string Code { get; }
        }
    }
}
