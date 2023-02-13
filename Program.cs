using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ExampleDbContext>(opt => opt.UseInMemoryDatabase("test"));

builder.Services.AddGraphQLServer()
    .RegisterDbContext<ExampleDbContext>()
    .AddDefaultTransactionScopeHandler()
    .UseDefaultPipeline()
    .AddProjections()
    .AddSorting()
    .AddQueryType<ExampleQuery>();

var app = builder.Build();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGraphQL();

app.Run();

public sealed class ExampleDbContext : DbContext
{
    /// <inheritdoc />
    public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options)
    {
    }
}

public sealed class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}

public sealed class ExampleQuery
{
    [UsePaging, UseSorting] // Does work
    // [UseSorting] // Doesn't work
    public async Task<IQueryable<Book>> GetBooks(ExampleDbContext database, CancellationToken cancellationToken)
    {
        var books = new List<Book>();

        return books.AsQueryable();
    }
}
