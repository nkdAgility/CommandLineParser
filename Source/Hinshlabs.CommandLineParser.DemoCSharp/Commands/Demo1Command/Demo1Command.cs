using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hinshlabs.CommandLineParser.DemoCSharp.Commands.Demo1Command
{
    public class Demo1Command : CommandBase<Demo1CommandLine>
    {



        private Uri m_PortalLocation;
        public override string Description
        {
            get { return "demo 1 command demonstrates a sinle nested command"; }
        }

        public override string Name
        {
            get { return "Demo1"; }
        }

        protected override bool ValidateCommand()
        {
            return true;
        }

        public override string Title
        {
            get { return "demo 1"; }
        }

        public override string Synopsis
        {
            get { return "demo 1 command"; }
        }

        public override ReadOnlyCollection<SwitchInfo> Switches
        {
            get { return CommandLine.Switches; }
        }

        public override string Qualifications
        {
            get { return string.Empty; }
        }

        public Demo1Command(CommandCollection parent)
            : base(parent)
        {
        }

        protected override int RunCommand()
        {
            try
            {
                CommandOut.Warning("running Demo1");
                return -1;
            }
            catch (Exception ex)
            {
                CommandOut.Error("Failed: {0}", ex.ToString());
                return -1;
            }
        }

    }
}
