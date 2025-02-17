using NUnit.Framework;

// ✅ Enable parallel execution at the feature level / Scenario Level
[assembly: Parallelizable(ParallelScope.Children)]

// ✅ Define the number of concurrent test workers (adjust as needed)
[assembly: LevelOfParallelism(4)]