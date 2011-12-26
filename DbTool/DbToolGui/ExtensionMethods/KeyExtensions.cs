﻿using System.Windows.Input;
using DbTool.Lib.ExtensionMethods;

namespace DbToolGui.ExtensionMethods
{
    public static class KeyExtensions
    {
        public static bool CombinationOf(this Key key, ModifierKeys modifier, params Key[] keys)
        {
            return (Keyboard.Modifiers | ModifierKeys.Control) > 0 && key.In(keys);
        }
    }
}