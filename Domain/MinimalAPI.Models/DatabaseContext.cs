using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace MinimalAPI.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Student>? StudentDB { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("LRN");


            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");
                entity.Property(e => e.StudentId).HasColumnName("StudentId");
                entity.Property(e => e.Name);
                entity.Property(e => e.Department);
                entity.Property(e => e.Address);
                entity.Property(e => e.ContactNo);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    public class PagedList<T> : IReadOnlyList<T>
    {
        private readonly IList<T> subset;
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            subset = items as IList<T> ?? new List<T>(items);
        }

        public int PageNumber { get; }

        public int TotalPages { get; }

        public bool IsFirstPage => PageNumber == 1;

        public bool IsLastPage => PageNumber == TotalPages;

        public int Count => subset.Count;

        public T this[int index] => subset[index];

        public IEnumerator<T> GetEnumerator() => subset.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => subset.GetEnumerator();
    }

    public static class PagedListQueryableExtensions
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        int count,
        CancellationToken token = default)
        {
            // var count = await source.CountAsync(token);
            if (count > 0)
            {
                var items = await source
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(token)
                    ;

                return new PagedList<T>(items, count, page, pageSize);
            }

            return new(Enumerable.Empty<T>(), 0, 0, 0);
        }
    }
}
