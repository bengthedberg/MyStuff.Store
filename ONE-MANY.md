## One to Many Relationship

### Required

One-to-many relationships are used when a single entity is associated with any number of other entities.

For example, a Blog can have many associated Posts, but each Post is associated with only one Blog.

```csharp
// Principal (parent)
public class Blog
{
    public int Id { get; set; }
    public ICollection<Post> Posts { get; } = new List<Post>(); // Collection navigation containing dependents
}

// Dependent (child)
public class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; } // Required foreign key property
    public Blog Blog { get; set; } = null!; // Required reference navigation to principal
}
```

> A required relationship ensures that every dependent entity must be associated with some principal entity. However, a principal entity can always exist without any dependent entities. That is, a required relationship does not indicate that there will always be at least one dependent entity. There is no way in the EF model, and also no standard way in a relational database, to ensure that a principal is associated with a certain number of dependents. If this is needed, then it must be implemented in application (business) logic. See Required navigations for more information.

In case the relationship is not discovered by convention:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasMany(e => e.Posts)
        .WithOne(e => e.Blog)
        .HasForeignKey(e => e.BlogId)
        .IsRequired();
}
```

Or you can use the dependent to detup the relationship:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Post>()
        .HasOne(e => e.Blog)
        .WithMany(e => e.Posts)
        .HasForeignKey(e => e.BlogId)
        .IsRequired();
}
```

Neither of these options is better than the other; they both result in exactly the same configuration.

### Optional

This is the same as the previous example, except that the foreign key property and navigation to the principal are now nullable.

This makes the relationship "optional" because a dependent (Post) can exist without being related to any principal (Blog).

```csharp
// Principal (parent)
public class Blog
{
    public int Id { get; set; }
    public ICollection<Post> Posts { get; } = new List<Post>(); // Collection navigation containing dependents
}

// Dependent (child)
public class Post
{
    public int Id { get; set; }
    public int? BlogId { get; set; } // Optional foreign key property
    public Blog? Blog { get; set; } // Optional reference navigation to principal
}
```

Configure explicitly:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasMany(e => e.Posts)
        .WithOne(e => e.Blog)
        .HasForeignKey(e => e.BlogId)
        .IsRequired(false);
}
```

### One-to-many without navigation to principal and with shadow foreign key

```csharp
// Principal (parent)
public class Blog
{
    public int Id { get; set; }
    public ICollection<Post> Posts { get; } = new List<Post>(); // Collection navigation containing dependents
}

// Dependent (child)
public class Post
{
    public int Id { get; set; }
}
```

This example combines two of the previous examples by removing both the foreign key property and the navigation on the dependent.

This relationship is discovered by convention as an optional relationship.

Since there is nothing in the code that could be used to indicate that it should be required,
some minimal configuration using IsRequired is needed to create a required relationship.

For example:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasMany(e => e.Posts)
        .WithOne()
        .HasForeignKey("BlogId")
        .IsRequired();
}
```
