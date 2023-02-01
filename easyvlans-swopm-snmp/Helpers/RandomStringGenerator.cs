namespace easyvlans.Helpers
{
    internal class RandomStringGenerator
    {
        // @source https://stackoverflow.com/a/1344242
        private static readonly Random random = new();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string RandomString(int length)
            => new(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
