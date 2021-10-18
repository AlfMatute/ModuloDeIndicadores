using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModuloDeIndicadores.Data;
using ModuloDeIndicadores.Models;
using ModuloDeIndicadores.Utility;

namespace ModuloDeIndicadores.Areas.Identity.Pages.Account
{
    //[Authorize(Roles = "Admin")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _db = db;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        //public List<SelectListItem> Perfiles { set; get; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "La {0} debe ser al menos {2} y como máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no conciden.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Identificador de usuario")]
            public string Identificador { get; set; }

            [Required]
            [Display(Name = "Numero de usuario")]
            public string Numero { get; set; }

            [Required]
            [Display(Name = "Rol de Usuario")]
            public string Perfil { get; set; }

            [Required]
            [Display(Name = "Nombre de Usuario")]
            public string NombreUsuario { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                bool correcto = true;
                var datoDeBd = await _db.AppUser.FirstOrDefaultAsync(b => b.Email == Input.Email);
                if (datoDeBd != null)
                    correcto = false;
                datoDeBd = await _db.AppUser.FirstOrDefaultAsync(b => b.Numero == Input.Numero);
                if (datoDeBd != null)
                    correcto = false;
                datoDeBd = await _db.AppUser.FirstOrDefaultAsync(b => b.Identificador == Input.Identificador);
                if (datoDeBd != null)
                    correcto = false;

                if(correcto)
                {
                    var user = new AppUser
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        Identificador = Input.Identificador,
                        Numero = Input.Numero,
                        NombreUsuario = Input.NombreUsuario
                    };
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(Perfiles.AdminEndUser))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(Perfiles.AdminEndUser));
                        }
                        if (!await _roleManager.RoleExistsAsync(Perfiles.ConsultaEndUser))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(Perfiles.ConsultaEndUser));
                        }
                        if (!await _roleManager.RoleExistsAsync(Perfiles.DigitadorEndUser))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(Perfiles.DigitadorEndUser));
                        }

                        await _userManager.AddToRoleAsync(user, Input.Perfil);
                        _logger.LogInformation("Usuario creado.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            //await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect("~/Usuarios/Index/");
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else 
                {
                    ModelState.AddModelError(string.Empty, "Existen datos que ya estan en la base de datos, favor validar");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
