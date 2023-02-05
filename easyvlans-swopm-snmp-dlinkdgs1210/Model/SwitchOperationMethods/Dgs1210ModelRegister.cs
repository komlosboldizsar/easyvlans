namespace easyvlans.Model.SwitchOperationMethods
{
    internal static class Dgs1210ModelRegister
    {

        private static Dictionary<string, Dgs1210Model> registeredModels = new();

        private static Dgs1210Model[] knownModels = new Dgs1210Model[]
        {
            new Dgs1210Model24ax(),
            new Dgs1210Model48ax(),
            new Dgs1210Model52bx(),
        };

        static Dgs1210ModelRegister()
        {
            foreach (Dgs1210Model model in knownModels)
                registeredModels.Add(model.Code, model);
        }

        public static Dgs1210Model GetByCode(string code)
            => registeredModels.TryGetValue(code, out Dgs1210Model model) ? model : null;

    }
}
