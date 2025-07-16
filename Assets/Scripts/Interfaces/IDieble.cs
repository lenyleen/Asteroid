using System;
using UniRx;

namespace Interfaces
{
    public interface IDieble
    {
        public ReactiveCommand OnDead { get; }
    }
}