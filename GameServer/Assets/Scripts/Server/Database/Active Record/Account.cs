using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Table("accounts")]
public class Account : ActiveRecord
{
    [Field("id")]
    public int Id;

    [Field("username")]
    public string Username;

    [Field("password")]
    public string Password;

    [Field("email")]
    public string Email;
}
