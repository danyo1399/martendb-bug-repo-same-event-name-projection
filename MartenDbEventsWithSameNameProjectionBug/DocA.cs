using System.Security.AccessControl;

namespace MartenDbEventsWithSameNameProjectionBug.A;

public class DocA
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public int Version { get; set; }
  public void Apply(Created evt)
  {
    Version++;
  }
}

public class Created
{
  
}