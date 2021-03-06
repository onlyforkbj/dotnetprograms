﻿namespace MongoTool.Core.Syntax
{
    public interface ISyntaxProvider
    {
        TagType GetTypeOf(string word);
        bool IsSeparator(char value);
        bool IsPropertyIndicator(char c);
    }
}