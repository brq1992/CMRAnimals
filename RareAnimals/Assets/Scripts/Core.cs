
using System;
using System.Collections;
using System.Collections.Generic;

namespace Core
{
    public class Singleton<T> where T : new()
    {
        private static T mSingleton = default(T);

        public static T getSingleton()
        {
            if (null == mSingleton)
                mSingleton = new T();
            return mSingleton;
        }

        public virtual void Destroy()
        {
            if (null != mSingleton)
                mSingleton = default(T);
        }
    }

    public class Observer<T>
    {
        private Action<T> callback = null;

        public Observer(Action<T> callback)
        {
            this.callback += callback;
        }

        public void Execute(T Params)
        {
            if (null != callback)
                callback(Params);
        }

        public void Execute()
        {
            if (null != callback)
                callback(default(T));
        }

        public void AddCallBack(Action<T> callback)
        {
            this.callback += callback;
        }

        public void Release(Action<T> callback)
        {
            this.callback -= callback;
        }
    }

    public class Notification : Core.Singleton<Notification>
    {
        private Dictionary<int, Observer<object[]>> observerList = null;
        private Dictionary<int, object> observerList_T = null;
        public Notification()
        {
            observerList = new Dictionary<int, Observer<object[]>>();
            observerList_T = new Dictionary<int, object>();
        }

        public void AddObserver(int Code, Action<object[]> callback)
        {
            if (null != observerList)
            {
                if (!observerList.ContainsKey(Code))
                    observerList.Add(Code, new Observer<object[]>(callback));
                else
                    observerList[Code].AddCallBack(callback);
            }
        }

        public void AddObserverGeneric<T>(int Code, Action<T> callback)
        {
            if (null != observerList)
            {
                if (!observerList.ContainsKey(Code))
                    observerList_T.Add(Code, new Observer<T>(callback));
                else
                    ((Observer<T>)observerList_T[Code]).AddCallBack(callback);
                // observerList_T[Code] = new Observer<T>(callback);
            }
        }

        public void RemoveObserver(int Code, Action<object[]> callback)
        {
            if ((null != observerList) && observerList.ContainsKey(Code))
                observerList[Code].Release(callback);
        }

        public void RemoveObserverGeneric<T>(int Code, Action<T> callback)
        {
            if ((null != observerList_T) && observerList_T.ContainsKey(Code))
                ((Observer<T>)observerList_T[Code]).Release(callback);
        }

        public void Release()
        {
            if (null != observerList)
                observerList.Clear();
        }

        public void Post(int Code, params object[] Params)
        {
            if ((null != observerList) && observerList.ContainsKey(Code))
            {
                Observer<object[]> observer = (Observer<object[]>)observerList[Code];
                if (null != observer)
                    observer.Execute(Params);
            }
        }

        public void PostGeneric<T>(int Code, T Param)
        {
            if (observerList_T != null)
            {
                Observer<T> observer = (Observer<T>)observerList_T[Code];
                if (null != observer)
                    observer.Execute(Param);
            }
        }
    }

    public class NotificationEx : Core.Singleton<NotificationEx>
    {
        private Hashtable observerList = null;

        public NotificationEx()
        {
            observerList = new Hashtable();
        }

        public void AddObserver<T>(int Code, Action<T> callback)
        {
            if (null != observerList)
            {
                if (!observerList.ContainsKey(Code))
                    observerList.Add(Code, new Observer<T>(callback));
                else
                {
                    Observer<T> observer = (Observer<T>)observerList[Code];
                    observer.AddCallBack(callback);
                }
            }
        }

        public void RemoveObserver<T>(int Code, Action<T> callback)
        {
            if ((null != observerList) && observerList.ContainsKey(Code))
            {
                Observer<T> observer = (Observer<T>)observerList[Code];
                observer.Release(callback);
            }
        }

        public void Release()
        {
            if (null != observerList)
                observerList.Clear();
        }

        public void Post<T>(int Code, T Params)
        {
            if ((null != observerList) && observerList.ContainsKey(Code))
            {
                Observer<T> observer = (Observer<T>)observerList[Code];
                if (null != observer)
                    observer.Execute(Params);
            }
        }

        public void Post<T>(int Code)
        {
            if ((null != observerList) && observerList.ContainsKey(Code))
            {
                Observer<T> observer = (Observer<T>)observerList[Code];
                if (null != observer)
                    observer.Execute();
            }
        }
    }
}
