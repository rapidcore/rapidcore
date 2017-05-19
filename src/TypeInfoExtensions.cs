using System;
using System.Reflection;
using System.Text;

namespace RapidCore.Reflection
{
    public static class TypeInfoExtensions
    {
        public static void ListMethodsRecursively(this TypeInfo ti)
        {
            Console.WriteLine($"=> {ti.Name}");
            foreach (var method in ti.DeclaredMethods)
            {
                var sb = new StringBuilder();
                
                foreach (var param in method.GetParameters())
                {
                    sb.Append($"{param.ParameterType.Name} {param.Name}, ");
                }

                Console.WriteLine($"\t{method.Name}({sb.ToString()})");
            }

            if (ti.BaseType != null)
            {
                ti.BaseType.GetTypeInfo().ListMethodsRecursively();
            }
        }
    }
}