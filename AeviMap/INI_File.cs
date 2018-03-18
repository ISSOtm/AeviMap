using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeviMap
{
    public class INI_File
    {
        /// <summary>
        /// Lists the accepted hexadecimal prefixes.
        /// </summary>
        // To anyone using any other prefix... why
        private static string[] hexPrefixes = {
            "0X",
            "$",
            "HEX:",
            "HEX::"
        };

        /// <summary>
        /// Lists the accepted property separators.
        /// </summary>
        private static char[] separators = { ':', '=' };


        private Dictionary<string, Property> properties = new Dictionary<string, Property>();
        private List<string> mapNames = new List<string>(new string[]
        {
            "Startham",
            "Debug Room",
            "Intro Map",
            "Startham Forest",
            "Player House 1F",
            "Player House 2F",
            "Startham Empty House",
            "Startham Large House"
        });
        private List<string> tilesetNames = new List<string>(new string[]
        {
            "Overworld",
            "Intro",
            "Interior",
            "Dark Interior",
            "Ruins",
            "Beach"
        });
        private Dictionary<string, SectionParser> parsers = new Dictionary<string, SectionParser>();


        /// <summary>
        /// Creates a new INI File object, initialized with default property values.
        /// </summary>
        public INI_File()
        {
            // Default properties
            properties.Add("nboftilesets", new Property(false, 6));
            properties.Add("nbofmaps", new Property(false, 8));
            properties.Add("nbofblocks", new Property(false, 64));
            properties.Add("mapptrsbank", new Property(false, 2));
            properties.Add("mapbanksptr", new Property(true, 0x4000));
            properties.Add("mapptrsptr", new Property(true, 0x4008));
            properties.Add("tilesetptrsbank", new Property(false, 2));
            properties.Add("tilesetbanksptr", new Property(true, 0x4300));
            properties.Add("tilesetptrsptr", new Property(true, 0x4306));
            properties.Add("palettesbank", new Property(false, 1));
            properties.Add("objpalette0ptr", new Property(true, 0x5168)); // Pointer to EvieDefaultPalette
            properties.Add("siblingpaletteptr", new Property(true, 0x517D)); // Pointer to TomDefaultPalette
            properties.Add("bgpalette0ptr", new Property(true, 0x5171));
            properties.Add("evietilesbank", new Property(false, 1)); // Full pointer to EvieTiles
            properties.Add("evietilesptr", new Property(true, 0x4670));
            properties.Add("sizeofblock", new Property(false, 16));


            // Register section parsers
            parsers.Add("properties", new PropertiesParser());
            parsers.Add("map names", new MapNamesParser());
            parsers.Add("tilesets", new TilesetParser());
        }


        /// <summary>
        /// Parses a INI file, and reads its contents.
        /// Note that all incorrect files are silently ignored.
        /// </summary>
        /// <param name="iniPath">The path to the file to be parsed.</param>
        public void ParseFile(string iniPath)
        {
            using (StreamReader iniFile = File.OpenText(iniPath))
            {
                // Read the info from the INI file
                string line;
                SectionParser curParser = null;
                while ((line = iniFile.ReadLine()) != null)
                {
                    // Ignore comment lines and empty lines
                    if (!line.StartsWith("#") && line.Length != 0)
                    {
                        // Check if line specifies a section type
                        if (line.StartsWith("["))
                        {
                            // Put to lower case to simplify checks
                            line = line.ToLower().Trim(new char[2] { '[', ']' });

                            curParser = this.parsers[line];
                            if (curParser != null)
                            {
                                curParser.BeginRead(this);
                            }
                        }
                        else // Parse line using the current section parser
                        {
                            // Ignore lines not in a section
                            if (curParser != null)
                            {
                                // Note : works using side-effects
                                curParser.ProcessLine(this, line);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Writes the current config to the specified INI file.
        /// </summary>
        /// <param name="iniPath">Path to the INI file to be written to.</param>
        public void WriteFile(string iniPath)
        {
            File.Delete(iniPath);
            using (StreamWriter iniFile = File.CreateText(iniPath))
            {
                foreach (var pair in this.parsers)
                {
                    // Write section name
                    iniFile.WriteLine("[" + pair.Key + "]");

                    List<string> lines = pair.Value.GetTextRepresentation(this);
                    foreach(string line in lines)
                    {
                        iniFile.WriteLine(line);
                    }

                    // Separate sections
                    iniFile.WriteLine();
                }
            }
        }


        /// <summary>
        /// Encodes one of the ROM's properties, such as base pointers.
        /// </summary>
        class Property
        {
            private bool isHex;
            private UInt16 value;

            /// <summary>
            /// Creates a new property with a default value.
            /// </summary>
            /// <param name="isPropertyHex">Whether the property should be parsed and output as hex.</param>
            /// <param name="defaultValue">The value the property will be set to by default.</param>
            public Property(bool isPropertyHex, UInt16 defaultValue)
            {
                this.isHex = isPropertyHex;
                this.value = defaultValue;
            }

            /// <summary>
            /// Gets the property's current value.
            /// </summary>
            public UInt16 GetValue()
            {
                return this.value;
            }

            /// <summary>
            /// Sets a new value for the property. Avoid using this after initial loading!
            /// </summary>
            /// <param name="value">The property's new value.</param>
            public void SetValue(UInt16 value)
            {
                this.value = value;
            }

            /// <summary>
            /// Tells whether the property is hex or not.
            /// </summary>
            /// <returns></returns>
            public bool IsHex()
            {
                return this.isHex;
            }
        }

        /// <summary>
        /// Gets a property from the INI file.
        /// </summary>
        /// <param name="key">The key of the demanded property.</param>
        /// <returns>The property, cast to a UInt16 (some properties should be valid to cast to byte).</returns>
        public UInt16 GetProperty(string key)
        {
            return properties[key].GetValue();
        }

        /// <summary>
        /// Sets a property from the INI file.
        /// </summary>
        /// <param name="key">The key of the demanded property.</param>
        /// <param name="value">The value to set the property to, cast to UInt16.</param>
        private void SetProperty(string key, UInt16 value)
        {
            properties[key].SetValue(value);
        }

        /// <summary>
        /// Tells if a property should be displayed and retrieved in hex format.
        /// </summary>
        /// <param name="key">The key of the demanded property.</param>
        /// <returns>True if the property is hex.</returns>
        private bool IsPropertyHex(string key)
        {
            return properties[key].IsHex();
        }

        /// <summary>
        /// Gets the name of each property in the INI file.
        /// </summary>
        /// <returns>The array of valid keys.</returns>
        private string[] GetPropertyNames()
        {
            return this.properties.Keys.ToArray();
        }

        /// <summary>
        /// Gets a property's value's display string.
        /// </summary>
        /// <param name="key">The key of the demanded property.</param>
        /// <returns>The value, either in decimal form or hex form (if appropriate).</returns>
        private string DisplayProperty(string key)
        {
            UInt16 property = this.GetProperty(key);

            if(this.IsPropertyHex(key))
            {
                return "0x" + MapEditor.decToHex(property);
            }
            else
            {
                return property.ToString();
            }
        }


        /// <summary>
        /// Gets a map's display name.
        /// </summary>
        /// <param name="mapID">The ID of the map.</param>
        /// <returns>The map's name, if defined. Otherwise, returns a placeholder name.</returns>
        public string GetMapName(byte mapID)
        {
            if(this.mapNames == null || mapID >= this.mapNames.Count || this.mapNames[mapID] == null)
            {
                return String.Format("??? (ID ${0})", MapEditor.decToHex(mapID, 2));
            }

            return this.mapNames[mapID];
        }

        /// <summary>
        /// Gets all defined map names.
        /// </summary>
        /// <returns>A List of all map names</returns>
        public List<string> GetMapNames()
        {
            return this.mapNames;
        }

        /// <summary>
        /// Sets a map's display name.
        /// </summary>
        /// <param name="mapID">The ID of the map to set the name of.</param>
        /// <param name="name">The name to give to the map.</param>
        private void SetMapName(byte mapID, string name)
        {
            this.mapNames[mapID] = name;
        }

        /// <summary>
        /// Adds a new map name at the end of the list.
        /// Please do not use unless reading from the INI file, or adding a new map.
        /// </summary>
        /// <param name="name">The name to be added.</param>
        private void AddMapName(string name)
        {
            this.mapNames.Add(name);
        }

        /// <summary>
        /// Remove all currently registered map names.
        /// </summary>
        private void ResetMapNames()
        {
            this.mapNames = new List<string>();
        }


        /// <summary>
        /// Gets a tileset's display name.
        /// </summary>
        /// <param name="tilesetID">The ID of the tileset.</param>
        /// <returns>The map's name, if defined. Otherwise, returns a placeholder name.</returns>
        public string GetTilesetName(byte tilesetID)
        {
            if (this.tilesetNames == null || tilesetID >= this.tilesetNames.Count || this.tilesetNames[tilesetID] == null)
            {
                return String.Format("??? (ID ${0})", MapEditor.decToHex(tilesetID, 2));
            }

            return this.mapNames[tilesetID];
        }

        /// <summary>
        /// Gets all defined tileset names.
        /// </summary>
        /// <returns>A List of all tileset names</returns>
        public List<string> GetTilesetNames()
        {
            return this.tilesetNames;
        }

        /// <summary>
        /// Sets a tileset's display name.
        /// </summary>
        /// <param name="tilesetID">The ID of the tileset to set the name of.</param>
        /// <param name="name">The name to give to the tileset.</param>
        private void SetTilesetName(byte tilesetID, string name)
        {
            this.tilesetNames[tilesetID] = name;
        }

        /// <summary>
        /// Adds a new tileset name at the end of the list.
        /// Please do not use unless reading from the INI file, or adding a new tileset.
        /// </summary>
        /// <param name="name">The name to be added.</param>
        private void AddTilesetName(string name)
        {
            this.tilesetNames.Add(name);
        }

        /// <summary>
        /// Remove all currently registered tileset names.
        /// </summary>
        private void ResetTilesetNames()
        {
            this.tilesetNames = new List<string>();
        }


        /// <summary>
        /// Defines a parser for one type of INI section.
        /// The parser is also responsible for generating the content of a new INI file.
        /// </summary>
        abstract class SectionParser
        {
            /// <summary>
            /// Initializes the parser.
            /// </summary>
            /// <param name="file">The object to init.</param>
            public abstract void BeginRead(INI_File file);

            /// <summary>
            /// Parses one line of the INI file.
            /// </summary>
            /// <param name="file">The object to modify.</param>
            /// <param name="line">The line to be parsed.</param>
            public abstract void ProcessLine(INI_File file, string line);


            /// <summary>
            /// Generates a List of lines corresponding to the parser's section of the INI file.
            /// </summary>
            /// <param name="file">The INI_File object to grab data from.</param>
            /// <returns>A List of lines to be written to the INI file. Section header not included.</returns>
            public abstract List<string> GetTextRepresentation(INI_File file);
        }

        class PropertiesParser : SectionParser
        {
            public override void BeginRead(INI_File file)
            {

            }

            public override void ProcessLine(INI_File file, string line)
            {
                string[] pieces = line.Split(INI_File.separators);
                if(pieces.Length == 2) // Silently ignore malformed lines
                {
                    string propName = pieces[0].Trim().ToLower();
                    if (file.properties.Keys.Contains(propName)) // Silently ignore unknown properties
                    {
                        UInt16 propValue;
                        bool isPropertyCorrect;
                        string rawPropValue = pieces[1].Trim().ToUpper();

                        if (!file.IsPropertyHex(propName))
                        {
                            isPropertyCorrect = UInt16.TryParse(rawPropValue, out propValue);
                        }
                        else
                        {
                            byte i = 0;
                            bool isPrefixValid = false;
                            // Attempt to remove a prefix
                            while (i < hexPrefixes.Length && !isPrefixValid)
                            {
                                if (rawPropValue.IndexOf(hexPrefixes[i]) == 0)
                                {
                                    rawPropValue = rawPropValue.Remove(0, hexPrefixes[i].Length);
                                    isPrefixValid = true;
                                }
                                i++;
                            }

                            // Also remove the suffix if there is one
                            isPropertyCorrect = MapEditor.hexToDec(rawPropValue.TrimEnd('H'), out propValue);
                        }

                        if (isPropertyCorrect) // Again, silently ignore malformed lines
                        {
                            file.SetProperty(propName, propValue);
                        }
                    }
                }
            }


            public override List<string> GetTextRepresentation(INI_File file)
            {
                List<string> propertyList = new List<string>();

                foreach(string propertyName in file.GetPropertyNames())
                {
                    propertyList.Add(String.Format(
                        "{0}{1} {2}", propertyName, INI_File.separators[0], file.DisplayProperty(propertyName)
                    ));
                }

                return propertyList;
            }
        }

        class MapNamesParser : SectionParser
        {
            public override void BeginRead(INI_File file)
            {
                file.ResetMapNames();
            }

            public override void ProcessLine(INI_File file, string line)
            {
                file.AddMapName(line);
            }


            public override List<string> GetTextRepresentation(INI_File file)
            {
                return file.GetMapNames();
            }
        }

        class TilesetParser : SectionParser
        {
            public override void BeginRead(INI_File file)
            {
                file.ResetTilesetNames();
            }

            public override void ProcessLine(INI_File file, string line)
            {
                file.AddTilesetName(line);
            }

            public override List<string> GetTextRepresentation(INI_File file)
            {
                return file.GetTilesetNames();
            }
        }
    }
}
