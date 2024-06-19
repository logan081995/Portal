using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QuickTie.Cloud.Services.Helpers
{
    public static class FilterByCollectionPropertyHelper
    {
        class FilterInfo
        {
            public Type CollectionItemType { get; set; }
            public Type CollectionType { get; internal set; }
            public string CollectionPropertyName { get; set; }
            public string CollectionItemPropertyName { get; set; }
        }

        static List<FilterInfo> filterInfos = new List<FilterInfo>();

        static MethodInfo GetFilteringPredicate(string filterOperation)
        {
            // selects a method overload that accepts a string as a parameter
            Func<string, MethodInfo> infoByName = (name) => typeof(string).GetMethod(name, new Type[] { typeof(string) });

            switch (filterOperation)
            {
                case "contains": return infoByName("Contains");
                case "=": return infoByName("Equals");
                case "startswith": return infoByName("StartsWith");
                case "endswith": return infoByName("EndsWith");
                default: throw new ArgumentException("This operation is not supported: " + filterOperation);
            }
        }

        static FilterByCollectionPropertyHelper()
        {
            DevExtreme.AspNet.Data.Helpers.CustomFilterCompilers.RegisterBinaryExpressionCompiler(compilerFunc =>
            {
                var info = filterInfos.FirstOrDefault(r => r.CollectionType == compilerFunc.DataItemExpression.Type && r.CollectionPropertyName == compilerFunc.AccessorText);
                if (info == null) return null;

                var collectionItemParameter = Expression.Parameter(info.CollectionItemType);
                var collectionItemProperty = Expression.Property(collectionItemParameter, info.CollectionItemPropertyName);

                // represents (collectionItem) => collectionItem.PropertyName.FilteringMethod("searchText")
                var pred = GetFilteringPredicate(compilerFunc.Operation);

                LambdaExpression innerLambda = null ;

                switch (pred.Name.Contains("Contains"))
                {
                    case true:
                        Expression indexOf = Expression.Call(collectionItemProperty, "IndexOf", null,
                       Expression.Constant(compilerFunc.Value),
                       Expression.Constant(StringComparison.OrdinalIgnoreCase));

                        Expression condition = Expression.NotEqual(indexOf, Expression.Constant(-1));

                       var  main = Expression.NotEqual(Expression.Call(collectionItemProperty, "IndexOf", null,
                                    Expression.Constant(compilerFunc.Value, typeof(string)),
                                    Expression.Constant(StringComparison.InvariantCultureIgnoreCase, typeof(StringComparison))),
                                    Expression.Constant(-1, typeof(int)));
                        innerLambda = Expression.Lambda(main, collectionItemParameter);
                        break;
                    default:
                        innerLambda = Expression.Lambda(Expression.Call(collectionItemProperty, GetFilteringPredicate(compilerFunc.Operation), Expression.Constant(compilerFunc.Value, typeof(string))), collectionItemParameter);
                        break;
                }

               
                // represents call to Enumerable.Any(collection, innerLambda)
                return Expression.Call(
                    typeof(Enumerable),
                    "Any",
                    new[] { info.CollectionItemType },
                    Expression.Property(compilerFunc.DataItemExpression, info.CollectionPropertyName),
                    innerLambda
                );
            });
        }

        public static void RegisterFor<TDataItem, TCollectionItem>(Expression<Func<TDataItem, IEnumerable<TCollectionItem>>> collectionPropertyAccessor, Expression<Func<TCollectionItem, string>> collectionItemPropertyAccessor)
        {
            var collectionPropertyName = ((MemberExpression)collectionPropertyAccessor.Body).Member.Name;
            var collectionItemPropertyName = ((MemberExpression)collectionItemPropertyAccessor.Body).Member.Name;
            filterInfos.Add(new FilterInfo()
            {
                CollectionType = typeof(TDataItem),
                CollectionItemType = typeof(TCollectionItem),
                CollectionPropertyName = collectionPropertyName,
                CollectionItemPropertyName = collectionItemPropertyName
            });
        }
    }

}
