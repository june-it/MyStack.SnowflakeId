namespace MyStack.SnowflakeIdGenerator
{
    public class SnawflakeIdOptions
    {
        /// <summary>
        /// Get or set Gourp Id
        /// </summary>
        /// <remarks>
        /// Used to distinguish between different machine groups, Available value range 0~31
        /// </remarks>
        public ushort GroupId { get; set; } = default!;
        /// <summary>
        /// Get or set Machine Id
        /// </summary>
        /// <remarks>
        /// The serial number of the machine,Available value range 0~31
        /// </remarks>
        public ushort MachineId { get; set; } = default!; 
    }
}
