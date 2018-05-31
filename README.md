# Instagram-API
Instagram-API is a Library used to Call Instagram API for ASP.Net MVC.
Also, Instagram-API integrated into Azure Application Insights API, All Endpoint in the library calls the Azure Application Insights API if any Exception is occured.

# Usage
To start use Instagram-API you need to Create Instance of the InstagramCaller Class.

# Version 1.0.1 
I add a feature to the library which is the enablement of Azure Application Insights of the Application.
all what is required to do to use this feature is to Enable Azure Application Insights for the application and get the telemetry key, then you can use this key optionally in each call of Instagram APIs.

```C#
protected override void OnActionExecuting(ActionExecutingContext filterContext)
{
  if (_InstagramCaller == null)
  {
      string tokent = "";
      string userid = "";
      string id = ConfigurationManager.AppSettings["clientid"].ToString();
      string secret = ConfigurationManager.AppSettings["clientsecret"].ToString();
      ApplicationUser user = _db.Users.Find(Session["current_user"].ToString());
      tokent = user.InstagramAccessToken;
      userid = user.InstagramID;

  if (tokent != "" && userid != "")
          _InstagramCaller = new InstagramCaller.InstagramCaller(id, secret, long.Parse(userid), tokent, "TELEMETRY_KEY");
  }
}
```

1. add Instagram as authenticatioin provider to your App
To do that Modyify the Startup.Auth.cs class 
Add required Scopes that your APP will access
```C#
string clientid = ConfigurationManager.AppSettings["clientid"].ToString();
string clientsecret = ConfigurationManager.AppSettings["clientsecret"].ToString();
app.UseInstagramInAuthentication(new InstagramAuthenticationOptions(){
                                    ClientId = clientid,
                                    ClientSecret = clientsecret,
                                    Scope = { "basic", "public_content", "follower_list", "comments", "relationships","likes" }
```

2. InstagramCaller as a Property in the Conroller Class 
```C#
public static InstagramCaller.InstagramCaller _InstagramCaller { get; set; }
```
3. Override the OnActionExecuting method to check if the InstagramCaller instance is execit , if not use the ClientID, ClientSecret, UserAccessToken and UserID (Instagram's UserID) to create the InstagramCaller instance Once in the Controller Life Cycle.
Also, you can optionally add the Telemetry Client ID of Azure Application Insights APIs.
```C#
protected override void OnActionExecuting(ActionExecutingContext filterContext)
{
  if (_InstagramCaller == null)
  {
      string tokent = "";
      string userid = "";
      string id = ConfigurationManager.AppSettings["clientid"].ToString();
      string secret = ConfigurationManager.AppSettings["clientsecret"].ToString();
      ApplicationUser user = _db.Users.Find(Session["current_user"].ToString());
      tokent = user.InstagramAccessToken;
      userid = user.InstagramID;

  if (tokent != "" && userid != "")
          _InstagramCaller = new InstagramCaller.InstagramCaller(id, secret, long.Parse(userid), tokent, "TELEMETRY_KEY");
  }
}
```
4. add these methods as well to the ApplicationUser Class to help you set the token and profile pictures.
```c#
public void setToken(string token) {
    InstagramAccessToken = token;
}

public void setProfilePicture(string profilepicture)
{
    InstagramProfilePicture = profilepicture;
}
```
5. To fetch the new AccessToken in Case the User ReInvoke the App Authentication 
you need to modify the ExternalLogin method in AccountContrller
```c#
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
            ApplicationUser user = UserManager.FindByName(loginInfo.DefaultUserName);
            string accesstoken = loginInfo.ExternalIdentity.Claims.First(c => c.Type == "urn:instagram:accesstoken").Value;
            long userid =long.Parse(loginInfo.ExternalIdentity.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            user.InstagramAccessToken = accesstoken;
            _instagramcaller = new InstagramCaller.InstagramCaller(ConfigurationManager.AppSettings["ClientID"], ConfigurationManager.AppSettings["ClientSecret"], userid, accesstoken);
            InstagramCaller.Models.User.User instagramuser = await _instagramcaller.UsersEndPoint.Self_GetInfo(user.InstagramAccessToken);
            user.InstagramFollow = instagramuser.data.counts.follows;
            user.InstagramFollowedby = instagramuser.data.counts.followed_by;
            user.InstagramMediaCount = instagramuser.data.counts.media;
            user.InstagramWebSite = instagramuser.data.website;
            user.InstagramBio = instagramuser.data.bio;
            user.setToken(user.InstagramAccessToken);
            UserManager.Update(user);
            Session["current_user"] = "";
            Session["current_user"] = user.Id;
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
```
6. Then you can call the Instagram APIs using the InstagramCaller 
Here we retrive the UserBasicInfo and his media of the current logged user
```c#
InstagramCaller.ViewModels.UserInfo_Media userWithMedia = await _InstagramCaller.UsersEndPoint.Self_GetInfoandMedia("CURRENT_USER_ACCESS_TOKEN");
```


