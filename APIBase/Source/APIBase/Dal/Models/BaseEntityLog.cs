using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIBase.Dal.Models
{
    public abstract class BaseEntityLog<TToLog, TToLogId> : BaseEntity<long>
        where TToLog : BaseEntity<TToLogId>
        where TToLogId : struct, IComparable, IFormattable, IComparable<TToLogId>, IEquatable<TToLogId>
    {
        [Required] public string Username { get; set; }
        [Required] public DateTime DateTime { get; set; }
        [Required] public string PreviousValue { get; set; }
        [Required] public string NewValue { get; set; }
        [Required] public string MethodName { get; set; }
        [NotMapped] public TToLog PreviousEntity { get; set; }
        [NotMapped] public TToLog NewEntity { get; set; }
    }
}
