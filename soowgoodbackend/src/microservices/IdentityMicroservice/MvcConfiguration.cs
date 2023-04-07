using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IdentityMicroservice
{
    public class MvcConfiguration: IDesignTimeMvcBuilderConfiguration
    {
        private class DirectReferenceAssemblyResolver : ICompilationAssemblyResolver
        {
            public bool TryResolveAssemblyPaths(CompilationLibrary library, List<string> assemblies)
            {
                if (!string.Equals(library.Type, "reference", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                var paths = new List<string>();

                foreach (var assembly in library.Assemblies)
                {
                    var path = Path.Combine(ApplicationEnvironment.ApplicationBasePath, assembly);

                    if (!File.Exists(path))
                    {
                        return false;
                    }

                    paths.Add(path);
                }

                assemblies.AddRange(paths);

                return true;
            }
        }

        public void ConfigureMvc(IMvcBuilder builder)
        {
            typeof(CompilationLibrary)
                .GetTypeInfo()
                .GetDeclaredField("<DefaultResolver>k__BackingField")
                .SetValue(null, new CompositeCompilationAssemblyResolver(new ICompilationAssemblyResolver[]
                {
                    new DirectReferenceAssemblyResolver(),
                    new AppBaseCompilationAssemblyResolver(),
                    new ReferenceAssemblyPathResolver(),
                    new PackageCompilationAssemblyResolver(),
                }));
        }
    }
}
