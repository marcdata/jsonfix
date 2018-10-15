using System;
using System.Collections.Generic;
using System.Text;

namespace FixServiceLib.FixSession
{
    public class XFixSessionFactory
    {
        public XFixSessionFactory()
        {

        }

        public XFixSession GetSession()
        {
            return new XFixSession();
        }

    }
}
