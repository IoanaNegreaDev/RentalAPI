using RentalAPI.DTOs;
using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentalAPI.DbAccessors.SortingServices
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _rentablesPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"}) },
                {"PricePerDay", new PropertyMappingValue(new List<string>() {"PricePerDay"}) },
           
            };

        private Dictionary<string, PropertyMappingValue> _vehiclesRentablesPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
                {"Producer", new PropertyMappingValue(new List<string>() {"Producer"}) },
                {"Model", new PropertyMappingValue(new List<string>() {"Model"}) },
                {"RegistrationNumber", new PropertyMappingValue(new List<string>() {"RegistrationNumber"}) },
                {"TankCapacity", new PropertyMappingValue(new List<string>() {"TankCapacity"}) }
        };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<RentableDTO, Rentable>(_rentablesPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<VehicleDTO, Vehicle>(_vehiclesRentablesPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for " + typeof(TSource) + ", " + typeof(TDestination));
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();
            if (string.IsNullOrWhiteSpace(fields))
                return true;

            var fieldsAfterSplit = fields.Split(',');

            foreach(var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();    

                var indexOfFirstSpace = trimmedField.IndexOf(' ');
                var propertyName = (indexOfFirstSpace == -1) ? trimmedField : trimmedField.Remove(indexOfFirstSpace);
                if (!propertyMapping.ContainsKey(propertyName))
                    return false;         
            }
            return true;
        }
    }
}
