using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace App.Server.src.Re_usables.Modules.DynamicsBC
{
    public class SearchFilterController
    {
        //search filter for odata v2
        async public Task<string> FnSearchFilterV2(HttpContext MyHttpContext, ClassProperties ClassProps, string customFilter)
        {
            try
            {
                //
                string fullUrl = MyHttpContext.Request.GetDisplayUrl();
                //check if there is a search query in url
                string query = "";
                if (customFilter != "")
                {
                    query = customFilter;
                }
                var anyQueryRecs = new List<JsonObject>();
                var addedPkeys = new List<string>();
                int maxTake = FnGetMaxTake(MyHttpContext);
                string result = "";
                if (fullUrl.IndexOf("filter=") > 0)
                {
                    int filterStart = fullUrl.IndexOf("filter=") + "filter=".Length;
                    int filterEnd = fullUrl.LastIndexOf("__filter");
                    string filterString = fullUrl.Substring(filterStart, filterEnd - filterStart);
                    //var searchQuery = fullUrl.Split("?filter=")[1];
                    var searchQuery = filterString;

                    List<string> fieldNames = new List<string>();
                    //when you only have one search group
                    if (searchQuery.IndexOf("&&") < 0)
                    {
                        var searchCol = searchQuery.Split("___")[0];
                        var searchValue = searchQuery.Split("___")[1];
                        if (searchCol == "any")
                        {
                            fieldNames = GeneralController.GetModelFieldNames(ClassProps.WSInstance);
                            var filter = await GV.WSclient.ODATAFilter(MyHttpContext, ClassProps.WSName, query, true);
                            var data = filter != null ? JsonSerializer.Deserialize<List<JsonObject>>(filter) : null;
                            bool isAdd = false;
                            if (data != null)
                            {
                                foreach (var rec in data)
                                {
                                    isAdd = false;
                                    if (!addedPkeys.Contains(rec[ClassProps.pKey].ToString()))
                                    {
                                        foreach (var fName in fieldNames)
                                        {
                                            var refName = rec[fName]?.ToString();
                                            if (rec.ContainsKey(fName) && refName != null && refName.ToLower().Contains(searchValue.ToString().ToLower()))
                                            {
                                                isAdd = true;
                                            }
                                        }

                                    }
                                    if (isAdd)
                                    {
                                        anyQueryRecs.Add(rec);
                                    }
                                }
                            }
                            result = JsonSerializer.Serialize(anyQueryRecs);
                        }
                        else
                        {
                            query = $"$filter=substringof('{searchValue}',{searchCol})";
                            var filter = await GV.WSclient.ODATAFilter(MyHttpContext, ClassProps.WSName, query, true);
                            result = filter != null ? filter : "";
                            //query = $"{searchCol}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
                        }
                    }
                    //when you have multiple filters
                    else
                    {
                        var searchGroups = searchQuery.Split("&&");
                        string searchCol;
                        string searchValue;

                        foreach (var searchGroup in searchGroups)
                        {
                            searchCol = searchGroup.Split("___")[0];
                            searchValue = searchGroup.Split("___")[1];
                            if (searchCol != "any")
                            {
                                if (query == "")
                                {
                                    query = $"$filter=substringof('{searchValue}',{searchCol})";
                                    //query = $"{searchCol}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
                                }
                                else
                                {
                                    query = query + $" and substringof('{searchValue}',{searchCol})";
                                    //query = query + $" && {searchCol}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
                                }
                            }
                            else
                            {
                                /*fieldNames = GeneralController.GetModelFieldNames(ClassProps.WSInstance);
                                var filter = await GV.WSclient.ODATAFilter(MyHttpContext,ClassProps.WSName, "", true);
                                var data = JsonSerializer.Deserialize<List<JsonObject>>(filter);
                                foreach (var rec in data)
                                {
                                    foreach (var fName in fieldNames)
                                    {
                                        var exists = anyQueryRecs.Where(x => x[ClassProps.pKey] == rec[ClassProps.pKey]).Count();
                                        if (exists == 0)
                                        {
                                            if (rec[fName].ToString().ToLower().Contains(searchValue.ToString().ToLower()))
                                            {
                                                anyQueryRecs.Add(rec.ToString());
                                            }
                                        }

                                    }
                                }
                                */
                                /*if (fieldNames != null)
                                {
                                    foreach (var fieldName in fieldNames)
                                    {
                                        if (anyQuery == "")
                                        {
                                            anyQuery = $"$filter=substringof('{searchValue}',{fieldName})";
                                            //anyQuery = $"{fieldName}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
                                        }
                                        else
                                        {
                                            anyQuery = anyQuery + $" and substringof('{searchValue}',{fieldName})";
                                            //anyQuery = anyQuery + $" || {fieldName}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
                                        }
                                    }
                                }*/
                            }
                        }
                        if (anyQueryRecs.Count != 0)
                        {
                            if (query != "")
                            {
                                //combine any results with filter results
                                //query = query + " and (" + anyQuery + ")";
                            }
                            else
                            {
                                //if it is only the any query that exists then pass it
                                //query = anyQuery;
                                result = JsonSerializer.Serialize(anyQueryRecs);
                            }
                        }

                    }
                    //
                    if (fullUrl.IndexOf("order_by=") < 0)
                    {
                        //result = baseResults.AsQueryable().Where(query).Take(maxTake).ToList();
                        if (result == "")
                        {
                            var data = await GV.WSclient.ODATAFilter(MyHttpContext, ClassProps.WSName, query, true);
                            result = data != null ? data : "";
                        }
                    }
                }
                //
                if (fullUrl.IndexOf("order_by=") > 0)
                {
                    int orderByStart = fullUrl.IndexOf("order_by=") + "order_by=".Length;
                    int orderByEnd = fullUrl.LastIndexOf("__order_by");
                    string orderByString = fullUrl.Substring(orderByStart, orderByEnd - orderByStart);
                    var orderCol = orderByString.Split("___")[0];
                    var orderValue = orderByString.Split("___")[1];
                    if (query != "")
                    {
                        query = query + $"&$orderby={orderCol} {orderValue}";
                        var data = await GV.WSclient.ODATAFilter(MyHttpContext, ClassProps.WSName, query, true);
                        result = data != null ? data : "";
                    }
                    else
                    {
                        query = query + $"$orderby={orderCol} {orderValue}";
                        var data = await GV.WSclient.ODATAFilter(MyHttpContext, ClassProps.WSName, "", true);
                        result = data != null ? data : "";
                    }
                }
                return result;
            }
            catch (Exception)
            {
                //return BadRequest(GeneralController.ProcessException(ex));
                return "";
            }
        }

        //public List<object> FnSearchFilter(HttpContext MyHttpContext, List<object> baseResults, ClassProperties ClassProps)
        //{
        //    //
        //    string fullUrl = MyHttpContext.Request.GetDisplayUrl();
        //    //check if there is a search query in url
        //    string query = "";
        //    string anyQuery = "";
        //    int maxTake = FnGetMaxTake(MyHttpContext);
        //    List<object> result = new List<object>();
        //    if (fullUrl.IndexOf("filter=") > 0)
        //    {
        //        int filterStart = fullUrl.IndexOf("filter=") + "filter=".Length;
        //        int filterEnd = fullUrl.LastIndexOf("__filter");
        //        string filterString = fullUrl.Substring(filterStart, filterEnd - filterStart);
        //        //var searchQuery = fullUrl.Split("?filter=")[1];
        //        var searchQuery = filterString;

        //        List<string> fieldNames = new List<string>();
        //        //when you only have one search group
        //        if (searchQuery.IndexOf("&&") < 0)
        //        {
        //            //var searchParams = searchQuery.Split("___");
        //            var searchCol = searchQuery.Split("___")[0];
        //            var searchValue = searchQuery.Split("___")[1];
        //            if (searchCol == "any")
        //            {
        //                fieldNames = GeneralController.GetModelFieldNames(ClassProps.WSInstance);
        //                //
        //                if (fieldNames != null)
        //                {
        //                    foreach (var fieldName in fieldNames)
        //                    {
        //                        if (query == "")
        //                        {
        //                            query = $"{fieldName}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
        //                        }
        //                        else
        //                        {
        //                            query = query + $" || {fieldName}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                query = $"{searchCol}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
        //            }
        //        }
        //        //when you have multiple filters
        //        else
        //        {
        //            var searchGroups = searchQuery.Split("&&");
        //            string searchCol;
        //            string searchValue;

        //            foreach (var searchGroup in searchGroups)
        //            {
        //                searchCol = searchGroup.Split("___")[0];
        //                searchValue = searchGroup.Split("___")[1];
        //                if (searchCol != "any")
        //                {
        //                    if (query == "")
        //                    {
        //                        query = $"{searchCol}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
        //                    }
        //                    else
        //                    {
        //                        query = query + $" && {searchCol}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
        //                    }
        //                }
        //                else
        //                {
        //                    fieldNames = GeneralController.GetModelFieldNames(ClassProps.WSInstance);
        //                    if (fieldNames != null)
        //                    {
        //                        foreach (var fieldName in fieldNames)
        //                        {
        //                            if (anyQuery == "")
        //                            {
        //                                anyQuery = $"{fieldName}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
        //                            }
        //                            else
        //                            {
        //                                anyQuery = anyQuery + $" || {fieldName}.ToString().ToLower().Contains(\"{searchValue.ToString().ToLower()}\")";
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (anyQuery != "")
        //            {
        //                if (query != "")
        //                {
        //                    query = query + " && (" + anyQuery + ")";
        //                }
        //                else
        //                {
        //                    query = anyQuery;
        //                }
        //            }

        //        }
        //        if (fullUrl.IndexOf("order_by=") < 0)
        //        {
        //            result = baseResults.AsQueryable().Where(query).Take(maxTake).ToList();
        //        }
        //    }
        //    if (fullUrl.IndexOf("order_by=") > 0)
        //    {
        //        int orderByStart = fullUrl.IndexOf("order_by=") + "order_by=".Length;
        //        int orderByEnd = fullUrl.LastIndexOf("__order_by");
        //        string orderByString = fullUrl.Substring(orderByStart, orderByEnd - orderByStart);
        //        var orderCol = orderByString.Split("___")[0];
        //        var orderValue = orderByString.Split("___")[1];
        //        if (query != "")
        //        {
        //            result = baseResults.AsQueryable().Where(query).OrderBy(orderCol + " " + orderValue).Take(maxTake).ToList();
        //        }
        //        else
        //        {
        //            result = baseResults.AsQueryable().OrderBy(orderCol + " " + orderValue).Take(maxTake).ToList();
        //        }
        //    }
        //    return result;
        //}

        public static int FnGetMaxTake(HttpContext currentContext)
        {
            string fullUrl = currentContext.Request.GetDisplayUrl();
            var takeMax = Config.ListMaxRecords;
            if (fullUrl.IndexOf("take_max=") > 0)
            {
                int takeMaxStart = fullUrl.IndexOf("take_max=") + "take_max=".Length;
                int takeMaxEnd = fullUrl.LastIndexOf("__take_max");
                string takeMaxString = fullUrl.Substring(takeMaxStart, takeMaxEnd - takeMaxStart);
                takeMax = int.Parse(takeMaxString);
            }
            return takeMax;
        }
    }
}
