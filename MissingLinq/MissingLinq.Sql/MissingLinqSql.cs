﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissingLinq.Sql
{
    public class MissingLinqSql
    {
        public string Verb { get; set; }
        public string Statement { get { return GetStatement(); } }
        public int Count { get; set; }
        public string What { get; set; }
        public string Table { get; set; }
        private readonly IList<string> _wheres = new List<string>();
        public string Where { get { return string.Join(" and ", _wheres); } }

        public string CommandText { get { return BuildCommandText(); } }
        public IDictionary<string, MissingLinqSqlParameter> Parameters { get; set; }
        public bool AllowDefault { get; set; }

        private readonly IList<OrderColumn> _orderBys = new List<OrderColumn>();

        public MissingLinqSql()
        {
            Verb = "select";
            What = "*";
            Parameters = new Dictionary<string, MissingLinqSqlParameter>();
        }

        private string GetStatement()
        {
            var builder = new StringBuilder(Verb);
            if (Verb.Equals("select") && Count > 0)
            {
                builder.AppendFormat(" top {0}", Count);
            }
            if (!string.IsNullOrWhiteSpace(What))
            {
                builder.AppendFormat(" {0}", What);
            }
            return builder.ToString();
        }

        private string BuildCommandText()
        {
            var builder = new StringBuilder(Statement)
                .AppendFormat(" from {0}", Table);
            if (_wheres.Any())
            {
                builder.AppendFormat(" where {0}", Where);
            }
            if (_orderBys.Any())
            {
                builder.AppendFormat(" order by {0}", string.Join(", ", _orderBys.Select(o => o.Statement)));
            }
            return builder.ToString();
        }

        public void AppendWhere(string where)
        {
            _wheres.Add(where);
        }

        public override string ToString()
        {
            var builder = new StringBuilder(CommandText).AppendLine();
            var parameters = Parameters.Values.Select(p => string.Format("{0}='{1}'", p.Name, p.Value));
            if (parameters.Any())
            {
                builder.AppendFormat("[ {0} ]", string.Join(" ", parameters));
            }
            return builder.ToString();
        }

        public MissingLinqSqlParameter NewParameter()
        {
            var parameter = new MissingLinqSqlParameter(NextParameterNumber());
            Parameters[parameter.Name] = parameter;
            return parameter;
        }

        private int NextParameterNumber()
        {
            return Parameters.Any()
                       ? Parameters.Values.Max(p => p.Number) + 1
                       : 1;
        }

        public void OrderBy(string column, bool ascending = true)
        {
            _orderBys.Add(new OrderColumn(column, ascending));
        }
    }
}