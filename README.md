# InstagramCaller
InstagramCaller is a Library used to Call Instagram API for ASP.Net MVC.Also, InstagramCaller integrated into Azure Application Insights API, All Endpoint in the library calls the Azure Application Insights API if any Exception is occured.

# Usage
To start use InstagramCaller you need to Create Instance of the InstagramCaller Class.

//add Instagram as authenticatioin provider to your App
//To do that Modyify the Startup.Auth.cs class 
//Add required Scopes that your APP will access
string clientid = ConfigurationManager.AppSettings["clientid"].ToString();
string clientsecret = ConfigurationManager.AppSettings["clientsecret"].ToString();
app.UseInstagramInAuthentication(new InstagramAuthenticationOptions(){
                                    ClientId = clientid,
                                    ClientSecret = clientsecret,
                                    Scope = { "basic", "public_content", "follower_list", "comments", "relationships","likes" }

//InstagramCaller as a Property in the Conroller Class 
public static InstagramCaller.InstagramCaller _InstagramCaller { get; set; }

//Override the OnActionExecuting method to check if the InstagramCaller instance is execit , if not use the ClientID, ClientSecret, UserAccessToken and UserID (Instagram's UserID) to create the InstagramCaller instance Once in the Controller Life Cycle.

// Also, you can optionally add the Telemetry Client ID of Azure Application Insights APIs.
protected override void OnActionExecuting(ActionExecutingContext filterContext)
{
    if (_InstagramCaller == null)
    {
        string tokent ="";
        string userid = "";
        string id = ConfigurationManager.AppSettings["clientid"].ToString();
        string secret = ConfigurationManager.AppSettings["clientsecret"].ToString();
        if (((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "_token").Count() > 0)
        {
            tokent = (((ClaimsIdentity)HttpContext.User.Identity).Claims.First(c => c.Type == "_token").Value ?? "");
        }
        if (((ClaimsIdentity)User.Identity).Claims.Where(c => c.Type == "_id").Count() > 0)
        {
            userid = (((ClaimsIdentity)HttpContext.User.Identity).Claims.First(c => c.Type == "_id").Value != null ? ((ClaimsIdentity)HttpContext.User.Identity).Claims.First(c => c.Type == "_id").Value : "");
        }

        if(tokent !="" && userid !="")
        _InstagramCaller = new InstagramCaller.InstagramCaller(id, secret, long.Parse(userid), tokent, "APPLICATION_INSIGHTS_KEY");
    }
}

//add Claims to UserIdentity Class to attach the AccessToken and UserID (Instagram's UserID) to each User
// To do that Modify IdentityModels.cs class 
public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
{

    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    // Add custom user claims here
    userIdentity.AddClaim(new Claim("_token", InstagramAccessToken));
    userIdentity.AddClaim(new Claim("_id", InstagramID.ToString()));
    userIdentity.AddClaim(new Claim("_username", InstagramUserName));
    return userIdentity;
}

//add these methods as well to the ApplicationUser Class to help you set the token and profile pictures.
public void setToken(string token) {
    InstagramAccessToken = token;
}

public void setProfilePicture(string profilepicture)
{
    InstagramProfilePicture = profilepicture;
}

//To fetch the new AccessToken in Case the User ReInvoke the App Authentication 
//you need to modify the ExternalLogin method in AccountContrller
public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
{
    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
    if (loginInfo == null)
    {
        return RedirectToAction("Login");
    }

    // Sign in the user with this external login provider if the user already has a login
    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
    switch (result)
    {
        case SignInStatus.Success:
            //we Get the User and Update the Local AccessToken.
            ApplicationUser user = UserManager.FindByName(loginInfo.DefaultUserName);
            user.InstagramAccessToken = loginInfo.ExternalIdentity.Claims.First(c => c.Type == "urn:instagram:accesstoken").Value;
            user.setToken(user.InstagramAccessToken);
            UserManager.Update(user);
            return RedirectToLocal(returnUrl);
        case SignInStatus.LockedOut:
            return View("Lockout");
        case SignInStatus.RequiresVerification:
            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        case SignInStatus.Failure:
        default:
            // If the user does not have an account, then prompt the user to create an account
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
    }
}

//Then you can call the Instagram APIs using the InstagramCaller 
//Here we retrive the UserBasicInfo and his media of the current logged user
InstagramCaller.ViewModels.UserInfo_Media userWithMedia = await _InstagramCaller.UsersEndPoint.Self_GetInfoandMedia();



