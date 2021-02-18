using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Observer_Pattern<Args> : Singleton<Observer_Pattern<Args>>
{
    public struct ObserverWrapper
    {
        public Action<Args> OnNotify { get; private set; }
        public bool bIsPlayOnce { get; private set; }

        public ObserverWrapper(Action<Args> OnNotify, bool bIsPlayOnce = false)
        {
            this.OnNotify = OnNotify; this.bIsPlayOnce = bIsPlayOnce;
        }
    }

    public int iObserverCount => _mapObserver.Count;

    public Args pLastArg { get; private set; }

    protected Dictionary<Action<Args>, ObserverWrapper> _mapObserver = new Dictionary<Action<Args>, ObserverWrapper>();
    protected HashSet<Action<Args>> _setRequestRemoveObserver = new HashSet<Action<Args>>();
    bool _bIsNotifying;


    public event Action<Args> Subscribe
    {
        add => DoRegist_Observer(value);
        remove => DoRemove_Observer(value);
    }

    public void DoRegist_Observer(Action<Args> OnNotify, bool bInstantNotify_To_ThisListener = false, bool bPlayOnce = false)
    {
        if (OnNotify == null)
            return;

        if (_mapObserver.ContainsKey(OnNotify))
            return;

        if (bInstantNotify_To_ThisListener)
        {
            OnNotify(pLastArg);

            if (bPlayOnce == false)
                _mapObserver.Add(OnNotify, new ObserverWrapper(OnNotify));
        }
        else
        {
            _mapObserver.Add(OnNotify, new ObserverWrapper(OnNotify, bPlayOnce));
        }
    }

    public void DoRemove_Observer(Action<Args> OnNotify)
    {
        if (_bIsNotifying)
        {
            _setRequestRemoveObserver.Add(OnNotify);
            return;
        }

        if (_mapObserver.ContainsKey(OnNotify))
            _mapObserver.Remove(OnNotify);
    }

    public void DoNotify(Args arg)
    {
        _bIsNotifying = true;

        foreach (var pListener in _mapObserver.Values)
        {
            pListener.OnNotify?.Invoke(arg);

            if (pListener.bIsPlayOnce)
                _setRequestRemoveObserver.Add(pListener.OnNotify);
        }

        pLastArg = arg;

        _bIsNotifying = false;
        if (_setRequestRemoveObserver.Count != 0)
        {
            foreach (var pRemoveAction in _setRequestRemoveObserver)
                DoRemove_Observer(pRemoveAction);
            _setRequestRemoveObserver.Clear();
        }
    }

    /// <summary>
    /// 기본 생성자
    /// </summary>
    public Observer_Pattern()
    {
    }

    /// <summary>
    /// 인자로 Args 를 받는 생성자
    /// </summary>
    /// <param name="arg"></param>
    public Observer_Pattern(Args arg)
    {
        pLastArg = arg;
    }
}
