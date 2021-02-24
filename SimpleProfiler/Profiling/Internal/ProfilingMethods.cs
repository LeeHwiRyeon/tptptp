using MonoMod.Utils;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleProfiler {
    internal static class ProfilingMethods {
        public const string StartProfilingStr = "StartProfiling";
        public const string StopProfilingStr = "StopProfiling";
        public const string StopProfilingAsyncStr = "StopProfilingAsync";
        

        public static void StartProfiling(out ProfilingState __state, MethodInfo __originalMethod)
        {
            var attrs = __originalMethod.GetCustomAttributes();
            IProfilerAttribute attr = null;
            foreach (var ca in attrs) {
                if (ca is IProfilerAttribute pa) {
                    attr = pa;
                    break;
                }
            }

            if (attr == null)
                __state = null;
            else
                __state = new ProfilingState(attr, __originalMethod.DeclaringType.FullName);
            __state.Start();
        }

        public static void StopProfiling(ProfilingState __state)
        {
            var globalState = GlobalProfilingState.Instance;
            var result = __state.Stop();
            var profiler = string.IsNullOrEmpty(__state.ProfilerName) ?
                                            globalState.GetDefault() :
                                            globalState.GetProfiler(__state.ProfilerName);
            profiler.AddProfilingResult(ref result);
        }

        public static void StopProfilingAsync(ref Task __result, ProfilingState __state)
        {
            var contin =
                __result.ContinueWith(T => {
                    var globalState = GlobalProfilingState.Instance;
                    var result = __state.Stop();
                    var profiler = string.IsNullOrEmpty(__state.ProfilerName) ?
                                                    globalState.GetDefault() :
                                                    globalState.GetProfiler(__state.ProfilerName);
                    profiler.AddProfilingResult(ref result);
                });

            contin.Wait();
        }

    }
}