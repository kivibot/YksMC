using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YksMC.Client.EventBus
{
    public interface IEventHandler<T>
    {
        void Handle(T args);
    }
}
