# Safir Common .NET Libraries

This directory contains the source for .NET implementations of Safir Common libraries.

## Safir.Protos

NOTE: The googleapis protos are required to resolve imports in custom protos, but we do NOT want to generate custom types from them.
Instead we use the types defined in `Google.Api.CommonProtos`.
This is because when using custom options such as `HttpRule`, we would end up with a class `HttpRule` defined in `Safir.Protos`, but other libraries that utilize it for metadata depend on `HttpRule` defined in `Google.Api.CommonProtos`.
So any pattern matching or reflection will fail, as the types are defined in different assemblies.

More info at [`googleapis/api-common-protos`](https://github.com/googleapis/api-common-protos#packages)
