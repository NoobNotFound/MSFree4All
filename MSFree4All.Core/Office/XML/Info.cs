using System.Xml.Serialization; 
namespace MSFree4All.Core.Office.XML{ 

[XmlRoot(ElementName="Info")]
public class Info { 

	[XmlAttribute(AttributeName="Description")] 
	public string Description { get; set; } 
}

}