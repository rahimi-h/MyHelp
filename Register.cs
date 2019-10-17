# MyHelp
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.UserName, Email = viewModel.Email, PhoneNumber = viewModel.PhoneNumber, RegisterDate = DateTime.Now, IsActive = true };
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded == true)
                {
                    var role = _roleManager.FindByIdAsync("کاربر");
                    if (role == null)
                        await _roleManager.CreateAsync(new ApplicationRole("کاربر"));

                    result = await _userManager.AddToRoleAsync(user, "کاربر");
                    if (result.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callBackUrl = Url.Action("ConfirmEmail", "Account", values: new { userId = user.Id, code = code }, protocol: Request.Scheme);
                        
                        
                       //The following code is for sending email templates//
                       
       await _emailSender.SendEmailAsync(viewModel.Email, "تایید ایمیل حساب کاربری", $"<div style='padding: 20px;background-color: #4CAF50;color: white;text-align: center;width: 50%;border-radius: 20px;" +
                            $"margin: auto;'><strong style='font - family: Verdana, Geneva, Tahoma, sans - serif;'>ثبت موفقیت آمیز!</strong>جهت تایید ایمیل روی لینک زیر کلیک نمایید</div><div style='text-align: center;'>" +
                            $"<a href='{HtmlEncoder.Default.Encode(callBackUrl)}'style='background-color: rgba(205, 232, 241, 0.856); border: none; color: rgb(32, 32, 32);padding: 40px;text-align: center; text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px;" +
                            $" cursor: pointer;width: 200px;border-radius: 20px;'>تایید ایمیل</a></div>");
                        return RedirectToAction("Index", "Account", new { id = "ConfirmEmail" });
                    }
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }


            }
            return Redirect("/#work");
        }
