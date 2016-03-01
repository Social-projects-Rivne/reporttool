using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.DAL.Entities;

namespace UnitTestProject
{
    public class FakeDb : IDB2
    {
        public void Dispose() { }
        public int SaveChanges() { return 0; }

        public DbSet<Member> Members { get; private set; }
        public DbSet<Team> Teams { get; private set; }
        public DbSet<Template> Templates { get; private set; }
        public DbSet<Field> Fields { get; private set; }
        public DbSet<FieldsInTemplate> FieldsInTemplates { get; private set; }
        public DbSet<FieldType> FieldTypes { get; set; }
        public FakeDb()
        {
            Members = new FakeDbSet<Member>();
            Teams = new FakeDbSet<Team>();
            Templates = new FakeDbSet<Template>();
            Fields = new FakeDbSet<Field>();
            FieldsInTemplates = new FakeDbSet<FieldsInTemplate>();
            FieldTypes = new FakeDbSet<FieldType>();
        }

    }
    public class FakeDbSet<T> : DbSet<T>, IQueryable, IEnumerable<T> where T : class
    {
        private readonly List<T> _data;
        private readonly IQueryable _query;

        public FakeDbSet()
        {
            _data = new List<T>();
            _query = _data.AsQueryable();
        }

        public override T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public override T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
