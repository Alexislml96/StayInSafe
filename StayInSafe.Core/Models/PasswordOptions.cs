﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Models
{
    public  class PasswordOptions
    {
        public PasswordOptions()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                Iterations = Convert.ToInt32(Environment.GetEnvironmentVariable("Iterations"));
                SaltSize = Convert.ToInt32(Environment.GetEnvironmentVariable("SaltSize"));
                KeySize = Convert.ToInt32(Environment.GetEnvironmentVariable("KeySize"));
            }
            else
            {
                Iterations = 10000;
                SaltSize = 16;
                KeySize = 32;
            }
        }
        public int SaltSize { get; private set; }
        public int KeySize { get; private set; }
        public int Iterations { get; private set; }
    }
}
