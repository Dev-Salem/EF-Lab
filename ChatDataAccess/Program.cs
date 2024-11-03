using Chat.Dal.EfStructres;
using Chat.Dal.Initialization;
using Chat.Dal.Tests;
using ChatModels.Entities;
using Microsoft.EntityFrameworkCore;

var factory = new ApplicationDbContextFactory();
var context = factory.CreateDbContext([]);
SampleDataInitialization.ClearData(context);
foreach (var u in context.Users) {
    Console.WriteLine(u);
}