﻿namespace Framework.Common.Timing
{
    public static class ClockProviders
    {
        public static UnspecifiedClockProvider Unspecified { get; } = new UnspecifiedClockProvider();
    }
}
