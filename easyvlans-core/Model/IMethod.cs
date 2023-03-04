namespace easyvlans.Model
{
    public interface IMethod
    {
        string DetailedCode { get; }
        public interface IFactory<TMethodInterface>
        {
            string Code { get; }
        }
    }
}
