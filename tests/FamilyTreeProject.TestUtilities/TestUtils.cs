#region Copyright

// Copyright 2011 - Charles Nurse

#endregion

using System.IO;
using System.Reflection;

namespace FamilyTreeProject.TestUtilities
{
    public class TestUtils
    {
        public static string GetResourceFile(string fileName)
        {
            return (new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream(fileName)).ReadToEnd());
        }
    }
}