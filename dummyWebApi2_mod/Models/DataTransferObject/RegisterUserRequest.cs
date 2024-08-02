﻿using System.ComponentModel.DataAnnotations;

namespace dummyWebApi2.Models.DataTransferObject
{
    public class RegisterUserRequest
    {
        [Required]
        public string Name { get; set; }
        [Required/*, EmailAddress*/]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}