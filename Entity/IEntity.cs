using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public interface IEntity
    {
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }

    public abstract class BaseEntity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
