//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SceneView.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class userInfo
    {
        public string userID { get; set; }
        public string nickname { get; set; }
        public string gender { get; set; }
        public string headPortrait { get; set; }
        public string introduction { get; set; }
        public Nullable<long> phoneNumber { get; set; }
        public string SECRETQUESTION { get; set; }
        public string SQANSWER { get; set; }
    
        public virtual user user { get; set; }
    }
}
