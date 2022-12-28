using Marten;
using Marten.Events.Projections;
using Marten.Schema;
using MartenDbEventsWithSameNameProjectionBug.A;
using MartenDbEventsWithSameNameProjectionBug.B;
using Shouldly;
using Weasel.Core;

namespace MartenDbEventsWithSameNameProjectionBug;

public class UnitTest1
{
  private const string connectionString =
    "Server=127.0.0.1;Port=6666;Database=postgres;User Id=postgres;Password=somepassword;";

  public UnitTest1()
  {
    
  }
  
  [Fact]
  public async Task CanRebuildProjectionCorrectly()
  {
    var options = new StoreOptions
    {
      AutoCreateSchemaObjects = AutoCreate.All,
      NameDataLength = 100
    };

    options.Connection(connectionString);
    options.Projections.SelfAggregate<DocA>(ProjectionLifecycle.Inline);
    options.Projections.SelfAggregate<DocB>(ProjectionLifecycle.Inline);
    
    var store = new DocumentStore(options);
    await store.Advanced.Clean.DeleteAllDocumentsAsync();
    await store.Advanced.Clean.DeleteAllEventDataAsync();


    var session = store.LightweightSession();
    session.Events.Append(Guid.NewGuid(), new A.Created());
    session.Events.Append(Guid.NewGuid(), new B.Created());
    await session.SaveChangesAsync();

    var aDocs = await session.Query<DocA>().ToListAsync();
    var bDocs = await session.Query<DocB>().ToListAsync();
    
    // await store.Advanced.Clean.DeleteAllDocumentsAsync();
    var daemon = await store.BuildProjectionDaemonAsync();
    await daemon.RebuildProjection<DocA>(default);
    await daemon.RebuildProjection<DocB>(default);

    var session2 = store.LightweightSession();
    var loadedA = await session2.Query<DocA>().ToListAsync();
    var loadedB = await session2.Query<DocB>().ToListAsync();

    loadedA.ShouldBeEquivalentTo(aDocs);
    loadedB.ShouldBeEquivalentTo(bDocs);
  }
}