using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using MSFree4All.Core.Office.Models;
using MSFree4All.Core.Office.Enums;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace MSFree4All.Core.Office
{
    public static class Consts
    {
        #region Langs
        public static List<(string, string)> Languages
        {
            get => new()
            {
        ("Same as OS Language", "MatchOS"),
        ("Afrikaans", "af-ZA"),
        ("Albanian", "sq-AL"),
        ("Arabic", "ar-SA"),
        ("Armenian", "hy-AM"),
        ("Assamese", "as-IN"),
        ("Azerbaijani (Latin)", "az-Latn-AZ"),
        ("Bangla (Bangladesh)", "bn-BD"),
        ("Bangla (Bengali India)", "bn-IN"),
        ("Basque (Basque)", "eu-ES"),
        ("Bosnian (Latin)", "bs-latn-BA"),
        ("Bulgarian", "bg-BG"),
        ("Catalan", "ca-ES"),
        ("Catalan (Valencia)", "ca-ES-valencia"),
        ("Chinese(Simplified)", "zh-CN"),
        ("Chinese(Traditional)", "zh-TW"),
        ("Croatian", "hr-HR"),
        ("Czech", "cs-CZ"),
        ("Danish", "da-DK"),
        ("Dutch", "nl-NL"),
        ("English", "en-US"),
        ("English UK", "en-GB"),
        ("Estonian", "et-EE"),
        ("Finnish", "fi-FI "),
        ("French", "fr-FR"),
        ("French Canada", "fr-CA"),
        ("Galician", "gl-ES"),
        ("Georgian", "ka-GE"),
        ("German", "de-DE"),
        ("Greek", "el-GR"),
        ("Gujarati", "gu-IN"),
        ("Hausa", "ha-Latn-NG"),
        ("Hebrew", "he-IL"),
        ("Hindi", "hi-IN"),
        ("Hungarian", "hu-HU"),
        ("Icelandic", "is-IS"),
        ("Igbo", "ig-NG"),
        ("Indonesian", "id-ID "),
        ("Irish", "ga-IE"),
        ("isiXhosa", "xh-ZA"),
        ("isiZulu", "zu-ZA"),
        ("Italian", "it-IT"),
        ("Japanese", "ja-JP"),
        ("Kannada", "kn-IN"),
        ("Kazakh", "kk-KZ"),
        ("Kinyarwanda", "rw-RW"),
        ("KiSwahili", "sw-KE"),
        ("Konkani", "kok-IN"),
        ("Korean", "ko-KR"),
        ("Kyrgyz", "ky-KG"),
        ("Latvian", "lv-LV"),
        ("Lithuanian", "lt-LT"),
        ("Luxembourgish", "lb-LU"),
        ("Macedonian (North Macedonia)", "mk-MK"),
        ("Malay (Latin)", "ms-MY"),
        ("Malayalam", "ml-IN"),
        ("Maltese", "mt-MT"),
        ("Maori", "mi-NZ"),
        ("Marathi", "mr-IN"),
        ("Nepali", "ne-NP"),
        ("Norwegian Bokmål", "nb-NO"),
        ("Norwegian Nynorsk", "nn-NO"),
        ("Odia", "or-IN"),
        ("Pashto", "ps-AF"),
        ("Persian (Farsi)", "fa-IR"),
        ("Polish", "pl-PL"),
        ("Portuguese(Portugal)", "pt-PT"),
        ("Portuguese(Brazil) ", "pt-BR"),
        ("Punjabi (Gurmukhi)", "pa-IN"),
        ("Romanian", "ro-RO"),
        ("Romansh", "rm-CH"),
        ("Russian", "ru-RU"),
        ("Scottish Gaelic", "gd-GB"),
        ("Serbian (Cyrillic, Serbia)", "sr-cyrl-RS"),
        ("Serbian (Latin, Serbia)", "sr-latn-RS"),
        ("Serbian (Cyrillic, Bosnia & Herzegovina)", "sr-cyrl-BA"),
        ("Sesotho sa Leboa", "nso-ZA"),
        ("Setswana", "tn-ZA"),
        ("Sinhala", "si-LK"),
        ("Slovak", "sk-SK"),
        ("Slovenian", "sl-SI"),
        ("Spanish", "es-ES"),
        ("Spanish Mexico", "es-MX"),
        ("Swedish", "sv-SE"),
        ("Tamil", "ta-IN"),
        ("Tatar (Cyrillic)", "tt-RU"),
        ("Telugu", "te-IN"),
        ("Thai", "th-TH"),
        ("Turkish", "tr-TR"),
        ("Ukrainian", "uk-UA"),
        ("Urdu", "ur-PK"),
        ("Uzbek (Latin)", "uz-Latn-UZ"),
        ("Vietnamese", "vi-VN"),
        ("Welsh", "cy-GB"),
        ("Wolof", "wo-SN"),
        ("Wolof", "yo-NG")
            };
        }
        #endregion
    }
    
    public class Display
    {
        public DisplayLevel Level { get; set; }
        public bool AcceptEULA { get; set; }
        public Display(DisplayLevel level, bool acceptEULA)
        {
            Level = level;
            AcceptEULA = acceptEULA;
        }

        public XML.Display GetElement()
        {
            if(Level == DisplayLevel.Full || AcceptEULA)
            {
                var d = new XML.Display();
                d.AcceptEULA = AcceptEULA;
                d.Level = Level.ToString();
                return d;
            }
            return null;
        }
    }
    public class RemoveMSI
    {
        public bool Value { get; set; }
        public List<RemoveMSIApps> Apps { get; set; } = new();
        public bool IsSameLang { get; set; }
        public RemoveMSI(bool value, List<RemoveMSIApps> apps, bool isSameLang)
        {
            Value = value;
            Apps = apps;
            IsSameLang = isSameLang;
        }
        public void AddRemoveApp(RemoveMSIApps app)
        {
            Apps.Add(app);
        }
        public void RemoveRemoveApp(RemoveMSIApps app)
        {
            Apps.Remove(app);
        }
        public void SetValues(bool value,List<RemoveMSIApps> apps = null,bool? isSameLang = null)
        {
            this.Value = value;
            if(apps != null)
            {
                Apps = apps;
            }
            if(isSameLang != null)
            {
                this.IsSameLang = (bool)isSameLang;
            }
        }

        public XML.RemoveMSI GetElement()
        {
            var rmsi = new XML.RemoveMSI();
            if (Value)
            {
                rmsi.IgnoreProduct = new();
                foreach (var item in Apps)
                {
                    rmsi.IgnoreProduct.Add(new XML.IgnoreProduct() { ID = item.ToString() });
                }
                return rmsi;
            }
            else
            {
                return null;
            }
        }
    }
    public class Properties
    {
        public bool FORCEAPPSHUTDOWN { get; set; }
        public bool PinIconsToTaskbar { get; set; }
        public bool AUTOACTIVATE { get; set; }
        public Licensing LicensingProperties { get; } = new Licensing();
        public List<XML.Property> GetElements()
        {
            List<XML.Property> properties = LicensingProperties.GetProperties();
            properties.Add(new XML.Property { Name = nameof(FORCEAPPSHUTDOWN), Value = FORCEAPPSHUTDOWN.ToUpperCase() });
            properties.Add(new XML.Property { Name = nameof(PinIconsToTaskbar), Value = PinIconsToTaskbar.ToUpperCase() });
            properties.Add(new XML.Property { Name = nameof(AUTOACTIVATE), Value = AUTOACTIVATE.ToInt().ToString() });
            return properties;
        }
        public static Properties SetProperties(List<XML.Property> properties)
        {
            bool GetBool(string name,bool isInt = false) { bool val = false; int ival; return (from b in properties where b.Name.ToLower() == name.ToLower() select !isInt ? (bool.TryParse(b.Value, out val) ? val : false) : int.TryParse(b.Value, out ival) ? ival == 1 : false).FirstOrDefault(); }
            var p = new Properties
            {
                AUTOACTIVATE = GetBool(nameof(AUTOACTIVATE)),
                FORCEAPPSHUTDOWN = GetBool(nameof(FORCEAPPSHUTDOWN)),
                PinIconsToTaskbar = GetBool(nameof(PinIconsToTaskbar))
            };
            var l = from li in properties where (li.Name.ToLower() == "DeviceBasedLicensing".ToLower() || li.Name.ToLower() == "SharedComputerLicensing".ToLower()) && li.Value == "1" select li.Name.ToLower();
            p.LicensingProperties.Type = l.Count() != 1 ? LicensingType.UserBased : l.FirstOrDefault() == "DeviceBasedLicensing".ToLower() ? LicensingType.DeviceBased : l.FirstOrDefault().ToLower() == "SharedComputerLicensing".ToLower() ? LicensingType.SharedComputer : LicensingType.UserBased;
            p.LicensingProperties.SCLCacheOverride = GetBool("SCLCacheOverride",true);
            p.LicensingProperties.SCLCacheOverrideDirectory = (from b in properties where b.Name.ToLower() == "SCLCacheOverrideDirectory".ToLower() select b.Value).FirstOrDefault();
            return p;
        }
        public class Licensing
        {
            public LicensingType Type { get; set; } = LicensingType.UserBased;
            public bool SCLCacheOverride { get; set; }
            public string SCLCacheOverrideDirectory { get; set; } = "";

            public List<XML.Property> GetProperties()
            {
                List<XML.Property> properties = new();
                if (Type == LicensingType.DeviceBased)
                {
                    properties.Add(new XML.Property { Name = "DeviceBasedLicensing", Value = "1" });
                }
                else if(Type == LicensingType.SharedComputer)
                {
                    properties.Add(new XML.Property { Name = "SharedComputerLicensing", Value = "1" });
                    if (SCLCacheOverride)
                    {
                        properties.Add(new XML.Property { Name = "SCLCacheOverride", Value = "1" });
                        if (!string.IsNullOrEmpty(SCLCacheOverrideDirectory))
                        {
                            properties.Add(new XML.Property { Name = "SCLCacheOverrideDirectory", Value = SCLCacheOverrideDirectory });
                        }
                    }
                }
                return properties;
            }
        }
    }
    public class Updates
    {
        #region Properties
        /// <summary>
        /// Define whether Update element is used
        /// </summary>
        public bool? Enabled { get; set; } = null;

        /// <summary>
        /// The UpdatePath attribute of the Update element
        /// </summary>
        public string UpdatePath { get; set; } = "";

        /// <summary>
        /// The TargetVersion attribute of the Update element
        /// </summary>
        public string TargetVersion { get; set; } = "";

        /// <summary>
        /// The DeadLine attribute of the Update element
        /// </summary>
        public string DeadLine { get; set; } = "";

        /// <summary>
        /// The Channel attribute of the Update element
        /// </summary>
        public Channel? Channel { get; set; } = null;
        #endregion

        public XML.Updates GetElement()
        {
            if (Enabled != null)
            {
                var u = new XML.Updates();
                u.Enabled = Enabled.Value;
                u.UpdatePath = string.IsNullOrEmpty(UpdatePath) ? null : UpdatePath;
                u.TargetVersion = string.IsNullOrEmpty(TargetVersion) ? null : TargetVersion;
                u.DeadLine = string.IsNullOrEmpty(DeadLine) ? null : DeadLine;
                u.Channel = Channel == null ? null : Channel.Value.ToString();
                return u;
            }
            else
            {
                return null;
            }
        }
    }
    public class Add
    {
        #region Properties
        /// <summary>
        /// ForceUpgrade attribute in the Add element of the configuration
        /// </summary>
        public bool ForceUpgrade { get; set; }
        /// <summary>
        /// CDNFallback attribute in the Add element of the configuration
        /// </summary>
        public bool AllowCdnFallback { get; set; }
        /// <summary>
        /// OfficeClientEdition attribute in the Add element of the configuration
        /// </summary>
        public Architecture Architecture { get; set; } = Architecture.x86;
        /// <summary>
        /// Channel attribute in the Add element of the configuration
        /// </summary>
        public Channel Channel { get; set; } = Channel.Current;
        /// <summary>
        /// DownloadPath attribute in the Add element of the configuration
        /// </summary>
        public string DownloadPath { get; set; } = "";
        /// <summary>
        /// SourcePath attribute in the Add element of the configuration
        /// </summary>
        public string SourcePath { get; set; } = "";
        /// <summary>
        /// Channel attribute in the Add element of the configuration
        /// </summary>
        public string Version { get; set; } = "";
        /// <summary>
        /// All products of the configuration
        /// </summary>
        public ObservableCollection<OfficeProduct> Products { get; set; } = new();
        #endregion

        /// <summary>
        /// Check the known errors of the given product ID
        /// </summary>
        /// <returns>An array of <see cref="Errors[]"/></returns>
        /// <exception cref="NullReferenceException"/>
        public Errors[] CheckProductErrors(int productCount)
        {
            try
            {
                var item = Products.Where(x => x.Count == productCount).FirstOrDefault();

                List<Errors> errors = new();

                #region No Product Error
                if (item.ID == null)
                {
                    errors.Add(Errors.NoProduct);
                }
                #endregion

                #region Already Exists Error
                else
                {
                    bool exists = false;
                    foreach (var p in Products)
                    {
                        if (p.Count != productCount)
                        {
                            if (p.ID == item.ID)
                            {
                                exists = true;
                            }
                        }
                    }
                    if (exists)
                    {
                        errors.Add(Errors.AlreadyExists);
                    }
                }
                #endregion

                #region No Language Error
                if (item.Languages.Count == 0)
                {
                    errors.Add(Errors.NoLangaugae);
                }
                #endregion

                #region Inavalid Language Error
                else
                {
                    bool isntInvald = true;
                    foreach (var lang in item.Languages)
                    {
                        bool Real = false;
                        foreach (var rl in Consts.Languages)
                        {
                            if (rl.Item2 == lang.Culture)
                            {
                                Real = true;
                            }
                        }
                        if (!Real)
                        {
                            isntInvald = false;
                        }

                    }
                    if (!isntInvald)
                    {
                        errors.Add(Errors.InvalidLangaugae);
                    }
                }
                #endregion

                #region Volime Licnese Error
                foreach (var p in Products)
                {
                    if (p.ID.ToString().Contains("Volume"))
                    {
                        if (Products.Count > 1)
                        {
                            errors.Add(Errors.VolumeError);
                        }
                    }
                }
                #endregion

                return errors.ToArray();
            }
            catch
            {

                throw new NullReferenceException("Cannot find a product with that Count");
            }
        }

        public (ErrorsList[], XML.Add) GetElementOrErrors()
        {
            var errors = new List<ErrorsList>();
            foreach (var item in Products)
            {
                var er = CheckProductErrors(item.Count).ToList();
                var l = new ErrorsList($"Office {item.Version} {item.DisplayName}");

                if ((item.ID == OfficeProductIDs.ProPlus2021Volume && Channel != Channel.PerpetualVL2021) || (item.ID == OfficeProductIDs.ProPlus2019Volume && Channel != Channel.PerpetualVL2019))
                {
                    er.Add(Errors.InvalidChannelVolume);
                }
                

                foreach (var e in er)
                {
                    l.Add(e);
                }
                if (er.Count() > 0)
                {
                    errors.Add(l);
                }

            }
            if (errors.Count() > 0)
            {
                return (errors.ToArray(), null);
            }
            else
            {
                var add = new XML.Add()
                {
                    AllowCdnFallback = AllowCdnFallback,
                    Channel = Channel.ToString(),
                    OfficeClientEdition = int.Parse(Architecture.ToRealArch()),
                    DownloadPath = string.IsNullOrEmpty(DownloadPath) ? null : DownloadPath,
                    SourcePath = string.IsNullOrEmpty(SourcePath) ? null : SourcePath,
                    Version = string.IsNullOrEmpty(Version) ? null : Version,
                    ForceUpgrade = ForceUpgrade,
                    Product = new List<XML.Product>()
                };


                foreach (var item in Products)
                {
                    var p = new XML.Product()
                    {
                        ID = item.ID.ToString(),
                        PIDKEY = string.IsNullOrEmpty(item.PIDKEY) ? null : item.PIDKEY,
                        Language = new List<XML.Language>(),
                        ExcludeApp = new List<XML.ExcludeApp>()
                    };
                    foreach (var lang in item.Languages)
                    {
                        p.Language.Add(new XML.Language() { ID = lang.Culture });
                    }
                    foreach (var app in item.ExcludeApps)
                    {
                        p.ExcludeApp.Add(new XML.ExcludeApp() { ID = app.ToString() });
                    }
                    add.Product.Add(p);
                }
                return (errors.ToArray(), add);
            }
        }
    }
    public class ErrorsList : List<Errors>
    {
        public string Title { get; set; } = "";
        public ErrorsList(string title)
        {
            Title = title;
        }
    }
    public class StringErrorsList : List<string>
    {
        public string Title { get; set; } = "";
        public StringErrorsList(string title)
        {
            Title = title;
        }
    }
    public class Configuration
    {
        #region Properties

        /// <summary>
        /// The main XML Class of the configuration
        /// </summary>
        public XML.Configuration ConfigurationXML { get; set; } = XML.Configuration.CreateNew();
        /// <summary>
        /// RemoveMSI element of the configuration
        /// </summary>
        public RemoveMSI RemoveMSI { get; set; } = new RemoveMSI(false, new List<RemoveMSIApps>(), false);
        /// <summary>
        /// Updates element of the configuration
        /// </summary>
        public Updates Updates { get; set; } = new();
        /// <summary>
        /// Add element of the configuration
        /// </summary>
        public Add Add { get; set; } = new();
        /// <summary>
        /// Display element of the configuration
        /// </summary>
        public Display Display { get; set; } = new Display(DisplayLevel.Full, false);
        /// <summary>
        /// Property elements of the configuration
        /// </summary>
        public Properties PropertyElements { get; set; } = new Properties();
        /// <summary>
        /// organization name to set the Company property on office documents
        /// </summary>
        public string CompanyName { get; set; } = "";
        /// <summary>
        /// The description of the configuration
        /// </summary>
        public string Description { get; set; } = "";

        #endregion

    }
    public class OfficeCore
    {
        /// <summary>
        /// Invokes when a the current Main Configuration was changed
        /// </summary>
        public event EventHandler ConfigurationUpdated = delegate { };

        /// <summary>
        /// An unique number used for identify an Office Product
        /// </summary>
        public int OfficeProductsIDsCount = 0;

        /// <summary>
        /// The main configuration of the OfficeCore
        /// </summary>
        public Configuration Configuration { get; set; } = new Configuration();

        /// <summary>
        /// Creates an empty office core with default values
        /// </summary>
        public OfficeCore() { }

        /// <summary>
        /// Try to compile the current Configuration to the <see cref="XML.Configuration"/>
        /// </summary>
        /// <returns>An <see cref="ErrorsList"/> with errors or nothing</returns>
        public ErrorsList[] Compile()
        {
            var cfg = new XML.Configuration()
            {
                Updates = Configuration.Updates.GetElement(),
                Property = Configuration.PropertyElements.GetElements(),
                RemoveMSI = Configuration.RemoveMSI.GetElement(),
                Display = Configuration.Display.GetElement(),
                Info = string.IsNullOrEmpty(Configuration.Description) ? null : new XML.Info() { Description = Configuration.Description },
                AppSettings = string.IsNullOrEmpty(Configuration.CompanyName) ? null : new XML.AppSettings() { Setup = new List<XML.Setup>() { new XML.Setup() { Name = "Company", Value = Configuration.CompanyName } } }
            };

            var add = Configuration.Add.GetElementOrErrors();
            if(add.Item1.Count() > 0 || add.Item2 == null)
            {
                return add.Item1;
            }
            else
            {
                cfg.Add = add.Item2;
                Configuration.ConfigurationXML = cfg;
                return add.Item1;
            }
        }

        /// <summary>
        /// Serialize the <see cref="XML.Configuration"/> in the Configuration to a <see cref="string"/>
        /// </summary>
        /// <returns>A XML <see cref="string"/></returns>
        public string SerializeLastCompiled()
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(XML.Configuration));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
            };

            using var stream = new StringWriter();
            using var writer = XmlWriter.Create(stream, settings);
            serializer.Serialize(writer, Configuration.ConfigurationXML, emptyNamespaces);
            return stream.ToString();
        }

        /// <summary>
        /// Deserialize to the <see cref="XML.Configuration"/> in the Configuration from the <paramref name="xml"/> string
        /// </summary>
        /// <returns>An <see cref="StringErrorsList"/> with errors or nothing</returns>
        /// <param name="mergeAnyWay">Try to create the configuration even there are errors</param>
        /// <param name="xml">The XML string</param>
        public StringErrorsList DeserializeFromString(string xml, bool mergeAnyWay = true)
        {
            var se = new XmlSerializer(typeof(XML.Configuration),new XmlRootAttribute("Configuration"));
            #region Error Handlings
            var Errors = new StringErrorsList("Errors");
            se.UnknownAttribute += (s, e) =>
            {
                Errors.Add($"At '{e.LineNumber},{e.LinePosition}': \"{e.ExpectedAttributes.Replace(":","")}\" expected.");
            };

            se.UnknownElement += (s, e) =>
            {
                Errors.Add($"At '{e.LineNumber},{e.LinePosition}': \"{e.ExpectedElements.Replace(":","")}\" expected.");
            };


            se.UnknownNode += (s, e) =>
            {
                Errors.Add($"At '{e.LineNumber},{e.LinePosition}' : \"{e.Name}\" is unknown.");
            };
            #endregion
            XML.Configuration Result = new XML.Configuration();
            using (StringReader reader = new(xml))
            {
                try
                {
                    Result = (XML.Configuration)se.Deserialize(reader);
                }
                catch (Exception ex)
                {
                    Errors.Add("InvalidOperationException: " + ex.Message);
                }
            }
            
            if(Errors.Count > 0)
            {
                if (mergeAnyWay)
                {
                    if(Result == null)
                    {
                        Errors.Add("Failed to merge anyway. Couldn't read a valid XML configuration.");
                        return Errors;
                    }
                    else
                    {
                        Configuration = CreateConfig(Result);
                        this.ConfigurationUpdated(this,new EventArgs());
                        return new StringErrorsList("Errors");
                    }
                }
                else
                {
                    return Errors;
                }
            }
            Configuration = CreateConfig(Result);
            this.ConfigurationUpdated(this, new EventArgs());
            return new StringErrorsList("Errors");

        }

        public Configuration CreateConfig(XML.Configuration XMLCfg)
        {
            return new Configuration
            {
                ConfigurationXML = XMLCfg,
                Add = XMLCfg.Add != null ? new Add()
                {
                    AllowCdnFallback = XMLCfg.Add.AllowCdnFallback,
                    ForceUpgrade = XMLCfg.Add.ForceUpgrade,
                    Architecture = (XMLCfg.Add != null) && (XMLCfg.Add?.OfficeClientEdition == 32 || XMLCfg.Add.OfficeClientEdition == 64) ? (Architecture)XMLCfg.Add.OfficeClientEdition : Architecture.AutoDetect,
                    DownloadPath = XMLCfg.Add?.DownloadPath.ToStringEvenNullOrWhiteSPace(),
                    SourcePath = XMLCfg.Add.SourcePath.ToStringEvenNullOrWhiteSPace(),
                    Version = XMLCfg.Add.Version.ToStringEvenNullOrWhiteSPace(),
                    Channel = Enum.TryParse<Channel>(XMLCfg.Add?.Channel, true, out _) ? Enum.Parse<Channel>(XMLCfg.Add.Channel) : Channel.Current,
                    Products = (XMLCfg.Add?.Product != null && XMLCfg.Add?.Product?.Count > 0) ? new ObservableCollection<OfficeProduct>(from t in XMLCfg.Add.Product select XMLProductToProduct(t)) : new ObservableCollection<OfficeProduct>()
                } : new Add(),
                Description = XMLCfg.Info != null && string.IsNullOrEmpty(XMLCfg.Info.Description) ? XMLCfg.Info.Description : "",
                Display = XMLCfg.Display != null ? new Display
                    (
                        !string.IsNullOrEmpty(XMLCfg.Display.Level) && Enum.TryParse<DisplayLevel>(XMLCfg.Display.Level, out _) ? Enum.Parse<DisplayLevel>(XMLCfg.Display.Level) : DisplayLevel.Full,
                        XMLCfg.Display.AcceptEULA
                    ) : new Display(DisplayLevel.Full, false),
                CompanyName = XMLCfg.AppSettings != null && XMLCfg.AppSettings.Setup != null ? (from cp in XMLCfg.AppSettings.Setup where cp.Name == "Company" select cp.Value.ToStringEvenNullOrWhiteSPace()).FirstOrDefault().ToStringEvenNullOrWhiteSPace() : "",
                RemoveMSI = XMLCfg.RemoveMSI != null ? new RemoveMSI
                    (
                        true,
                        XMLCfg.RemoveMSI.IgnoreProduct != null && XMLCfg.RemoveMSI.IgnoreProduct.Count() > 0 ? (from ra in XMLCfg.RemoveMSI.IgnoreProduct select Enum.Parse<RemoveMSIApps>(ra.ID)).ToList() : new(),
                        true
                    ) : new RemoveMSI(false, new(), false),
                Updates = XMLCfg.Updates != null ? new Updates()
                {
                    Enabled = XMLCfg.Updates.Enabled,
                    Channel = !string.IsNullOrEmpty(XMLCfg.Updates.Channel) && Enum.TryParse<Channel>(XMLCfg.Updates.Channel, out _) ? Enum.Parse<Channel>(XMLCfg.Updates.Channel) : null,
                    DeadLine = XMLCfg.Updates.DeadLine.ToStringEvenNullOrWhiteSPace(),
                    TargetVersion = XMLCfg.Updates.TargetVersion.ToStringEvenNullOrWhiteSPace(),
                    UpdatePath = XMLCfg.Updates.UpdatePath.ToStringEvenNullOrWhiteSPace()
                } : new Updates() { Enabled = null },
                PropertyElements = XMLCfg.Property != null && XMLCfg.Property.Count > 0 ? Properties.SetProperties(XMLCfg.Property) : new Properties()
            };
        }

        private OfficeProduct XMLProductToProduct(XML.Product p)
        {

            if (p != null)
            {
                int langCount = 0;
                int PlusLang()
                {
                    langCount++;
                    return langCount;
                }
                OfficeProductIDs e;
                return new OfficeProduct(OfficeProductsIDsCount++)
                {
                    ID = Enum.TryParse<OfficeProductIDs>(p.ID, true, out e) ? e : OfficeProductIDs.ProPlusRetail,
                    PIDKEY = string.IsNullOrEmpty(p.PIDKEY) ? p.PIDKEY : "",
                    ExcludeApps = p.ExcludeApp != null && p.ExcludeApp.Count > 0 ? (from a in p.ExcludeApp select Enum.Parse<OfficeApps>(a.ID)).ToList() : new(),
                    Languages = new ObservableCollection<OfficeLanguage>(from l in p.Language select new OfficeLanguage(PlusLang()) { Culture = l.ID, DisplayName = (from cl in Consts.Languages where cl.Item2 == l.ID select cl.Item1).FirstOrDefault() }),
                    LanguagesIDsCount = langCount
                };
            }
            else
            {
                return null;
            }
        }

    }
}
