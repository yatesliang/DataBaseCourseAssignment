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
    
    public partial class strategy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public strategy()
        {
            this.strategyImage = new HashSet<strategyImage>();
        }
    
        public short strategyID { get; set; }
        public Nullable<short> scenicID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    
        public virtual scenicSpot scenicSpot { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<strategyImage> strategyImage { get; set; }
    }
}
