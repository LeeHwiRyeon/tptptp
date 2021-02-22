using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleProfiler {
    public class ProfilerBuilder {
        private readonly List<Assembly> Assemblies;
        private readonly string name;
        private Func<ProfilingResult, string> format;

        internal ProfilerBuilder(string name)
        {
            Assemblies = new List<Assembly>();
            this.name = name;
            format = (r => $"{r.DisplayName}.{r.DisplayName} took {r.Time.Ticks} ticks | {r.Time.Milliseconds} ms to execute");
        }

        public ProfilerBuilder UseAssemblies(params Assembly[] assemblies)
        {
            Assemblies.AddRange(assemblies);

            return this;
        }

        public ProfilerBuilder UseFormat(Func<ProfilingResult, string> format)
        {
            this.format = format;
            return this;
        }

        public Profiler Build(bool run = true) => new Profiler(name, Assemblies, run, format);
    }
}