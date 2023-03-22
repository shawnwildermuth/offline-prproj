using System.Xml;
using static System.Console;

WriteLine("offline-prproj - Tool for removing ")

if (args.Length != 2)
{
  WriteLine("You must supply the source and desination and they must be different");
  WriteLine("e.g. Offline-Prproj ./foo ./bar");
  return;
}

if (args[0].ToLower() == args[1].ToLower())
{
  WriteLine("Source and Desination and they must be different");
  return;
}

var source = args[0];
var dest = args[1];


WriteLine("Reading .prproj");

XmlDocument doc = new XmlDocument();
doc.Load(source);

if (doc is null) return;

WriteLine("prproj Loaded");

var elements = doc.SelectNodes("/PremiereData/Media[not(IsProxy) and VideoStream]");
WriteLine($"Total Media Elements: {elements?.Count}");

var changes = 0;
if (elements is not null)
{
  // Make Offline
  foreach (XmlNode media in elements)
  {
    if (media.InnerXml.Contains(".mp4") || media.InnerXml.Contains(".mov"))
    {
      changes++;
      XmlNode? path = media.SelectSingleNode("RelativePath");
      if (path is not null)
      {
        media.RemoveChild(path);
      }

      if (media.SelectSingleNode("OfflineReason") == null)
      {
        var reasonNode = doc.CreateElement("OfflineReason");
        reasonNode.InnerText = "5";
        media.AppendChild(reasonNode);
      }
    }
  }
  WriteLine();
  WriteLine($"Changed Elements: {changes}");
  WriteLine();
  WriteLine("Saving...");
  doc.Save(dest);
  WriteLine("Saved...");

}

