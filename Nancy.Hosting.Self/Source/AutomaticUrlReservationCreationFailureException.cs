namespace Nancy.Hosting.Self
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Exception for when automatic address reservation creation fails.
    /// Provides the user with manual instructions.
    /// </summary>
    public class AutomaticUrlReservationCreationFailureException : Exception
    {
        private readonly IEnumerable<string> prefixes;
        private readonly string user;

        public AutomaticUrlReservationCreationFailureException(IEnumerable<string> prefixes, string user)
        {
            this.prefixes = prefixes;
            this.user = user;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>
        /// The error message that explains the reason for the exception, or an empty string("").
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override string Message
        {
            get
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("The Nancy self host was unable to start, as no namespace reservation existed for the provided url(s).");
                stringBuilder.AppendLine();

                stringBuilder.AppendLine("Please either enable UrlReservations.CreateAutomatically on the HostConfiguration provided to ");
                stringBuilder.AppendLine("the NancyHost, or create the reservations manually with the (elevated) command(s):");
                stringBuilder.AppendLine();

                foreach (var command in prefixes.Select(prefix => NetSh.GetParameters(prefix, user)))
                    stringBuilder.AppendLine($"netsh {command}");

                return stringBuilder.ToString();
            }
        }
    }
}
