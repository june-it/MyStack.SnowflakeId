namespace Microsoft.Extensions.SnowflakeIdGenerator
{
    /// <summary>
    /// Represents the Snowflake ID interface
    /// </summary>
    public interface ISnowflakeId
    {
        /// <summary>
        /// Generates a new ID
        /// </summary>
        /// <returns>Returns a long type Snowflake ID</returns>
        long NewId();
    }
}
