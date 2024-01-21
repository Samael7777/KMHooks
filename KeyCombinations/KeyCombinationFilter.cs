using System;

namespace KMHooks.KeyCombinations;

public class KeyCombinationFilter
{
    private readonly bool _suppressIfTriggered;
    private readonly Action _onTriggered;

    public EventHandler<KeyCombinationFilterArgs>? Next;

    public KeyCombinationFilter(KeyCombination keyCombination, bool suppressIfTriggered, Action onTriggered)
    {
        KeyCombination = keyCombination;
        _suppressIfTriggered = suppressIfTriggered;
        _onTriggered = onTriggered;
    }

    public KeyCombination KeyCombination { get; }

    public void Process(object? sender, KeyCombinationFilterArgs args)
    {
        if (KeyCombination.Equals(args.Combination))
        {
            _onTriggered.Invoke();
            args.SuppressCombination = _suppressIfTriggered;
            return;
        }
        Next?.Invoke(this, args);
    }
    
    public override int GetHashCode() => KeyCombination.GetHashCode();
}