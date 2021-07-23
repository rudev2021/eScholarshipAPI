using GraphQL;
using GraphQL.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace eScholarshipAPI_Client.Models
{
    public class ItemConsumer
    {
        private readonly IGraphQLClient _client;

        public ItemConsumer(IGraphQLClient client)
        {
            _client = client;
        }


        public static List<Item> GetItemsWithinPeriod(List<Item> list, DateTime dStart, DateTime dEnd) 
        {
            // #1: Find all of the items published between April 2, 2020 and June 30, 2021, and the units they are associated with.
            //DateTime dStart = new DateTime(2020, 4, 2);
            //DateTime dEnd = new DateTime(2021, 7, 1);

            var list1 = (from c in list
                         where
                          (DateTime.Parse(c.published) >= dStart
                          && DateTime.Parse(c.published) < dEnd)
                                 select c).ToList();
            // list1: #1  (count: 76) 
            
            return list1;   
        }

        public static List<ItemsPerUnit> GetItemsPerUnit(List<Item> list)
        {
            // #2: present a count of the number of items per unit, published during that time frame.
            List<Unit> units = new List<Unit>();
            foreach (var lst in list)
            {
                var tmp = (from c in lst.units
                           select c);
                units.AddRange(tmp);
            }

            // distinct unit id's  (count: 19)
            var unitList = units.GroupBy(x => x.id)
                          .Select(g => g.First())
                          .ToList();

            List<ItemsPerUnit> itemUnitList = new List<ItemsPerUnit>();

            foreach (var u in unitList)
            {
                ItemsPerUnit items = new ItemsPerUnit();
                var lst = (from c in list
                           where
                            c.units.Any(m => m.id == u.id)
                           select c).ToList();
                items.unitID = u.id;
                items.items = lst;
                itemUnitList.Add(items);
            }

            return itemUnitList;
        }

        public async Task<List<Item>> GetItemsFromFile()
        {
            // test deserailize with local file - okay!
            using (StreamReader file = File.OpenText(@"Models\Data.json"))
            {
                var jsonString = await file.ReadToEndAsync();
                ItemRootobject itemRootObj = (ItemRootobject)JsonSerializer.Deserialize(jsonString, typeof(ItemRootobject));
                var list = itemRootObj.data.items.item.ToList();
                return list;
            }
        } 

        public async Task<List<Item>> GetPublishedItems()
        {
            var query = new GraphQLRequest
            {
                Query = @"query itemsQuery {
                          items (include : PUBLISHED) {
                            item : nodes {
                              title
                              id
                              isbn
                              bookTitle
                              authors {
                                total
                                author : nodes {
                                  name
                                }
                              }
                              published
                              units {
                                id
                              }
                            }
                          }
                        } "
            };

            try
            {
                // connect to API
                var response = await _client.SendQueryAsync<JObject>(query);
      
                if (response.Data != null)
                {
                    string jsonString = response.Data.ToString();   // verify json response
                    var responseItems = JsonSerializer.Deserialize<Data>(jsonString);
                    return responseItems.items.item.ToList();
                }
                else
                {
                    return null;
                }
    
            }
            catch(Exception ex)
            {
                throw ex;
            }
     
        }
    }


    public class ItemsPerUnit
    {
        public string unitID { get; set; }
        public List<Item> items { get; set; }
        public int count { 
            get {
                if (items == null)
                    return 0;
                else
                    return items.Count;
            } 
        }

    }

    public class ResponseItemsObject
    {
        public Items Items { get; set; }
    }

}
