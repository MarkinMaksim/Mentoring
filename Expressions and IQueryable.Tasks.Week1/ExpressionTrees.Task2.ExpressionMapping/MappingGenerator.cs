using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Dictionary<string, string> _propertyMap = new Dictionary<string, string>();

        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));
            var destinationClass = Expression.New(typeof(TDestination));
            var parametrs = Expression.MemberInit(destinationClass, BindMembers(sourceParam, Expression.Parameter(destinationClass.Type)));

            var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(parametrs, sourceParam);

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        public IEnumerable<MemberBinding> BindMembers(ParameterExpression source, ParameterExpression destination)
        {
            var parametrBindings = new List<MemberBinding>();
            var sourceParametrs = source.Type.GetProperties();
            var destParametrs = destination.Type.GetProperties();

            foreach(var sourceParametr in sourceParametrs)
            {
                PropertyInfo destParametr;
                var map = _propertyMap.FirstOrDefault(x => x.Key == sourceParametr.Name);

                if (map.Value != null)
                {
                    destParametr = destParametrs.FirstOrDefault(x => x.Name == map.Value);
                }
                else
                {
                    destParametr = destParametrs.FirstOrDefault(x => x.Name == sourceParametr.Name);
                }

                if (destParametr == null)
                {
                    continue;
                }

                var access = Expression.MakeMemberAccess(source, sourceParametr);
                var assign = Expression.Bind(destParametr, access);
                parametrBindings.Add(assign);
            }

            return parametrBindings;
        }

        public void AddPropertyMap(string source, string destination)
        {
            _propertyMap.Add(source, destination);
        }
    }
}
