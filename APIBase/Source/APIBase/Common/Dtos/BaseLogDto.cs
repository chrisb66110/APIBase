using System;

namespace APIBase.Common.Dtos
{
    public abstract class BaseLogDto<TToLogDto>
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public DateTime DateTime { get; set; }
        public string MethodName { get; set; }
        public TToLogDto PreviousEntity { get; set; }
        public TToLogDto NewEntity { get; set; }
    }
}
