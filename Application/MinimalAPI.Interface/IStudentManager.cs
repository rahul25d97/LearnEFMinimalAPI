using MinimalAPI.Models;

namespace MinimalAPI.Interface
{
    public interface IStudentManager
    {
        public Student Get_SudentById(int? Id = null);

        public Task<PagedList<vStudent>> Get_AllStudent(string? sSearchText = null,int PageNumber = 1, int PageSize = 5, string? SortBy = null, string? SortOrder = null);

        public void Remove_Student(int? Id = null);

        public int? Save_Student(Student obj);

    }
}
