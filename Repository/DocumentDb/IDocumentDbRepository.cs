using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DocumentDb
{
    public interface IDocumentDbRepository
    {

        Task<IEnumerable<Item>> GetItemsAsync(Expression<Func<Item, bool>> predicate);

        Task<Item> GetItemAsync(string id);

        Task<Document> CreateItemAsync(Item item);

        Task<Document> UpdateItemAsync(string id, Item item);

        Task DeleteItemAsync(string id);
    }
}
