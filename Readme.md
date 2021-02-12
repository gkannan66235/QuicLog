# Logging Best Practices:

## Guide For configuration:
1. Need to create Standard Logging library to abstract logging standards -- Create and add this package to nuget package manager (Probably Azure Artifacts).
2. Provide logging configuration through file (appsettings.json) for different application if required.
3. Have different HEC token for different environment/Application/Agency.

## Guide for development: 
1. Install the Standard logging library.
2. Application should not fail if the logger fails and it should write the event to debug console.
3. Write logs (do structured logging)
`SookLogger.Info("Processing Claim {ClaimNumber} of {ClaimantID}", claimNumber, claimantID);`

## For Production:
 1. Console logging should be restricted to Error. In .NET writing to Console is a blocking call and can have a significant performance impact. However considering the tracability requiremnet, we need to have Info level ?
 2. Global logging should be configured for Information and above.
 

## Ref:
Best Practices : https://benfoster.io/blog/serilog-best-practices/#http-logging
Serilog.AspNetCore: This package routes ASP.NET Core log messages through Serilog- https://github.com/serilog/serilog-aspnetcore

## Questions:
 1. Is this generic library for .NET Core & ASP.NET ?
 2. Splunk Enterprise or Splunk Cloud ? -Cloud