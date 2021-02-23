using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleProfiler {
    public class Profiler {
        public string Name { get; private set; }

        private bool isRunning;
        private bool isPatched;
        private readonly List<PatchMethod> methods;
        private readonly Harmony harmony;
        private readonly Func<ProfilingResult, string> Format;
        private readonly StringBuilder m_history = new StringBuilder();


        internal Profiler(string name, IEnumerable<Assembly> assemblies, bool runOnBuild, Func<ProfilingResult, string> format)
        {
            if (GlobalProfilingState.Instance.DoesProfilerExists(name)) {
                throw new Exception($"A profiler with the name '{name}' already exists for this application");
            }

            Name = name;
            Format = format;

            harmony = new Harmony(name);
            methods = new List<PatchMethod>();

            if (!assemblies.Any()) {
                m_history.AppendLine("No assemblies specified, so no profiling can be done");
            }

            foreach (var assembly in assemblies) {
                methods.AddRange(assembly.GetTypes()
                          .SelectMany(t => t.GetMethods())
                          .Where(m => {
                              var attr = (IProfilerAttribute)m.GetCustomAttributes().Where(a => a is IProfilerAttribute).FirstOrDefault();
                              return attr != null && (attr.ProfilerName == Name || string.IsNullOrEmpty(attr.ProfilerName));
                          }).Select(m => {
                              //TODO make this better, don't return attributes twice
                              var attr = m.GetCustomAttributes().Where(a => a is IProfilerAttribute).FirstOrDefault();
                              if (attr is TestAttribute) {
                                  return new PatchMethod(m, ProfilingPatchType.Normal);
                              }

                              return new PatchMethod(m, ProfilingPatchType.Async);
                          }));
            }

            m_history.Append("Found ").Append(methods.Count).Append(" profileable methods in ").Append(assemblies.Count()).AppendLine("assemblies");

            if (runOnBuild) {
                PatchAll();
                isRunning = true;
            }

            GlobalProfilingState.Instance.RegisterProfiler(this);
            m_history.Append("Profiler created: ").AppendLine(assemblies.ToString());
        }

        internal void AddProfilingResult(ProfilingResult result)
        {
            var str = Format(result);
            m_history.AppendLine(str);
        }

        public void Start()
        {
            m_history.AppendLine("Starting profiling");

            if (isRunning) {
                return;
            }

            if (!isPatched) {
                PatchAll();
            }

            isRunning = true;
        }

        public void Stop()
        {
            m_history.AppendLine("Stopping profiling");

            if (!isRunning) {
                return;
            }

            isRunning = false;
        }

        private void PatchAll()
        {
            var profilerPrefix = AccessTools.Method(typeof(ProfilingMethods), "StartProfiling");
            var profilerPostfix = AccessTools.Method(typeof(ProfilingMethods), "StopProfiling");
            var profilerPostfixAsync = AccessTools.Method(typeof(ProfilingMethods), "StopProfilingAsync");

            int patchedMethods = 0;
            foreach (var info in methods) {
                var method = info.Method;
                try {
                    var postfix = info.PatchType == ProfilingPatchType.Normal ? profilerPostfix : profilerPostfixAsync;
                    m_history.Append("Trying to patch ").Append(method.Name).Append(" with ").Append(profilerPrefix.Name).Append(" and ").AppendLine(postfix.Name);
                    harmony.Patch(info.Method, new HarmonyMethod(profilerPrefix), new HarmonyMethod(postfix));
                    patchedMethods++;
                } catch (Exception e) {
                    m_history.Append("Failed to patch ").Append(method.Name).Append("Exception: ").AppendLine(e.Message);
                }
            }
            m_history.Append("Patched ").Append(patchedMethods).AppendLine(" methods");
            isPatched = true;
        }

        public static ProfilerBuilder Create(string profilerName = "profiler") => new ProfilerBuilder(profilerName);

        public void Print(string fullPath = null)
        {
            if (fullPath == null)
                fullPath = $"{Directory.GetCurrentDirectory()}/{DateTime.Now.ToString("yyyyMdd_HHmmss")}_UpdateFrameTooSlow.md";
            using (StreamWriter textWrite = File.CreateText(fullPath)) {
                textWrite.WriteLine(m_history.ToString());
                textWrite.Dispose();
            }
        }

        public void Clear()
        {
            m_history.Clear();
        }

    }
}