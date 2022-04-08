using Backend.Application.Interfaces.Shared;
using System;

namespace Backend.Infrastructure.Utils
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}