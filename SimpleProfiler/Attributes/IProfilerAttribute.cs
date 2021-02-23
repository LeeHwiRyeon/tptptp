namespace SimpleProfiler {
    public interface IProfilerAttribute {
        string SourceMethodName { get; }
        string SourceFilePath { get; }
        int SourceLineNumber { get; }

        string ProfilerName { get; }
    }
}