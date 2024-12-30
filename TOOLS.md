## dotnet counters

`dotnet tool install --global dotnet-counters`

`dotnet-counters` is a performance monitoring tool for ad-hoc health monitoring and first-level performance investigation. It can observe performance counter values that are published via the EventCounter API or the Meter API. For example, you can quickly monitor things like the CPU usage or the rate of exceptions being thrown in your .NET Core application.

### Usage

`
dotnet-counters collect [-h|--help] [-p|--process-id] [-n|--name] [--diagnostic-port] [--refresh-interval] [--counters <COUNTERS>] [--format] [-o|--output] [-- <command>]`

`dotnet-counters list [-h|--help]`

`dotnet-counters monitor [-h|--help] [-p|--process-id] [-n|--name] [--diagnostic-port] [--refresh-interval] [--counters] [-- <command>]`
