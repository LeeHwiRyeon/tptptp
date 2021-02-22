namespace SimpleProfiler {
    public interface IProfilerAttribute {
        string DisplayName { get; }
        string ParentDisplayName { get; }
        string ProfilerName { get; }
    }
}