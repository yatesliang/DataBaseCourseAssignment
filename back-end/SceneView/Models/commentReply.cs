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
    
    public partial class commentReply
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public commentReply()
        {
            this.commentReplyMes = new HashSet<commentReplyMes>();
        }
    
        public long commentReplyID { get; set; }
        public long replyToCommentID { get; set; }
        public string userID { get; set; }
        public string commentContent { get; set; }
        public System.DateTime commentTime { get; set; }
    
        public virtual comment comment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<commentReplyMes> commentReplyMes { get; set; }
        public virtual user user { get; set; }
    }
}
