using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Devity.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// DistinctBy for usage in EF Core with pure IQueryable.
        /// </summary>
        public static IQueryable<TSource> DistinctByDb<TSource, TKey>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector
        ) => source.GroupBy(keySelector).Select(x => x.First());
    }
}
