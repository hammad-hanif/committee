﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Committee.Interfaces
{
    public interface IAzureTableStorage<T> where T : TableEntity, new()
    {
        Task Delete(string partitionKey, string rowKey);
        Task<T> GetItem(string partitionKey, string rowKey);
        Task<List<T>> GetList();
        Task<List<T>> GetList(string partitionKey);
        Task Insert(T item);
        Task Update(T item);
    }
}
