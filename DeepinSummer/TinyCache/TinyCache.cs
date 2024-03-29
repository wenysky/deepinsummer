using System;
using System.Web.Caching;

namespace Natsuhime
{
    public class TinyCache
    {
        private static System.Web.Caching.Cache webcache = System.Web.HttpRuntime.Cache;

        private bool _IsWriteLogs = true;
        /// <summary>
        /// 是否记录缓存到日志文件(默认是)
        /// </summary>
        public bool IsWriteLogs
        {
            get { return _IsWriteLogs; }
            set { _IsWriteLogs = value; }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="CacheName">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="TimeOut">过期时间(0表示无限期)</param>
        public void AddObject(string CacheName, object o, int TimeOut)
        {

            if (CacheName == null || CacheName.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(CacheOnRemove);

            if (TimeOut > 0)
            {
                webcache.Insert(CacheName, o, null, DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webcache.Insert(CacheName, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中,并对相关文件建立依赖
        /// </summary>
        /// <param name="CacheName">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="files">监视的路径文件</param>
        /// <param name="TimeOut">过期时间(0表示无限期)</param>
        public void AddObjectWithFileChange(string CacheName, object o, string[] files, int TimeOut)
        {
            if (CacheName == null || CacheName.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(CacheOnRemove);

            CacheDependency dep = new CacheDependency(files, DateTime.Now);
            if (TimeOut > 0)
            {
                webcache.Insert(CacheName, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webcache.Insert(CacheName, o, dep, System.DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }

        /// <summary>
        /// 加入当前对象到缓存中,并使用依赖键
        /// </summary>
        /// <param name="CacheName">对象的键值</param>
        /// <param name="o">缓存的对象</param>
        /// <param name="dependKey">依赖关联的键值</param>
        /// <param name="TimeOut">过期时间(0表示无限期)</param>
        public void AddObjectWithDepend(string CacheName, object o, string[] DependKey, int TimeOut)
        {
            if (CacheName == null || CacheName.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(CacheOnRemove);

            CacheDependency dep = new CacheDependency(null, DependKey, DateTime.Now);
            if (TimeOut > 0)
            {
                webcache.Insert(CacheName, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
            else
            {
                webcache.Insert(CacheName, o, dep, System.DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
            }
        }



        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="CacheName">对象的关键字</param>
        public void RemoveObject(string CacheName)
        {
            if (CacheName == null || CacheName.Length == 0)
            {
                return;
            }
            webcache.Remove(CacheName);
        }


        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <param name="CacheName">对象的关键字</param>
        /// <returns>对象</returns>
        public object RetrieveObject(string CacheName)
        {
            if (CacheName == null || CacheName.Length == 0)
            {
                return null;
            }

            return webcache.Get(CacheName);
        }



        /// <summary>
        /// 缓存失效回调委托的一个实例,以后增加日志记录
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="reason"></param>
        public void CacheOnRemove(string Key, object Val, CacheItemRemovedReason Reason)
        {
            //switch (Reason)
            //{
            //    case CacheItemRemovedReason.DependencyChanged:
            //        break;
            //    case CacheItemRemovedReason.Expired:
            //        {
            //            break;
            //        }
            //    case CacheItemRemovedReason.Removed:
            //        {
            //            break;
            //        }
            //    case CacheItemRemovedReason.Underused:
            //        {
            //            break;
            //        }
            //    default: break;
            //}
            //如需要使用缓存日志,则需要使用下面代码
            //myLogVisitor.WriteLog(this,key,val,reason);
            if (this._IsWriteLogs)
            {
                string logfilepath = Common.Utils.GetMapPath(string.Format("~\\LiteCMSLogs\\{0}.txt", DateTime.Now.ToString("yyMMdd")));
                Val = Val == null ? "" : Val.ToString();
                string message = string.Format("Key:{0},Value:{1},Reason:{2}", Key, Val, Reason.ToString());
                Logs.TinyLogs.InsertLog(logfilepath, DateTime.Now, "Cache_Removed", message);
            }
        }
    }
}
