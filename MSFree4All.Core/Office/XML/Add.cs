using System.Xml.Serialization; 
using System.Collections.Generic; 
namespace MSFree4All.Core.Office.XML{ 

[XmlRoot(ElementName="Add")]
public class Add { 

	[XmlElement(ElementName="Product")] 
	public List<Product> Product { get; set; } 

	[XmlAttribute(AttributeName="OfficeClientEdition")] 
	public int OfficeClientEdition { get; set; } 

	[XmlAttribute(AttributeName="Channel")] 
	public string Channel { get; set; } 

	[XmlAttribute(AttributeName="SourcePath")] 
	public string SourcePath { get; set; } 

	[XmlAttribute(AttributeName="DownloadPath")] 
	public int DownloadPath { get; set; } 

	[XmlAttribute(AttributeName="Version")] 
	public int Version { get; set; } 

	[XmlAttribute(AttributeName="ForceUpgrade")] 
	public bool ForceUpgrade { get; set; } 

	[XmlAttribute(AttributeName="MigrateArch")] 
	public bool MigrateArch { get; set; } 

	[XmlAttribute(AttributeName="AllowCdnFallback")] 
	public bool AllowCdnFallback { get; set; } 

	[XmlAttribute(AttributeName="OfficeMgmtCOM")] 
	public bool OfficeMgmtCOM { get; set; } 
}

}