namespace CourseProject.Interface.Repositories
{
    public interface IBaseRepository<T> : IBaseQueryRepository<T>, IBaseCommandRepository<T>
    { }

    public interface IBaseQueryRepository<T>
    {
        IEnumerable<T> GetAll();

        T? Get(Guid id);

        bool Any();

        int Count();
    }

    public interface IBaseCommandRepository<T>
    {
        Guid Add(T data);

        void Delete(T data);

        void Delete(Guid id);
    }
}
