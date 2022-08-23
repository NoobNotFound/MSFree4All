using System.Xml.Serialization; 
using System.Collections.Generic;
namespace MSFree4All.Core.Office.XML
{

	[XmlRoot(Namespace = "", ElementName = "Configuration")]
	public class Configuration
	{

		[XmlElement(Namespace = "",ElementName = "Info")]
		public Info Info { get; set; }

		[XmlElement(Namespace = "",ElementName = "Add")]
		public Add Add { get; set; }

		[XmlElement(Namespace = "",ElementName = "Display")]
		public Display Display { get; set; }

		[XmlElement(Namespace = "",ElementName = "Property")]
		public List<Property> Property { get; set; }

		[XmlElement(Namespace = "",ElementName = "Updates")]
		public Updates Updates { get; set; }

		[XmlElement(Namespace = "",ElementName = "RemoveMSI")]
		public RemoveMSI RemoveMSI { get; set; }

		[XmlElement(Namespace = "",ElementName = "AppSettings")]
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