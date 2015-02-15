using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;
using System.Threading;
using WebConsole.Interfaces;

namespace WebConsole.Helpers
{
  public abstract class GatewayBase
  {
    private ICacheManager _cacheManager = new CacheManager();
    private static object cacheLock;

    private Dictionary<string, object> ChannelFactoryCache
    {
      get
      {
        Dictionary<string, object> dictionary = (Dictionary<string, object>) null;
        if (!this._cacheManager.TryCacheLoad<Dictionary<string, object>>("OpenChannels", ref dictionary))
        {
          bool lockTaken = false;
          object obj = "null";
          try
          {
            Monitor.Enter(obj = GatewayBase.cacheLock, ref lockTaken);
            if (!this._cacheManager.TryCacheLoad<Dictionary<string, object>>("OpenChannels", ref dictionary))
            {
              dictionary = new Dictionary<string, object>();
              this._cacheManager.CacheAdd<Dictionary<string, object>>("OpenChannels", dictionary);
            }
          }
          finally
          {
            if (lockTaken)
              Monitor.Exit(obj);
          }
        }
        return dictionary;
      }
    }

    protected abstract string EndpointName { get; }

    static GatewayBase()
    {
      GatewayBase.cacheLock = new object();
    }

    //public GatewayBase(ICacheManager cacheManager)
    //{
    //  this._cacheManager = (ICacheManager) null;
    //  this._cacheManager = cacheManager;
    //}

    protected T WcfWrapper<T, S>(Func<S, T> targetFunction)
    {
      ICommunicationObject communicationObject = (ICommunicationObject) null;

      if (targetFunction == null)
        throw new ArgumentNullException("targetFunction");

      if (string.IsNullOrEmpty(this.EndpointName))
        throw new InvalidOperationException("EndPointName must be set");

      T obj = default (T);
      S s = default (S);

      try
      {
        if (Convert.ToBoolean(ConfigurationManager.AppSettings["cachechannels"]))
        {
          this.CreateChannelFactory<S>();
          S channel = ((ChannelFactory<S>) this.ChannelFactoryCache[this.EndpointName]).CreateChannel();
          obj = targetFunction(channel);
          communicationObject = (object) channel as ICommunicationObject;
        }
        else
        {
          S channel = new ChannelFactory<S>(this.EndpointName).CreateChannel();
          obj = targetFunction(channel);
          communicationObject = (object)channel as ICommunicationObject;
        }
      }
      catch (Exception ex)
      {
        if (communicationObject != null)
          communicationObject.Abort();
          throw ex;
      }
      finally
      {
        if (communicationObject != null && communicationObject.State == CommunicationState.Opened)
          communicationObject.Close();
      }
      return obj;
    }

    private void CreateChannelFactory<S>()
    {
      if (this.ChannelFactoryCache.ContainsKey(this.EndpointName))
        return;
      bool lockTaken = false;
      object obj = "null";
      try
      {
        Monitor.Enter(obj = GatewayBase.cacheLock, ref lockTaken);
        if (!this.ChannelFactoryCache.ContainsKey(this.EndpointName))
        {
          ChannelFactory<S> channelFactory = new ChannelFactory<S>(this.EndpointName);
          this.ChannelFactoryCache.Add(this.EndpointName, (object) channelFactory);
        }
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(obj);
      }
    }

    protected void WcfWrapper<T>(Action<T> targetVoid)
    {
      ICommunicationObject communicationObject = (ICommunicationObject) null;
      if (string.IsNullOrEmpty(this.EndpointName))
        throw new InvalidOperationException("EndPointName must be set");
      if (targetVoid == null)
        throw new ArgumentNullException("targetVoid");
      T obj = default (T);
      try
      {
        if (Convert.ToBoolean(ConfigurationManager.AppSettings["cachechannels"]))
        {
          this.CreateChannelFactory<T>();
          T channel = ((ChannelFactory<T>) this.ChannelFactoryCache[this.EndpointName]).CreateChannel();
          targetVoid(channel);
          communicationObject = (object) channel as ICommunicationObject;
        }
        else
        {
          T channel = new ChannelFactory<T>(this.EndpointName).CreateChannel();
          targetVoid(channel);
          communicationObject = (object) channel as ICommunicationObject;
        }
      }
      catch (Exception ex)
      {
        if (communicationObject != null)
          communicationObject.Abort();
        //if (!GatewayBase.RethrowError(ex))
        //  return;
        throw;
      }
      finally
      {
        if (communicationObject != null && communicationObject.State == CommunicationState.Opened)
          communicationObject.Close();
      }
    }

    //private static bool RethrowError(Exception ex)
    //{
    //  bool flag;
    //  if (ex.GetType() == typeof (FaultException) || ex.GetType().BaseType == typeof (FaultException))
    //  {
    //    Type underlyingGenericType = TypeExtensions.FindUnderlyingGenericType(ex.GetType());
    //    flag = underlyingGenericType != (Type) null && underlyingGenericType.BaseType == typeof (FaultBase) || ExceptionPolicy.HandleException(ex, "WcfFaultExceptionPolicy");
    //  }
    //  else
    //    flag = !(ex.GetType() == typeof (CommunicationException)) ? (!(ex.GetType() == typeof (TimeoutException)) ? ExceptionPolicy.HandleException(ex, "WcfUnexpectedExceptionPolicy") : ExceptionPolicy.HandleException(ex, "WcfTimeoutExceptionExceptionPolicy")) : ExceptionPolicy.HandleException(ex, "WcfCommunicationExceptionPolicy");
    //  return flag;
    //}
  }
}
