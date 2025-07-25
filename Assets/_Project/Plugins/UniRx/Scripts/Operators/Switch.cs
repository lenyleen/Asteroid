﻿using System;

namespace UniRx.Operators
{
    internal class SwitchObservable<T> : OperatorObservableBase<T>
    {
        private readonly IObservable<IObservable<T>> sources;

        public SwitchObservable(IObservable<IObservable<T>> sources)
            : base(true)
        {
            this.sources = sources;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            return new SwitchObserver(this, observer, cancel).Run();
        }

        private class SwitchObserver : OperatorObserverBase<IObservable<T>, T>
        {
            private readonly object gate = new();
            private readonly SerialDisposable innerSubscription = new();
            private readonly SwitchObservable<T> parent;
            private bool hasLatest;
            private bool isStopped;
            private ulong latest;

            public SwitchObserver(SwitchObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(
                observer, cancel)
            {
                this.parent = parent;
            }

            public IDisposable Run()
            {
                var subscription = parent.sources.Subscribe(this);
                return StableCompositeDisposable.Create(subscription, innerSubscription);
            }

            public override void OnNext(IObservable<T> value)
            {
                var id = default(ulong);
                lock (gate)
                {
                    id = unchecked(++latest);
                    hasLatest = true;
                }

                var d = new SingleAssignmentDisposable();
                innerSubscription.Disposable = d;
                d.Disposable = value.Subscribe(new Switch(this, id));
            }

            public override void OnError(Exception error)
            {
                lock (gate)
                {
                    try
                    {
                        observer.OnError(error);
                    }
                    finally
                    {
                        Dispose();
                    }
                }
            }

            public override void OnCompleted()
            {
                lock (gate)
                {
                    isStopped = true;
                    if (!hasLatest)
                        try
                        {
                            observer.OnCompleted();
                        }
                        finally
                        {
                            Dispose();
                        }
                }
            }

            private class Switch : IObserver<T>
            {
                private readonly ulong id;
                private readonly SwitchObserver parent;

                public Switch(SwitchObserver observer, ulong id)
                {
                    parent = observer;
                    this.id = id;
                }

                public void OnNext(T value)
                {
                    lock (parent.gate)
                    {
                        if (parent.latest == id) parent.observer.OnNext(value);
                    }
                }

                public void OnError(Exception error)
                {
                    lock (parent.gate)
                    {
                        if (parent.latest == id) parent.observer.OnError(error);
                    }
                }

                public void OnCompleted()
                {
                    lock (parent.gate)
                    {
                        if (parent.latest == id)
                        {
                            parent.hasLatest = false;
                            if (parent.isStopped) parent.observer.OnCompleted();
                        }
                    }
                }
            }
        }
    }
}