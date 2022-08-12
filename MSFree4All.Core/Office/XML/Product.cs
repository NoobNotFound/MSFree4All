using System.Xml.Serialization; 
using System.Collections.Generic;
namespace MSFree4All.Core.Office.XML
{

	[XmlRoot(ElementName = "Product")]
	public class Product
	{

		[XmlElement(ElementName = "Language")]
		public List<Language> Language { get; set; }

		[XmlElement(ElementName = "ExcludeApp")]
		public List<ExcludeApp> ExcludeApp { get; set; }

		[XmlAttribute(AttributeName = "ID")]
		public string ID { get; set; }

		[XmlAttribute(AttributeName = "PIDKEY")]
		public string PIDKEY { get; set; }
	}

}