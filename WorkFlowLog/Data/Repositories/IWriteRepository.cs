﻿using WorkFlowLog.Data.Entities;

namespace WorkFlowLog.Data.Repositories;

public interface IWriteRepository<in T> where T : class, IEntity
{
    void Add(T item);

    void Remove(T item);

    void RemoveRange(IEnumerable<T> items);
    
    void Save();
}
