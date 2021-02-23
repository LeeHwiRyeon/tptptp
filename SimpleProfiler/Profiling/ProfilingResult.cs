namespace SimpleProfiler {
    public class ProfilingResult {
        /// <summary> Watch.Elapsed �� Watch.ElapsedTicks�� �񵿱� ���ؽ�Ʈ���� �������� �ʱ� ������ �̰��� ����մϴ�. </summary>
        public ProfilingTime Time { get; private set; }
        public string DisplayName { get; private set; }
        public ProfilingResult(string displayName, ProfilingTime time)
        {
            DisplayName = displayName;
            Time = time;
        }
    }
}