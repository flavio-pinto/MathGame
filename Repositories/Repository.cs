﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MathGame.Data;

namespace MathGame.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MathGameContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(MathGameContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
