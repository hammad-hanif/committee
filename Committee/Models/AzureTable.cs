﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Committee.Interfaces;

namespace Committee.Models
{
    public class AzureTable<T>: IAzureTableStorage<T> where T : TableEntity, new()
    {
        private readonly string _storageAccount;
        private readonly string _storageKey;
        private readonly string _tableName;

        public AzureTable(string storageAccount, string storageKey, string tableName)
        {
            _storageAccount = storageAccount;
            _storageKey = storageKey;
            _tableName = tableName;
        }

        private async Task<CloudTable> GetTableAsync()
        {
            //Account  
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(
                new StorageCredentials(_storageAccount, _storageKey), false);

            //Client  
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();

            //Table  
            CloudTable table = tableClient.GetTableReference(_tableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }

        public async Task<List<T>> GetList()
        {
            //Table  
            CloudTable table = await GetTableAsync();

            //Query  
            TableQuery<T> query = new TableQuery<T>();

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }

        public async Task<List<T>> GetList(string partitionKey)
        {
            //Table
            CloudTable table = await GetTableAsync();

            //Query
            TableQuery<T> query = new TableQuery<T>()
                                        .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                                                QueryComparisons.Equal, partitionKey));

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;

                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }

        public async Task<T> GetItem(string partitionKey, string rowKey)
        {
            //Table  
            CloudTable table = await GetTableAsync();

            //Operation  
            TableOperation operation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            //Execute  
            TableResult result = await table.ExecuteAsync(operation);

            return (T)(dynamic)result.Result;
        }

        public async Task Insert(T item)
        {
            //Table  
            CloudTable table = await GetTableAsync();

            //Operation  
            TableOperation operation = TableOperation.Insert(item);

            //Execute  
            await table.ExecuteAsync(operation);
        }

        public async Task Update(T item)
        {
            //Table  
            CloudTable table = await GetTableAsync();

            //Operation  
            TableOperation operation = TableOperation.InsertOrReplace(item);

            //Execute  
            await table.ExecuteAsync(operation);
        }

        public async Task Delete(string partitionKey, string rowKey)
        {
            //Item
            T item = await GetItem(partitionKey, rowKey);

            //Table
            CloudTable table = await GetTableAsync();

            //Operation
            TableOperation operation = TableOperation.Delete(item);

            //Execute
            await table.ExecuteAsync(operation);
        }
    }
}
