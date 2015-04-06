using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using ServiceStack.Text;

namespace AngularJSWebApplication2.ServiceModel.Helpers
{
    public static class SortHelper
    {
        private static readonly Regex GroupRegex = new Regex(@"^filter\[(logic)\]$", RegexOptions.IgnoreCase);
        private static readonly Regex GroupAggregateRegex = new Regex(@"^filter\[filters\]\[(\d*)\]\[(field|operator|value)\]$", RegexOptions.IgnoreCase);

        public static IEnumerable<SortObject> Parse(NameValueCollection queryString)
        {
            // If there is a group query parameter, try to parse the value as json
            if (queryString.AllKeys.Contains("filter"))
            {
                string groupAsJson = queryString["filter"];
                if (!string.IsNullOrEmpty(groupAsJson))
                {
                    return GetGroupObjects(groupAsJson);
                }
            }
            else
            {
                // Just get the groups the old way
                return GetGroupObjects(queryString, queryString.AllKeys.Where(k => k.StartsWith("filter")));
            }

            return null;
        }

        public static IEnumerable<SortObject> Map(IEnumerable<SortObject> groups)
        {
            return groups.Any() ? groups : null;
        }

        private static IEnumerable<SortObject> GetGroupObjects(string groupAsJson)
        {
            var result = JsonSerializer.DeserializeFromString<List<SortObject>>(groupAsJson);

            return Map(result);
        }

        private static IEnumerable<SortObject> GetGroupObjects(NameValueCollection queryString, IEnumerable<string> requestKeys)
        {
            
            var result = new Dictionary<int, SortObject>();

            var enumerable = requestKeys as IList<string> ?? requestKeys.ToList();
            foreach (var x in enumerable
                .Select(k => new { Key = k, Match = GroupRegex.Match(k) })
                .Where(x => x.Match.Success)
            )
            {
                Console.WriteLine((x.Match.Groups));
                String sortLogic = x.Match.Groups[1].Value;
                    result.Add(0, new SortObject());
               
                string value = queryString[x.Key];
                if (x.Key.Contains("logic"))
                {
                    result[0].Logic = value;
                }
              
            }

            //            for (var groupObject = 0; groupObject < result.Count(); groupObject++)

            foreach (var groupObject in new List<int>(){0, 1})
            {
                var aggregates = new Dictionary<int, FiltersObj>();
                var aggregateKey = string.Format("filter[filters][{0}]", groupObject);

                foreach (var x in enumerable
                    .Where(k => k.StartsWith(aggregateKey))
                    .Select(k => new { Key = k, Match = GroupAggregateRegex.Match(k) })
                    .Where(x => x.Match.Success)
                    )
                {
                    int aggregateId = int.Parse(x.Match.Groups[1].Value);
                    if (!aggregates.ContainsKey(aggregateId))
                    {
                        aggregates.Add(aggregateId, new FiltersObj());
                    }

                    string value = queryString[x.Key];
                    if (x.Key.Contains("field"))
                    {
                        aggregates[aggregateId].Field = value;
                    }
                    else if (x.Key.Contains("operator"))
                    {
                        aggregates[aggregateId].Operator = value;
                    }
                    else if (x.Key.Contains("value"))
                    {
                        aggregates[aggregateId].Value = value;
                    }
                }
               if(aggregates.Values.Any())
                result[0].Filters[groupObject] = aggregates.Values.ToList();
            }

            return result.Any() ? result.Values.ToList() : null;
        }
    }
}