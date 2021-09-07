using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;

namespace VSIXSpotify.AddIn.Infrastructure
{
    public static class DTEHelper
    {
        private static DTE _dte = null;
        private static List<object> _events = new List<object>();

        public static DTE GetObjectDTE()
        {
            if (_dte == null)
                _dte = Package.GetGlobalService(typeof(DTE)) as DTE;
            if (_dte == null)
                throw new ArgumentNullException(nameof(_dte));
            return _dte;
        }

        public static void AddEvents(object events)
        {
            _events.Add(events);
        }

        public static void ClearEvents()
        {
            _events.Clear();
        }

        public static void Dispose()
        {
            if(_dte != null)
            {
                _dte = null;
                ClearEvents();
            }
        }
    }
}
