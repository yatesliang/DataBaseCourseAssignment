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
    
    public partial class scenicPos
    {
        public short scenicID { get; set; }
        public Nullable<decimal> longitue { get; set; }
        public Nullable<decimal> latitude { get; set; }
        public string address { get; set; }
    
        public virtual scenicSpot scenicSpot { get; set; }
    }
}
