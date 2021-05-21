using log4net.Core;
using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MESWebApi.Util
{
    public class IPatternLayout: log4net.Layout.PatternLayout
    {
        public IPatternLayout()
        {
            this.AddConverter("log_IP", typeof(IPPatternConverter));
        }
    }

    internal sealed class IPPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
             
        }
    }
}