namespace TestWebAPI.Library.HealthChecks
{
    /// <summary>
    /// The memory check options.
    /// </summary>
    public class MemoryCheckOptions
    {
        /// <summary>
        /// Gets or sets the threshold.
        /// Failure threshold (in bytes)
        /// </summary>
        public long Threshold { get; set; } = 1024L * 1024L * 1024L;
    }
}
