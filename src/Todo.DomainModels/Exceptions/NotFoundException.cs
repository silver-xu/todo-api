using System;

namespace Todo.DomainModels.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not Found") { }
    }
}
