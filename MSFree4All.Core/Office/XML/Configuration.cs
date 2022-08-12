using System.Xml.Serialization; 
using System.Collections.Generic;
namespace MSFree4All.Core.Office.XML
{

	[XmlRoot(ElementName = "Configuration")]
	public class Configuration
	{

		[XmlElement(ElementName = "Info")]
		public Info Info { get; set; }

		[XmlElement(ElementName = "Add")]
		public Add Add { get; set; }

		[XmlElement(ElementName = "Display")]
		public Display Display { get; set; }

		[XmlElement(ElementName = "Property")]
		public List<Property> Property { get; set; }

		[XmlElement(ElementName = "Updates")]
		public Updates Updates { get; set; }

		[XmlElement(ElementName = "RemoveMSI")]
		public RemoveMSI RemoveMSI { get; set; }

		[XmlElement(ElementName = "AppSettings")]
		public AppSettings AppSettings { get; set; }

		public static Configuration CreateNew()
        {
            var c = new Configuration
            {
                Property = new(),
                Updates = new(),
                Add = new Add
                {
                    Product = new()
                },
                AppSettings = new(),
                Display = new()
            };
            c.AppSettings.Setup = new();
			c.Info = new();
            c.RemoveMSI = new RemoveMSI
            {
                IgnoreProduct = new()
            };
            return c;
		}
	}

}