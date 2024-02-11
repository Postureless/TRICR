using System.Threading.Tasks;
using SQLite;
using System.Collections.ObjectModel;
using System.IO;
using System;
using System.Collections.Generic;
using TRICR.Models;

namespace TRICR.Controllers;

public class DatabaseController
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseController()
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SearchDatabase.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<SearchEntity>().Wait();
    }

    public Task<List<SearchEntity>> GetSearchQueriesAsync()
    {
        return _database.Table<SearchEntity>().ToListAsync();
    }

    public Task<int> SaveSearchQueryAsync(SearchEntity query)
    {
        return _database.InsertAsync(query);
    }
}