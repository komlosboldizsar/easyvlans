namespace easyvlans.Model.Remote
{
    public interface IRemoteMethod
    {
        string Code { get; }
        void MeetConfig(Config config);
        void Start();
    }
}
