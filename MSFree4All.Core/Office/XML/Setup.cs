using System.Xml.Serialization; 
namespace MSFree4All.Core.Office.XML{ 

[XmlRoot(ElementName="Setup")]
public class Setup { 

	[XmlAttribute(AttributeName="Name")] 
	public string Name { get; set; } 

	[XmlAttribute(AttributeName="Value")] 
	public string Value { get; set; } 
}

}