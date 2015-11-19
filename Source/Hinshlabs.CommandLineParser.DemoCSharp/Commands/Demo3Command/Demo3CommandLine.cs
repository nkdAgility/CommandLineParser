using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hinshlabs.CommandLineParser.DemoCSharp.Commands.Demo3Command
{
    public class Demo3CommandLine : CommandLineBase
    {

        private string m_value1;

        private Value2Values m_value2 = Value2Values.Value1;
        [CommandLineSwitch("Value1", "[CommandLineSwitch.Description]"), CommandLineAlias("v1")]
        public string Value1
        {
            get { return this.m_value1; }
            set { this.m_value1 = value; }
        }

        [CommandLineSwitch("Value2", "[CommandLineSwitch.Description]"), CommandLineAlias("v2")]
        public Value2Values Value2
        {
            get { return this.m_value2; }
            set { this.m_value2 = value; }
        }

        public enum Value2Values
        {
            Value1,
            Value2,
            Value3
        }

    }

}
