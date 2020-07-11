using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIBase.Dal.Models
{
    public abstract class BaseEntityLog<TToLog, TToLogId> : BaseEntity<long>
        where TToLog : BaseEntity<TToLogId>
        where TToLogId : struct, IComparable, IFormattable, IComparable<TToLogId>, IEquatable<TToLogId>
    {
        public string Username { get; set; }
        public DateTime DateTime { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        [NotMapped] public TToLog PreviousEntity { get; set; }
        [NotMapped] public TToLog NewEntity { get; set; }
    }
}
