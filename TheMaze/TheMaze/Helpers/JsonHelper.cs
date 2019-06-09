using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace TheMaze
{
    class JsonHelper<T>
    {
        //public Type Type { get; set; }

        public static T Read(string i_path)
        {
            using (FileStream stream = new FileStream(i_path, FileMode.Open))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

                return (T)ser.ReadObject(stream);
            }
        }

        public static void Write(T i_entity, string i_path)
        {
            using (FileStream stream = new FileStream(i_path, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(stream, i_entity);
            }
        }
    }
}
