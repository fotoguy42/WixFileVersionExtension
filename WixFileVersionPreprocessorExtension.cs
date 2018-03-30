using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Microsoft.Tools.WindowsInstallerXml;
using Microsoft.Deployment.WindowsInstaller;

namespace WixFileVersionExtension
{
    public class WixFileVersionPreprocessorExtension : PreprocessorExtension
    {        
        private static readonly string[] prefixes = { "fileVersion" };

        public override string[] Prefixes
        {
            get { return prefixes; }
        }

		public override string EvaluateFunction(string prefix, string function, string[] args)
		{
			switch (prefix)
			{
				case "fileVersion":
					// Make sure there actually is a file name
					if (args.Length == 0 || args[0].Length == 0)
						throw new ArgumentException("File name not specified");

					// Make sure the file exists
					if (!File.Exists(args[0]))
						throw new ArgumentException(string.Format("File name {0} does not exist", args[0]));

					//msi does not have "normal" ProductVersion or FileVersion, 
					if (string.Compare(Path.GetExtension(args[0]), ".msi", true) == 0)
					{
						return PropertyTable.Get(args[0], function);
					}

					// Get the file version information for the given file
					FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(args[0]);

					if (fileVersionInfo == null)
					{
						throw new ArgumentException(string.Format("Could not get FileVerisonInfo for File name {0}", args[0]));
					}

					// Get the property that matches the name of the function
					PropertyInfo propertyInfo = fileVersionInfo.GetType().GetProperty(function);

					// Make sure the property exists
					if (propertyInfo == null)
						throw new ArgumentException(string.Format("Unable to find property {0} in FileVersionInfo type", function));

					// Return the value of the property as a string
					object funcValue = propertyInfo.GetValue(fileVersionInfo, null);
					if (funcValue != null)
					{
						return funcValue.ToString();
					}
					else
					{
						throw new ArgumentException($"Property {0} does not exist on file {args[0]}.");
					}
			}

			return null;
		}

		//from https://stackoverflow.com/a/10234425/232226
		public class PropertyTable
		{
			public static string Get(string msi, string name)
			{
				using (Database db = new Database(msi))
				{
					return db.ExecuteScalar("SELECT `Value` FROM `Property` WHERE `Property` = '{0}'", name) as string;
				}
			}
			public static void Set(string msi, string name, string value)
			{
				using (Database db = new Database(msi, DatabaseOpenMode.Direct))
				{
					db.Execute("UPDATE `Property` SET `Value` = '{0}' WHERE `Property` = '{1}'", value, name);
				}
			}
		}

	}


}
