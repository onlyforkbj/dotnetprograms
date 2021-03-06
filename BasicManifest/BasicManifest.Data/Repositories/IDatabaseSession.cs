﻿using System.Linq;

namespace BasicManifest.Data.Repositories
{
    public interface IDatabaseSession
    {
        T Get<T>(object id);
        IQueryable<T> GetAll<T>();
        object Delete(object item);
        object Save(object item);
        void Flush();
    }
}