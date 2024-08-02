﻿using System.ComponentModel.DataAnnotations;

namespace dummyWebApi2.Models.DTO
{
    public class SigninUserRequest
    {
        [Required/*, EmailAddress*/]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
