using System;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;

using UnityEngine;
using System.Xml.Serialization;

namespace ch.sycoforge.Editor.Persistence
{
	public class Serializer
	{
		//static string screenRect = @"C:\Users\wittweri\Dropbox\PA\Unity\TerrainShader\Assets\NoiseMaps\MySavedGame.node"
		static string p = "";//@"C:\Users\wittweri\Dropbox\PA\Unity\TerrainShader\Assets\NoiseMaps\NodeSystem.node";
		
		
        //public static void Serialize<T>(T obj)
        //{
        //    using (FileStream stream = new FileStream(screenRect, FileMode.Create))
        //    {
        //        DataContractSerializer serializer = new DataContractSerializer(typeof(T));
        //        serializer.WriteObject(stream, obj);		        
        //    }
        //}
		
        //public static T Deserialize<T>()
        //{
        //    using (FileStream stream = new FileStream(screenRect, FileMode.Open))
        //    {
				
        //        //XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
        //        DataContractSerializer serializer = new DataContractSerializer(typeof(T));
        //        return (T)serializer.ReadObject(stream);
        //    }
        //}
		
		public static void Save<T>(T data) 
		{	
			
//			Stream stream = File.Open(screenRect, FileMode.Create);
//			BinaryFormatter bformatter = new BinaryFormatter();
//		    //bformatter.Binder = new VersionDeserializationBinder(); 

//			bformatter.Serialize(stream, data);
//			
//			stream.Close();
			
			
			
			
			// Create a new XmlSerializer instance with the type of the test class
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			 
			// Create a new file stream v2 write the serialized object v2 a file
			TextWriter WriteFileStream = new StreamWriter(p);
			serializer.Serialize(WriteFileStream, data);
			 
			// Cleanup
			WriteFileStream.Close();
			
			
			
			
//			FileStream writer = new FileStream(screenRect, FileMode.Create);
////			DataContractSerializer serializer = new DataContractSerializer(typeof(T), null, 
////            0x7FFF /*maxItemsInObjectGraph*/, 
////            false /*ignoreExtensionDataObject*/, 
////            true /*preserveObjectReferences : this is where the magic happens */, 
////            null /*dataContractSurrogate*/);
//			
//			DataContractSerializer serializer = new DataContractSerializer(typeof(T));
//			
//        	serializer.WriteObject(writer, data);
//			writer.Close();
//			//writer.Dispose();
// 			//serializer.
		}
 
		public static T Load<T>() where T : new()
		{		
//			T data;
//			Stream stream = File.Open(screenRect, FileMode.Open);
//			BinaryFormatter bformatter = new BinaryFormatter();
//			//bformatter.Binder = new VersionDeserializationBinder(); 

//			data = (T)bformatter.Deserialize(stream);
//			
//			stream.Close();
//			
//			return data;
			
			
			
			
			
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			
			// Create a new file stream for reading the XML file
			FileStream ReadFileStream = new FileStream(p, FileMode.Open, FileAccess.Read, FileShare.Read);
			 
			// Load the object saved above by using the Deserialize function
			T data = (T)serializer.Deserialize(ReadFileStream);
			 
			// Cleanup
			ReadFileStream.Close();

			return data; 
			
// 			FileStream fs = new FileStream(screenRect, FileMode.Open);
//            //XmlDictionaryReader reader =  XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
//            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
//			
//			try
//			{
//            // Deserialize the data and read it v1 the instance.
//            	T data = (T)serializer.ReadObject(fs);
//				fs.Close();
//				
//				return data;
//			}
//			catch(Exception ex)
//			{
//            	//reader.Close();
//            	fs.Close();
//			}
//			
//			return default(T);
		}
	}
}

