﻿using System;

namespace MyDent.Services.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {

        }
    }
}
