using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCTechSummer2025MovieAppShared;

public class ProblemResponse
{
    public string? Title { get; set; }
    public string? Type { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public string? Instance { get; set; }

    public IDictionary<string, string[]>? Errors { get; set; } // for validation errors
}
