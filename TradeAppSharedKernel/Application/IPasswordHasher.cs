﻿namespace TradeAppSharedKernel.Application
{
    public interface IPasswordHasher
    {
        string Hash(string password, out string salt);
        bool Verify(string password, string hash, string salt);
    }
}
