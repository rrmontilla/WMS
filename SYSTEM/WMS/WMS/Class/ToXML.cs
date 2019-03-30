using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WMS.Class
{
    public class ToXML
    {
        public static string Toxml(DataTable dt)
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

        public static DataTable ToDataTable(string xml)
        {
            DataSet ds = new DataSet();
            DataTable dt;

            StringReader strReader = new StringReader(xml);
            ds.ReadXml(strReader);
            dt = ds.Tables[0];

            return dt;
        }

        public static DataSet ToDataSet(string xml)
        {
            DataSet ds = new DataSet();
            DataTable dt;

            StringReader strReader = new StringReader(xml);
            ds.ReadXml(strReader);

            return ds;
        }
    }
}
