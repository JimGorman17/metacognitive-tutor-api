using System.Collections.Generic;

namespace MetacognitiveTutor.DataLayer.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        T Find(int id);
        List<T> GetAll();
        T Add(T entity);
        T Update(T entity);
        void Remove(int id);

        void Save(T entity);
    }
}
