using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Persistence;

namespace ToDoList.Test.IntegrationTests;
public class TestToDoItemsContext : ToDoItemsContext
{
    public TestToDoItemsContext(DbContextOptions<ToDoItemsContext> options)
        : base("Data Source=../../../IntegrationTests/data/localdb_test.db")
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=../../../IntegrationTests/data/localdb_test.db");
    }
}
