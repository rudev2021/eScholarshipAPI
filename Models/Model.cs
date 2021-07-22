using GraphQL;
using GraphQL.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eScholarshipAPI_Client.Models
{
    public class ItemConsumer
    {
        private readonly IGraphQLClient _client;

        public ItemConsumer(IGraphQLClient client)
        {
            _client = client;
        }

        public async Task<List<Item>> GetPublishedItems()
        {
            var query = new GraphQLRequest
            {
                Query = @"
                    query itemsQuery {
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
                    }"
            };

            try
            {
                var response = await _client.SendQueryAsync<ItemCollectionType>(query);
                return response.Data.Items;
            }
            catch(Exception ex)
            {
                throw ex;
            }
     
        }
    }

    public class ItemCollectionType
    {
        public List<Item> Items { get; set; }
    }

    public class ResponseOwnerType
    {
        public Item Item { get; set; }
    }


}
