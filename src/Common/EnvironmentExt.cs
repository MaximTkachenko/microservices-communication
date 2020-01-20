using System;

namespace Common
{
    public static class EnvironmentExt
    {
        static EnvironmentExt()
        {
            IsInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
        }

        public static readonly bool IsInContainer;
    }
}
