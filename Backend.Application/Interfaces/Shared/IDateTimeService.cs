using System;

namespace Backend.Application.Interfaces.Shared
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}