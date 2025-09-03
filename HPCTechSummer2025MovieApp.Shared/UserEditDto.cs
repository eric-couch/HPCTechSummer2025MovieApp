using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCTechSummer2025MovieAppShared;

public class UserEditDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool Admin { get; set; }
    [MaxLength(10)]
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
