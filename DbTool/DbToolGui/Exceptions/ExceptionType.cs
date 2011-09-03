﻿using System.ComponentModel;

namespace DbToolGui.Exceptions
{
    public enum ExceptionType
    {
        [Description("Already connected to {0}")]
        AlreadyConnected,
        [Description("Not connected to database")]
        NotConnected,
        [Description("Unknown database command '{0}'")]
        UnknownDatabaseCommand
    }
}