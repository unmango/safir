# Safir.Cli.EndToEndTests

A quick note about the test-per-class structure of this project:

XUnit 2.1 doesn't allow parallelizing tests within a collection, and a test class is the smallest "collection" we can get.
These tests run slow enough as it is, we can squeeze a little more performance out by running them in parallel.
Each test will get its own set of containers with unique names/ids so in theory there shouldn't be any conflicts.
