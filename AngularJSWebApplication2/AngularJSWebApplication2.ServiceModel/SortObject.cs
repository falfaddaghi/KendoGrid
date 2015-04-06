using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace AngularJSWebApplication2.ServiceModel
{
    public class SortObject
    {
        public string Logic { get; set; }

        public SortObject()
        {
            Filters = new IEnumerable<FiltersObj>[2];
        }
        public IEnumerable<FiltersObj>[] Filters { get; set; }
        // public IEnumerable<AggregateObject> Aggregates { get; set; }
    }

    public class FiltersObj
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// Get the Linq Aggregate
        /// </summary>
        /// <param name="fieldConverter">Convert which can be used to convert ViewModel to Model if needed.</param>
        /// <returns>string</returns>
      /*  public string GetLinqAggregate(Func<string, string> fieldConverter = null)
        {
            string convertedField = fieldConverter != null ? fieldConverter.Invoke(Field) : Field;
           /* switch (Aggregate)
            {
                case "count":
                    return string.Format("Count() as count__{0}", Field);

                case "sum":
                    return string.Format("Sum(TEntity__.{0}) as sum__{1}", convertedField, Field);

                case "max":
                    return string.Format("Max(TEntity__.{0}) as max__{1}", convertedField, Field);

                case "min":
                    return string.Format("Min(TEntity__.{0}) as min__{1}", convertedField, Field);

                case "average":
                    return string.Format("Average(TEntity__.{0}) as average__{1}", convertedField, Field);

                default:
                    return string.Empty;
            }#1#
        }
*/

    }
}