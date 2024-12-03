# MyStack.SnowflakeIdGenerator

Open-source lightweight Snowflake ID generator

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