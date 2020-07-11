using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace APIBase.Dal.Models
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseEntity<T>
        where T : struct, IComparable, IFormattable, IComparable<T>, IEquatable<T>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public T Id { get; set; }
    }
}
