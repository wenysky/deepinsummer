using System;
using System.IO;
using System.Xml.Serialization;

namespace Natsuhime.Common
{
    public class SerializationHelper
    {
        /// <summary>
        /// XML 序列化
        /// </summary>
        /// <param name="o"></param>
        /// <param name="t"></param>
        /// <param name="fullXmlPath"></param>
        public static void XmlSerialize(object o, string fullXmlPath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fullXmlPath, FileMode.OpenOrCreate);
                XmlSerializer xs = new XmlSerializer(o.GetType());
                xs.Serialize(fs, o);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fullXmlPath"></param>
        /// <returns></returns>
        public static object XmlDeSerialize(Type t, string fullXmlPath)
        {
            FileStream fs = null;
            object o = null;

            try
            {
                fs = new FileStream(fullXmlPath, FileMode.Open);
                o = XmlDeSerialize(t, fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return o;
        }
        /// <summary>
        /// XML反序列化(重载读入流,记得手动关闭流)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object XmlDeSerialize(Type t, Stream s)
        {
            XmlSerializer xs = new XmlSerializer(t);
            return xs.Deserialize(s);
        }
    }
}
