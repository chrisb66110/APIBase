using System;

namespace APIBase.Api.Responses
{
    public abstract class BaseLogResponse<TToLogResponse>
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public DateTime DateTime { get; set; }
        public string MethodName { get; set; }
        public TToLogResponse PreviousEntity { get; set; }
        public TToLogResponse NewEntity { get; set; }
    }
}
