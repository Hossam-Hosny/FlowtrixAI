using System;
using System.Linq;
using System.Reflection;

namespace Test
{
    public class Program
    {
        public static void Main()
        {
            try {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var swashAssembly = assemblies.FirstOrDefault(a => a.GetName().Name == "Swashbuckle.AspNetCore.SwaggerGen");
                if (swashAssembly == null) {
                    Console.WriteLine("Swashbuckle.AspNetCore.SwaggerGen assembly not found.");
                    return;
                }
                var type = swashAssembly.GetType("Microsoft.Extensions.DependencyInjection.SwaggerGenOptionsExtensions") 
                           ?? swashAssembly.GetType("Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions");
                
                if (type == null) {
                     Console.WriteLine("SwaggerGenOptions or Extensions not found.");
                     return;
                }

                Console.WriteLine($"Methods for {type.FullName}:");
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                                           .Where(m => m.Name == "AddSecurityRequirement"))
                {
                    var pars = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                    Console.WriteLine($" - {method.Name}({pars})");
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
