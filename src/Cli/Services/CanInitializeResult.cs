using System;
using System.Collections.Generic;

namespace Cli.Services
{
    internal class CanInitializeResult
    {
        /// <summary>
        /// Result when validation was skipped due to name not matching.
        /// </summary>
        public static readonly CanInitializeResult Skip = new() { Skipped = true };

        /// <summary>
        /// Validation was successful.
        /// </summary>
        public static readonly CanInitializeResult Success = new() { Succeeded = true };

        /// <summary>
        /// True if validation was successful.
        /// </summary>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// True if validation was not run.
        /// </summary>
        public bool Skipped { get; protected set; }

        /// <summary>
        /// True if validation failed.
        /// </summary>
        public bool Failed { get; protected set; }

        /// <summary>
        /// Used to describe why validation failed.
        /// </summary>
        public string FailureMessage { get; protected set; } = string.Empty;

        /// <summary>
        /// Full list of failures (can be multiple).
        /// </summary>
        public IEnumerable<string> Failures { get; protected set; } = Array.Empty<string>();

        /// <summary>
        /// Returns a failure result.
        /// </summary>
        /// <param name="failureMessage">The reason for the failure.</param>
        /// <returns>The failure result.</returns>
        public static CanInitializeResult Fail(string failureMessage)
            => new() { Failed = true, FailureMessage = failureMessage, Failures = new[] { failureMessage } };

        /// <summary>
        /// Returns a failure result.
        /// </summary>
        /// <param name="failures">The reasons for the failure.</param>
        /// <returns>The failure result.</returns>
        public static CanInitializeResult Fail(IEnumerable<string> failures)
            => new() { Failed = true, FailureMessage = string.Join("; ", failures), Failures = failures };
    }
}
