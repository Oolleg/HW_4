using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace HW_4.Models.User
{
    public class NewAvatarModel
    {
        [FromForm(Name = "new-avatar-input-profile")]
        [JsonIgnore]
        public IFormFile Avatar { get; set; } = null!;
    }
}
