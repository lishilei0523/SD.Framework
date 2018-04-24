using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SD.Infrastructure.Repository.RavenDB.Toolkits
{
    internal sealed class ConnectionStringParser
    {
        private static readonly Regex _ConnectionStringRegex;
        private static readonly Regex _ConnectionStringArgumentsSplitterRegex;
        static ConnectionStringParser()
        {
            ConnectionStringParser._ConnectionStringArgumentsSplitterRegex = new Regex(@"; (?=\s* \w+ \s* =)", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
            ConnectionStringParser._ConnectionStringRegex = new Regex(@"(\w+) \s* = \s* (.*)", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        }

        private readonly string _connectionString;
        private readonly RavenConnectionStringOptions _connectionStringOptions;
        public ConnectionStringParser(string connectionString)
        {
            this._connectionStringOptions = new RavenConnectionStringOptions();
            this._connectionString = connectionString;
        }


        public RavenConnectionStringOptions Parse()
        {
            string[] strings = _ConnectionStringArgumentsSplitterRegex.Split(this._connectionString);

            foreach (string text in strings)
            {
                string arg = text.Trim(';');
                Match match = _ConnectionStringRegex.Match(arg);

                if (match.Success == false)
                {
                    throw new ArgumentException("Connection string could not be parsed");
                }

                string key = match.Groups[1].Value.ToLower();
                string value = match.Groups[2].Value.Trim();

                // I am sure there are more elegant solutions than this one. But it makes the job done. 
                // Clear separation and same parsing logic as long as inheritance tree is well constructed and the calls are topologically ordered.
                bool processed = this.ProcessConnectionStringOption(this._connectionStringOptions, key, value);

                if (!processed)
                {
                    throw new ArgumentException($"Connection could not be parsed, unknown option: '{key}'");
                }
            }

            return this._connectionStringOptions;
        }

        private bool ProcessConnectionStringOption(RavenConnectionStringOptions options, string key, string value)
        {
            switch (key)
            {
                case "urls":
                    string[] urls = value.Split(',');
                    urls = urls.Select(x => x.EndsWith("/") ? x.Substring(0, x.Length - 1) : x).ToArray();
                    options.Urls = urls;
                    break;
                case "database":
                case "defaultdatabase":
                    options.DefaultDatabase = value;
                    break;

                // Couldn't process the option.
                default: return false;
            }

            // Could process therefore we didn't enter in default.
            return true;
        }
    }
}