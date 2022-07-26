using easyvlans.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace easyvlans.Modules
{
    internal class ModuleLoader
    {

        private static readonly List<IModule> initializedModules = new();
        private const string DLL_EXTENSION = ".dll";
        private static readonly Type MODULE_DESCRIPTOR_TYPE = typeof(IModule);
        private static readonly Type[] EMPTY_TYPE_ARRAY = Array.Empty<Type>();
        private static readonly object[] EMPTY_OBJECT_ARRAY = Array.Empty<object>();

        public static void LoadAndInitModules()
        {
            DirectoryInfo moduleDirectory = new(Directory.GetCurrentDirectory());
            foreach (FileInfo fileInfo in moduleDirectory.GetFiles())
            {
                if (fileInfo.Extension != DLL_EXTENSION)
                    continue;
                try
                {
                    Assembly assembly = Assembly.LoadFrom(fileInfo.FullName);
                    IEnumerable<TypeInfo> moduleTypeInfos = assembly.DefinedTypes.Where(ti => ti.ImplementedInterfaces.Contains(MODULE_DESCRIPTOR_TYPE));
                    foreach (TypeInfo moduleTypeInfo in moduleTypeInfos)
                    {
                        ConstructorInfo constuctorInfo = moduleTypeInfo.GetConstructor(EMPTY_TYPE_ARRAY);
                        if (constuctorInfo == null)
                            continue;
                        if (constuctorInfo.Invoke(EMPTY_OBJECT_ARRAY) is not IModule moduleInstance)
                            continue;
                        moduleInstance.Init();
                        initializedModules.Add(moduleInstance);
                        LogDispatcher.V($"Found and initialized module [{fileInfo.FullName}].");
                    }
                }
                catch
                {
                    LogDispatcher.E($"Couldn't open module file [{fileInfo.FullName}].");
                }
            }
        }

        public static int InitializedModuleCount => initializedModules.Count;

    }
}
