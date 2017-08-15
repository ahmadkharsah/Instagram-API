using InstagramCaller.Models.Media;
using InstagramCaller.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramCaller.ViewModels
{
    public class UserInfo_Media
    {
        public Models.User.User UserInfo { get; set; }
        public Models.Media.UserMedia Media { get; set; }
    }
}
