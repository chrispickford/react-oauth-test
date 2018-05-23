using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactOAuthTest.Data.Entities
{
    public class RefreshToken
    {
        public DateTime ExpiresUtc { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public DateTime IssuedUtc { get; set; }

        public string Token { get; set; }

        [ForeignKey("UserId")] public User User { get; set; }

        public string UserId { get; set; }
    }
}