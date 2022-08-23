using System.Collections.Generic;
using System.Xml.Serialization; 
namespace MSFree4All.Core.Office.XML{ 

[XmlRoot(ElementName="AppSettings")]
public class AppSettings { 

	[XmlElement(Namespace = "",ElementName="Setup")] 
	public List<Setup> Setup { get; set; } 
}

}