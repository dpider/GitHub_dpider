using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace pde.pub
{
   public class AssemblyDynamicLoader
    {
        private AppDomain appDomain;
        private RemoteLoader remoteLoader;
        private string _dllName;
        private string _className;

        /// <summary>
        /// 加载dll
        /// </summary>
        /// <param name="dllName">dll文件</param>
        /// <param name="className">调用的类名称</param>
        public AssemblyDynamicLoader(string dllName, string className)
        {
            _dllName = dllName;
            _className = className;
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationName = "ApplicationLoader";
            setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            setup.PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "private");
            setup.CachePath = setup.ApplicationBase;
            setup.ShadowCopyFiles = "true";
            setup.ShadowCopyDirectories = setup.ApplicationBase;
            this.appDomain = AppDomain.CreateDomain("ApplicationLoaderDomain", null, setup);
            string name = Assembly.GetExecutingAssembly().GetName().FullName;
            this.remoteLoader = (RemoteLoader)this.appDomain.CreateInstanceAndUnwrap(name, typeof(RemoteLoader).FullName);
        }
        

        /// <summary>
        /// 调用类中的方法
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <param name="Parameters">参数集合</param>
        /// <returns></returns>
        public object InvokeMethod(string methodName, object[] Parameters)
        {
            return remoteLoader.InvokeMethod(_dllName, _className, methodName, Parameters);
        }


        /// <summary>
        /// 卸载dll
        /// </summary>
                　 
        public void Unload()
        {
            try
            {
                AppDomain.Unload(appDomain);
                appDomain = null;
            }
            catch (CannotUnloadAppDomainException ex)
            {
                throw new Exception(string.Format("卸载dll出错：" + ex.Message));
            }
        }

    }


    #region RemoteLoader类，实例化dll中的类与方法
    public class RemoteLoader : MarshalByRefObject
    {
        private Assembly assembly = null;
        private Type dllClass = null;
        private object obj = null;      //实例化对象


        public object InvokeMethod(string dllFileName, string className, string methodName, object[] Parameters)
        {
            try
            {
                if ((assembly == null) || (obj == null))
                {
                    assembly = Assembly.LoadFrom(dllFileName);
                    Type[] type = assembly.GetTypes();
                    foreach (Type t in type)
                    {
                        if (t.Name.Equals(className))         //模块中指定类名 
                        {
                            dllClass = t;
                            obj = Activator.CreateInstance(t);  //实例化该类
                            break;
                        }
                    }
                }
                if (dllClass == null)
                {
                    throw new Exception(string.Format("初始化模块失败：没有找到类：{0}！", className));
                }
                MethodInfo m = null;
                try
                {
                    m = dllClass.GetMethod(methodName);
                    return m.Invoke(obj, Parameters);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    m = null;
                }

                /* 备用方法
                 if (assembly != null)
                 {
                     dllClass = assembly.GetType(className, true, true);
                 }
                 else
                 {
                     dllClass = Type.GetType(className, true, true);
                 }  

                 BindingFlags defaultBinding = BindingFlags.DeclaredOnly  | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | 
                                               BindingFlags.IgnoreCase  | BindingFlags.InvokeMethod | BindingFlags.Static;
                 CultureInfo cultureInfo = new CultureInfo("es-ES", false);
                 MethodInfo methisInfo = dllClass.GetMethod(methodName);
                 if (methisInfo == null)
                 {
                     throw new Exception(string.Format("方法：{0}在模块中不存在！", methodName));
                 }
                 if (methisInfo.IsStatic)
                 {
                     return dllClass.InvokeMember(methodName, defaultBinding, null, null, Parameters, cultureInfo);
                 }
                 else
                 {
                     object pgmClass = Activator.CreateInstance(dllClass);
                     if (methisInfo.GetParameters().Length == 0)
                     {                        
                         return dllClass.InvokeMember(methodName, defaultBinding, null, pgmClass, null, cultureInfo);
                     }
                     else
                     {
                         return dllClass.InvokeMember(methodName, defaultBinding, null, pgmClass, Parameters, cultureInfo);
                     }
                 }
                 */
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void InvokeEvent(string dllFileName, string className, string reflectioneventName, MethodInfo currentMethodHandler, object[] Parameters)
        {
            try
            {
                if ((assembly == null) || (obj == null))
                {
                    assembly = Assembly.LoadFrom(dllFileName);
                    Type[] type = assembly.GetTypes();
                    foreach (Type t in type)
                    {
                        if (t.Name.Equals(className))         //模块中指定类名 
                        {
                            dllClass = t;
                            obj = Activator.CreateInstance(t);  //实例化该类
                            break;
                        }
                    }
                }
                if (dllClass == null)
                {
                    throw new Exception(string.Format("初始化模块失败：没有找到类：{0}！", className));
                }
                EventInfo eventInfo = null;
                try
                {
                    //反射执行的成员和类型搜索
                    const BindingFlags myBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
                    eventInfo = dllClass.GetEvent(reflectioneventName, myBindingFlags);
                    Type tDelegate = eventInfo.EventHandlerType;
                    ///   MethodInfo methodHandler = currentWindow.GetType().GetMethod(currentEventName, myBindingFlags);
                    //创建委托 
                    Delegate d = Delegate.CreateDelegate(tDelegate, null, currentMethodHandler);
                    //获取将要处理的事件委托
                    MethodInfo minAddHandler = eventInfo.GetAddMethod();
                    object[] addHandlerArgs = { d };
                    //调用
                    minAddHandler.Invoke(obj, addHandlerArgs);
                    FieldInfo field = dllClass.GetField(reflectioneventName, myBindingFlags);
                    if (field != null)
                    {
                        Object fieldValue = field.GetValue(obj);
                        if (fieldValue != null && fieldValue is Delegate)
                        {
                            Delegate objectDelegate = fieldValue as Delegate;
                            //动态调用
                            objectDelegate.DynamicInvoke(Parameters);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    eventInfo = null;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
    #endregion


}
