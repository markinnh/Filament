using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DataDefinitions.Interfaces;

namespace DataDefinitions.JsonSupport
{
    /// <summary>
    /// CounterProvider for classes that need key references, mainly filaments which are reused and PrintSettings which are usually specific to a vendor and filament type
    /// </summary>
    /// <remarks>
    /// This branch is mainly for JSON so the referential integrity requirements will be relax to only as necessary
    /// </remarks>
    public class CounterProvider : Dictionary<string, int>, ITrackModified,IXmlSerializable
    {
        const int startIndex = 1;
        public bool InDataOperations=>false;
        public bool IsModified { get;  set; }
        public void SuccessfullySaved()
        {
            IsModified = false;
        }
        /// <summary>
        /// Returns the next sequential number for the type of objects that are passed.  Does not reuse ids, once the number is provided it will not occur again
        /// </summary>
        /// <param name="obj">object to provide id for</param>
        /// <returns>a sequential integer for the type</returns>
        public int NextID(object obj,int StartIndex=startIndex)
        {
            System.Diagnostics.Debug.Assert(obj is Type, "You are supposed to pass an object, not the object type to this method");
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), $"cannot process a null object in the NextID function");
            
            var type = obj.GetType();
            
            if (ContainsKey(type.FullName))
            {
                //++this[type.FullName];
                //this[type.FullName] = result;
                IsModified = true;
                return ++this[type.FullName];
            }
            else
            {

                Add(type.FullName, StartIndex);
                IsModified = true;
                return this[type.FullName];
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement) { return; }

            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                object key = reader.GetAttribute("Key");
                object value = reader.GetAttribute("Value");
                this.Add((string)key, (int)value);
                reader.Read();
            }

            //throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var key in this.Keys)
            {
                writer.WriteStartElement("Counter");
                writer.WriteAttributeString("Key", key.ToString());
                writer.WriteAttributeString("Value", this[key].ToString());
                writer.WriteEndElement();
            }
            //throw new NotImplementedException();
        }
    }
}
