namespace MartenDbEventsWithSameNameProjectionBug.B;

public class DocB
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