using System;

namespace APIBase.Api.Responses
{
    public abstract class BaseLogResponse<TDto>
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public DateTime DateTime { get; set; }
        public TDto PreviousEntity { get; set; }
        public TDto NewEntity { get; set; }
    }
}
