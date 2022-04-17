using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Utils.Enums;

namespace Extensions
{
    public static class IQueryableExtensions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName) =>
            (IOrderedQueryable<TSource>)query.OrderBy(propertyName, SortingOptions.Ascending);

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> query, string propertyName) =>
            (IOrderedQueryable<TSource>)query.OrderBy(propertyName, SortingOptions.Descending);

        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName, SortingOptions? sorting)
        {
            if (sorting == default || propertyName == default)
                return query;

            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var arg = Expression.Parameter(entityType, "x");
            var property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderByDescending() method.
            var enumarableType = typeof(Queryable);
            var sortingOrder = sorting == SortingOptions.Descending ? "OrderByDescending" : "OrderBy";
            var method = enumarableType.GetMethods()
                .Where(m => m.Name == sortingOrder && m.IsGenericMethodDefinition)
                .Single(m =>
                {
                    var parameters = m.GetParameters().ToList();
                    //Put more restriction here to ensure selecting the right overload                
                    return parameters.Count == 2; //overload that has 2 parameters
                });
            //The linq's OrderByDescending<TSource, TKey> has two generic types, which provided here
            var genericMethod = method
                .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderByDescending(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            return (IOrderedQueryable<TSource>)genericMethod
                .Invoke(genericMethod, new object[] { query, selector });
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> querable, bool condition, Expression<Func<T, bool>> f) =>
            condition ? querable.Where(f).AsQueryable() : querable;

        public static IQueryable<T> WhereIfElse<T>(this IQueryable<T> querable, bool condition, Expression<Func<T, bool>> @if, Expression<Func<T, bool>> @else) =>
            condition ? querable.Where(@if).AsQueryable() : querable.Where(@else).AsQueryable();

        public static IQueryable<T> AsNoTrackingIf<T>(this IQueryable<T> querable, bool condition) where T : class =>
            condition ? querable.AsNoTracking() : querable;

        public static IQueryable<T> If<T, TP>(
                this IIncludableQueryable<T, TP> source,
                bool condition, Func<IIncludableQueryable<T, TP>,
                IQueryable<T>> transform
            )
            where T : class
        {
            return condition ? transform(source) : source;
        }

        public static IQueryable<T> If<T, TP>(
                this IIncludableQueryable<T, IEnumerable<TP>> source,
                bool condition,
                Func<IIncludableQueryable<T, IEnumerable<TP>>, IQueryable<T>> transform
            )
            where T : class
        {
            return condition ? transform(source) : source;
        }

        public static Task<List<TSource>> ToListAsyncSafe<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (source is not IAsyncEnumerable<TSource>)
                return Task.FromResult(source.ToList());
            return source.ToListAsync();
        }
    }
}
