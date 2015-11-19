using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hinshlabs.CommandLineParser;

namespace $rootnamespace$.Commands
{
    public class Demo3Command : CommandBase<Demo3CommandLine>
    {


        private Uri m_PortalLocation;
        public override string Description
        {
            get { return "[Demo3Command.Description]"; }
        }

        public override string Name
        {
            get { return "Demo3"; }
        }

        public Demo3Command(CommandCollection parent)
            : base(parent)
        {
        }

        protected override int RunCommand()
        {
            try
            {
                CommandOut.Warning("running Demo3");
                return -1;
            }
            catch (Exception ex)
            {
                CommandOut.Error("Failed: {0}", ex.ToString());
                return -1;
            }
        }

        protected override bool ValidateCommand()
        {
            return true;
        }

        public override string Title
        {
            get { return "[Demo3Command.Title]"; }
        }

        public override string Synopsis
        {
            get { return "[Demo3Command.Synopsis]"; }
        }

        public override ReadOnlyCollection<SwitchInfo> Switches
        {
            get { return CommandLine.Switches; }
        }

        public override string Qualifications
        {
            get { return "[Demo3Command.Qualifications]"; }
        }

    }
}
