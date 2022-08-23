using System.Xml.Serialization; 
using System.Collections.Generic; 
namespace MSFree4All.Core.Office.XML{ 

[XmlRoot(ElementName="RemoveMSI")]
public class RemoveMSI { 

	[XmlElement(Namespace = "",ElementName="IgnoreProduct")] 
	public List<IgnoreProduct> IgnoreProduct { get; set; } 
}

}