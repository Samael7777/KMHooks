using System;
using System.Collections.Generic;
using System.Linq;
using Hooks.Extensions;

namespace Hooks.KeyCombinations;

public class KeyCombination : IEquatable<KeyCombination>
{
    private readonly HashSet<Key> _keys;
    private readonly HashSet<Key> _modifiers;

    public KeyCombination(IEnumerable<Key> modifiers, IEnumerable<Key> keys)
    {
        _modifiers = modifiers.OrderBy(key => key).ToHashSet();
        _keys = keys.OrderBy(key=>key).ToHashSet();
        IsSideInvariant = _modifiers.Any(key => key.IsSideInvariant());
    }

    public bool IsSideInvariant { get; }

    public IReadOnlyCollection<Key> Modifiers => _modifiers;
    public IReadOnlyCollection<Key> Keys => _keys;
    public IReadOnlyCollection<Key> Combination => _modifiers.Concat(_keys).ToArray();

    public static KeyCombination ParseKeySequence(IEnumerable<Key> sequence)
    {
        var modifiers = new HashSet<Key>();
        var keys = new HashSet<Key>();
        foreach (var key in sequence)
        {
            if (key.IsModifier())
            {
                modifiers.Add(key);
            }
            else
            {
                keys.Add(key);
            }
        }

        return new KeyCombination(modifiers, keys);
    }

    #region Equals

    public bool Equals(KeyCombination? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        if (Keys.Count != other.Keys.Count) return false;
        if (!Keys.SequenceEqual(other.Keys)) return false;
        if (!IsSideInvariant 
            && !Modifiers.SequenceEqual(other.Modifiers)) return false;
            
        return Modifiers.SequenceEqual(ToSideInvariant(other.Modifiers));
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((KeyCombination)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return _modifiers
                .Concat(_keys)
                .Aggregate(0, (current, b) => current * 31 ^ (int)b);
        }
    }

    private static IEnumerable<Key> ToSideInvariant(IEnumerable<Key> keys)
    {
        return keys.Select(key => key.ToSideInvariant()).OrderBy(key=>key).ToHashSet();
    }

    #endregion
}