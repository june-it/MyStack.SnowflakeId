using Microsoft.Extensions.Options;
using System;
using System.Text;

namespace MyStack.SnowflakeIdGenerator
{
    public class SnowflakeId : ISnowflakeId
    {
        // 开始时间截((new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)-Jan1st1970).TotalMilliseconds)
        private const long _twepoch = 1577836800000L;
        // 机器id所占的位数
        private const int _machineIdBits = 5;
        // 数据标识id所占的位数
        private const int _groupIdBits = 5;
        // 支持的最大机器id，结果是31 (这个移位算法可以很快的计算出几位二进制数所能表示的最大十进制数)
        private const long _maxWorkerId = -1L ^ (-1L << _machineIdBits);
        // 支持的最大数据标识id，结果是31
        private const long _maxDatacenterId = -1L ^ (-1L << _groupIdBits);
        // 序列在id中占的位数
        private const int _sequenceBits = 12;
        // 数据标识id向左移17位(12+5)
        private const int _groupIdShift = _sequenceBits + _machineIdBits;
        // 机器ID向左移12位
        private const int _machineIdShift = _sequenceBits;
        // 时间截向左移22位(5+5+12)
        private const int _timestampLeftShift = _sequenceBits + _machineIdBits + _groupIdBits;
        // 生成序列的掩码，这里为4095 (0b111111111111=0xfff=4095)
        private const long _sequenceMask = -1L ^ (-1L << _sequenceBits);
        public long _lastTimestamp;
        public long _sequence;

        /// <summary>
        /// 雪花ID
        /// </summary>
        /// <param name="datacenterId">数据中心ID</param>
        /// <param name="workerId">工作机器ID</param>
        public SnowflakeId(IOptions<SnawflakeIdOptions> options)
        {
            if (options.Value.GroupId > _maxDatacenterId || options.Value.GroupId < 0)
            {
                throw new Exception(string.Format("Group Id can't be greater than {0} or less than 0", _maxDatacenterId));
            }
            if (options.Value.MachineId > _maxWorkerId || options.Value.MachineId < 0)
            {
                throw new Exception(string.Format("Machine Id can't be greater than {0} or less than 0", _maxWorkerId));
            }
            MachineId = options.Value.GroupId;
            GroupId = options.Value.MachineId;
            _sequence = 0L;
            _lastTimestamp = -1L;
        }
        public long GroupId { get; }
        public long MachineId { get; }
        /// <summary>
        /// Generate a new Id
        /// </summary>
        /// <returns></returns>
        public long NewId()
        {
            lock (this)
            {
                long timestamp = GetCurrentTimestamp();
                // When the timestamp changes, the sequence resets within milliseconds
                if (timestamp > _lastTimestamp)
                {
                    _sequence = 0L;
                }
                // If generated at the same time, perform sequence sorting within milliseconds
                else if (timestamp == _lastTimestamp)
                {
                    _sequence = (_sequence + 1) & _sequenceMask;
                    if (_sequence == 0) 
                    {
                        timestamp = GetNextTimestamp(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = (_sequence + 1) & _sequenceMask;
                    if (_sequence > 0)
                    {
                        timestamp = _lastTimestamp;
                    }
                    else
                    {
                        timestamp = _lastTimestamp + 1;
                    }
                }

                _lastTimestamp = timestamp;

                return ((timestamp - _twepoch) << _timestampLeftShift)
                        | (GroupId << _groupIdShift)
                        | (MachineId << _machineIdShift)
                        | _sequence;
            }
        }

        /// <summary>
        /// Block until the next millisecond until a new timestamp is obtained
        /// </summary>
        /// <param name="lastTimestamp">Last generated ID timestamp</param>
        /// <returns></returns>
        private static long GetNextTimestamp(long lastTimestamp)
        {
            long timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetCurrentTimestamp();
            }
            return timestamp;
        }

        /// <summary>
        /// Get the current timestamp
        /// </summary>
        /// <returns></returns>
        private static long GetCurrentTimestamp()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        protected SnawflakeIdOptions Options { get; }
    }
}
