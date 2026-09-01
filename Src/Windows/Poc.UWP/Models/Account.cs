using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace Poc.UWP.Models
{
    public class AccountFeatureFlags
    {
        public AccountFeatureFlags(List<UserFeature> featureFlags)
        {
            FeatureFlags = featureFlags;
        }

        [JsonInclude]
        public List<UserFeature> FeatureFlags { get; private set; } = new List<UserFeature>(0);
    }

    public class Account
    {
        public Account(
            bool isGuest,
            string sessionId,
            DateTime signInDate,
            string username,
            Guid userId,
            bool isSignedIn,
            List<string> roles,
            string bearerToken,
            DateTimeOffset bearerTokenExpiration)
        {
            IsGuest = isGuest;
            SessionId = sessionId;
            SignInDate = signInDate;
            Username = username;
            HashedUserName = HashHelper.Sha256ToString(username);
            UserId = userId;
            IsSignedIn = isSignedIn;
            Roles = roles;
            BearerToken = bearerToken;
            BearerTokenExpiration = bearerTokenExpiration;
        }

        [JsonInclude]
        public string BearerToken { get; private set; }

        [JsonInclude]
        public DateTimeOffset BearerTokenExpiration { get; private set; }

        /// <summary>
        /// Guest: 5ed8944a85a9763fd315852f448cb7de36c5e928e13b3be427f98f7dc455f141
        /// </summary>
        [JsonInclude]
        public string HashedUserName { get; private set; }

        [JsonInclude]
        public bool IsGuest { get; private set; }

        [JsonInclude]
        public bool IsSignedIn { get; private set; }

        [JsonInclude]
        public List<string> Roles { get; private set; } = new List<string>(0);

        [JsonInclude]
        public string SessionId { get; private set; }

        [JsonInclude]
        public DateTime SignInDate { get; private set; }

        [JsonInclude]
        public Guid UserId { get; private set; }

        [JsonInclude]
        public string Username { get; private set; }
    }

    public class UserFeature
    {
        public string Description { get; set; }

        public int Id { get; set; }

        public string Identifier { get; set; }

        public string Name { get; set; }

        public string OriginId { get; set; }

        public string OriginSource { get; set; }

        public string Scope { get; set; }

        public string Settings { get; set; }

        public string SettingsContentType { get; set; }

        //public string[] Types { get; set; }
        public List<string> Types { get; set; }
    }

    public static class HashHelper
    {
        public static string Sha256ToString(string text)
        {
            text ??= string.Empty;
            using var alg = SHA256.Create();
            return string.Join(null, alg.ComputeHash(Encoding.UTF8.GetBytes(text))
                         .Select(x => x.ToString("x2")));
        }
    }
}

