﻿using System.ComponentModel.DataAnnotations;
using System;

namespace BorsaBlogProjesi.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Lütfen adınızı giriniz.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lütfen soyadınızı giriniz.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Tarih alanını lütfen giriniz.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Lütfen email alanına uygun bir adres girin.")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Telefon numaranızı girin.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
        public string Password { get; set; }
    }
}
