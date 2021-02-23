using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimpleProfiler {
    [AttributeUsage(AttributeTargets.Method)]
    public class SimpleProfileAttribute : Attribute, IProfilerAttribute {
        public string ProfilerName { get; private set; }
        public string Name { get; private set; }

        public string SourceMethodName { get; private set; }
        public string SourceFilePath { get; private set; }
        public int SourceLineNumber { get; private set; }
        public int Depth { get; private set; }

        public SimpleProfileAttribute(int depth = 0,
                                      [CallerMemberName] string methodName = "",
                                      [CallerFilePath] string filePath = "",
                                      [CallerLineNumber] int lineNumber = 0)
        {
            SourceMethodName = methodName;
            SourceFilePath = filePath;
            SourceLineNumber = lineNumber;
            Depth = depth;
        }
    }
}

