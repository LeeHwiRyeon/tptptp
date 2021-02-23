using System.Diagnostics;

namespace SimpleProfiler {
    internal class ProfilingState {
        public Stopwatch Watch { get; set; }
        public string DisplayName { get; set; }
        public string ProfilerName { get; set; }

        public ProfilingState(IProfilerAttribute profilerAttribute)
        {
            Watch = new Stopwatch();
            DisplayName = profilerAttribute.SourceMethodName;
            ProfilerName = profilerAttribute.ProfilerName;
        }

        public void Start()
        {
            Watch.Start();
        }

        public ProfilingResult Stop()
        {
            Watch.Stop();
            return new ProfilingResult(DisplayName, new ProfilingTime(Watch));
        }
    }
}