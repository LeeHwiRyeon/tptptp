using System;
using System.Runtime.CompilerServices;

namespace SimpleProfiler {
    [AttributeUsage(AttributeTargets.Method)]
    public class ProfileMeAsyncAttribute : Attribute, IProfilerAttribute {
        public string DisplayName { get; private set; }
        public string ParentDisplayName { get; private set; }
        public string ProfilerName { get; private set; }

        public ProfileMeAsyncAttribute([CallerMemberName] string displayName = "", string parentDisplayName = "", string profilerName = null)
        {
            DisplayName = displayName;
            ProfilerName = profilerName;
            ParentDisplayName = parentDisplayName;
        }
    }
}