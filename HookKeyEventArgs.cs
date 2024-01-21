//------------------------------------------------------------------------------
// <copyright file="KeyEvent.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

using System;

namespace KMHooks;

public enum KeyState
{
    Unknown,
    Up,
    Down
}

public class HookKeyEventArgs : EventArgs 
{
    public HookKeyEventArgs(Key keyData, KeyState state) 
    {
            KeyData = keyData;
            KeyState = state;
        }

    public KeyState KeyState { get; protected set; }

    public bool Handled { get; set; }

    public Key KeyData { get; }
}