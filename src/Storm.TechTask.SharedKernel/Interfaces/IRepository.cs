using Storm.TechTask.SharedKernel.Entities;

namespace Storm.TechTask.SharedKernel.Interfaces
{
    public interface IRepository : IReadRepository
    {
        

        Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : BaseEntity, IAggregateRoot;
        Task UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : BaseEntity, IAggregateRoot;
        Task DeleteAsync<T>(T entity, CancellationToken cancellationToken) where T : BaseEntity, IAggregateRoot;


        // returns a queryable object (in our case we want to query a project)
        public IQueryable<T> Query<T>() where T : class;
        

    }


}
