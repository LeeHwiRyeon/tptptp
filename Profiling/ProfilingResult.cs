namespace SimpleProfiler {
    public class ProfilingResult {
        /// <summary> Watch.Elapsed 및 Watch.ElapsedTicks가 비동기 컨텍스트에서 동일하지 않기 때문에 이것을 사용합니다. </summary>
        public ProfilingTime Time { get; private set; }
        public string DisplayName { get; private set; }
        public ProfilingResult(string displayName, ProfilingTime time)
        {
            DisplayName = displayName;
            Time = time;
        }
    }
}