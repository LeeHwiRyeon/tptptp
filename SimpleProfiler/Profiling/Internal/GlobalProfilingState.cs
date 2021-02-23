using System.Collections.Generic;
using System.Linq;

namespace SimpleProfiler {
    internal class GlobalProfilingState {
        private static GlobalProfilingState m_instance;
        public static GlobalProfilingState Instance {
            get {
                if (m_instance == null) {
                    m_instance = new GlobalProfilingState();
                }

                return m_instance;
            }
        }

        private readonly Dictionary<string, Profiler> Profilers;

        public GlobalProfilingState()
        {
            Profilers = new Dictionary<string, Profiler>();
        }

        public void RegisterProfiler(Profiler profiler) => Profilers[profiler.Name] = profiler;

        public bool DoesProfilerExists(string name) => Profilers.Keys.Contains(name);

        public Profiler GetProfiler(string name)
        {
            if (Profilers.Count > 1) {
                return Profilers[name];
            }

            return GetDefault();
        }

        public Profiler GetDefault() => Profilers.Values.First();


    }
}