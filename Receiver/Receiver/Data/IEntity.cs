using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver.Data
{
    public interface IEntity
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
