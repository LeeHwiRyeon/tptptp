using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
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
        private readonly StringBuilder m_patchMethod = new StringBuilder();
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
                m_patchMethod.AppendLine("* No assemblies specified, so no profiling can be done");
            }

            foreach (var assembly in assemblies) {
                methods.AddRange(assembly.GetTypes()
                          .SelectMany(t => t.GetMethods())
                          .Where(m => {
                              foreach (var customAttr in m.GetCustomAttributes()) {
                                  if (customAttr is IProfilerAttribute attr)
                                      return attr != null && (attr.ProfilerName == Name || string.IsNullOrEmpty(attr.ProfilerName));
                              }
                              return false;
                          }).Select(m => {
                              //TODO make this better, don't return attributes twice
                              foreach (var customAttr in m.GetCustomAttributes()) {
                                  if (customAttr is SimpleProfileAttribute) {
                                      return new PatchMethod(m, ProfilingPatchType.Normal);
                                  }
                              }

                              return new PatchMethod(m, ProfilingPatchType.Async);
                          }));
            }

            m_patchMethod.Append("* Found ").Append(methods.Count).Append(" profileable methods in ").Append(assemblies.Count()).AppendLine("assemblies");

            if (runOnBuild) {
                PatchAll();
                isRunning = true;
            }

            GlobalProfilingState.Instance.RegisterProfiler(this);
            m_patchMethod.Append("* Profiler created: ").AppendLine(assemblies.ToString());
        }

        internal void AddProfilingResult(ref ProfilingResult result)
        {
            var str = Format(result);
            m_history.AppendLine(str);
        }

        public void Start()
        {
            m_patchMethod.AppendLine("* Starting profiling");

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
            m_patchMethod.AppendLine("* Stopping profiling");

            if (!isRunning) {
                return;
            }

            isRunning = false;
        }

        private void PatchAll()
        {
            var profilerPrefix = AccessTools.Method(typeof(ProfilingMethods), ProfilingMethods.StartProfilingStr);
            var profilerPostfix = AccessTools.Method(typeof(ProfilingMethods), ProfilingMethods.StopProfilingStr);
            var profilerPostfixAsync = AccessTools.Method(typeof(ProfilingMethods), ProfilingMethods.StopProfilingAsyncStr);

            int patchedMethods = 0;
            foreach (var info in methods) {
                var method = info.Method;
                try {
                    var postfix = info.PatchType == ProfilingPatchType.Normal ? profilerPostfix : profilerPostfixAsync;
                    m_patchMethod.Append("* Trying to patch ")
                                 .Append(method.Name)
                                 .Append(" with ")
                                 .Append(profilerPrefix.Name)
                                 .Append(" and ")
                                 .AppendLine(postfix.Name);
                    harmony.Patch(info.Method, new HarmonyMethod(profilerPrefix), new HarmonyMethod(postfix));
                    patchedMethods++;
                } catch (Exception e) {
                    m_patchMethod.Append("* Failed to patch ")
                                 .Append(method.Name)
                                 .Append("Exception: ")
                                 .AppendLine(e.Message);
                }
            }
            m_patchMethod.Append("* Patched ")
                         .Append(patchedMethods)
                         .AppendLine(" methods");
            isPatched = true;
        }

        public static ProfilerBuilder Create(string profilerName) => new ProfilerBuilder(profilerName);

        public void Print(string fullPath = null)
        {
            if (fullPath == null)
                fullPath = $"{Directory.GetCurrentDirectory()}/{DateTime.Now:yyyyMdd_HHmmss}_UpdateFrameTooSlow.md";
            using (StreamWriter textWrite = File.CreateText(fullPath)) {
                textWrite.WriteLine(m_history.ToString());
                textWrite.Dispose();
            }
        }

        public void Clear()
        {
            m_history.Clear();
            m_patchMethod.Clear();
        }

    }
}