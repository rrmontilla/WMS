using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Uploading.Connection
{
    public class Converter
    {
        public string ToXML(DataTable dt)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                using (MemoryStream memorystream = new MemoryStream())
                {
                    using (TextWriter steamwriter = new StreamWriter(memorystream))
                    {
                        XmlSerializer xmlserializer = new XmlSerializer(typeof(DataSet));
                        xmlserializer.Serialize(steamwriter, ds);
                        string xml = Encoding.UTF8.GetString(memorystream.ToArray());
                        return xml;
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
                throw new Exception(ex.Message);
            }
        }

        public DataTable ToDataTable(string xml)
        {
            DataSet ds = new DataSet();
            DataTable dt;

            StringReader strReader = new StringReader(xml);
            ds.ReadXml(strReader);
            dt = ds.Tables[0];

            return dt;
        }

        public DataSet ToDataSet(string xml)
        {
            DataSet ds = new DataSet();
            DataTable dt;

            StringReader strReader = new StringReader(xml);
            ds.ReadXml(strReader);
            //d//s.Tables.Add(dt

            return ds;
        }
    }
}
