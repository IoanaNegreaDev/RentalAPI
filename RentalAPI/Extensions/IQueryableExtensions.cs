using RentalAPI.DbAccessors.SortingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace RentalAPI.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source,
                string orderBy, Dictionary<string,
                PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (mappingDictionary == null)
                throw new ArgumentNullException(nameof(mappingDictionary));

            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            var orderByAfterSplit = orderBy.Split(',');
            string orderByString = string.Empty;

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = trimmedOrderByClause;
                if (indexOfFirstSpace != -1)
                     propertyName = indexOfFirstSpace == 1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                    throw new ArgumentException($"Key mapping for {propertyName} is missing.");

                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null)
                    throw new ArgumentNullException("propertyMappingValue");

                
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                {
                    if (propertyMappingValue.Revert)
                        orderDescending = !orderDescending;

                    orderByString = orderByString +
                                    (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ") +
                                    destinationProperty +
                                    (orderDescending ? " descending" : " ascending");
                }              
            }
            return source.OrderBy(orderByString);
        }
    }
}

