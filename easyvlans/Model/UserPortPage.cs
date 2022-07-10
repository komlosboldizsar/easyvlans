using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Model
{
    public class UserPortPage
    {
        public string Title { get; init; }
        public bool IsDefault { get; init; }
        public UserPortPage(string title, bool isDefault)
        {
            Title = title;
            IsDefault = isDefault;
        }
    }
}
