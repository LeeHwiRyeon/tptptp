using System;
using System.Runtime.CompilerServices;

namespace SimpleProfiler {
    [AttributeUsage(AttributeTargets.Method)]
    public class SimpleProfileAsyncAttribute : Attribute, IProfilerAttribute {
        public string ParentDisplayName { get; private set; }
        public string ProfilerName { get; private set; }

        public string SourceMethodName { get; private set; }
        public string SourceFilePath { get; private set; }
        public int SourceLineNumber { get; private set; }
        public int Depth { get; private set; }

        public SimpleProfileAsyncAttribute(int depth = 0,
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