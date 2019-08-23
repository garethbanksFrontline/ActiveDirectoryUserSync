using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlActiveDirectoryServices;

namespace ActiveDirectoryUserCheck
{
    public class Program
    {
        static void Main(string[] args)
        {
            ADServices.UpdateSQLUsers();

        }
    }
}
