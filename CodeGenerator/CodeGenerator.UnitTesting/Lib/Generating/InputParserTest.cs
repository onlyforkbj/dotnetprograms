﻿using System.Linq;
using System.Text;
using AutoMoq;
using CodeGenerator.Lib.Generating;
using NUnit.Framework;

namespace CodeGenerator.UnitTesting.Lib.Generating
{
    [TestFixture]
    public class InputParserTest
    {
        private InputParser _parser;

        [SetUp]
        public void Setup()
        {
            var mocker = new AutoMoqer();
            _parser = mocker.Create<InputParser>();
        }

        [Test]
        public void Parse_ReturnsRecord()
        {
            const string input = @"a b c";
            var records = _parser.Parse(input, 1, " ");
            Assert.That(records.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Record_IsIndexed()
        {
            const string input = @"a b c";
            var record = _parser.Parse(input, 1, " ").First();
            Assert.That(record[0], Is.EqualTo("a"));
            Assert.That(record[1], Is.EqualTo("b"));
            Assert.That(record[2], Is.EqualTo("c"));
        }

        [Test]
        public void Parse_ReturnsOneRecordPerLine()
        {
            var input = new StringBuilder("a b c")
                .AppendLine("c d e")
                .AppendLine("f g h").ToString();

            var records = _parser.Parse(input, 2, " ");
            Assert.That(records.Count(), Is.EqualTo(2));
        }
    }
}