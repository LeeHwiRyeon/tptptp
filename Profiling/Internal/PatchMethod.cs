using System.Reflection;

namespace SimpleProfiler {
    internal class PatchMethod {
        public MethodBase Method { get; private set; }
        public ProfilingPatchType PatchType { get; private set; }

        public PatchMethod(MethodBase method, ProfilingPatchType patchType)
        {
            Method = method;
            PatchType = patchType;
        }
    }


}