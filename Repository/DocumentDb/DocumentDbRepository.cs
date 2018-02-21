using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Services.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DocumentDb
{
    public class DocumentDbRepository : IDocumentDbRepository 
    {
        DocumentDbConnection _object;

        public DocumentDbRepository()
        {
            _object = DocumentDbConnection.GetObject();
        }

        public async Task<Item> GetItemAsync(string id)
        {
            try
            {
                Document document = await _object.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_object.DatabaseId, _object.CollectionId, id));
                return (Item)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public  async Task<IEnumerable<Item>> GetItemsAsync(Expression<Func<Item, bool>> predicate)
        {
            IDocumentQuery<Item> query = _object.client.CreateDocumentQuery<Item>(
                UriFactory.CreateDocumentCollectionUri(_object.DatabaseId, _object.CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<Item> results = new List<Item>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Item>());
            }

            return results;
        }

        public  async Task<Document> CreateItemAsync(Item item)
        {
            return await _object.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_object.DatabaseId, _object.CollectionId), item);
        }

        public  async Task<Document> UpdateItemAsync(string id, Item item)
        {
            return await _object.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_object.DatabaseId, _object.CollectionId, id), item);
        }

        public  async Task DeleteItemAsync(string id)
        {
            await _object.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_object.DatabaseId, _object.CollectionId, id));
        }
    }
}
