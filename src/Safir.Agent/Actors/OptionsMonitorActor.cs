using System;
using System.Collections.Generic;
using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;

namespace Safir.Agent.Actors
{
    internal sealed class OptionsMonitorActor : ReceiveActor
    {
        private readonly IServiceScope _serviceScope;
        private readonly IOptionsMonitor<AgentOptions> _options;
        private readonly HashSet<IActorRef> _subscribers = new();
        private IDisposable? _changeToken;

        public OptionsMonitorActor(IServiceProvider services)
        {
            _serviceScope = services.CreateScope();
            _options = _serviceScope.ServiceProvider.GetRequiredService<IOptionsMonitor<AgentOptions>>();

            Receive<GetOptionsValue>(OnGetOptionsValue);
            Receive<Subscribe>(OnSubscribe);
            Receive<Unsubscribe>(OnUnsubscribe);
        }

        protected override void PreStart()
        {
            _changeToken = _options.OnChange(OnOptionsChange);
        }

        protected override void PostStop()
        {
            _changeToken?.Dispose();
            _serviceScope.Dispose();
        }

        private void OnGetOptionsValue(GetOptionsValue message)
        {
            Sender.Tell(new OptionsValue(_options.CurrentValue));
        }

        private void OnOptionsChange(AgentOptions options)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Tell(new OptionsChange(options));
            }
        }

        private void OnSubscribe(Subscribe message)
        {
            _subscribers.Add(message.Actor);
        }

        private void OnUnsubscribe(Unsubscribe message)
        {
            _subscribers.Remove(message.Actor);
        }
    }
}
