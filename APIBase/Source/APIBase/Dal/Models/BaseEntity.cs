using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace APIBase.Dal.Models
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity<T>
        where T : struct
    {
        [Key]
        public T Id { get; set; }
    }
}
