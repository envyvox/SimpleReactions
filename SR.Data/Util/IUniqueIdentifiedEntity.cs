using System;

namespace SR.Data.Util
{
    public interface IUniqueIdentifiedEntity
    {
        Guid Id { get; set; }
    }
}
