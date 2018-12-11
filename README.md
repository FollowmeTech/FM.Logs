# FM.Logs
An extension Microsoft.Extensions.Logging

[所有系统的对接总标准](https://github.com/FollowmeTech/FM.Logs/wiki/%E6%9C%8D%E5%8A%A1%E6%97%A5%E5%BF%97%E5%8F%8A%E8%B0%83%E7%94%A8%E6%97%A5%E5%BF%97%E8%A7%84%E8%8C%83)

### 统一的log4net.config
[统一的log4net.config](https://github.com/FollowmeTech/FM.Logs/blob/master/Samples/TestLog4NetProvider/log4net.config)

##  关键点是用log4net里面的loggername来区分,具体看sample代码

### 业务log使用方式
```csharp
var logger = loggerFactory.CreateLogger(typeof(Program));
logger.LogInformation("hello world");
```

### grpc accesss log使用方式
```csharp
var accesslog = loggerFactory.CreateLogger("grpc.access");
accesslog.LogDebug("grpc access log");
```