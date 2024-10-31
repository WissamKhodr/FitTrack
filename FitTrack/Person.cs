﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack
{
    public abstract class Person
    {
        protected Person()
        {
            _username = string.Empty;
            _password = string.Empty;
        }

        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Fel! Namnfältet får ej vara tom!");
                if (value.Length < 3)
                    throw new ArgumentException("Minst 3 karaktärer!");
                _username = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Fel! Lösenordsfältet får ej vara tom!");
                if (value.Length < 5)
                    throw new ArgumentException("Minst 5 karaktärer!");
                _password = value;
            }
        }

        public virtual void SignIn()
        {
            
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                throw new InvalidOperationException("Du behöver lägga till ett namn och lösenord!");
        }
    }
}