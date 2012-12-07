using System;
namespace Shared
{
	public static class Parse
	{
		
		public static float Float(string s)
		{
			return float.Parse(s, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);			
		}

		public static int Int(string s)
		{
			return int.Parse(s, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}
	}
}

