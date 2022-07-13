using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public class PortPage
    {
        public string Title { get; init; }
        public bool IsDefault { get; init; }
        public PortPage(string title, bool isDefault)
        {
            Title = title;
            IsDefault = isDefault;
        }
    }
}
