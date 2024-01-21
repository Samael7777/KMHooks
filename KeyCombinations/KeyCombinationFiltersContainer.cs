using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace KMHooks.KeyCombinations;

public class KeyCombinationFiltersContainer
{
    private readonly HashSet<Key> _combination = new();
    private readonly List<KeyCombinationFilter> _filters = new();

    public ImmutableList<KeyCombinationFilter> Filters
    {
        get
        {
            lock (_filters)
            {
                return _filters.ToImmutableList();
            }
        }
    }

    public void ProcessCombination(HookKeyEventArgs hookArgs)
    {
        var combination = BuildCurrentCombination(hookArgs);
        lock (_filters)
        {
            if (_filters.Count <= 0) return;

            var filterArgs = new KeyCombinationFilterArgs(combination);
            _filters[0].Process(this, filterArgs);
            hookArgs.Handled = filterArgs.SuppressCombination;
        }
    }

    public bool TryAddFilter(KeyCombinationFilter filter)
    {
        lock (_filters)
        {
            if (_filters.Contains(filter)) return false;

            if (_filters.Count > 0 && _filters.Last() is { } lastFilter)
            {
                lastFilter.Next = filter.Process;
            }
            _filters.Add(filter);
            return true;
        }
    }

    public bool TryRemoveFilter(KeyCombinationFilter filter)
    {
        lock (_filters)
        {
            var index = _filters.IndexOf(filter);
            switch (index)
            {
                case -1:
                    return false;
                case > 0:
                    _filters[index - 1].Next = (index + 1 >= _filters.Count) 
                        ? null 
                        : _filters[index + 1].Process;
                    break;
            }
            return _filters.Remove(filter);
        }
    }

    private KeyCombination BuildCurrentCombination(HookKeyEventArgs args)
    {
        switch (args.KeyState)
        {
            case KeyState.Up:
                _combination.Remove(args.KeyData);
                break;
            case KeyState.Down:
                _combination.Add(args.KeyData);
                break;
            case KeyState.Unknown:
            default:
                throw new ArgumentOutOfRangeException(nameof(args));
        }

        return KeyCombination.ParseKeySequence(_combination);
    }
}