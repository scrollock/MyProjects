using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using ProcessorIndeed.Processing.Interfaces;

namespace ProcessorIndeed.Processing
{
    public static class PipeFactory
    {
        public static IPipeServer GetPipeClient(Uri uri)
        {
            var pipeBinding = new NetNamedPipeBinding();
            pipeBinding.CloseTimeout = new TimeSpan(0, 0, 20);
            pipeBinding.OpenTimeout = new TimeSpan(0, 0, 10);
            pipeBinding.ReceiveTimeout = new TimeSpan(0, 0, 20);
            pipeBinding.SendTimeout = new TimeSpan(0, 0, 20);
            pipeBinding.Security.Mode = NetNamedPipeSecurityMode.None;
            pipeBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;
            ChannelFactory<IPipeServer> channelPipeFactory = new ChannelFactory<IPipeServer>(
                pipeBinding,
                new EndpointAddress(uri.ToString()));
            return channelPipeFactory.CreateChannel();
        }

        public static IJetCommand GetJetCommand(IPipeServer server)
        {
            return new JetCommand(server);
        }

        public static ServiceHost GetHost(string url)
        {
            var host = new ServiceHost(typeof(PipeServer), new Uri(url));
            var pipeBinding = new NetNamedPipeBinding();
            pipeBinding.CloseTimeout = new TimeSpan(0, 30, 0);
            pipeBinding.OpenTimeout = new TimeSpan(0, 0, 1000);
            pipeBinding.ReceiveTimeout = new TimeSpan(0, 0, 3000);
            pipeBinding.SendTimeout = new TimeSpan(0, 0, 3000);
            pipeBinding.Security.Mode = NetNamedPipeSecurityMode.None;
            pipeBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;
            host.AddServiceEndpoint(typeof(IPipeServer), pipeBinding, url);
            foreach (ChannelDispatcher channelDipsatcher in host.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in channelDipsatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.AutomaticInputSessionShutdown = false;
                }
            }
            return host;
        }
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T item;
            while (queue.TryDequeue(out item))
            {
                // do nothing
            }
        }

    }
}
