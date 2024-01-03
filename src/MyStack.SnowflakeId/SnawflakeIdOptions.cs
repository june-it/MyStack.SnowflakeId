namespace MyStack.SnowflakeIdGenerator
{
    public class SnawflakeIdOptions
    {
        /// <summary>
        /// 群组Id
        /// </summary>
        /// <remarks>
        /// 可用值范围0~31
        /// </remarks>
        public long GroupId { get; set; } = default!;
        /// <summary>
        /// 机器Id
        /// </summary>
        /// <remarks>
        /// 可用值范围0~31
        /// </remarks>
        public long MachineId { get; set; } = default!; 
    }
}
