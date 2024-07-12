using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappings
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Quet qua source va destination, tat ca property field kiem tra giua source va des khong co field do thi bo qua
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestiantion"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDestiantion> IgnoreAllNonExisting<TSource, TDestiantion>(
            this IMappingExpression<TSource, TDestiantion> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestiantion).GetProperties(flags);
            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }
    }
}
