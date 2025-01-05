namespace easyvlans.Model.SwitchOperationMethods
{
    internal static class ModelRegister
    {

        private static Dictionary<string, Model> registeredModels = new();

        private static Model[] knownModels = new Model[]
        {
            new Model24ax(),
            new Model28cx(),
            new Model48ax(),
            new Model52bx(),
        };

        static ModelRegister()
        {
            foreach (Model model in knownModels)
                registeredModels.Add(model.Code, model);
        }

        public static Model GetByCode(string code)
            => registeredModels.TryGetValue(code, out Model model) ? model : null;

    }
}
