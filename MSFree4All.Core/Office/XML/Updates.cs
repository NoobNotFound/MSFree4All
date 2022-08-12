using System.Xml.Serialization; 
namespace MSFree4All.Core.Office.XML{ 

[XmlRoot(ElementName="Updates")]
public class Updates { 

	[XmlAttribute(AttributeName="Enabled")] 
	public bool Enabled { get; set; } 

	[XmlAttribute(AttributeName="UpdatePath")] 
	public string UpdatePath { get; set; } 

	[XmlAttribute(AttributeName="Channel")] 
	public string Channel { get; set; } 

	[XmlAttribute(AttributeName="TargetVersion")] 
	public string TargetVersion { get; set; } 

	[XmlAttribute(AttributeName="DeadLine")] 
	public string DeadLine { get; set; } 
}

}