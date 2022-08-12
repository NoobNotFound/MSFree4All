using System.Xml.Serialization; 
namespace MSFree4All.Core.Office.XML{ 

[XmlRoot(ElementName="IgnoreProduct")]
public class IgnoreProduct { 

	[XmlAttribute(AttributeName="ID")] 
	public string ID { get; set; } 
}

}