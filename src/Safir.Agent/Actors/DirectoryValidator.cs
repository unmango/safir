using System.IO;
using Akka.Actor;

namespace Safir.Agent.Actors
{
    internal sealed class DirectoryValidator : ReceiveActor
    {
        private interface IValidationResult { }
        
        public record ValidationError(string Message) : IValidationResult;

        public record ValidationSuccess : IValidationResult;

        public DirectoryValidator()
        {
            Receive<string?>(OnString);
        }
        
        private void OnString(string? directory)
        {
            Sender.Tell(Validate(directory));
        }

        private static IValidationResult Validate(string? directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                return new ValidationError("Directory was null or empty");
            }

            if (!Directory.Exists(directory))
            {
                return new ValidationError("Directory does not exist");
            }

            return new ValidationSuccess();
        }
    }
}
