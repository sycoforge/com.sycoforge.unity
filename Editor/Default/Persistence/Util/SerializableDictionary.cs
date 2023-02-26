using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ch.sycoforge.Unity.Editor.Persistence.Util
{ 
    [XmlRoot("dictionary")]
    public class SerializableDictionary<K, V> : Dictionary<K, V>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
 
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(K));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(V));
 
            bool isEmpty = reader.IsEmptyElement;
            reader.Read();
 
            if (isEmpty)
			{
                return;
			}
 
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
 
                reader.ReadStartElement("key");
                K key = (K)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
 
                reader.ReadStartElement("value");
                V value = (V)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
 
                this.Add(key, value);
 
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }
 
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(K));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(V));
 
            foreach (K key in this.Keys)
            {
                writer.WriteStartElement("item");
 
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
 
                writer.WriteStartElement("value");
                V value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
 
                writer.WriteEndElement();
            }
        }
        #endregion
    }
}

