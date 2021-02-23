namespace SimpleProfiler {
    public interface IProfilerAttribute {
        string SourceMethodName { get; }
        string SourceFilePath { get; }
        int SourceLineNumber { get; }
        int Depth { get; }

        string ProfilerName { get; }
    }
}