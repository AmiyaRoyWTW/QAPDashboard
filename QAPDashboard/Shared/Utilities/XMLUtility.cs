using System.Xml.Linq;

namespace QAPDashboard.Shared.Utilities
{
    public static class XMLUtility
    {
        public static string GetTestDescriptionFromXmlFile(string docPath, string testCaseName)
        {
            if (File.Exists(docPath))
            {
                XDocument xmlDoc = XDocument.Load(docPath);
                return xmlDoc.Descendants("member")
                                .Where(t => t.Attribute("name").Value.Contains(testCaseName))
                                .Select(t => t.Element("summary")?.Value?.Trim())
                                .FirstOrDefault();
            }
            return null;
        }
    }
}
