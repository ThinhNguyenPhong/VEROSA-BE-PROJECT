﻿namespace VEROSA.BussinessLogicLayer.PasswordHash
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string hash, string password);
    }
}
