namespace Microsoft.Extensions.SnowflakeIdGenerator
{
    /// <summary>
    /// 表示雪花Id接口
    /// </summary>
    public interface ISnowflakeId
    {
        /// <summary>
        /// 生成一个新的Id
        /// </summary>
        /// <returns>返回long类型的雪花Id</returns>
        long NewId();
    }
}
