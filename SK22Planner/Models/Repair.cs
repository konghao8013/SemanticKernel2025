using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK20PluginBasic.Models
{
    public sealed class Repair
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }
        public string? Date { get; set; }
        public string? Image { get; set; }
    }
}
