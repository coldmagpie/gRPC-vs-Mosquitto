// See https://aka.ms/new-console-template for more information

using Benchmark;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

BenchmarkRunner.Run<BenchmarkTest>(
    DefaultConfig.Instance
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .AddValidator(ExecutionValidator.FailOnError));