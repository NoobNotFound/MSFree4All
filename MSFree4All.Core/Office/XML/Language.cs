using System.Xml.Serialization; 
namespace MSFree4All.Core.Office.XML{ 

[XmlRoot(ElementName="Language")]
public class Language { 

	[XmlAttribute(AttributeName="ID")] 
	public string ID { get; set; } 
}

}