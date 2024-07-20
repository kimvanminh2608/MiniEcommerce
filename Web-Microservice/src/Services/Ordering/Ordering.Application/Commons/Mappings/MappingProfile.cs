using AutoMapper;
using AutoMapper.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Commons.Mappings
{
    public class MappingProfile : Profile
    {
        // lay tat ca nhung object dang thuc thi
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var mapFromType = typeof(IMapFrom<>);

            const string mappingMethodName = nameof(IMapFrom<object>.Mapping);

            bool hasInterface(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == mapFromType;

            var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(hasInterface)).ToList();

            var argumentTypes = new Type[] {typeof(Profile)};

            foreach (var type in types) 
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod(mappingMethodName);

                if (methodInfo != null)
                {
                    methodInfo.Invoke(instance, new object[] { this});
                }
                else 
                {
                    var interfaces = type.GetInterfaces().Where(hasInterface).ToList();

                    if (!interfaces.Any()) continue;

                    foreach (var iface in interfaces)
                    {
                        var interfaceMethodInfo = iface.GetMethod(mappingMethodName, argumentTypes);

                        interfaceMethodInfo?.Invoke(instance, new object[] { this });
                    }
                }
            }
        }
    }
}
