﻿using ETicaretAPI.Domain.Entities.Identity;

namespace ETicaretAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Token.Token CreateAccessToken(int minute, AppUser user);
        string CreateRefreshToken();
    }
}
