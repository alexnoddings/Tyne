namespace Tyne.Example.Client.Common.Forms;

public partial class FormStateMachineDebugView : TyneFormComponentBase
{
    private static string GetStateDebugName(ITyneFormState state)
    {
        // For debugging, assumes all states are of the form TyneFormStateXyz, optionally with a generic ` after
        var name = state.GetType().Name;
        var start = "TyneFormState".Length;
        var end = name.IndexOf('`', StringComparison.OrdinalIgnoreCase);
        if (end > 0)
            return name[start..end];

        return name[start..];
    }

    private sealed record Node(string Key, string DisplayName, int X, int Y);

    private Node CurrentNode => GetStateDebugName(Form.State) switch
    {
        nameof(Nodes.Uninitialised) => Nodes.Uninitialised,
        nameof(Nodes.Initialising) => Nodes.Initialising,
        nameof(Nodes.InitialisingError) => Nodes.InitialisingError,
        nameof(Nodes.Inactive) => Nodes.Inactive,
        nameof(Nodes.Loading) => Nodes.Loading,
        nameof(Nodes.LoadingError) => Nodes.LoadingError,
        nameof(Nodes.Active) => Nodes.Active,
        nameof(Nodes.Saving) => Nodes.Saving,
        _ => throw new InvalidOperationException()
    };

    private static class Nodes
    {
        public static readonly Node Uninitialised = new("Uninitialised", "uninitialised", 60, 10);
        public static readonly Node Initialising = new("Initialising", "initialising", 60, 40);
        public static readonly Node InitialisingError = new("InitialisationError", "init error", 30, 55);
        public static readonly Node Inactive = new("Inactive", "inactive", 60, 70);
        public static readonly Node Loading = new("Loading", "loading", 60, 120);
        public static readonly Node LoadingError = new("LoadingError", "load error", 30, 95);
        public static readonly Node Active = new("Active", "active", 90, 95);
        public static readonly Node Saving = new("Saving", "saving", 90, 135);
    }

    // Describes the possible state transitions
    private static readonly Dictionary<Node, List<Node>> _transitions = new()
    {
        { Nodes.Uninitialised, [Nodes.Initialising] },
        { Nodes.Initialising, [Nodes.Inactive, Nodes.InitialisingError] },
        { Nodes.Inactive, [Nodes.Loading] },
        { Nodes.Loading, [Nodes.LoadingError, Nodes.Inactive, Nodes.Active] },
        { Nodes.LoadingError, [Nodes.Inactive] },
        { Nodes.Active, [Nodes.Saving, Nodes.Inactive] },
        { Nodes.Saving, [Nodes.Active] }
    };
}
