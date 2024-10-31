﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitTrack
{
    public class User : Person
    {
        private string _country;
        private string _securityQuestion;
        private string _securityAnswer;

        public User()
        {
            _country = string.Empty;
            _securityQuestion = string.Empty;
            _securityAnswer = string.Empty;
        }

        public string Country
        {
            get => _country;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Välj land!");
                _country = value;
            }
        }

        public string SecurityQuestion
        {
            get => _securityQuestion;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Fel säkerhetsfråga!");
                _securityQuestion = value;
            }
        }

        public string SecurityAnswer
        {
            get => _securityAnswer;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Fel säkerhetsfråga!");
                _securityAnswer = value;
            }
        }

        public override void SignIn()
        {
            base.SignIn();
            
            if (string.IsNullOrWhiteSpace(Country))
                throw new InvalidOperationException("Välj land!");
        }

        public void ResetPassword(string securityAnswer, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(securityAnswer))
                throw new ArgumentException("Fel säkerhetsfråga!");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Nya lösenordet kan inte vara tom!");

            if (newPassword.Length < 5)
                throw new ArgumentException("Lösenordet måste vara minst 5 karaktärer lång!");

            if (securityAnswer.Trim().ToLower() != SecurityAnswer.Trim().ToLower())
                throw new InvalidOperationException("Fel säkerhetsfråga!");

            Password = newPassword;
        }
    }
}