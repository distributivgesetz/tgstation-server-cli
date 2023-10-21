namespace Tgstation.Server.CommandLineInterface.Commands.Remotes;

using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using Services;

[Command("remote",
     Description = "Displays the currently used remote. If a name param is given, tries to set the current remote.")]
public sealed class RemoteCommand : BaseCommand
{
    private readonly IRemoteRegistry remotes;

    [CommandParameter(0, Description = "The name of the remote to set.", IsRequired = false)]
    public string? Name { get; init; }

    public RemoteCommand(IRemoteRegistry registry) => this.remotes = registry;

    protected override ValueTask RunCommandAsync(IConsole console)
    {
        if (this.Name == null)
        {
            console.Output.WriteLine(this.remotes.HasCurrentRemote()
                ? this.remotes.GetCurrentRemote().Name
                : "No remote currently in use.");
        }
        else
        {
            if (this.remotes.HasCurrentRemote() && this.remotes.GetCurrentRemote().Name == this.Name)
            {
                return default;
            }

            if (!this.remotes.ContainsRemote(this.Name))
            {
                throw new CommandException("This remote has not been registered before.");
            }

            this.remotes.SetCurrentRemote(this.Name);
            this.remotes.SaveRemotes();
        }

        return default;
    }
}
