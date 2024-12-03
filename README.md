# MyStack.SnowflakeIdGenerator
Open-source lightweight Snowflake ID generator


| nuget      | stats |
| ----------- | ----------- |
| [![nuget](https://img.shields.io/nuget/v/MyStack.SnowflakeIdGenerator.svg?style=flat-square)](https://www.nuget.org/packages/MyStack.SnowflakeIdGenerator)       |  [![stats](https://img.shields.io/nuget/dt/MyStack.SnowflakeIdGenerator.svg?style=flat-square)](https://www.nuget.org/stats/packages/MyStack.SnowflakeIdGenerator?groupby=Version)        |

# Getting Started

## Add Service Support

```
services.AddSnowflakeId(configure =>
{
    configure.GroupId = context.Configuration.GetValue<ushort>("SnowflakeIdGenerator:GroupId");
    configure.MachineId = context.Configuration.GetValue<ushort>("SnowflakeIdGenerator:MachineId");
});
```

## Generate a new Snowflake Id

```
var snowflakeId = ServiceProvider.GetRequiredService<ISnowflakeId>();
var id = snowflakeId.NewId();
```

# License 
MIT