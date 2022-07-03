namespace Safir.Agent.GrpcServices

open System
open System.Net
open System.Threading.Tasks
open Safir.Protos

type HostService() =
    inherit Host.HostBase()

    override this.GetInfo(_, _) =
        Task.FromResult(HostInfo(MachineName = Environment.MachineName, HostName = Dns.GetHostName()))
