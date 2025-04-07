using CourseProject.Data.Models;
using CourseProject.Interface.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseData
    {
        protected WebDbContext _webDbContext;
        protected DbSet<T> _dbSet;

        public BaseRepository(WebDbContext webDbContext)
        {
            _webDbContext = webDbContext;
            _dbSet = webDbContext.Set<T>();
        }

        public virtual Guid Add(T data)
        {
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
            }

            _webDbContext.Add(data);
            _webDbContext.SaveChanges();

            return data.Id;
        }

        public virtual bool Any()
        {
            return _dbSet.Any();
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public virtual void Delete(T data)
        {
            _dbSet.Remove(data);
            _webDbContext.SaveChanges();
        }

        public virtual void Delete(Guid id)
        {
            var data = Get(id);
            if (data != null)
            {
                Delete(data);
            }
        }

        public virtual T? Get(Guid id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }
    }
}
