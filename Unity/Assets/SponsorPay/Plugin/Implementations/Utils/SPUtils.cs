using System;
using System.Diagnostics;
using June;

namespace SponsorPay
{
		public class SPUtils
		{
			public static void printWarningMessage()
			{
				Util.Log( "WARNING: SponsorPay plugin is not available on this platform." );
				Util.Log( "WARNING: the \"" + GetMethodName() + "\" method does not do anything" );
			}
			
			private static string GetMethodName()
			{
				StackTrace st = new StackTrace ();
				StackFrame sf = st.GetFrame (2);
				
				return sf.GetMethod().Name;
			}
		}
}

