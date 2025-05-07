using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event
{
    public class BeginningEditEvent
    {
        public string? columnName { get; set; }
        public bool canEdit { get; set; }
    }
}