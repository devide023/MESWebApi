﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace MESWebApi.Util
{
    public class JWTHelper
    {
		string key = "f47b558d-7654-458c-99f2-13b190ef0199";
		SecurityKey securityKey = null;
		public JWTHelper()
        {
			securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
		}
        public string CreateToken()
        {
            DateTime utcNow = DateTime.UtcNow;
            var claims = new List<Claim>() { 
                new Claim("ID", "1"), 
                new Claim("Name", "fan") 
            };
            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: "fan",
                audience: "audi~~!",
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddHours(2),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );
            string token1 = new JwtSecurityTokenHandler().WriteToken(jwtToken);

			var tokenDescriptor = new SecurityTokenDescriptor // 创建一个 Token 的原始对象
			{
				Issuer = "fan",
				Audience = "audi",
				Subject = new ClaimsIdentity(new[]
			   {
					new Claim(ClaimTypes.Name, "")
				}),
				Expires = DateTime.Now.AddMinutes(60),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256)
			};
			//生成token方式2
			SecurityToken securityToken = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
			var token2 = new JwtSecurityTokenHandler().WriteToken(securityToken);


			return token1;
        }

		public bool CheckToken(string token)
		{
			//校验token
			var validateParameter = new TokenValidationParameters()
			{
				ValidateLifetime = true,
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = securityKey,
			};
			//不校验，直接解析token
			//jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token1);
			try
			{
				//校验并解析token
				var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, validateParameter, out SecurityToken validatedToken);//validatedToken:解密后的对象
				var jwtPayload = ((JwtSecurityToken)validatedToken).Payload.SerializeToJson(); //获取payload中的数据 

				return true;
			}
			catch (SecurityTokenExpiredException)
			{
				//表示过期

				return false;
			}
			catch (SecurityTokenException)
			{
				//表示token错误
				return false;
			}
		}

    }
}