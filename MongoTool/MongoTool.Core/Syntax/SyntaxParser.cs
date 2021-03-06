﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MongoTool.Core.CSharp;
using MongoTool.Core.ExtensionMethods;

namespace MongoTool.Core.Syntax
{
    public class SyntaxParser : ISyntaxParser
    {
        private readonly List<Tag> _tags;
        public IEnumerable<Tag> Tags { get { return _tags.AsReadOnly(); } }

        private readonly List<Suggestion> _suggestions;
        public IEnumerable<Suggestion> Suggestions
        {
            get { return _suggestions.AsReadOnly(); }
        }

        private readonly ISyntaxProvider _syntaxProvider;
        private readonly ICSharpEvaluator _cSharpEvaluator;

        public SyntaxParser(ISyntaxProvider syntaxProvider, ICSharpEvaluator cSharpEvaluator)
        {
            _tags = new List<Tag>();
            _suggestions = new List<Suggestion>();
            _syntaxProvider = syntaxProvider;
            _cSharpEvaluator = cSharpEvaluator;
        }

        public void FindSuggestions(string text, int cursor)
        {
            if (text.IsNullOrEmpty())
            {
                return;
            }
            _suggestions.Clear();
            _suggestions.AddRange(GetCompletions(text));
        }

        private IEnumerable<Suggestion> GetCompletions(string text)
        {
            var completions = _cSharpEvaluator.GetCompletions(text).OrderBy(c => c.Text);
            if (completions.Any())
            {
                return completions;
            }
            return Enumerable.Empty<Suggestion>();
        }

        public void Parse(string text, int start, int end)
        {
            _tags.Clear();
            if (text.IsNullOrEmpty() || end <= start)
            {
                return;
            }

            var wordStart = 0;
            for (var ii = start; ii < end; ii++)
            {
                if (!_syntaxProvider.IsSeparator(text[ii]))
                {
                    continue;
                }
                if (ii > 0 && !(_syntaxProvider.IsSeparator(text[ii - 1])))
                {
                    var wordEnd = ii;
                    var word = text.Substring(wordStart, wordEnd - wordStart);

                    var type = _syntaxProvider.GetTypeOf(word);
                    if (type != TagType.Nothing)
                    {
                        var tag = new Tag
                        {
                            Type = type,
                            Word = word,
                            StartPosition = wordStart,
                            EndPosition = wordEnd
                        };
                        _tags.Add(tag);
                    }
                }
                wordStart = ii + 1;
            }

            _tags.AddRange(GetStringsIn(text));
        }

        private static IEnumerable<Tag> GetStringsIn(string text)
        {
            var strings = new List<Tag>();
            strings.AddRange(MatchesOf(text, "\"{1}[^\"]*\"{1}", TagType.String));
            strings.AddRange(MatchesOf(text, @"/{2}[^\n]*", TagType.CSharpComment));
            strings.AddRange(MatchesOf(text, @"(/\*){1}[\w\W]*(\*/){1}", TagType.CSharpComment));
            return strings;
        }

        private static IEnumerable<Tag> MatchesOf(string text, string pattern, TagType tagType)
        {
            var matches = Regex.Matches(text, pattern);
            var strings = new List<Tag>();
            for (var ii = 0; ii < matches.Count; ii++)
            {
                var group = matches[ii].Groups[0];
                strings.Add(new Tag { StartPosition = group.Index, EndPosition = group.Index + group.Length, Type = tagType, Word = group.Value });
            }
            return strings;
        }
    }
}