﻿using System;

namespace KMHooks.KeyCombinations;

public class KeyCombinationFilterArgs : EventArgs
{
    public KeyCombination Combination { get; }
    public bool SuppressCombination { get; set; }

    public KeyCombinationFilterArgs(KeyCombination combination)
    {
        Combination = combination;
        SuppressCombination = false;
    }
}