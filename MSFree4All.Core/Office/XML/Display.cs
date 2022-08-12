using System.Xml.Serialization; 
namespace MSFree4All.Core.Office.XML{ 

[XmlRoot(ElementName="Display")]
public class Display { 

	[XmlAttribute(AttributeName="Level")] 
	public string Level { get; set; } 

	[XmlAttribute(AttributeName="AcceptEULA")] 
	public bool AcceptEULA { get; set; } 
}

}