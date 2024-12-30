## One To One relationship

One-to-one relationships are used when one entity is associated with at most one other entity. For example, a Blog has one BlogHeader, and that BlogHeader belongs to a single Blog.

```csharp
// Principal (parent)
public class Blog
{
    public int Id { get; set; }
    public BlogHeader? Header { get; set; } // Reference navigation to dependent
}

// Dependent (child)
public class BlogHeader
{
    public int Id { get; set; }
    public int BlogId { get; set; } // Required foreign key property
    public Blog Blog { get; set; } = null!; // Required reference navigation to principal
}
```

> TIP: It is not always obvious which side of a one-to-one relationship should be the principal, and which side should be the dependent. Some considerations are:

- If the database tables for the two types already exist, then the table with the foreign key column(s) must map to the dependent type.
- A type is usually the dependent type if it cannot logically exist without the other type. For example, it makes no sense to have a header for a blog that does not exist, so BlogHeader is naturally the dependent type.
- If there is a natural parent/child relationship, then the child is usually the dependent type.

Explicit configuration:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasOne(e => e.Header)
        .WithOne(e => e.Blog)
        .HasForeignKey<BlogHeader>(e => e.BlogId)
        .IsRequired();
}
```

### optional

```csharp
// Principal (parent)
public class Blog
{
    public int Id { get; set; }
    public BlogHeader? Header { get; set; } // Reference navigation to dependent
}

// Dependent (child)
public class BlogHeader
{
    public int Id { get; set; }
    public int? BlogId { get; set; } // Optional foreign key property
    public Blog? Blog { get; set; } // Optional reference navigation to principal
}
```

This is the same as the previous example, except that the foreign key property and navigation to the principal are now nullable. This makes the relationship "optional" because a dependent (BlogHeader) can not be related to any principal (Blog) by setting its foreign key property and navigation to null.

Explicit configuration

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasOne(e => e.Header)
        .WithOne(e => e.Blog)
        .HasForeignKey<BlogHeader>(e => e.BlogId)
        .IsRequired(false);
}
```

### One-to-one without navigation to principal and with shadow foreign key

```csharp
// Principal (parent)
public class Blog
{
    public int Id { get; set; }
    public BlogHeader? Header { get; set; } // Reference navigation to dependent
}

// Dependent (child)
public class BlogHeader
{
    public int Id { get; set; }
}
```

This example combines two of the previous examples by removing both the foreign key property and the navigation on the dependent.

As before, this relationship needs some configuration to indicate the principal and dependent ends:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasOne(e => e.Header)
        .WithOne()
        .HasForeignKey<BlogHeader>("BlogId")
        .IsRequired();
}
```
