using BToolbox.XmlDeserializer.Attributes;

namespace easyvlans.Model.SwitchOperationMethods
{
    internal record TPLinkDot1qThreePartPortId(string Prefix, int Counter)
    {

        internal class XmlAttributeConverter : IAttributeOrInnerConverter<TPLinkDot1qThreePartPortId>
        {

            public static readonly XmlAttributeConverter Instance = new();

            public TPLinkDot1qThreePartPortId Convert(string stringValue)
            {
                string[] parts = stringValue.Split("/");
                if (parts.Length != 3)
                    throw new ArgumentException("Invalid format of three part ID.");
                Func<int, string, int> checkPart = (int partIndex, string partIndexStr) =>
                {
                    if (!int.TryParse(parts[partIndex], out int partAsInt) || (partAsInt < 0))
                        throw new ArgumentOutOfRangeException($"{partIndexStr} part of three part ID must be a non-negative integer!", (Exception)null);
                    return partAsInt;
                };
                checkPart(0, "First");
                checkPart(1, "Second");
                int part2 = checkPart(2, "Third");
                return new($"{parts[0]}/{parts[1]}", part2);
            }

        }

    }
}
