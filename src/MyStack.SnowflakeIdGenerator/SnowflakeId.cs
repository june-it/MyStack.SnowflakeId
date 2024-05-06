using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.SnowflakeIdGenerator
{
    public class SnowflakeId : ISnowflakeId
    {
        // The timestamp of the start time
        private const long _twepoch = 691171200000L;
        // The bits of Machine ID
        private const int _machineIdBits = 5;
        // The bits of Group ID
        private const int _groupIdBits = 5;
        // Max value of Machine ID
        private const long _maxMachineId = -1L ^ (-1L << _machineIdBits);
        // Max value of Group ID
        private const long _maxGroupId = -1L ^ (-1L << _groupIdBits);
        // The bits of Sequence
        private const int _sequenceBits = 12;
        // GroupId shifted to the left by 17 bits
        private const int _groupIdShift = _sequenceBits + _machineIdBits;
        // MachineId shifted to the left by 12 bits
        private const int _machineIdShift = _sequenceBits;
        // Timestamp shifted to the left by 22 bits
        private const int _timestampLeftShift = _sequenceBits + _machineIdBits + _groupIdBits;
        // Mask of sequence 
        private const long _sequenceMask = -1L ^ (-1L << _sequenceBits);
        public long _lastTimestamp;
        public long _sequence;

        /// <summary>
        /// Initialize a SnowflakeId
        /// </summary>
        /// <param name="options">The options</param>
        /// <exception cref="Exception"></exception>
        public SnowflakeId(IOptions<SnawflakeIdOptions> options)
        {
            if (options.Value.GroupId > _maxGroupId || options.Value.GroupId < 0)
            {
                throw new Exception(string.Format("Group Id can't be greater than {0} or less than 0", _maxGroupId));
            }
            if (options.Value.MachineId > _maxMachineId || options.Value.MachineId < 0)
            {
                throw new Exception(string.Format("Machine Id can't be greater than {0} or less than 0", _maxMachineId));
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
    }
}
