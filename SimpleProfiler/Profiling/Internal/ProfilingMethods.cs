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
            var attr = (IProfilerAttribute)__originalMethod.GetCustomAttributes().Where(a => a is IProfilerAttribute).FirstOrDefault();
            __state = new ProfilingState(attr, "");
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