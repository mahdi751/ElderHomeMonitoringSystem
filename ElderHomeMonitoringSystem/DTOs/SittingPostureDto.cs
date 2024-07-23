namespace ElderHomeMonitoringSystem.DTOs
{
    public class SittingPostureDto
    {
        public DateTime Time { get; set; }
        public bool GoodPosture { get; set; }
        public double PostureDuration { get; set; }
    }
}
