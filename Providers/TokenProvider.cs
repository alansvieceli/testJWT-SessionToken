﻿using JWToken.CustomAttributes;
using JWToken.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWToken.Providers
{
    public class TokenProvider
    {

        private List<User> UserList = new List<User>
        {
            new User { USERID = "jsmith@email.com", PASSWORD = "test", EMAILID = "jsmith@email.com", FIRST_NAME = "John", LAST_NAME = "Smith", PHONE = "356-735-2748", ACCESS_LEVEL = Roles.DIRECTOR.ToString(), WRITE_ACCESS = "WRITE_ACCESS" },
            new User { USERID = "srob@email.com", PASSWORD = "test", FIRST_NAME = "Steve", LAST_NAME = "Rob", EMAILID = "srob@email.com", PHONE = "567-479-8537", ACCESS_LEVEL = Roles.SUPERVISOR.ToString(), WRITE_ACCESS = "WRITE_ACCESS" },
            new User { USERID = "dwill@email.com", PASSWORD = "test", FIRST_NAME = "DJ", LAST_NAME = "Will", EMAILID = "dwill@email.com", PHONE = "599-306-6010", ACCESS_LEVEL = Roles.ANALYST.ToString(), WRITE_ACCESS = "WRITE_ACCESS" },
            new User { USERID = "JBlack@email.com", PASSWORD = "test", FIRST_NAME = "Joe", LAST_NAME = "Black", EMAILID = "JBlack@email.com", PHONE = "764-460-8610", ACCESS_LEVEL = Roles.ANALYST.ToString(), WRITE_ACCESS = "" }
        };

        public string LoginUser(string UserID, string Password)
        {
            //Get user details for the user who is trying to login
            var user = UserList.SingleOrDefault(x => x.USERID == UserID);

            //Authenticate User, Check if it’s a registered user in Database 
            if (user == null)
                return null;

            //If it is registered user, check user password stored in Database
            //For demo, password is not hashed. It is just a string comparision 
            //In reality, password would be hashed and stored in Database. 
            //Before comparing, hash the password again.
            if (Password == user.PASSWORD)
            {
                //Authentication successful, Issue Token with user credentials 
                //Provide the security key which is given in 
                //Startup.cs ConfigureServices() method 
                var key = Encoding.ASCII.GetBytes("ALN174-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
                //Generate Token for user 
                var JWToken = new JwtSecurityToken(
                    issuer: "http://localhost:45092/",
                    audience: "http://localhost:45092/",
                    claims: GetUserClaims(user),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                    //Using HS256 Algorithm to encrypt Token  
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                return token;
            }
            else
            {
                return null;
            }
        }

        //Using hard coded collection list as Data Store for demo. 
        //In reality, User data comes from Database or other Data Source 
        private IEnumerable<Claim> GetUserClaims(User user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim(ClaimTypes.Name, user.FIRST_NAME + " " + user.LAST_NAME);
            claims.Add(_claim);
            _claim = new Claim("USERID", user.USERID);
            claims.Add(_claim);
            _claim = new Claim("EMAILID", user.EMAILID);
            claims.Add(_claim);
            _claim = new Claim("PHONE", user.PHONE);
            claims.Add(_claim);
            _claim = new Claim(user.ACCESS_LEVEL, user.ACCESS_LEVEL);
            claims.Add(_claim);

            if (user.WRITE_ACCESS != "")
            {
                _claim = new Claim(user.WRITE_ACCESS, user.WRITE_ACCESS);
                claims.Add(_claim);
            }
            return claims.AsEnumerable<Claim>();
        }

    }
}
