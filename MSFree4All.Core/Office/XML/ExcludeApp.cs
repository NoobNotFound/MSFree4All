using System.Xml.Serialization; 
namespace MSFree4All.Core.Office.XML{ 

[XmlRoot(ElementName="ExcludeApp")]
public class ExcludeApp { 

	[XmlAttribute(AttributeName="ID")] 
	public string ID { get; set; } 
}

}