using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCTechSummer2025MovieAppShared;

public class UserEditDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool Admin { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
