using MinimalAPI.Interface;
using MinimalAPI.Models;

namespace MinimalAPI.Repository
{
    public class StudentManagerRepository : IStudentManager
    {
        readonly DatabaseContext _dbContext = new();

        public StudentManagerRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<vStudent>> Get_AllStudent(string? sSearchText = null, int PageNumber = 1, int PageSize = 5, string? SortBy = null, string? SortOrder = null)
        {
            var query = _dbContext.StudentDB
                 .Where(x => string.IsNullOrEmpty(sSearchText)
                 || x.Name.Contains(sSearchText)
                 || x.Address.Contains(sSearchText)
                 || x.Department.Contains(sSearchText)
                 || x.ContactNo.Contains(sSearchText)
                 );

            int count = query.Count();

            return await query
                .Select(x => new vStudent
                {
                    StudentId = x.StudentId,
                    Name = x.Name,
                    Department = x.Department,
                    Address = x.Address,
                    ContactNo = x.ContactNo,
                    NOR = count
                })
                .ToPagedListAsync(PageNumber, PageSize, count);
        }

        public Student Get_SudentById(int? Id = null)
        {
            return _dbContext.StudentDB.Find(Id);
        }

        public void Remove_Student(int? Id = null)
        {
            if (Id > 0)
            {
                Student? obj = _dbContext.StudentDB.Find(Id);

                _dbContext.StudentDB.Remove(obj);
                _dbContext.SaveChanges();
            }
        }

        public int? Save_Student(Student obj)
        {
            if (obj.StudentId > 0)
            {
                _dbContext.StudentDB.Update(obj);
            }
            else
            {
                obj.StudentId = null;
                _dbContext.StudentDB.Add(obj);
            }

            _dbContext.SaveChanges();

            return obj.StudentId;
        }
    }
}
